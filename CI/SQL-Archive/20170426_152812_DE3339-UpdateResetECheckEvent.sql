USE [MinistryPlatform]
GO
/****** Object:  StoredProcedure [dbo].[api_crds_ResetEcheckEvent]    Script Date: 4/26/2017 3:13:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      Dustin Kocher
-- Create date: 2017-04-26
-- Description: Update to only delete Event_Groups with a Group Type of Age or Grade Group
-- =============================================
ALTER PROCEDURE [dbo].[api_crds_ResetEcheckEvent]
  @EventId int = 4534848
AS
BEGIN
  DECLARE @Success INT = 1;

  BEGIN TRANSACTION
  BEGIN TRY
    DELETE FROM Event_Groups 
	WHERE Event_Group_ID IN (
      SELECT Event_Group_ID FROM Event_Groups
      INNER JOIN Groups ON Groups.Group_ID = Event_Groups.Group_ID
      WHERE Event_Groups.Event_ID = @EventId AND Groups.Group_Type_ID = 4
	);

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

