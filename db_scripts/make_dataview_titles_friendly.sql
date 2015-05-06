
drop function udf_TitleCase
CREATE FUNCTION udf_TitleCase (@x varchar(7999))
RETURNS varchar(7999)
AS
  BEGIN

	DECLARE @y int
	SET @y = 1
	IF LEN(@x) < 2
	  RETURN @x
	  
	SELECT @x = UPPER(SUBSTRING(@x,1,1))+LOWER(SUBSTRING(@x,2,LEN(@x)-1))+' '
	
	WHILE @y < LEN(@x)
	  BEGIN
		SELECT @y=CHARINDEX(' ',@x,@y)
		SELECT @x=SUBSTRING(@x,1,@y)+UPPER(SUBSTRING(@x,@y+1,1))+SUBSTRING(@x,@y+2,LEN(@x)-@y+1)	
		SELECT @y=@y+1
	  END
	RETURN @x
END



begin tran
update sec_dataview_lang set description = title;

update sec_dataview_lang set title = rtrim(dbo.udf_TitleCase(replace(sd.dataview_name + ' ', '_', ' '))) + ' Dataview'
from sec_dataview sd
where
	sec_dataview_lang.sec_dataview_id = sd.sec_dataview_id

--select * from sec_dataview;
--select * from sec_dataview_lang;
commit tran