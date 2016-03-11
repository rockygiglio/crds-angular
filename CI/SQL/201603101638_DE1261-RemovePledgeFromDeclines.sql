USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      Joe Kerstanoff
-- Create date: 3/10/2016
-- Update date: 3/10/2016
-- Description: Remove Pledges from declined donations so they are not counted on reports / profile give page.
-- =============================================
Create TRIGGER [dbo].[crds_tr_Update_RemovePledgeIfDeclined]
  ON  [dbo].[Donations]
  AFTER INSERT, UPDATE
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