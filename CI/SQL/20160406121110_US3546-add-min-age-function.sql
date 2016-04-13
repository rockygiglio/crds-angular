USE [MinistryPlatform]
GO

/****** Object:  UserDefinedFunction [dbo].[crds_GoVolunteerGroupConnectMinAge]    Script Date: 4/6/2016 12:02:49 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[crds_GoVolunteerGroupConnectMinAge]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
CREATE FUNCTION [dbo].[crds_GoVolunteerGroupConnectMinAge](@PrimaryRegistrationId INT, @ProjectId INT)
RETURNS INT
AS
BEGIN
	DECLARE @MinAge AS INT;
	IF (@ProjectID > 0)
	BEGIN
		SET @MinAge = (SELECT pt.Minimum_Age FROM [dbo].[cr_Projects] AS p
		JOIN [dbo].[cr_Project_Types] AS pt ON p.Project_Type_ID = pt.Project_Type_ID
		WHERE p.Project_ID = @ProjectID)
	END
	ELSE
	BEGIN
		SET @MinAge = (SELECT MAX(pt.Minimum_Age) FROM [dbo].[cr_Registration_Project_Type] AS r
		JOIN [dbo].[cr_Project_Types] AS pt ON r.Project_Type_ID = pt.Project_Type_ID
		WHERE r.Registration_ID = @PrimaryRegistrationID)
	END
	RETURN @MinAge;
END
' 
END

GO


