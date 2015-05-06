USE [gringlobal]
GO

/****** Object:  Table [dbo].[feedback_form_trait]    Script Date: 12/14/2010 23:53:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[feedback_form_trait](
	[feedback_form_trait_id] [int] IDENTITY(1,1) NOT NULL,
	[feedback_form_id] [int] NOT NULL,
	[crop_trait_id] [int] NOT NULL,
	[created_date] [datetime2](7) NOT NULL,
	[created_by] [int] NOT NULL,
	[modified_date] [datetime2](7) NULL,
	[modified_by] [int] NULL,
	[owned_date] [datetime2](7) NOT NULL,
	[owned_by] [int] NOT NULL,
 CONSTRAINT [PK_feedback_form_trait] PRIMARY KEY CLUSTERED 
(
	[feedback_form_trait_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


USE [gringlobal]
/****** Object:  Index [ndx_uniq_fft]    Script Date: 12/14/2010 23:53:14 ******/
CREATE UNIQUE NONCLUSTERED INDEX [ndx_uniq_fft] ON [dbo].[feedback_form_trait] 
(
	[feedback_form_id] ASC,
	[crop_trait_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[feedback_form_trait]  WITH CHECK ADD  CONSTRAINT [FK_feedback_form_trait_feedback_form] FOREIGN KEY([feedback_form_id])
REFERENCES [dbo].[feedback_form] ([feedback_form_id])
GO

ALTER TABLE [dbo].[feedback_form_trait] CHECK CONSTRAINT [FK_feedback_form_trait_feedback_form]
GO

ALTER TABLE [dbo].[feedback_form_trait]  WITH CHECK ADD  CONSTRAINT [fk_fft_created] FOREIGN KEY([created_by])
REFERENCES [dbo].[cooperator] ([cooperator_id])
GO

ALTER TABLE [dbo].[feedback_form_trait] CHECK CONSTRAINT [fk_fft_created]
GO

ALTER TABLE [dbo].[feedback_form_trait]  WITH CHECK ADD  CONSTRAINT [fk_fft_modified] FOREIGN KEY([modified_by])
REFERENCES [dbo].[cooperator] ([cooperator_id])
GO

ALTER TABLE [dbo].[feedback_form_trait] CHECK CONSTRAINT [fk_fft_modified]
GO

ALTER TABLE [dbo].[feedback_form_trait]  WITH CHECK ADD  CONSTRAINT [fk_fft_owned] FOREIGN KEY([owned_by])
REFERENCES [dbo].[cooperator] ([cooperator_id])
GO

ALTER TABLE [dbo].[feedback_form_trait] CHECK CONSTRAINT [fk_fft_owned]
GO

