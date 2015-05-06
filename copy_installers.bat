@ECHO OFF
REM **************************************************************************************************************************
REM main paths
REM **************************************************************************************************************************
set GG=C:\projects\gringlobal
set NONSVN=C:\projects\gringlobal_non-svn
set SERVER=http://grin-global-dev4.agron.iastate.edu/gringlobal/uploads/installers
set LOCALIISDIR=C:\inetpub\wwwroot\gringlobal
set APPVERSION=latest


REM ***************************************************************************************************************************
REM Copy msi / exe files to version-specific installer path on local webserver (so Updater can locate them after a client downloads them from the local website)
REM ***************************************************************************************************************************
echo %date% %time% Copying setup files and data cab files to local web server...
mkdir "%LOCALIISDIR%\uploads\installers\%APPVERSION%\"
copy "%NONSVN%\web_installer\*.*" "%LOCALIISDIR%\uploads\installers\%APPVERSION%\"

REM ***************************************************************************************************************************
REM Copy Updater msi / exe files to installer path on local webserver (so Updater can locate them after a client downloads them)
REM ***************************************************************************************************************************
echo %date% %time% Copying Updater to download area on local web server...
copy "%NONSVN%\web_installer\GrinGlobal_Updater_Setup.*" "%LOCALIISDIR%\uploads\installers\"

REM ***************************************************************************************************************************
REM Note time when completed
REM ***************************************************************************************************************************
echo %date% %time% Done

pause