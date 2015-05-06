@ECHO OFF
REM **************************************************************************************************************************
REM main paths
REM **************************************************************************************************************************
set GG=C:\projects\gringlobal
set NONSVN=C:\projects\gringlobal_non-svn

REM Copy file descriptor file to relative path
copy "%GG%\offline_install.xml" "%NONSVN%\offline_install.xml"
copy "%GG%\Setup.bat" "%NONSVN%\Setup.bat"


REM **************************************************************************************************************************
REM subfolders from main paths
REM **************************************************************************************************************************
set WEBINST=%NONSVN%\web_installer


REM **************************************************************************************************************************
REM cabarc tool from CAB SDK -- http://support.microsoft.com/kb/310618
REM **************************************************************************************************************************

set CABEXE=%NONSVN%\cabsdk\bin\cabarc.exe


REM **************************************************************************************************************************
REM Add in Installers, data cabs, and descriptor file for the local CD cab file
REM **************************************************************************************************************************
del "%WEBINST%\GrinGlobal_CD.cab"
%CABEXE% n "%NONSVN%\GrinGlobal_CD.cab" "%WEBINST%\*.exe" "%WEBINST%\*.msi" "%WEBINST%\*_data.cab" "%WEBINST%\*_search.cab" "%GG%\offline_install.xml" "%GG%\Setup.bat"
move "%NONSVN%\GrinGlobal_CD.cab" "%WEBINST%\GrinGlobal_CD.cab"


echo Done!
pause