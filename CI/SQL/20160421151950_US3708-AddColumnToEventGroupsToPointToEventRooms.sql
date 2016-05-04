USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER TABLE [dbo].[Event_Groups]
ADD Event_Room_ID int NULL

ALTER TABLE [dbo].[Event_Groups]
ADD FOREIGN KEY (Event_Room_ID) REFERENCES Event_Rooms(Event_Room_ID)

Go
