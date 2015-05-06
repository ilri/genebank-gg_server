@ECHO OFF
set LOG=migration_log.txt
echo . > %LOG%

echo Dropping GRINGLOBAL schema (aka user)...
sqlplus.exe SYS as SYSDBA @migrate_0_drop_gringlobal_user.sql < SYSlogin.txt >> %LOG%

echo Creating GRINGLOBAL schema (aka user)...
sqlplus.exe SYS as SYSDBA @migrate_1_create_gringlobal_user.sql < SYSlogin.txt >> %LOG%

echo Creating GRINGLOBAL tables...
sqlplus.exe SYS as SYSDBA @migrate_2_create_tables.sql < SYSlogin.txt >> %LOG%

REM schema is now created.  copy data from PROD/MAIN into GRINGLOBAL, doing data conversion as we go.

echo Migrating data from PROD and MAIN into GRINGLOBAL
sqlplus.exe SYS as SYSDBA @migrate_3_import_prod_main_data.sql < SYSlogin.txt >> %LOG%

REM echo Creating data in GRINGLOBAL for sec_* tables...
REM sqlplus.exe SYS as SYSDBA @migrate_4_import_sec_data.sql < SYSlogin.txt >> %LOG%

REM all data is there.  now create other objects (indexes, constraints, etc)

REM echo creating indexes...
REM sqlplus.exe SYS as SYSDBA @migrate_5_create_indexes.sql < SYSlogin.txt >> %LOG%

REM echo creating constraints...
REM sqlplus.exe SYS as SYSDBA @migrate_6_create_constraints.sql < SYSlogin.txt >> %LOG%

start %LOG%