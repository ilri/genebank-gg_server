use gringlobal;

drop user gg_user;
drop login gg_user;

drop user gg_search;
drop login gg_search;

create login gg_user with password='gg_user_PA55w0rd!'
create user gg_user for login gg_user;
exec sp_addrolemember 'db_datareader', 'gg_user';
exec sp_addrolemember 'db_datawriter', 'gg_user';
exec sp_MSforeachtable 'grant select, insert, update, delete on gringlobal.? to gg_user;';

create login gg_search with password='gg_search_PA55w0rd!'
create user gg_search for login gg_search;
exec sp_addrolemember 'db_datareader', 'gg_search';
exec sp_MSforeachtable 'grant select on gringlobal.? to gg_search;';


-- MACHINE\ASPNET on XP
declare @machinename varchar(255)
select @machinename = convert(sysname, SERVERPROPERTY('ComputerNamePhysicalNetBIOS'));
print @machinename

drop user ASPNET
exec('drop login [' + @machinename + '\ASPNET]');
exec('create login [' + @machinename + '\ASPNET] from windows');
exec('create user ASPNET for login [' + @machinename + '\ASPNET]');
exec sp_addrolemember 'db_datareader', 'ASPNET';
exec sp_addrolemember 'db_datawriter', 'ASPNET';
exec sp_MSforeachtable 'grant select, insert, update, delete on gringlobal.? to ASPNET;';

drop user NETWORKSERVICE;
drop login [NT AUTHORITY\NETWORK SERVICE];

-- NETWORK SERVICE on Vista
create login [NT AUTHORITY\NETWORK SERVICE] from windows;
create user NETWORKSERVICE for login [NT AUTHORITY\NETWORK SERVICE];
exec sp_addrolemember 'db_datareader', 'NETWORKSERVICE';
exec sp_addrolemember 'db_datareader', 'NETWORKSERVICE';
exec sp_MSforeachtable 'grant select, insert, update, delete on gringlobal.? to NETWORKSERVICE;';

drop user DEFAULTAPPPOOL;
drop login [IIS AppPool\DefaultAppPool];

-- DefaultAppPool on Windows7
create login [IIS AppPool\DefaultAppPool] from windows;
create user DEFAULTAPPPOOL for login [IIS AppPool\DefaultAppPool];
exec sp_addrolemember 'db_datareader', 'DEFAULTAPPPOOL';
exec sp_addrolemember 'db_datareader', 'DEFAULTAPPPOOL';
exec sp_MSforeachtable 'grant select, insert, update, delete on gringlobal.? to DEFAULTAPPPOOL;';

