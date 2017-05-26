USE [MinistryPlatform]
GO

/****** Object:  UserDefinedFunction [dbo].[crds_GetGroupCategories]    Script Date: 5/26/2016 11:40:04 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[crds_GetGroupCategories]') AND type = 'FN')
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[crds_GetGroupCategories](@GroupId INT) RETURNS NVARCHAR(MAX) AS BEGIN; RETURN 1; END'
END
GO


-- =============================================
-- Author:      Jim Kriz
-- Create date: 09/07/2016
-- Description: Get all categories and details for a given group id.
-- =============================================
ALTER FUNCTION [dbo].[crds_GetGroupCategories](@GroupId INT)
RETURNS NVARCHAR(MAX)
AS
BEGIN
	DECLARE @Categories AS NVARCHAR(MAX);
	SELECT @Categories = STUFF((
		SELECT 
			   ', ' + AC.Attribute_Category + '/' + A.Attribute_Name
		FROM  
			   Group_Attributes GA, Attributes A LEFT OUTER JOIN Attribute_Categories AC ON A.Attribute_Category_ID = AC.Attribute_Category_ID
		WHERE  
			   GA.Attribute_ID = A.Attribute_ID
			   AND GA.Group_ID = @GroupId
			   AND GETDATE() BETWEEN GA.Start_Date
			   AND ISNULL(GA.End_Date,GETDATE())
			   AND AC.Attribute_Category_ID IN (
				   17, -- Life Stages
				   18, -- Neighborhoods
				   19, -- Spiritual Growth
				   20, -- Interest
				   21  -- Healing
			   )
        FOR XML PATH('')), 1, 2, '');
	
	RETURN @Categories;
END
GO