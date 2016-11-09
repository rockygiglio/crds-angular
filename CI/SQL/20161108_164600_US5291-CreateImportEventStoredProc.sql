USE MinistryPlatform;

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[api_crds_ImportEcheckEvent]') AND type in (N'P', N'PC'))
BEGIN
  EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[api_crds_ImportEcheckEvent] AS'
END
GO

-- =============================================
-- Author:      Jim Kriz
-- Create date: 2016-11-08
-- Description: Copy all Event Groups, Event Rooms, and Bumping Rules for an event to another event
-- =============================================
ALTER PROCEDURE api_crds_ImportEcheckEvent
  @DestinationEventId int,
  @SourceEventId int
AS
BEGIN
  DECLARE @Success INT = 1;

  BEGIN TRANSACTION
  BEGIN TRY
    -- Insert Event_Rooms
    INSERT INTO Event_Rooms (Event_ID,            Room_ID, Domain_ID, _Approved, Hidden, Capacity, Label, Allow_Checkin, Volunteers)
      SELECT                 @DestinationEventId, Room_ID, Domain_ID, _Approved, Hidden, Capacity, Label, Allow_Checkin, Volunteers
      FROM  Event_Rooms
      WHERE Event_ID = @SourceEventId;

    -- Insert Bumping_Rules
    INSERT INTO cr_Bumping_Rules (From_Event_Room_ID,        To_Event_Room_ID,        Priority_Order,   Bumping_Rule_Type_ID,   Domain_ID)
      SELECT                      destFromRes.Event_Room_ID, destToRes.Event_Room_ID, b.Priority_Order, b.Bumping_Rule_Type_ID, b.Domain_ID
      FROM   
                cr_Bumping_Rules b
              , Event_Rooms srcFromRes
              , Event_Rooms srcToRes
              , Event_Rooms destFromRes
              , Event_Rooms destToRes
              , Rooms srcFromRoom
              , Rooms srcToRoom
              , Rooms destFromRoom
              , Rooms destToRoom
      WHERE  
              srcFromRes.Event_ID = @SourceEventId
              AND srcFromRes.Room_ID = srcFromRoom.Room_ID
              AND srcToRes.Event_ID = @SourceEventId
              AND srcToRes.Room_ID = srcToRoom.Room_ID
              AND destFromRes.Event_ID = @DestinationEventId
              AND destFromRes.Room_ID = destFromRoom.Room_ID
              AND destToRes.Event_ID = @DestinationEventId
              AND destToRes.Room_ID = destToRoom.Room_ID
              AND srcFromRoom.Room_ID = destFromRoom.Room_ID
              AND srcToRoom.Room_ID = destToRoom.Room_ID
              AND b.From_Event_Room_ID = srcFromRes.Event_Room_ID
              AND b.To_Event_Room_ID = srcToRes.Event_Room_ID;

    -- Insert Event_Groups for Event_Room
    INSERT INTO Event_Groups (Event_ID,            Group_ID,               Room_ID,               Domain_ID,               Closed,               Event_Room_ID)
      SELECT                  @DestinationEventId, srcEventGroup.Group_ID, srcEventGroup.Room_ID, srcEventGroup.Domain_ID, srcEventGroup.Closed, destRes.Event_Room_ID
      FROM
                Event_Groups srcEventGroup
              , Event_Rooms srcRes
              , Event_Rooms destRes
      WHERE
              srcEventGroup.Event_ID = @SourceEventId
              AND srcEventGroup.Event_Room_ID = srcRes.Event_Room_ID
              AND srcRes.Room_ID = destRes.Room_ID
              AND destRes.Event_ID = @DestinationEventId;
  END TRY
  BEGIN CATCH
    PRINT 'Rolling back transaction due to error ' + ERROR_MESSAGE();
    IF @@TRANCOUNT > 0
      ROLLBACK TRANSACTION;

    SET @Success = 0;
  END CATCH;

  IF @@TRANCOUNT > 0
    COMMIT TRANSACTION;

  RETURN @Success;
END
GO