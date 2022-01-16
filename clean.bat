@echo off

msbuild -t:clean
rmdir /s /q .\bin\Debug
rmdir /s /q .\bin\Release
pause