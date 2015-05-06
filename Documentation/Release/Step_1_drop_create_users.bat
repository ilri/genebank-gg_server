@ECHO OFF

echo Dropping / recreating users...
cd \projects\gringlobal\documentation\release
call .\sql_scripts\sqlrunner.bat "C:\projects\gringlobal\db_scripts\drop_create_users.sql"
echo.
echo.
echo.
echo **********************************************************************
echo Done!  Ignore any errors about user not existing or not having rights.
echo **********************************************************************
echo.
echo.
pause