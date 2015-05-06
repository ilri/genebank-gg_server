@ECHO OFF
set LOG=output.txt
echo . > %LOG%

echo dropping/creating GRINGLOBAL schema (aka user)...
sqlplus.exe SYS as SYSDBA @0_drop_gringlobal_user.sql < SYSlogin.txt >> %LOG%

start %LOG%