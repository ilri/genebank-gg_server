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

REM do not package up the user settings tables, those should be empty from the get-go
del %RAWDATA%\app_user_item_list.txt
del %RAWDATA%\sys_user_gui_setting.txt


del %GG%\gringlobal.database.setup\system_data.cab
%CABEXE% n %GG%\gringlobal.database.setup\system_data.cab %RAWDATA%\sys_*.* + %RAWDATA%\cooperator*.* + %RAWDATA%\app_*.* + %RAWDATA%\web_user.* + %RAWDATA%\web_user_preference.* + %RAWDATA%\web_cooperator.* + %RAWDATA%\site.* + %RAWDATA%\region*.* + %RAWDATA%\geo*.* + %RAWDATA%\code*.* 



REM **************************************************************************************************************************
REM All other data is dynamically downloaded, and therefore not included in an MSI.  Just put it in the web installer folder.
REM **************************************************************************************************************************


REM Taxonomy data
del %WEBINST%\taxonomy_data.cab
%CABEXE% n %WEBINST%\taxonomy_data.cab %RAWDATA%\taxonomy*.* + %RAWDATA%\crop.* + %RAWDATA%\citation*.* + %RAWDATA%\literature*.* 

REM Accession / Inventory data
del %WEBINST%\accession_data.cab 
%CABEXE% n %WEBINST%\accession_data.cab %RAWDATA%\accession*.* + %RAWDATA%\inventory*.* + %RAWDATA%\method*.* + %RAWDATA%\site_*.* + %RAWDATA%\name_*.*
REM + %RAWDATA%\web_user_cart*.*

REM Observation data
del %WEBINST%\observation_data.cab 
%CABEXE% n %WEBINST%\observation_data.cab %RAWDATA%\crop_*.* + %RAWDATA%\genetic_*.*

REM Order data
del %WEBINST%\order_data.cab 
%CABEXE% n %WEBINST%\order_data.cab %RAWDATA%\order*.*
REM + %RAWDATA%\web_order*.*

echo %date% %time% Done
pause