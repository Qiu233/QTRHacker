@echo off

set CFG=%1
IF NOT [%CFG%]==[] GOTO BUILD
set /p CFG=Enter the configuration to build:
echo %CFG%
:BUILD
msbuild -p:Configuration=%CFG%
pause