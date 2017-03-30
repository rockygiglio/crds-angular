USE MINISTRYPLATFORM
GO	

/****** Object:  Trigger [dbo].[crds_tr_Update_BMT_Donation_Status]    Script Date: 3/24/2017 10:26:05 AM ******/

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[crds_tr_Update_BMT_Donation_Status]') AND type in (N'TR'))
BEGIN
    EXEC ('CREATE TRIGGER [dbo].[crds_tr_Update_BMT_Donation_Status]
           ON  [dbo].[Donations] AFTER INSERT AS BEGIN SELECT ''STUB'' END');
END;

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Lakshmi Nair	
-- Create date: 3/24/2017
-- Description:	updating donation status 
--              to 'Pending' for BMT donations
-- =============================================

ALTER TRIGGER [dbo].[crds_tr_Update_BMT_Donation_Status]
  ON  [dbo].[Donations]
  AFTER INSERT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @DonationId INT;
	DECLARE @DonationStatusId INT;
	DECLARE @BatchEntryTypeId INT;
	DECLARE @BatchFinalizeDate DATETIME;

	SELECT @DonationId = d.Donation_ID, @DonationStatusId = d.Donation_Status_ID, @BatchEntryTypeId = b.Batch_Entry_Type_ID, @BatchFinalizeDate = b.[Finalize_Date]
		FROM [dbo].[Donations] d
		JOIN [dbo].[Batches] b ON b.batch_id = d.batch_id

	-- Set Donation staus to 'Pending' if Batch Entry type is Batch Manager Tool (id# - 12)	and
	-- Batch finalize date is null
	IF (@BatchEntryTypeId = 12 ) AND (@DonationStatusId =2) AND (@BatchFinalizeDate IS NULL)
		SET @DonationStatusId = 1;

	UPDATE [dbo].[Donations]
		SET Donation_Status_ID = @DonationStatusId
		WHERE Donation_ID = @DonationId;

END
GO