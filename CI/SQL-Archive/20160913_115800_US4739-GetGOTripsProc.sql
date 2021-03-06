USE MinistryPlatform
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[report_CRDS_filter_trips]') AND TYPE IN (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[report_CRDS_filter_trips] AS' 
END
GO
​
​
ALTER PROCEDURE [dbo].[report_CRDS_filter_trips]​
AS
BEGIN
​
DECLARE @CampaignTypeID INT
SET @CampaignTypeID = (SELECT Top 1 Pledge_Campaign_Type_ID FROM Pledge_Campaign_Types WHERE Pledge_Campaign_Type_ID = 2)
​
​
SELECT E.Event_Title, E.Event_ID
	FROM DBO.Events E
	WHERE E.Event_ID IN (SELECT Event_ID FROM Pledge_Campaigns PC WHERE PC.Pledge_Campaign_Type_ID = @CampaignTypeID) 
​    ORDER BY Event_Title
​
END
GO
​