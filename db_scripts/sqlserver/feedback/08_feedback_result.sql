USE [gringlobal]
GO

/****** Object:  Table [dbo].[feedback_result]    Script Date: 12/14/2010 23:54:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[feedback_result](
	[feedback_result_id] [int] IDENTITY(1,1) NOT NULL,
	[feedback_report_id] [int] NOT NULL,
	[participant_cooperator_id] [int] NOT NULL,
	[order_request_id] [int] NOT NULL,
	[inventory_id] [int] NULL,
	[created_date] [datetime2](7) NOT NULL,
	[created_by] [int] NOT NULL,
	[modified_date] [datetime2](7) NULL,
	[modified_by] [int] NULL,
	[owned_date] [datetime2](7) NOT NULL,
	[owned_by] [int] NOT NULL,
 CONSTRAINT [PK_feedback_result] PRIMARY KEY CLUSTERED 
(
	[feedback_result_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


USE [gringlobal]
/****** Object:  Index [ndx_uniq_fres]    Script Date: 12/14/2010 23:54:03 ******/
CREATE UNIQUE NONCLUSTERED INDEX [ndx_uniq_fres] ON [dbo].[feedback_result] 
(
	[feedback_report_id] ASC,
	[participant_cooperator_id] ASC,
	[order_request_id] ASC,
	[inventory_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[feedback_result]  WITH CHECK ADD  CONSTRAINT [fk_fres_created] FOREIGN KEY([created_by])
REFERENCES [dbo].[cooperator] ([cooperator_id])
GO

ALTER TABLE [dbo].[feedback_result] CHECK CONSTRAINT [fk_fres_created]
GO

ALTER TABLE [dbo].[feedback_result]  WITH CHECK ADD  CONSTRAINT [fk_fres_fr] FOREIGN KEY([feedback_report_id])
REFERENCES [dbo].[feedback_report] ([feedback_report_id])
GO

ALTER TABLE [dbo].[feedback_result] CHECK CONSTRAINT [fk_fres_fr]
GO

ALTER TABLE [dbo].[feedback_result]  WITH CHECK ADD  CONSTRAINT [fk_fres_modified] FOREIGN KEY([modified_by])
REFERENCES [dbo].[cooperator] ([cooperator_id])
GO

ALTER TABLE [dbo].[feedback_result] CHECK CONSTRAINT [fk_fres_modified]
GO

ALTER TABLE [dbo].[feedback_result]  WITH CHECK ADD  CONSTRAINT [fk_fres_owned] FOREIGN KEY([owned_by])
REFERENCES [dbo].[cooperator] ([cooperator_id])
GO

ALTER TABLE [dbo].[feedback_result] CHECK CONSTRAINT [fk_fres_owned]
GO

