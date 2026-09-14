#requires -Version 5.1
<#
.SYNOPSIS
Packages existing Visual Studio publish output without building anything.
.DESCRIPTION
Run after publishing in Visual Studio. Defaults are relative to this script,
not the current working directory. Creates bin/Publish/<version>.zip.
Use Pack-WithRuntime.ps1 for the portable .NET 8 desktop runtime package.
ZIP entry names use UTF-8; file contents are copied without text conversion.
Enables LAA on the staged execution host: QTRHacker.exe, or dotnet/dotnet.exe
when bundling the runtime. Original publish files and runtime caches are untouched.
.EXAMPLE
.\scripts\Pack.ps1
#>
[CmdletBinding()]
param(
    [string]$PublishDirectory,
    [string]$OutputDirectory,
    [switch]$WithRuntime,
    [ValidatePattern('^8\.0\.\d+$')]
    [string]$RuntimeVersion,
    [string]$LauncherPath
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.IO.Compression.FileSystem

if (-not $PublishDirectory) { $PublishDirectory = Join-Path $PSScriptRoot '..\bin\Publish\Vanilla' }
if (-not $OutputDirectory) { $OutputDirectory = Join-Path $PSScriptRoot '..\bin\Publish' }
if (-not $LauncherPath) { $LauncherPath = Join-Path $PSScriptRoot '..\bin\Launcher\Win32_Release\QTRHacker.exe' }

function Assert-X86Executable([string]$Path) {
    $stream = [IO.File]::OpenRead($Path)
    $reader = [IO.BinaryReader]::new($stream)
    try {
        if ($reader.ReadUInt16() -ne 0x5A4D) { throw "Not a Windows executable: $Path" }
        $stream.Position = 0x3C
        $peOffset = $reader.ReadInt32()
        $stream.Position = $peOffset
        if ($reader.ReadUInt32() -ne 0x4550 -or $reader.ReadUInt16() -ne 0x014C) {
            throw "Expected an x86 executable: $Path"
        }
    }
    finally { $reader.Dispose() }
}

function Get-RuntimeArchive($File, [string]$CacheDirectory) {
    $ProgressPreference = 'SilentlyContinue'
    $fileName = [IO.Path]::GetFileName(([uri]$File.url).AbsolutePath)
    $cached = Join-Path $CacheDirectory $fileName
    if ((Test-Path -LiteralPath $cached -PathType Leaf) -and
        (Get-FileHash -LiteralPath $cached -Algorithm SHA512).Hash -eq $File.hash) {
        Write-Host "Using verified cache: $fileName"
        return $cached
    }

    $download = "$cached.$([guid]::NewGuid().ToString('N')).download"
    try {
        Write-Host "Downloading $fileName"
        Invoke-WebRequest -Uri $File.url -UseBasicParsing -OutFile $download
        if ((Get-FileHash -LiteralPath $download -Algorithm SHA512).Hash -ne $File.hash) {
            throw "SHA-512 verification failed: $fileName"
        }
        # Windows PowerShell converts $null to an empty string; the backup path must be a true .NET null.
        if ([IO.File]::Exists($cached)) { [IO.File]::Replace($download, $cached, [NullString]::Value) }
        else { [IO.File]::Move($download, $cached) }
        return $cached
    }
    finally {
        if ([IO.File]::Exists($download)) { [IO.File]::Delete($download) }
    }
}

function Expand-RuntimeArchive([string]$ArchivePath, [string]$Destination) {
    # Overlay the core and desktop archives, including shared license files.
    $root = [IO.Path]::GetFullPath($Destination).TrimEnd('\') + '\'
    [void][IO.Directory]::CreateDirectory($root)
    $archive = [IO.Compression.ZipFile]::OpenRead($ArchivePath)
    try {
        foreach ($entry in $archive.Entries) {
            $target = [IO.Path]::GetFullPath((Join-Path $root $entry.FullName))
            if (-not $target.StartsWith($root, [StringComparison]::OrdinalIgnoreCase)) {
                throw "Archive entry escapes its destination: $($entry.FullName)"
            }
            if ([string]::IsNullOrEmpty($entry.Name)) {
                [void][IO.Directory]::CreateDirectory($target)
            }
            else {
                [void][IO.Directory]::CreateDirectory([IO.Path]::GetDirectoryName($target))
                [IO.Compression.ZipFileExtensions]::ExtractToFile($entry, $target, $true)
            }
        }
    }
    finally { $archive.Dispose() }
}

$publishRoot = (Resolve-Path -LiteralPath $PublishDirectory).ProviderPath.TrimEnd('\')
$outputRoot = $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath($OutputDirectory).TrimEnd('\')
if ($outputRoot.Equals($publishRoot, [StringComparison]::OrdinalIgnoreCase) -or
    $outputRoot.StartsWith($publishRoot + '\', [StringComparison]::OrdinalIgnoreCase)) {
    throw 'OutputDirectory must be outside PublishDirectory.'
}
foreach ($name in @('QTRHacker.exe', 'QTRHacker.dll', 'QTRHacker.deps.json', 'QTRHacker.runtimeconfig.json',
        'QTRHacker.Core.dll', 'QTRHacker.Patches.dll', 'QHackCLR.dll', 'QHackLib.dll', 'Ijwhost.dll')) {
    if (-not (Test-Path -LiteralPath (Join-Path $publishRoot $name) -PathType Leaf)) {
        throw "Missing publish output: $name. Publish in Visual Studio first."
    }
}
if (Test-Path -LiteralPath (Join-Path $publishRoot 'dotnet')) {
    throw 'Vanilla already contains a dotnet directory. Use the runtime-free Visual Studio publish output.'
}
Assert-X86Executable (Join-Path $publishRoot 'QTRHacker.exe')

$projectPath = Join-Path $PSScriptRoot '..\src\QTRHacker\QTRHacker.csproj'
$project = [xml](Get-Content -LiteralPath $projectPath -Raw -Encoding UTF8)
$version = $project.SelectSingleNode('/Project/PropertyGroup/Version').InnerText.Trim()
if ($version -notmatch '^\d+\.\d+\.\d+\.\d+$') { throw "Invalid project version: $version" }
$publishedVersion = [Diagnostics.FileVersionInfo]::GetVersionInfo((Join-Path $publishRoot 'QTRHacker.dll')).FileVersion
if ($publishedVersion -ne $version) {
    throw "Published version $publishedVersion differs from project version $version. Publish in Visual Studio first."
}
$runtimeConfig = Get-Content -LiteralPath (Join-Path $publishRoot 'QTRHacker.runtimeconfig.json') -Raw -Encoding UTF8 | ConvertFrom-Json
if ($runtimeConfig.runtimeOptions.tfm -ne 'net8.0') { throw 'Expected .NET 8 publish output.' }

$release = $null
if ($WithRuntime) {
    $LauncherPath = (Resolve-Path -LiteralPath $LauncherPath).ProviderPath
    Assert-X86Executable $LauncherPath
    # Microsoft's release metadata supplies the download URLs and SHA-512 hashes.
    $metadata = Invoke-RestMethod -Uri 'https://builds.dotnet.microsoft.com/dotnet/release-metadata/8.0/releases.json'
    if (-not $RuntimeVersion) { $RuntimeVersion = $metadata.'latest-release' }
    $release = @($metadata.releases | Where-Object { $_.'release-version' -eq $RuntimeVersion })
    if ($release.Count -ne 1) { throw "No .NET 8 release found for $RuntimeVersion." }
    $release = $release[0]
    foreach ($framework in $runtimeConfig.runtimeOptions.frameworks) {
        $provided = switch ($framework.name) {
            'Microsoft.NETCore.App' { $release.runtime.version }
            'Microsoft.WindowsDesktop.App' { $release.windowsdesktop.version }
            default { throw "Unsupported framework: $($framework.name)" }
        }
        if ($provided -notmatch '^8\.0\.\d+$' -or [version]$provided -lt [version]$framework.version) {
            throw "Runtime $provided does not satisfy $($framework.name) $($framework.version)."
        }
    }
}

[void][IO.Directory]::CreateDirectory($outputRoot)
$suffix = if ($WithRuntime) { '_with_runtime' } else { '' }
$archivePath = Join-Path $outputRoot "$version$suffix.zip"
$workRoot = Join-Path $outputRoot ('.package-' + [guid]::NewGuid().ToString('N'))
$payload = Join-Path $workRoot 'payload'
$temporaryArchive = Join-Path $workRoot 'package.zip'
try {
    [void][IO.Directory]::CreateDirectory($workRoot)
    Copy-Item -LiteralPath $publishRoot -Destination $payload -Recurse -Force

    if ($WithRuntime) {
        $cache = Join-Path $outputRoot ".runtime-cache\$RuntimeVersion"
        [void][IO.Directory]::CreateDirectory($cache)
        $runtimeRoot = Join-Path $payload 'dotnet'
        foreach ($component in @($release.runtime, $release.windowsdesktop)) {
            $files = @($component.files | Where-Object { $_.rid -eq 'win-x86' -and $_.name.EndsWith('.zip') })
            if ($files.Count -ne 1) { throw "Expected one win-x86 ZIP for runtime $($component.version)." }
            $downloaded = Get-RuntimeArchive $files[0] $cache
            Expand-RuntimeArchive $downloaded $runtimeRoot
        }
        foreach ($relative in @('dotnet.exe', "host\fxr\$($release.runtime.version)\hostfxr.dll",
                "shared\Microsoft.NETCore.App\$($release.runtime.version)\coreclr.dll",
                "shared\Microsoft.WindowsDesktop.App\$($release.windowsdesktop.version)\PresentationFramework.dll")) {
            if (-not (Test-Path -LiteralPath (Join-Path $runtimeRoot $relative) -PathType Leaf)) {
                throw "Incomplete portable runtime: $relative"
            }
        }
        Assert-X86Executable (Join-Path $runtimeRoot 'dotnet.exe')
        # Only replace the staged apphost. The original Vanilla output is untouched.
        Copy-Item -LiteralPath $LauncherPath -Destination (Join-Path $payload 'QTRHacker.exe') -Force
    }

    # The runtime package's Launcher only starts dotnet.exe; LAA belongs on the
    # process actually hosting the CLR. Also handle older non-LAA publish output.
    $executionHost = if ($WithRuntime) { Join-Path $runtimeRoot 'dotnet.exe' } else { Join-Path $payload 'QTRHacker.exe' }
    & (Join-Path $PSScriptRoot 'Set-LargeAddressAware.ps1') -Path $executionHost

    Write-Host "Creating $archivePath"
    [IO.Compression.ZipFile]::CreateFromDirectory($payload, $temporaryArchive,
        [IO.Compression.CompressionLevel]::Optimal, $false, [Text.Encoding]::UTF8)
    # Publish only a completed ZIP; a failed run leaves the previous package intact.
    # Use a true .NET null for the backup path, including on Windows PowerShell 5.1.
    if ([IO.File]::Exists($archivePath)) { [IO.File]::Replace($temporaryArchive, $archivePath, [NullString]::Value) }
    else { [IO.File]::Move($temporaryArchive, $archivePath) }
}
finally {
    # Verify the exact absolute staging directory before recursive cleanup.
    $cleanupPath = [IO.Path]::GetFullPath($workRoot)
    if (-not $cleanupPath.StartsWith($outputRoot + '\', [StringComparison]::OrdinalIgnoreCase) -or
        [IO.Path]::GetFileName($cleanupPath) -notmatch '^\.package-[0-9a-f]{32}$') {
        throw "Refusing to remove unexpected staging directory: $cleanupPath"
    }
    if (Test-Path -LiteralPath $cleanupPath) { Remove-Item -LiteralPath $cleanupPath -Recurse -Force }
}

[pscustomobject]@{
    Archive = $archivePath
    Version = $version
    WithRuntime = $WithRuntime.IsPresent
    RuntimeVersion = if ($WithRuntime) { $RuntimeVersion } else { $null }
    SizeBytes = (Get-Item -LiteralPath $archivePath).Length
}
