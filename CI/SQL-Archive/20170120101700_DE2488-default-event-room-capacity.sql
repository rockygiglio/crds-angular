USE [MinistryPlatform]
GO

ALTER TABLE [dbo].[Event_Rooms] ADD CONSTRAINT
	[DF_Event_Rooms_Capacity] DEFAULT 0 FOR [Capacity];

ALTER TABLE [dbo].[Event_Rooms] ADD CONSTRAINT
	[DF_Event_Rooms_Volunteers] DEFAULT 0 FOR [Volunteers];
  
