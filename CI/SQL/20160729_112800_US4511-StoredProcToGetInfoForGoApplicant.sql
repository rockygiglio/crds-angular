USE MinistryPlatform
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ===============================================================
-- Author: Phil Lachmann
-- Create date: 7/29/2016
-- Description:	Get information for GO trip application
-- ===============================================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[api_crds_GetPledgeCampaignInfoForGoApplication]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[api_crds_GetPledgeCampaignInfoForGoApplication] AS'
END
GO

ALTER PROCEDURE [dbo].[api_crds_GetPledgeCampaignInfoForGoApplication]
	-- Add the parameters for the stored procedure here
	@Pledge_Campaign_ID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @GoTripEventTypeID INT = 6

	SELECT pc.Destination_ID, pc.Fundraising_Goal, g.group_id
	FROM [dbo].[Pledge_Campaigns] pc
	JOIN dbo.events e ON e.program_id = pc.program_id AND e.Event_Type_ID = @GoTripEventTypeID
	JOIN dbo.event_groups eg ON eg.event_id = e.event_id
	JOIN groups g ON g.group_id = eg.group_id
	WHERE pledge_campaign_id = @Pledge_Campaign_ID

END
GO
