@echo off
set PFDIR=%PROGRAMFILES(x86)%
if "%PFDIR%" == "" set PFDIR=%PROGRAMFILES%
echo %PFDIR%
"%PFDIR%\GRIN-Global\GRIN-Global Database\GrinGlobal.DatabaseCopier.exe" /precache sqlserver "Data Source=localhost\sqlexpress;initial catalog=gringlobal;integrated security=sspi" localhost