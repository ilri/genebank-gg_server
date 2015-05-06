USE [gringlobal]
GO

/****** Object:  Table [dbo].[feedback_form]    Script Date: 12/14/2010 23:52:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[feedback_form](
	[feedback_form_id] [int] IDENTITY(1,1) NOT NULL,
	[title] [nvarchar](100) NOT NULL,
	[created_date] [datetime2](7) NOT NULL,
	[created_by] [int] NOT NULL,
	[modified_date] [datetime2](7) NULL,
	[modified_by] [int] NULL,
	[owned_date] [datetime2](7) NOT NULL,
	[owned_by] [int] NOT NULL,
 CONSTRAINT [PK_feedback_form] PRIMARY KEY CLUSTERED 
(
	[feedback_form_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


USE [gringlobal]
/****** Object:  Index [ndx_uniq_ff]    Script Date: 12/14/2010 23:52:03 ******/
CREATE UNIQUE NONCLUSTERED INDEX [ndx_uniq_ff] ON [dbo].[feedback_form] 
(
	[title] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[feedback_form]  WITH CHECK ADD  CONSTRAINT [fk_ff_created] FOREIGN KEY([created_by])
REFERENCES [dbo].[cooperator] ([cooperator_id])
GO

ALTER TABLE [dbo].[feedback_form] CHECK CONSTRAINT [fk_ff_created]
GO

ALTER TABLE [dbo].[feedback_form]  WITH CHECK ADD  CONSTRAINT [fk_ff_modified] FOREIGN KEY([modified_by])
REFERENCES [dbo].[cooperator] ([cooperator_id])
GO

ALTER TABLE [dbo].[feedback_form] CHECK CONSTRAINT [fk_ff_modified]
GO

ALTER TABLE [dbo].[feedback_form]  WITH CHECK ADD  CONSTRAINT [fk_ff_owned] FOREIGN KEY([owned_by])
REFERENCES [dbo].[cooperator] ([cooperator_id])
GO

ALTER TABLE [dbo].[feedback_form] CHECK CONSTRAINT [fk_ff_owned]
GO

