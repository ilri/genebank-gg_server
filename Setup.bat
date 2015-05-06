@ECHO OFF

REM install updater...
echo Installing Updater...

start /w GrinGlobal_Updater_Setup.exe /passive


REM echo %ERRORLEVEL%
REM pause

set PFILE=%ProgramFiles(x86)%
IF "%PFILE%" == "" set PFILE=%ProgramFiles%
REM echo %PFILE%

set CURDIR=%CD%
set OFFLINEXML=%CURDIR%\offline_install.xml

REM echo offlinexml=%OFFLINEXML%

REM pause

set DESTDIR=%PFILE%\GRIN-Global\GRIN-Global Updater

echo Waiting for Updater install to finish...
:INSTALLDONE
ping 127.0.0.1 -n 3 >NUL
IF EXIST "%DESTDIR%\GrinGlobal.Updater.exe" (
echo Launching Updater...
cd /d "%DESTDIR%"
start GrinGlobal.Updater.exe /conn "%OFFLINEXML%" /refresh
exit
) else (
GOTO :INSTALLDONE
)
