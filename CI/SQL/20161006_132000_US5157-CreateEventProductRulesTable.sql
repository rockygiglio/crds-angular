USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cr_Event_Product_Rules]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[cr_Event_Product_Rules](
	[EventProductRule_ID] [INT] IDENTITY(1,1) NOT NULL,
	[Event_ID] [INT] NOT NULL,
	[ProductRule_ID] [INT] NOT NULL,
	[RemainingCapacity] [INT] NOT NULL,
	[RemainingWaitlistCapacity] [INT] NOT NULL,
	[Domain_ID] [INT] NOT NULL,
 CONSTRAINT [PK_cr_Event_Product_Rules] PRIMARY KEY CLUSTERED 
(
	[EventProductRule_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

-------------------------------------------------------------

ALTER TABLE [dbo].[cr_Event_Product_Rules]  WITH CHECK ADD  CONSTRAINT [FK_cr_Event_Product_Rules_Events] FOREIGN KEY([Event_ID])
REFERENCES [dbo].[Events] ([Event_ID])


ALTER TABLE [dbo].[cr_Event_Product_Rules] CHECK CONSTRAINT [FK_cr_Event_Product_Rules_Events]

-------------------------------------------------------------

-------------------------------------------------------------

ALTER TABLE [dbo].[cr_Event_Product_Rules]  WITH CHECK ADD  CONSTRAINT [FK_cr_Event_Product_Rules_dp_Domains] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])


ALTER TABLE [dbo].[cr_Event_Product_Rules] CHECK CONSTRAINT [FK_cr_Event_Product_Rules_dp_Domains]

-------------------------------------------------------------

ALTER TABLE [dbo].[cr_Event_Product_Rules]  WITH CHECK ADD  CONSTRAINT [FK_cr_Event_Product_Rules_Product_Rules] FOREIGN KEY([ProductRule_ID])
REFERENCES [dbo].[cr_Product_Rules] ([ProductRule_ID])


ALTER TABLE [dbo].[cr_Event_Product_Rules] CHECK CONSTRAINT [FK_cr_Event_Product_Rules_Product_Rules]


END

