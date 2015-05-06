@ECHO OFF
set GG=C:\projects\GrinGlobal

cscript.exe "%GG%\NewSetupVersion.vbs" "%GG%\GrinGlobal.Admin.Setup\GrinGlobal.Admin.Setup.vdproj"
cscript.exe "%GG%\NewSetupVersion.vbs" "%GG%\GrinGlobal.Web.Setup\GrinGlobal.Web.Setup.vdproj"
cscript.exe "%GG%\NewSetupVersion.vbs" "%GG%\GrinGlobal.Search.Engine.Setup\GrinGlobal.Search.Engine.Setup.vdproj"
cscript.exe "%GG%\NewSetupVersion.vbs" "%GG%\GrinGlobal.Updater.Setup\GrinGlobal.Updater.Setup.vdproj"
cscript.exe "%GG%\NewSetupVersion.vbs" "%GG%\GrinGlobal.Database.Setup\GrinGlobal.Database.Setup.vdproj"



echo %date% %time% Done
pause