#requires -Version 5.1
<#
.SYNOPSIS
Packages existing publish output with the Release Launcher and x86 .NET 8.
.DESCRIPTION
Creates bin/Publish/<version>_with_runtime.zip without building or publishing.
Downloads the latest stable .NET 8 core and Windows Desktop runtime ZIPs from
Microsoft, verifies their SHA-512 hashes, and caches them in
bin/Publish/.runtime-cache. Nothing is installed on the packaging machine.
The packaged dotnet.exe is marked Large Address Aware after extraction.
Use -RuntimeVersion to select a specific 8.0 patch release.
.EXAMPLE
.\scripts\Pack-WithRuntime.ps1
.EXAMPLE
.\scripts\Pack-WithRuntime.ps1 -RuntimeVersion 8.0.31
#>
[CmdletBinding()]
param(
    [string]$PublishDirectory,
    [string]$OutputDirectory,
    [ValidatePattern('^8\.0\.\d+$')]
    [string]$RuntimeVersion,
    [string]$LauncherPath
)

# Reuse staging, version checks, LAA on the packaged CLR host, and UTF-8 ZIPs.
$ErrorActionPreference = 'Stop'
& (Join-Path $PSScriptRoot 'Pack.ps1') @PSBoundParameters -WithRuntime
