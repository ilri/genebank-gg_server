@ECHO OFF

:RELAUNCH
echo stopping search engine service...
net stop ggse

echo launching search engine tester...
C:\projects\GrinGlobal\GrinGlobal.Search.Engine.Tester\bin\Debug\GrinGlobal.Search.Engine.Tester.exe /autostart

SET /P RUNAGAIN=Run again? (y/n)
IF "%RUNAGAIN%"=="y" GOTO RELAUNCH