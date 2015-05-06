@ECHO OFF
set URL=http://grin-global-test1.agron.iastate.edu/gringlobal/uploads/installers/latest
IF "%1" == "" GOTO CONTINUE
set URL=%1
:CONTINUE
rem echo url=%URL%
@ECHO ON
"C:\Program Files\Microsoft SDKs\Windows\v6.0A\Bin\msistuff.exe" GrinGlobal_Updater_Setup.exe /u %URL% /d GrinGlobal_Updater_Setup.msi


echo %date% %time% Done
pause