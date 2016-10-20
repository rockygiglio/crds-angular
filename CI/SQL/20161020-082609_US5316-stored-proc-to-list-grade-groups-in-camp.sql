USE [MinistryPlatform]
GO

/****** Object:  StoredProcedure [dbo].[api_crds_Grade_Group_For_Camps]    Script Date: 10/20/2016 8:16:50 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[api_crds_Grade_Group_For_Camps]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[api_crds_Grade_Group_For_Camps]
GO

/****** Object:  StoredProcedure [dbo].[api_crds_Grade_Group_For_Camps]    Script Date: 10/20/2016 8:16:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[api_crds_Grade_Group_For_Camps]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[api_crds_Grade_Group_For_Camps] AS' 
END
GO


-- =============================================
-- Author:      Matt Silbernagel
-- Create date: 10/20/2016
-- Description: Find all unique grade groups in any camp that is currently open for registration 
-- =============================================
ALTER PROCEDURE [dbo].[api_crds_Grade_Group_For_Camps]
AS
BEGIN

	DECLARE @GroupType NVARCHAR(50) = N'Age or Grade Group';
	
	SELECT distinct(g.Group_ID) as Group_ID, g.Group_Name as Group_Name from [dbo].[Events] e
	JOIN Event_Groups eg on eg.Event_ID = e.Event_ID
	JOIN Groups g on eg.Group_ID = g.Group_ID AND g.Group_Type_ID in (
	
		SELECT gt.Group_Type_ID FROM [MinistryPlatform].[dbo].[Groups] g
		JOIN [dbo].[Group_Types] gt on gt.[Group_Type_ID] = g.Group_Type_ID
		WHERE gt.Group_Type = @GroupType)

	WHERE e.[Event_Type_ID] = 8 
	AND getDate() between e.[Registration_Start] and e.[Registration_End]
	AND e.[Cancelled] = 0

END
GO


