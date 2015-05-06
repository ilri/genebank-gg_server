USE [gringlobal]
GO

/****** Object:  Table [dbo].[feedback_result_field]    Script Date: 12/14/2010 23:54:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[feedback_result_field](
	[feedback_result_field_id] [int] IDENTITY(1,1) NOT NULL,
	[feedback_result_id] [int] NOT NULL,
	[feedback_form_field_id] [int] NOT NULL,
	[string_value] [nvarchar](500) NULL,
	[created_date] [datetime2](7) NOT NULL,
	[created_by] [int] NOT NULL,
	[modified_date] [datetime2](7) NULL,
	[modified_by] [int] NULL,
	[owned_date] [datetime2](7) NOT NULL,
	[owned_by] [int] NOT NULL,
 CONSTRAINT [PK_feedback_result_field] PRIMARY KEY CLUSTERED 
(
	[feedback_result_field_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


USE [gringlobal]
/****** Object:  Index [ndx_uniq_fresf]    Script Date: 12/14/2010 23:54:25 ******/
CREATE UNIQUE NONCLUSTERED INDEX [ndx_uniq_fresf] ON [dbo].[feedback_result_field] 
(
	[feedback_result_id] ASC,
	[feedback_form_field_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[feedback_result_field]  WITH CHECK ADD  CONSTRAINT [fk_fresf_created] FOREIGN KEY([created_by])
REFERENCES [dbo].[cooperator] ([cooperator_id])
GO

ALTER TABLE [dbo].[feedback_result_field] CHECK CONSTRAINT [fk_fresf_created]
GO

ALTER TABLE [dbo].[feedback_result_field]  WITH CHECK ADD  CONSTRAINT [fk_fresf_fres] FOREIGN KEY([feedback_result_id])
REFERENCES [dbo].[feedback_result] ([feedback_result_id])
GO

ALTER TABLE [dbo].[feedback_result_field] CHECK CONSTRAINT [fk_fresf_fres]
GO

ALTER TABLE [dbo].[feedback_result_field]  WITH CHECK ADD  CONSTRAINT [fk_fresf_modified] FOREIGN KEY([modified_by])
REFERENCES [dbo].[cooperator] ([cooperator_id])
GO

ALTER TABLE [dbo].[feedback_result_field] CHECK CONSTRAINT [fk_fresf_modified]
GO

ALTER TABLE [dbo].[feedback_result_field]  WITH CHECK ADD  CONSTRAINT [fk_fresf_owned] FOREIGN KEY([owned_by])
REFERENCES [dbo].[cooperator] ([cooperator_id])
GO

ALTER TABLE [dbo].[feedback_result_field] CHECK CONSTRAINT [fk_fresf_owned]
GO

