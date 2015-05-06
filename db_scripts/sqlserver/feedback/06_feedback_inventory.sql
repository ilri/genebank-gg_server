USE [gringlobal]
GO

/****** Object:  Table [dbo].[feedback_inventory]    Script Date: 12/14/2010 23:53:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[feedback_inventory](
	[feedback_inventory_id] [int] IDENTITY(1,1) NOT NULL,
	[feedback_id] [int] NOT NULL,
	[inventory_id] [int] NOT NULL,
	[created_date] [datetime2](7) NOT NULL,
	[created_by] [int] NOT NULL,
	[modified_date] [datetime2](7) NULL,
	[modified_by] [int] NULL,
	[owned_date] [datetime2](7) NULL,
	[owned_by] [int] NOT NULL,
 CONSTRAINT [PK_feedback_inventory] PRIMARY KEY CLUSTERED 
(
	[feedback_inventory_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


USE [gringlobal]
/****** Object:  Index [ndx_uniq_fi]    Script Date: 12/14/2010 23:53:37 ******/
CREATE UNIQUE NONCLUSTERED INDEX [ndx_uniq_fi] ON [dbo].[feedback_inventory] 
(
	[feedback_id] ASC,
	[inventory_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[feedback_inventory]  WITH CHECK ADD  CONSTRAINT [FK_feedback_inventory_inventory] FOREIGN KEY([inventory_id])
REFERENCES [dbo].[inventory] ([inventory_id])
GO

ALTER TABLE [dbo].[feedback_inventory] CHECK CONSTRAINT [FK_feedback_inventory_inventory]
GO

ALTER TABLE [dbo].[feedback_inventory]  WITH CHECK ADD  CONSTRAINT [fk_fi_created] FOREIGN KEY([created_by])
REFERENCES [dbo].[cooperator] ([cooperator_id])
GO

ALTER TABLE [dbo].[feedback_inventory] CHECK CONSTRAINT [fk_fi_created]
GO

ALTER TABLE [dbo].[feedback_inventory]  WITH CHECK ADD  CONSTRAINT [fk_fi_modified] FOREIGN KEY([modified_by])
REFERENCES [dbo].[cooperator] ([cooperator_id])
GO

ALTER TABLE [dbo].[feedback_inventory] CHECK CONSTRAINT [fk_fi_modified]
GO

ALTER TABLE [dbo].[feedback_inventory]  WITH CHECK ADD  CONSTRAINT [fk_fi_owned] FOREIGN KEY([owned_by])
REFERENCES [dbo].[cooperator] ([cooperator_id])
GO

ALTER TABLE [dbo].[feedback_inventory] CHECK CONSTRAINT [fk_fi_owned]
GO

ALTER TABLE [dbo].[feedback_inventory]  WITH CHECK ADD  CONSTRAINT [fk_fpi_fp] FOREIGN KEY([feedback_id])
REFERENCES [dbo].[feedback] ([feedback_id])
GO

ALTER TABLE [dbo].[feedback_inventory] CHECK CONSTRAINT [fk_fpi_fp]
GO

