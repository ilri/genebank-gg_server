@ECHO OFF
set PFILE=%ProgramFiles(x86)%
IF "%PFILE%" == "" set PFILE=%ProgramFiles%
echo %PFILE%
REM "%PFILE%\GRIN-Global\GRIN-Global Updater\GrinGlobal.Updater.exe" /conn ".\offline.xml" /refresh
REM pause
