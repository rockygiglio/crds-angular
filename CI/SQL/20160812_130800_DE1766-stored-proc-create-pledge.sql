USE [MinistryPlatform]
GO

/****** Object:  StoredProcedure [dbo].[api_crds_CreatePledge]    Script Date: 8/16/2016 11:16:24 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[api_crds_CreatePledge]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[api_crds_CreatePledge]
GO

/****** Object:  StoredProcedure [dbo].[api_crds_CreatePledge]    Script Date: 8/16/2016 11:16:24 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[api_crds_CreatePledge]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[api_crds_CreatePledge] AS' 
END
GO

ALTER PROCEDURE [dbo].[api_crds_CreatePledge]
	-- Add the parameters for the stored procedure here
	@ContactID int,
	@PledgeCampaignID int,
	@PledgeId int OUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @DonorID int = 0;
	DECLARE @FundraisingGoal money;

	SELECT TOP 1 @FundraisingGoal = [Fundraising_Goal]
	FROM [dbo].[Pledge_Campaigns]
	WHERE [Pledge_Campaign_ID] = @PledgeCampaignID;

	SELECT @DonorID = Donor_Record FROM Contacts WHERE Contact_ID = @ContactID;

	-- CREATE DONOR RECORD IF IT DOESN'T EXIST
	IF NOT EXISTS ( SELECT 1 WHERE @DonorID > 0 )
	BEGIN
		INSERT INTO [dbo].[Donors] (
			Contact_ID,
			Statement_Frequency_ID,
			Statement_Type_ID,
			Statement_Method_ID,
			Setup_Date,
			Domain_ID
		) VALUES (
			@ContactID,
			1,
			1,
			2,
			GETDATE(),
			1
		)
		SELECT @DonorID = @@IDENTITY; 
	END

	-- CREATE PLEDGE RECORD IF IT DOESN'T EXIST
	IF NOT EXISTS (SELECT 1 FROM [dbo].[Pledges] p
				   JOIN [Pledge_Statuses] ps on ps.Pledge_Status_ID = p.Pledge_Status_ID
				   WHERE [Pledge_Campaign_ID] = @PledgeCampaignID 
				   AND [Donor_ID] = @DonorID AND ps.Pledge_Status != N'Discontinued')
	BEGIN
		INSERT INTO [dbo].[Pledges] (
			[Donor_ID],
			[Pledge_Campaign_ID],
			[Pledge_Status_ID],
			[Total_Pledge],
			[Installments_Planned],
			[Installments_Per_Year],
			[First_Installment_Date],
			[Domain_ID]
		) VALUES (
			@DonorID,
			@PledgeCampaignID,
			1,
			@FundraisingGoal,
			0,
			0,
			GETDATE(),
			1
		)
		SELECT @PledgeID = @@IDENTITY;
	END
	ELSE 
	BEGIN
		SELECT @PledgeID = Pledge_ID FROM [dbo].[Pledges] p
				   JOIN [Pledge_Statuses] ps on ps.Pledge_Status_ID = p.Pledge_Status_ID
				   WHERE [Pledge_Campaign_ID] = @PledgeCampaignID 
				   AND [Donor_ID] = @DonorID AND ps.Pledge_Status != N'Discontinued'
	END
	-- END PLEDGE
END

GO
