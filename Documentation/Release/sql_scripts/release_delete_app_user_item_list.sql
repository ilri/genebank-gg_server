use gringlobal;

begin try

begin tran;
delete from app_user_item_list;

print 'all app_user_item_list records have been deleted.';
commit tran;

end try
begin catch

print 'Error: ' + coalesce(convert(varchar, ERROR_NUMBER()), '') + 
	' sev ' + coalesce(convert(varchar, ERROR_SEVERITY()), '') +
	' state ' + coalesce(convert(varchar, ERROR_STATE()), '') +
	' proc ' + coalesce(convert(varchar, ERROR_PROCEDURE()), '') +
	' line ' + coalesce(convert(varchar, ERROR_LINE()), '') + 
	' msg ' + coalesce(ERROR_MESSAGE(), '');
    
rollback tran;
print 'transaction rolled back';

end catch