USE [MinistryPlatform]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[crds_tr_Update_BMT_Donation_Status]') AND type in (N'TR'))
BEGIN
    EXEC ('CREATE TRIGGER [dbo].[crds_tr_Update_BMT_Donation_Status]
           ON  [dbo].[Donations] AFTER INSERT AS BEGIN SELECT ''STUB'' END');
END;
GO


ALTER TRIGGER [dbo].[crds_tr_Update_BMT_Donation_Status]
  ON  [dbo].[Donations]
  AFTER INSERT
AS
BEGIN
	-- Donations created by Batch Manager are automatically set to status = Deposited.  We want
	-- these to be status = Pending initially until Stripe payment processing is completed.
	UPDATE d
	SET
		d.Donation_Status_ID = 1		-- Pending
	FROM
		[dbo].[Donations] d
		INNER JOIN [dbo].[Batches] b ON b.Batch_Id = d.Batch_Id
		INNER JOIN INSERTED ON INSERTED.Donation_Id = d.Donation_Id
	WHERE
		d.Donation_Status_ID = 2 AND	-- Deposited
		b.Batch_Entry_Type_ID = 12 AND	-- Batch Manager type
		b.Finalize_Date IS NULL			-- not yet finalized 
	;
END
GO
