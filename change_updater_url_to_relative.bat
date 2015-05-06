@ECHO OFF
set URL=
IF "%1" == "" GOTO CONTINUE
set URL=%1
:CONTINUE
rem echo url=%URL%
@ECHO ON
"C:\Program Files\Microsoft SDKs\Windows\v6.0A\Bin\msistuff.exe" C:\Inetpub\wwwroot\gringlobal\uploads\installers\GrinGlobal_Updater_Setup.exe /u %URL% /d GrinGlobal_Updater_Setup.msi


echo %date% %time% Done
pause