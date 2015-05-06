USE [gringlobal]
GO

/****** Object:  Table [dbo].[feedback]    Script Date: 12/14/2010 23:50:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[feedback](
	[feedback_id] [int] IDENTITY(1,1) NOT NULL,
	[title] [nvarchar](100) NOT NULL,
	[start_date] [datetime2](7) NOT NULL,
	[end_date] [datetime2](7) NOT NULL,
	[is_restricted_by_inventory] [nchar](1) NOT NULL,
	[is_notified_30days_prior] [nchar](1) NOT NULL,
	[is_notified_15days_prior] [nchar](1) NOT NULL,
	[email_text] [nvarchar](max) NULL,
	[created_date] [datetime2](7) NOT NULL,
	[created_by] [int] NOT NULL,
	[modified_date] [datetime2](7) NULL,
	[modified_by] [int] NULL,
	[owned_date] [datetime2](7) NOT NULL,
	[owned_by] [int] NOT NULL,
 CONSTRAINT [PK_feedback] PRIMARY KEY CLUSTERED 
(
	[feedback_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


USE [gringlobal]
/****** Object:  Index [ndx_uniq_f]    Script Date: 12/14/2010 23:50:54 ******/
CREATE UNIQUE NONCLUSTERED INDEX [ndx_uniq_f] ON [dbo].[feedback] 
(
	[title] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[feedback]  WITH CHECK ADD  CONSTRAINT [fk_f_created] FOREIGN KEY([created_by])
REFERENCES [dbo].[cooperator] ([cooperator_id])
GO

ALTER TABLE [dbo].[feedback] CHECK CONSTRAINT [fk_f_created]
GO

ALTER TABLE [dbo].[feedback]  WITH CHECK ADD  CONSTRAINT [fk_f_modified] FOREIGN KEY([modified_by])
REFERENCES [dbo].[cooperator] ([cooperator_id])
GO

ALTER TABLE [dbo].[feedback] CHECK CONSTRAINT [fk_f_modified]
GO

ALTER TABLE [dbo].[feedback]  WITH CHECK ADD  CONSTRAINT [fk_f_owned] FOREIGN KEY([owned_by])
REFERENCES [dbo].[cooperator] ([cooperator_id])
GO

ALTER TABLE [dbo].[feedback] CHECK CONSTRAINT [fk_f_owned]
GO


