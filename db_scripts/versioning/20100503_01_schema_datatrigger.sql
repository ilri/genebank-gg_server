USE [gringlobal]
GO

/****** Object:  Table [dbo].[sec_datatrigger]    Script Date: 05/03/2010 09:49:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[sec_datatrigger](
	[sec_datatrigger_id] [int] IDENTITY(1,1) NOT NULL,
	[sec_dataview_id] [int] NULL,
	[sec_table_id] [int] NULL,
	[virtual_file_path] [nvarchar](255) NULL,
	[assembly_name] [nvarchar](255) NOT NULL,
	[fully_qualified_class_name] [nvarchar](255) NOT NULL,
	[is_enabled] [nvarchar](1) NOT NULL,
	[is_system] [nvarchar](1) NOT NULL,
	[sort_order] [int] NOT NULL,
	[created_date] [datetime2](7) NOT NULL,
	[created_by] [int] NOT NULL,
	[modified_date] [datetime2](7) NULL,
	[modified_by] [int] NULL,
	[owned_date] [datetime2](7) NOT NULL,
	[owned_by] [int] NOT NULL,
 CONSTRAINT [PK_sec_datatrigger] PRIMARY KEY CLUSTERED 
(
	[sec_datatrigger_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[sec_datatrigger]  WITH CHECK ADD  CONSTRAINT [fk_sdt_created] FOREIGN KEY([created_by])
REFERENCES [dbo].[cooperator] ([cooperator_id])
GO

ALTER TABLE [dbo].[sec_datatrigger] CHECK CONSTRAINT [fk_sdt_created]
GO

ALTER TABLE [dbo].[sec_datatrigger]  WITH CHECK ADD  CONSTRAINT [fk_sdt_dv] FOREIGN KEY([sec_dataview_id])
REFERENCES [dbo].[sec_dataview] ([sec_dataview_id])
GO

ALTER TABLE [dbo].[sec_datatrigger] CHECK CONSTRAINT [fk_sdt_dv]
GO

ALTER TABLE [dbo].[sec_datatrigger]  WITH CHECK ADD  CONSTRAINT [fk_sdt_modified] FOREIGN KEY([modified_by])
REFERENCES [dbo].[cooperator] ([cooperator_id])
GO

ALTER TABLE [dbo].[sec_datatrigger] CHECK CONSTRAINT [fk_sdt_modified]
GO

ALTER TABLE [dbo].[sec_datatrigger]  WITH CHECK ADD  CONSTRAINT [fk_sdt_owned] FOREIGN KEY([owned_by])
REFERENCES [dbo].[cooperator] ([cooperator_id])
GO

ALTER TABLE [dbo].[sec_datatrigger] CHECK CONSTRAINT [fk_sdt_owned]
GO

ALTER TABLE [dbo].[sec_datatrigger]  WITH CHECK ADD  CONSTRAINT [fk_sdt_st] FOREIGN KEY([sec_table_id])
REFERENCES [dbo].[sec_table] ([sec_table_id])
GO

ALTER TABLE [dbo].[sec_datatrigger] CHECK CONSTRAINT [fk_sdt_st]
GO









USE [gringlobal]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_filt_created]') AND parent_object_id = OBJECT_ID(N'[dbo].[sec_filter]'))
ALTER TABLE [dbo].[sec_filter] DROP CONSTRAINT [fk_filt_created]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_filt_dv]') AND parent_object_id = OBJECT_ID(N'[dbo].[sec_filter]'))
ALTER TABLE [dbo].[sec_filter] DROP CONSTRAINT [fk_filt_dv]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_filt_modified]') AND parent_object_id = OBJECT_ID(N'[dbo].[sec_filter]'))
ALTER TABLE [dbo].[sec_filter] DROP CONSTRAINT [fk_filt_modified]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_filt_owned]') AND parent_object_id = OBJECT_ID(N'[dbo].[sec_filter]'))
ALTER TABLE [dbo].[sec_filter] DROP CONSTRAINT [fk_filt_owned]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_filt_st]') AND parent_object_id = OBJECT_ID(N'[dbo].[sec_filter]'))
ALTER TABLE [dbo].[sec_filter] DROP CONSTRAINT [fk_filt_st]
GO

USE [gringlobal]
GO

/****** Object:  Table [dbo].[sec_filter]    Script Date: 05/03/2010 09:55:14 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sec_filter]') AND type in (N'U'))
DROP TABLE [dbo].[sec_filter]
GO


