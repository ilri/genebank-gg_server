@ECHO OFF
REM **************************************************************************************************************************
REM main paths
REM **************************************************************************************************************************
set GG=C:\projects\gringlobal
set NONSVN=C:\projects\gringlobal_non-svn


REM **************************************************************************************************************************
REM subfolders from main paths
REM **************************************************************************************************************************
set RAWDATA=%NONSVN%\raw_data_files
set RAWSEARCH=%NONSVN%\raw_search_files
set WEBINST=%NONSVN%\web_installer
set PREREQ=%NONSVN%\preqrequisites_installer


REM **************************************************************************************************************************
REM cabarc tool from CAB SDK -- http://support.microsoft.com/kb/310618
REM **************************************************************************************************************************

set CABEXE=%NONSVN%\cabsdk\bin\cabarc.exe


REM **************************************************************************************************************************
REM System data -- is included in MSI, so copy to svn folder since the installer project directly references it
REM **************************************************************************************************************************

del %GG%\gringlobal.database.setup\utility64.cab
"%CABEXE%" n "%GG%\gringlobal.database.setup\utility64.cab" "%GG%\gringlobal.utility64\bin\Debug\GrinGlobal.Utility64.exe"


echo %date% %time% Done
pause