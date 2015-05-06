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
set WEBINST=%NONSVN%\web_installer
set PREREQ=%NONSVN%\preqrequisites_installer

REM **************************************************************************************************************************
REM Note this path MUST match what is in the GrinGlobal.Search.Engine.Tester\bin\Debug\gringlobal.search.config file!!!
REM **************************************************************************************************************************
set INDEXSOURCE=%ALLUSERSPROFILE%\GRIN-Global\GRIN-Global Search Engine\indexes


REM **************************************************************************************************************************
REM cabarc tool from CAB SDK -- http://support.microsoft.com/kb/310618
REM **************************************************************************************************************************

set CABEXE=%NONSVN%\cabsdk\bin\cabarc.exe


REM **************************************************************************************************************************
REM All search data is dynamically downloaded, and therefore not included in an MSI.  Just put it in the web installer folder.
REM **************************************************************************************************************************


REM Accession data
%CABEXE% n %WEBINST%\accession_search.cab "%INDEXSOURCE%\accession*.*" + "%INDEXSOURCE%\geography*.*"

REM Cooperator data
%CABEXE% n %WEBINST%\cooperator_search.cab "%INDEXSOURCE%\cooperator*.*"

REM Observation data
%CABEXE% n %WEBINST%\observation_search.cab "%INDEXSOURCE%\crop_trait_observation*.*"

REM Inventory data
%CABEXE% n %WEBINST%\inventory_search.cab "%INDEXSOURCE%\inventory*.*" + "%INDEXSOURCE%\order_*.*"

REM Taxonomy data
%CABEXE% n %WEBINST%\taxonomy_search.cab "%INDEXSOURCE%\taxonomy*.*"


echo %date% %time% Done
pause