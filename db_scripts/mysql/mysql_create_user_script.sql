drop user 'gg_user'@'*';
create user 'gg_user'@'*' identified by 'gg_user_passw0rd!';
grant select, insert, update, delete on gringlobal.* to 'gg_user'@'*';

drop user 'gg_search'@'*';
create user 'gg_search'@'*' identified by 'gg_search_passw0rd!';
grant select on gringlobal.* to 'gg_search'@'*';

flush privileges;