USE [MinistryPlatform]
GO

/****** Object:  StoredProcedure [dbo].[report_filter_campaigns]    Script Date: 4/21/2017 3:13:56 PM ******/
IF object_id ('report_filter_campaigns') IS NULL 
  EXEC ('create procedure dbo.report_filter_campaigns as select 1')
GO

/****** Object:  StoredProcedure [dbo].[report_filter_campaigns]    Script Date: 4/21/2017 3:13:56 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[report_filter_campaigns]

	@DomainID varchar(40)

AS
BEGIN

	SELECT C.[Campaign_Name], C.Pledge_Campaign_ID
	FROM DBO.Pledge_Campaigns C
		INNER JOIN dp_Domains ON dp_Domains.Domain_ID = C.Domain_ID
	WHERE   --dp_Domains.Domain_GUID = @DomainID AND 
	     C.Pledge_Campaign_Type_ID = 1 --Capital Campaign

	ORDER BY C.End_Date DESC, C.Campaign_Name
END

GO


