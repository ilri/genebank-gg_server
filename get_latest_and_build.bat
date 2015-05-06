@ECHO OFF
REM **************************************************************************************************************************
REM main paths
REM **************************************************************************************************************************
set GG=C:\projects\gringlobal
set NONSVN=C:\projects\gringlobal_non-svn
set SERVER=http://grin-global-dev4.agron.iastate.edu/gringlobal/uploads/installers
set LOCALIISDIR=C:\inetpub\wwwroot\gringlobal
set APPVERSION=beta3


REM ***************************************************************************************************************************
REM Calculate a good, descriptive name for the log file using current date/time
REM ***************************************************************************************************************************
for /f "tokens=1,2" %%u in ('date /t') do set d=%%v
for /f "tokens=1" %%u in ('time /t') do set t=%%u
if "%t:~1,1%"==":" set t=0%t%
set timestr=%d:~6,4%%d:~0,2%%d:~3,2%_%t:~0,2%%t:~3,2%

set LOGFILE=%NONSVN%\logs\log-%timestr%.txt

echo Logfile=%LOGFILE%

REM ***************************************************************************************************************************
REM Pull latest from SVN
REM ***************************************************************************************************************************

echo %date% %time% Getting latest from SVN...
echo %date% %time% Getting latest from SVN... > %LOGFILE% 2>&1
tortoiseproc.exe /command:update /path:"%GG%" /logmsg:"pulling latest for nightly build" /closeonend:1 >> %LOGFILE% 2>&1


REM ***************************************************************************************************************************
REM Rebuild the solution
REM ***************************************************************************************************************************

call "c:\Program Files\Microsoft Visual Studio 9.0\VC\vcvarsall.bat" x86 

echo %date% %time% Rebuilding GrinGlobal_Autobuild.sln ...
echo %date% %time% Rebuilding GrinGlobal_Autobuild.sln ... >> %LOGFILE% 2>&1
devenv.exe "%GG%\GrinGlobal_BuildAll.sln" /Rebuild "Debug" /Out %LOGFILE%


REM ***************************************************************************************************************************
REM Point Updater's Setup.exe at absolute url for target http server (and the corresponding msi be relative to that url)
REM ***************************************************************************************************************************
echo %date% %time% Pointing Updater's Setup.exe at aboslute url (%SERVER%)...
echo %date% %time% Pointing Updater's Setup.exe at aboslute url (%SERVER%)... >> %LOGFILE% 2>&1
msistuff.exe "%NONSVN%\web_installer\GrinGlobal_Updater_Setup.exe" /u %SERVER% /d GrinGlobal_Updater_Setup.msi >> %LOGFILE% 2>&1


REM ***************************************************************************************************************************
REM Publish GrinGlobal.Web to local webserver at /gringlobal vdir
REM ***************************************************************************************************************************
echo %date% %time% Precompiling web site and publishing to local web server...
echo %date% %time% Precompiling web site and publishing to local web server... >> %LOGFILE% 2>&1
rmdir /s /q "%LOCALIISDIR%\uploads"
aspnet_compiler.exe -nologo -v /gringlobal -p "%GG%\GrinGlobal.Web" -f -u -fixednames "%LOCALIISDIR%"


REM ***************************************************************************************************************************
REM Copy msi / exe files to version-specific installer path on local webserver (so Updater can locate them after a client downloads them from the local website)
REM ***************************************************************************************************************************
echo %date% %time% Copying setup files and data cab files to local web server...
echo %date% %time% Copying setup files and data cab files to local web server... >> %LOGFILE% 2>&1
mkdir "%LOCALIISDIR%\uploads\installers\%APPVERSION%\"
copy "%NONSVN%\web_installer\*.*" "%LOCALIISDIR%\uploads\installers\%APPVERSION%\"

REM ***************************************************************************************************************************
REM Copy Updater msi / exe files to installer path on local webserver (so Updater can locate them after a client downloads them)
REM ***************************************************************************************************************************
echo %date% %time% Copying Updater to download area on local web server...
echo %date% %time% Copying Updater to download area on local web server... >> %LOGFILE% 2>&1
copy "%NONSVN%\web_installer\GrinGlobal_Updater_Setup.*" "%LOCALIISDIR%\uploads\installers\"

REM ***************************************************************************************************************************
REM Note time when completed
REM ***************************************************************************************************************************
echo %date% %time% Done
echo %date% %time% Completed >> %LOGFILE% 2>&1

notepad.exe %LOGFILE%

pause