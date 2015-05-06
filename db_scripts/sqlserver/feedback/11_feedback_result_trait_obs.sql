USE [gringlobal]
GO

/****** Object:  Table [dbo].[feedback_result_trait_obs]    Script Date: 12/14/2010 23:54:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[feedback_result_trait_obs](
	[feedback_result_trait_obs_id] [int] IDENTITY(1,1) NOT NULL,
	[feedback_result_id] [int] NOT NULL,
	[inventory_id] [int] NOT NULL,
	[crop_trait_id] [int] NOT NULL,
	[crop_trait_code_id] [int] NULL,
	[numeric_value] [decimal](18, 5) NULL,
	[string_value] [nvarchar](255) NULL,
	[method_id] [int] NOT NULL,
	[is_archived] [nvarchar](1) NOT NULL,
	[data_quality_code] [nvarchar](20) NULL,
	[original_value] [nvarchar](30) NULL,
	[frequency] [decimal](18, 5) NULL,
	[rank] [int] NULL,
	[mean_value] [decimal](18, 5) NULL,
	[maximum_value] [decimal](18, 5) NULL,
	[minimum_value] [decimal](18, 5) NULL,
	[standard_deviation] [decimal](18, 5) NULL,
	[sample_size] [int] NULL,
	[note] [nvarchar](max) NULL,
	[created_date] [datetime2](7) NOT NULL,
	[created_by] [int] NOT NULL,
	[modified_date] [datetime2](7) NULL,
	[modified_by] [int] NULL,
	[owned_date] [datetime2](7) NOT NULL,
	[owned_by] [int] NOT NULL,
 CONSTRAINT [PK_feedback_result_observation] PRIMARY KEY CLUSTERED 
(
	[feedback_result_trait_obs_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


USE [gringlobal]
/****** Object:  Index [ndx_fk_fro_created]    Script Date: 12/14/2010 23:54:38 ******/
CREATE NONCLUSTERED INDEX [ndx_fk_fro_created] ON [dbo].[feedback_result_trait_obs] 
(
	[created_by] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


USE [gringlobal]
/****** Object:  Index [ndx_fk_fro_ct]    Script Date: 12/14/2010 23:54:38 ******/
CREATE NONCLUSTERED INDEX [ndx_fk_fro_ct] ON [dbo].[feedback_result_trait_obs] 
(
	[crop_trait_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


USE [gringlobal]
/****** Object:  Index [ndx_fk_fro_ctc]    Script Date: 12/14/2010 23:54:38 ******/
CREATE NONCLUSTERED INDEX [ndx_fk_fro_ctc] ON [dbo].[feedback_result_trait_obs] 
(
	[crop_trait_code_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


USE [gringlobal]
/****** Object:  Index [ndx_fk_fro_i]    Script Date: 12/14/2010 23:54:38 ******/
CREATE NONCLUSTERED INDEX [ndx_fk_fro_i] ON [dbo].[feedback_result_trait_obs] 
(
	[inventory_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


USE [gringlobal]
/****** Object:  Index [ndx_fk_fro_m]    Script Date: 12/14/2010 23:54:38 ******/
CREATE NONCLUSTERED INDEX [ndx_fk_fro_m] ON [dbo].[feedback_result_trait_obs] 
(
	[method_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


USE [gringlobal]
/****** Object:  Index [ndx_fk_fro_modified]    Script Date: 12/14/2010 23:54:38 ******/
CREATE NONCLUSTERED INDEX [ndx_fk_fro_modified] ON [dbo].[feedback_result_trait_obs] 
(
	[modified_by] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


USE [gringlobal]
/****** Object:  Index [ndx_fk_fro_owned]    Script Date: 12/14/2010 23:54:38 ******/
CREATE NONCLUSTERED INDEX [ndx_fk_fro_owned] ON [dbo].[feedback_result_trait_obs] 
(
	[owned_by] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


USE [gringlobal]
/****** Object:  Index [ndx_uniq_fro]    Script Date: 12/14/2010 23:54:38 ******/
CREATE UNIQUE NONCLUSTERED INDEX [ndx_uniq_fro] ON [dbo].[feedback_result_trait_obs] 
(
	[feedback_result_id] ASC,
	[inventory_id] ASC,
	[crop_trait_id] ASC,
	[crop_trait_code_id] ASC,
	[numeric_value] ASC,
	[string_value] ASC,
	[method_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[feedback_result_trait_obs]  WITH CHECK ADD  CONSTRAINT [fk_fresto_created] FOREIGN KEY([created_by])
REFERENCES [dbo].[cooperator] ([cooperator_id])
GO

ALTER TABLE [dbo].[feedback_result_trait_obs] CHECK CONSTRAINT [fk_fresto_created]
GO

ALTER TABLE [dbo].[feedback_result_trait_obs]  WITH CHECK ADD  CONSTRAINT [fk_fresto_modified] FOREIGN KEY([modified_by])
REFERENCES [dbo].[cooperator] ([cooperator_id])
GO

ALTER TABLE [dbo].[feedback_result_trait_obs] CHECK CONSTRAINT [fk_fresto_modified]
GO

ALTER TABLE [dbo].[feedback_result_trait_obs]  WITH CHECK ADD  CONSTRAINT [fk_fresto_owned] FOREIGN KEY([owned_by])
REFERENCES [dbo].[cooperator] ([cooperator_id])
GO

ALTER TABLE [dbo].[feedback_result_trait_obs] CHECK CONSTRAINT [fk_fresto_owned]
GO

ALTER TABLE [dbo].[feedback_result_trait_obs]  WITH CHECK ADD  CONSTRAINT [fk_fro_created] FOREIGN KEY([created_by])
REFERENCES [dbo].[cooperator] ([cooperator_id])
GO

ALTER TABLE [dbo].[feedback_result_trait_obs] CHECK CONSTRAINT [fk_fro_created]
GO

ALTER TABLE [dbo].[feedback_result_trait_obs]  WITH CHECK ADD  CONSTRAINT [fk_fro_ct] FOREIGN KEY([crop_trait_id])
REFERENCES [dbo].[crop_trait] ([crop_trait_id])
GO

ALTER TABLE [dbo].[feedback_result_trait_obs] CHECK CONSTRAINT [fk_fro_ct]
GO

ALTER TABLE [dbo].[feedback_result_trait_obs]  WITH CHECK ADD  CONSTRAINT [fk_fro_ctc] FOREIGN KEY([crop_trait_code_id])
REFERENCES [dbo].[crop_trait_code] ([crop_trait_code_id])
GO

ALTER TABLE [dbo].[feedback_result_trait_obs] CHECK CONSTRAINT [fk_fro_ctc]
GO

ALTER TABLE [dbo].[feedback_result_trait_obs]  WITH CHECK ADD  CONSTRAINT [fk_fro_i] FOREIGN KEY([inventory_id])
REFERENCES [dbo].[inventory] ([inventory_id])
GO

ALTER TABLE [dbo].[feedback_result_trait_obs] CHECK CONSTRAINT [fk_fro_i]
GO

ALTER TABLE [dbo].[feedback_result_trait_obs]  WITH CHECK ADD  CONSTRAINT [fk_fro_m] FOREIGN KEY([method_id])
REFERENCES [dbo].[method] ([method_id])
GO

ALTER TABLE [dbo].[feedback_result_trait_obs] CHECK CONSTRAINT [fk_fro_m]
GO

ALTER TABLE [dbo].[feedback_result_trait_obs]  WITH CHECK ADD  CONSTRAINT [fk_fro_modified] FOREIGN KEY([modified_by])
REFERENCES [dbo].[cooperator] ([cooperator_id])
GO

ALTER TABLE [dbo].[feedback_result_trait_obs] CHECK CONSTRAINT [fk_fro_modified]
GO

ALTER TABLE [dbo].[feedback_result_trait_obs]  WITH CHECK ADD  CONSTRAINT [fk_fro_owned] FOREIGN KEY([owned_by])
REFERENCES [dbo].[cooperator] ([cooperator_id])
GO

ALTER TABLE [dbo].[feedback_result_trait_obs] CHECK CONSTRAINT [fk_fro_owned]
GO

ALTER TABLE [dbo].[feedback_result_trait_obs]  WITH CHECK ADD  CONSTRAINT [fk_frto_fr] FOREIGN KEY([feedback_result_id])
REFERENCES [dbo].[feedback_result] ([feedback_result_id])
GO

ALTER TABLE [dbo].[feedback_result_trait_obs] CHECK CONSTRAINT [fk_frto_fr]
GO

