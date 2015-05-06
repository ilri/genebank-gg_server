USE [gringlobal]
GO

/****** Object:  Table [dbo].[feedback_report]    Script Date: 12/14/2010 23:53:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[feedback_report](
	[feedback_report_id] [int] IDENTITY(1,1) NOT NULL,
	[feedback_id] [int] NOT NULL,
	[feedback_form_id] [int] NOT NULL,
	[title] [nvarchar](100) NOT NULL,
	[is_observation_data] [nchar](1) NOT NULL,
	[is_web_visible] [nchar](1) NOT NULL,
	[due_interval] [int] NOT NULL,
	[interval_length_code] [nvarchar](20) NOT NULL,
	[interval_type_code] [nvarchar](20) NOT NULL,
	[created_date] [datetime2](7) NOT NULL,
	[created_by] [int] NOT NULL,
	[modified_date] [datetime2](7) NULL,
	[modified_by] [int] NULL,
	[owned_date] [datetime2](7) NOT NULL,
	[owned_by] [int] NOT NULL,
 CONSTRAINT [PK_feedback_report] PRIMARY KEY CLUSTERED 
(
	[feedback_report_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


USE [gringlobal]
/****** Object:  Index [ndx_uniq_fr]    Script Date: 12/14/2010 23:53:50 ******/
CREATE UNIQUE NONCLUSTERED INDEX [ndx_uniq_fr] ON [dbo].[feedback_report] 
(
	[title] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[feedback_report]  WITH CHECK ADD  CONSTRAINT [FK_feedback_report_feedback_form] FOREIGN KEY([feedback_form_id])
REFERENCES [dbo].[feedback_form] ([feedback_form_id])
GO

ALTER TABLE [dbo].[feedback_report] CHECK CONSTRAINT [FK_feedback_report_feedback_form]
GO

ALTER TABLE [dbo].[feedback_report]  WITH CHECK ADD  CONSTRAINT [fk_fr_created] FOREIGN KEY([created_by])
REFERENCES [dbo].[cooperator] ([cooperator_id])
GO

ALTER TABLE [dbo].[feedback_report] CHECK CONSTRAINT [fk_fr_created]
GO

ALTER TABLE [dbo].[feedback_report]  WITH CHECK ADD  CONSTRAINT [fk_fr_f] FOREIGN KEY([feedback_id])
REFERENCES [dbo].[feedback] ([feedback_id])
GO

ALTER TABLE [dbo].[feedback_report] CHECK CONSTRAINT [fk_fr_f]
GO

ALTER TABLE [dbo].[feedback_report]  WITH CHECK ADD  CONSTRAINT [fk_fr_modified] FOREIGN KEY([modified_by])
REFERENCES [dbo].[cooperator] ([cooperator_id])
GO

ALTER TABLE [dbo].[feedback_report] CHECK CONSTRAINT [fk_fr_modified]
GO

ALTER TABLE [dbo].[feedback_report]  WITH CHECK ADD  CONSTRAINT [fk_fr_owned] FOREIGN KEY([owned_by])
REFERENCES [dbo].[cooperator] ([cooperator_id])
GO

ALTER TABLE [dbo].[feedback_report] CHECK CONSTRAINT [fk_fr_owned]
GO

