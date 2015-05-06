@ECHO OFF

REM **************************************************************************************************************************
REM main paths
REM **************************************************************************************************************************
set GG=C:\projects\gringlobal
set NONSVN=C:\projects\gringlobal_non-svn


REM ***************************************************************************************************************************
REM Stop search engine if it's going
REM ***************************************************************************************************************************
net stop ggse

REM ***************************************************************************************************************************
REM Use search engine test app to recreate indexes
REM ***************************************************************************************************************************
"%GG%\GrinGlobal.Search.Engine.Tester\bin\Debug\GrinGlobal.Search.Engine.Tester.exe" /recreateallenabled

REM ***************************************************************************************************************************
REM Restart search engine if needed
REM ***************************************************************************************************************************
REM net start ggse


echo %date% %time% Done
pause