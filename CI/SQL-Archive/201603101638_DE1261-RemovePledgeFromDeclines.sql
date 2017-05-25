USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Create a placeholder Trigger, so the below ALTER will always work.
-- This allows the same SQL file to be run in any environment, without errors.
-- This is preferable to dropping the trigger, as permissions will be maintained
-- on an existing trigger.
IF OBJECT_ID('dbo.crds_tr_Update_RemovePledgeIfDeclined', 'TR') IS NULL
    EXEC('CREATE TRIGGER dbo.crds_tr_Update_RemovePledgeIfDeclined ON [dbo].[Donations] AFTER UPDATE AS SELECT 1')
GO

-- =============================================
-- Author:      Joe Kerstanoff
-- Create date: 3/10/2016
-- Update date: 3/10/2016
-- Description: Remove Pledges from declined donations so they are not counted on reports / profile give page.
-- =============================================
ALTER TRIGGER [dbo].[crds_tr_Update_RemovePledgeIfDeclined]
  ON  [dbo].[Donations]
  AFTER UPDATE
AS
BEGIN

  SET NOCOUNT ON;
   DECLARE @Donation_Cursor CURSOR;
    SET @Donation_Cursor = CURSOR FOR
        SELECT
            I.Donation_ID,
            I.Donation_Status_ID
        FROM INSERTED I
        JOIN DELETED D ON I.Donation_ID = D.Donation_ID

    DECLARE @Donation_ID INT;
	DECLARE @Donation_Status INT;
    OPEN @Donation_Cursor
    FETCH NEXT FROM @Donation_Cursor INTO @Donation_ID, @Donation_Status
    WHILE @@FETCH_STATUS = 0
    BEGIN
        IF (@Donation_Status = 3)
            UPDATE Donation_Distributions set pledge_id = null where Donation_ID = @Donation_ID;
        FETCH NEXT FROM @Donation_Cursor INTO @Donation_ID, @Donation_Status
    END
    CLOSE @Donation_Cursor
    DEALLOCATE @Donation_Cursor
END