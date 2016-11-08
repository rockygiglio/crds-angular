USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

-- ===============================================================
-- Authors: John Cleaver <john.cleaver@ingagepartners.com>
-- Create date: 11/04/2016
-- Description:	Bumping Rules for Bumping Rule Configurations
-- ===============================================================

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE table_name='cr_Bumping_Rules')
BEGIN
	CREATE TABLE [dbo].[cr_Bumping_Rules](
		[Bumping_Rules_ID] [int] IDENTITY(1,1) NOT NULL,
		[From_Event_Room_ID] [int] NOT NULL,
		[To_Event_Room_ID] [int] NOT NULL,
		[Priority_Order] [int] NOT NULL,
		[Bumping_Rule_Type_ID] [int] NOT NULL,
		[Domain_ID] [int] NOT NULL CONSTRAINT [DF_cr_Bumping_Rules_Domain_ID]  DEFAULT ((1)),
	 CONSTRAINT [PK_cr_Bumping_Rules] PRIMARY KEY CLUSTERED 
	(
		[Bumping_Rules_ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	END

	ALTER TABLE [dbo].[cr_Bumping_Rules]  WITH CHECK ADD CONSTRAINT [FK_cr_Bumping_Rules_cr_Bumping_Rule_Types] FOREIGN KEY([Bumping_Rule_Type_ID])
	REFERENCES [dbo].[cr_Bumping_Rule_Types] ([Bumping_Rule_Type_ID])
	GO

	ALTER TABLE [dbo].[cr_Bumping_Rules] CHECK CONSTRAINT [FK_cr_Bumping_Rules_cr_Bumping_Rule_Types]
	GO
	
	ALTER TABLE [dbo].[cr_Bumping_Rules]  WITH CHECK ADD CONSTRAINT [FK_cr_Bumping_Rules_cr_From_Event_Room] FOREIGN KEY([From_Event_Room_ID])
	REFERENCES [dbo].[Event_Rooms] ([Event_Room_ID])
	GO

	ALTER TABLE [dbo].[cr_Bumping_Rules] CHECK CONSTRAINT [FK_cr_Bumping_Rules_cr_From_Event_Room]
	GO
	
	ALTER TABLE [dbo].[cr_Bumping_Rules]  WITH CHECK ADD CONSTRAINT [FK_cr_Bumping_Rules_cr_To_Event_Room] FOREIGN KEY([To_Event_Room_ID])
	REFERENCES [dbo].[Event_Rooms] ([Event_Room_ID])
	GO

	ALTER TABLE [dbo].[cr_Bumping_Rules] CHECK CONSTRAINT [FK_cr_Bumping_Rules_cr_To_Event_Room]
	GO

	ALTER TABLE [dbo].[cr_Bumping_Rules]  WITH CHECK ADD CONSTRAINT [FK_cr_Bumping_Rules_Dp_Domains] FOREIGN KEY([Domain_ID])
	REFERENCES [dbo].[dp_Domains] ([Domain_ID])
	GO

	ALTER TABLE [dbo].[cr_Bumping_Rules] CHECK CONSTRAINT [FK_cr_Bumping_Rules_Dp_Domains]
	GO

GO

SET ANSI_PADDING OFF
GO



