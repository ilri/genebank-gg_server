/* drop the gringlobal users (and implicity their permissions as well) */

-- XP
use gringlobal;
drop user __WWWUSER__;
drop login [__MACHINENAME__\__WWWUSER__];

-- Vista
drop user NETWORKSERVICE;
drop login [NT AUTHORITY\NETWORK SERVICE];

-- Windows 7
drop user DEFAULTAPPPOOL;
drop login [IIS AppPool\DefaultAppPool];
