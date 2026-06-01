# QTRHacker

QTRHacker is a WPF-based Terraria helper/patch tool. This branch targets
Terraria 1.4.5.6.

## Requirements
* Windows 10 x64
* [.NET 7 Desktop Runtime](https://dotnet.microsoft.com/zh-cn/download/dotnet/thank-you/runtime-desktop-7.0.20-windows-x86-installer?cid=getdotnetcore) x86
* .NET Framework 4.6 for Terraria
* Visual Studio C++ build tools for the native `QHackCLR` project when building
  the full application

## Build

Build the managed patch assembly with the solution directory set explicitly
when using `dotnet` from the command line:

```powershell
dotnet build src\QTRHacker.Patches\QTRHacker.Patches.csproj -c Release /p:SolutionDir="$PWD\"
```

When testing a Release build, make sure the generated
`QTRHacker.Patches.dll` is copied to the Release output directory. A stale
Debug or Release patch assembly can leave Terraria running older patch code.

## Notes

* Supports Terraria 1.4.5.6 game references.
* Fixes right-click full-screen map teleport coordinate conversion so map
  clicks resolve to the clicked world position.
* If Terraria has already loaded `QTRHacker.Patches.dll`, restart Terraria or
  reload the patch assembly before retesting changes.

## Contact
Discord: https://discord.gg/bzKc9vM

QQ: 2393868407

Group: 850984295
