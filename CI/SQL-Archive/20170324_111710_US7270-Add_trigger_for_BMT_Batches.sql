USE MINISTRYPLATFORM
GO	

/****** Object:  Trigger [dbo].[crds_tr_Update_Donations_Status_for_Finalized_BMT_Batch]    Script Date: 3/24/2017 11:26:05 AM ******/

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[crds_tr_Update_Donations_Status_for_Finalized_BMT_Batch]') AND type in (N'TR'))
BEGIN
    EXEC ('CREATE TRIGGER [dbo].[crds_tr_Update_Donations_Status_for_Finalized_BMT_Batch]
           ON  [dbo].[Batches] AFTER UPDATE AS BEGIN SELECT ''STUB'' END');
END;

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ================================================
-- Author:		Lakshmi Nair	
-- Create date: 3/24/2017
-- Description:	updating donation status to 
--              'Deposited' for BMT finalized batch
-- ================================================

ALTER TRIGGER [dbo].[crds_tr_Update_Donations_Status_for_Finalized_BMT_Batch]
  ON  [dbo].[Batches]
  AFTER UPDATE
AS
IF(UPDATE(Finalize_Date))

BEGIN    
	DECLARE @BatchId AS INT

	SELECT @BatchId = B.Batch_ID 
	FROM Batches B
		JOIN  INSERTED ON INSERTED.Batch_ID = B.batch_ID
	WHERE INSERTED.Batch_Entry_Type_ID = 12

	IF(@BatchId IS NOT NULL) 
	BEGIN
		  
		SET NOCOUNT ON;
		DECLARE @Donation_Cursor CURSOR;
		SET @Donation_Cursor = CURSOR FOR
		SELECT
			D.Donation_ID,
			D.Donation_Status_ID	
		FROM Donations D 
		WHERE D.Batch_ID = @BatchId

		DECLARE @Donation_ID INT;
	    DECLARE @Donation_Status INT;

        OPEN @Donation_Cursor
        FETCH NEXT FROM @Donation_Cursor INTO @Donation_ID, @Donation_Status
        WHILE @@FETCH_STATUS = 0
        BEGIN
            IF (@Donation_Status = 1) 
                UPDATE Donations set Donation_Status_ID = 2 where Donation_ID = @Donation_ID;
            FETCH NEXT FROM @Donation_Cursor INTO @Donation_ID, @Donation_Status
        END
    CLOSE @Donation_Cursor
    DEALLOCATE @Donation_Cursor
	END
END
GO

	