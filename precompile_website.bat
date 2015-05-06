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
REM Rebuild the solution
REM ***************************************************************************************************************************

call "c:\Program Files\Microsoft Visual Studio 9.0\VC\vcvarsall.bat" x86 

iisreset

REM ***************************************************************************************************************************
REM Publish GrinGlobal.Web to local webserver at /gringlobal vdir
REM ***************************************************************************************************************************
rmdir /s /q "%LOCALIISDIR%\uploads"
aspnet_compiler.exe -nologo -v /gringlobal -p "%GG%\GrinGlobal.Web" -f -u -fixednames "%LOCALIISDIR%"

pause