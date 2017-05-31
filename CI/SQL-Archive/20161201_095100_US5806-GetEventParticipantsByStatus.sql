USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      Tim Giblin
-- Create date: 11/30/2016
-- Description: Get total count of Event Participants by Participation_Status_ID
-- =============================================
CREATE FUNCTION [dbo].[crds_getEventParticipantStatusCount](@Event_ID INT, @Room_ID INT, @Status_ID INT)
RETURNS INT
AS
BEGIN
    DECLARE @Participants_Count INT

    SELECT @Participants_Count = COUNT(*)
      FROM [dbo].event_rooms er
      LEFT OUTER JOIN Event_Participants ep ON er.Room_Id = ep.Room_Id
      WHERE ep.Event_Id = @Event_ID
      AND er.Event_Id = @Event_ID
      AND er.Room_Id = @Room_ID
      AND ep.Room_Id = @Room_ID
      AND ep.Participation_Status_ID = @Status_ID
      GROUP BY ep.Participation_Status_ID;

    RETURN COALESCE(@Participants_Count, 0)

END

GO
