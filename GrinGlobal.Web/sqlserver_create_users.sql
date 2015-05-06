use gringlobal;


-- MACHINE\ASPNET on XP
create login [__MACHINENAME__\__WWWUSER__] from windows;
create user __WWWUSER__ for login [__MACHINENAME__\__WWWUSER__];
exec sp_addrolemember 'db_datareader', '__WWWUSER__';
exec sp_addrolemember 'db_datawriter', '__WWWUSER__';
exec sp_MSforeachtable 'grant select, insert, update, delete on gringlobal.? to __WWWUSER__;';

-- NETWORK SERVICE on Vista
create login [NT AUTHORITY\NETWORK SERVICE] from windows;
create user NETWORKSERVICE for login [NT AUTHORITY\NETWORK SERVICE];
exec sp_addrolemember 'db_datareader', 'NETWORKSERVICE';
exec sp_addrolemember 'db_datareader', 'NETWORKSERVICE';
exec sp_MSforeachtable 'grant select, insert, update, delete on gringlobal.? to NETWORKSERVICE;';

-- DefaultAppPool on Windows7
create login [IIS AppPool\DefaultAppPool] from windows;
create user DEFAULTAPPPOOL for login [IIS AppPool\DefaultAppPool];
exec sp_addrolemember 'db_datareader', 'DEFAULTAPPPOOL';
exec sp_addrolemember 'db_datareader', 'DEFAULTAPPPOOL';
exec sp_MSforeachtable 'grant select, insert, update, delete on gringlobal.? to DEFAULTAPPPOOL;';

