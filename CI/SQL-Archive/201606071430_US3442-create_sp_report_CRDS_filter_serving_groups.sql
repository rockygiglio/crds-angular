USE [MinistryPlatform]
GO

/****** Object:  StoredProcedure [dbo].[report_CRDS_filter_serving_groups]    Script Date: 6/7/2016 2:05:52 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- ====================================================================================
-- Author:		Mike Roberts
-- Create date: 6/7/2016
-- Description:	SP to filter serving groups based on congregation
-- Inputs: congregations 
-- ===================================================================================
IF NOT EXISTS
 (SELECT *
  FROM sys.objects
  WHERE object_id = object_id(N'[dbo].[report_CRDS_filter_serving_groups]')
    AND TYPE IN (N'P',
                  N'PC')) BEGIN EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[report_CRDS_filter_serving_groups] AS';

END 
GO

-- =============================================

ALTER PROCEDURE [dbo].[report_CRDS_filter_serving_groups]
-- Add the parameters for the stored procedure here
      --@DomainID VARCHAR(40), 
	 -- @UserID VARCHAR(40), 
	  --@PageID INT, 
	  @congregationid AS VARCHAR(MAX)  

AS
     BEGIN
	 --specific query to pull serving teams
         select g.Group_Name, g.Group_ID, g.Congregation_ID, (g.Group_Name + ' (' + c.Congregation_Name + ')') AS DisplayName
			  from MinistryPlatform.dbo.Groups g
			  inner join MinistryPlatform.dbo.Congregations c on g.Congregation_ID = c.Congregation_ID
			  where g.Group_Type_ID = 9
			  and g.End_Date is null
			  AND g.congregation_id IN (SELECT Item FROM dbo.dp_Split(@congregationid, ','))
			  order by g.Group_Name, c.Congregation_Name
     END;



GO


