@ECHO OFF
set LOG=import_log.txt
echo . > %LOG%


echo Dropping MAIN and PROD users
sqlplus.exe SYS as SYSDBA @import_0_drop_users.sql < SYSlogin.txt >> %LOG%


echo Creating MAIN and PROD users
sqlplus.exe SYS as SYSDBA @import_1_create_users.sql < SYSlogin.txt >> %LOG%


echo Importing MAIN data...
imp 'SYS as SYSDBA' FULL=Y LOG=import_log_main.txt FILE=main1.dmp < SYSlogin.txt >> %LOG%

echo Importing PROD data...
imp 'SYS as SYSDBA' FULL=Y LOG=import_log_prod.txt FILE=prod1.dmp, prod2.dmp < SYSlogin.txt >> %LOG%

start %LOG%