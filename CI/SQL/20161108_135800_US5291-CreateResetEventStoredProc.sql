USE MinistryPlatform;

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[api_crds_ResetEcheckEvent]') AND type in (N'P', N'PC'))
BEGIN
  EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[api_crds_ResetEcheckEvent] AS'
END
GO

-- =============================================
-- Author:      Jim Kriz
-- Create date: 2016-11-08
-- Description: Delete all Event Groups, Event Rooms, and Bumping Rules for an event
-- =============================================
ALTER PROCEDURE api_crds_ResetEcheckEvent
  @EventId int
AS
BEGIN
  DECLARE @Success INT = 1;

  BEGIN TRANSACTION
  BEGIN TRY
    DELETE FROM Event_Groups WHERE Event_ID = @EventId;
    DELETE FROM cr_Bumping_Rules WHERE From_Event_Room_ID IN (
      SELECT Event_Room_ID FROM Event_Rooms WHERE Event_ID = @EventId
    );
    DELETE FROM Event_Rooms WHERE Event_ID = @EventId;
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