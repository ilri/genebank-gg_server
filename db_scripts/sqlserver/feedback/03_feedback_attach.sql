USE [gringlobal]
GO

/****** Object:  Table [dbo].[feedback_attach]    Script Date: 12/14/2010 23:52:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[feedback_attach](
	[feedback_attach_id] [int] IDENTITY(1,1) NOT NULL,
	[feedback_id] [int] NOT NULL,
	[virtual_path] [nvarchar](255) NOT NULL,
	[thumbnail_virtual_path] [nvarchar](255) NULL,
	[sort_order] [int] NULL,
	[title] [nvarchar](500) NULL,
	[description] [nvarchar](500) NULL,
	[content_type] [nvarchar](100) NULL,
	[category_code] [nvarchar](20) NULL,
	[is_web_visible] [nvarchar](1) NOT NULL,
	[note] [nvarchar](max) NULL,
	[created_date] [datetime2](7) NOT NULL,
	[created_by] [int] NOT NULL,
	[modified_date] [datetime2](7) NULL,
	[modified_by] [int] NULL,
	[owned_date] [datetime2](7) NOT NULL,
	[owned_by] [int] NOT NULL,
 CONSTRAINT [PK_feedback_attach] PRIMARY KEY CLUSTERED 
(
	[feedback_attach_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


USE [gringlobal]
/****** Object:  Index [ndx_fk_created]    Script Date: 12/14/2010 23:52:34 ******/
CREATE NONCLUSTERED INDEX [ndx_fk_created] ON [dbo].[feedback_attach] 
(
	[created_by] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


USE [gringlobal]
/****** Object:  Index [ndx_fk_f]    Script Date: 12/14/2010 23:52:34 ******/
CREATE NONCLUSTERED INDEX [ndx_fk_f] ON [dbo].[feedback_attach] 
(
	[feedback_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


USE [gringlobal]
/****** Object:  Index [ndx_fk_modified]    Script Date: 12/14/2010 23:52:34 ******/
CREATE NONCLUSTERED INDEX [ndx_fk_modified] ON [dbo].[feedback_attach] 
(
	[modified_by] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


USE [gringlobal]
/****** Object:  Index [ndx_fk_owned]    Script Date: 12/14/2010 23:52:34 ******/
CREATE NONCLUSTERED INDEX [ndx_fk_owned] ON [dbo].[feedback_attach] 
(
	[owned_by] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


USE [gringlobal]
/****** Object:  Index [ndx_uniq_fa]    Script Date: 12/14/2010 23:52:34 ******/
CREATE UNIQUE NONCLUSTERED INDEX [ndx_uniq_fa] ON [dbo].[feedback_attach] 
(
	[feedback_id] ASC,
	[virtual_path] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[feedback_attach]  WITH CHECK ADD  CONSTRAINT [fk_fa_created] FOREIGN KEY([created_by])
REFERENCES [dbo].[cooperator] ([cooperator_id])
GO

ALTER TABLE [dbo].[feedback_attach] CHECK CONSTRAINT [fk_fa_created]
GO

ALTER TABLE [dbo].[feedback_attach]  WITH CHECK ADD  CONSTRAINT [fk_fa_f] FOREIGN KEY([feedback_id])
REFERENCES [dbo].[feedback] ([feedback_id])
GO

ALTER TABLE [dbo].[feedback_attach] CHECK CONSTRAINT [fk_fa_f]
GO

ALTER TABLE [dbo].[feedback_attach]  WITH CHECK ADD  CONSTRAINT [fk_fa_modified] FOREIGN KEY([modified_by])
REFERENCES [dbo].[cooperator] ([cooperator_id])
GO

ALTER TABLE [dbo].[feedback_attach] CHECK CONSTRAINT [fk_fa_modified]
GO

ALTER TABLE [dbo].[feedback_attach]  WITH CHECK ADD  CONSTRAINT [fk_fa_owned] FOREIGN KEY([owned_by])
REFERENCES [dbo].[cooperator] ([cooperator_id])
GO

ALTER TABLE [dbo].[feedback_attach] CHECK CONSTRAINT [fk_fa_owned]
GO

