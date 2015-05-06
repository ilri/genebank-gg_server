USE [gringlobal]
GO

/****** Object:  Table [dbo].[feedback_form_field]    Script Date: 12/14/2010 23:52:59 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[feedback_form_field](
	[feedback_form_field_id] [int] IDENTITY(1,1) NOT NULL,
	[feedback_form_id] [int] NOT NULL,
	[title] [nvarchar](100) NULL,
	[description] [nvarchar](max) NULL,
	[field_type_code] [nvarchar](20) NOT NULL,
	[default_value] [nvarchar](100) NULL,
	[created_date] [datetime2](7) NOT NULL,
	[created_by] [int] NOT NULL,
	[modified_date] [datetime2](7) NULL,
	[modified_by] [int] NULL,
	[owned_date] [datetime2](7) NOT NULL,
	[owned_by] [int] NOT NULL,
 CONSTRAINT [PK_feedback_form_field] PRIMARY KEY CLUSTERED 
(
	[feedback_form_field_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


USE [gringlobal]
/****** Object:  Index [ndx_uniq_fff]    Script Date: 12/14/2010 23:52:59 ******/
CREATE UNIQUE NONCLUSTERED INDEX [ndx_uniq_fff] ON [dbo].[feedback_form_field] 
(
	[feedback_form_id] ASC,
	[title] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[feedback_form_field]  WITH CHECK ADD  CONSTRAINT [FK_feedback_form_field_feedback_form] FOREIGN KEY([feedback_form_id])
REFERENCES [dbo].[feedback_form] ([feedback_form_id])
GO

ALTER TABLE [dbo].[feedback_form_field] CHECK CONSTRAINT [FK_feedback_form_field_feedback_form]
GO

ALTER TABLE [dbo].[feedback_form_field]  WITH CHECK ADD  CONSTRAINT [fk_fff_owned] FOREIGN KEY([owned_by])
REFERENCES [dbo].[cooperator] ([cooperator_id])
GO

ALTER TABLE [dbo].[feedback_form_field] CHECK CONSTRAINT [fk_fff_owned]
GO

