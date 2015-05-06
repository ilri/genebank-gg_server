USE [gringlobal]
GO


/* get rid of the code_group table and all constraints pointing to it, they're no longer needed */

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_srf_cgc]') AND parent_object_id = OBJECT_ID(N'[dbo].[sec_dataview_field]'))
ALTER TABLE [dbo].[sec_dataview_field] DROP CONSTRAINT [fk_srf_cgc]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_sdf_cgc]') AND parent_object_id = OBJECT_ID(N'[dbo].[sec_dataview_field]'))
ALTER TABLE [dbo].[sec_dataview_field] DROP CONSTRAINT [fk_sdf_cgc]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_stf_cdgrp]') AND parent_object_id = OBJECT_ID(N'[dbo].[sec_table_field]'))
ALTER TABLE [dbo].[sec_table_field] DROP CONSTRAINT [fk_stf_cdgrp]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_cdval_cdgrp]') AND parent_object_id = OBJECT_ID(N'[dbo].[code_value]'))
ALTER TABLE [dbo].[code_value] DROP CONSTRAINT [fk_cdval_cdgrp]
GO


IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_cdgrp_created]') AND parent_object_id = OBJECT_ID(N'[dbo].[code_group]'))
ALTER TABLE [dbo].[code_group] DROP CONSTRAINT [fk_cdgrp_created]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_cdgrp_modified]') AND parent_object_id = OBJECT_ID(N'[dbo].[code_group]'))
ALTER TABLE [dbo].[code_group] DROP CONSTRAINT [fk_cdgrp_modified]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_cdgrp_owned]') AND parent_object_id = OBJECT_ID(N'[dbo].[code_group]'))
ALTER TABLE [dbo].[code_group] DROP CONSTRAINT [fk_cdgrp_owned]
GO

USE [gringlobal]
GO

/****** Object:  Table [dbo].[code_group]    Script Date: 03/31/2010 14:48:19 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[code_group]') AND type in (N'U'))
DROP TABLE [dbo].[code_group]
GO


/*******************************
  Rename, add, and resize fields
  ******************************/

/* rename sec_table_field.code_group_code to group_name */
exec sp_rename 'sec_table_field.code_group_code', 'group_name', 'COLUMN'
/* grow group_name out to 100 */
alter table sec_table_field alter column group_name nvarchar(100) null;


/* rename code_group_code to group_name */
exec sp_rename 'sec_dataview_field.code_group_code', 'group_name', 'COLUMN'
/* grow group_name out to 100 */
alter table sec_dataview_field alter column group_name nvarchar(100) null;
/* add the gui_hint column */
alter table sec_dataview_field add gui_hint nvarchar(100) null;

/* rename code_value.code_group_code to group_name */
exec sp_rename 'code_value.code_group_code', 'group_name', 'COLUMN'
/* grow group_name out to 100 */
alter table code_value alter column group_name nvarchar(100) null;

