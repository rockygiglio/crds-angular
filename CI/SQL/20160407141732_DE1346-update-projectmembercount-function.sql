USE [MinistryPlatform]
GO

/****** Object:  UserDefinedFunction [dbo].[crds_GoVolunteerProjectMemberCount]    Script Date: 4/6/2016 12:47:56 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



ALTER FUNCTION [dbo].[crds_GoVolunteerProjectMemberCount](@ProjectId INT)
RETURNS INT
AS
BEGIN
    RETURN(
	   select SUM(r._Contributor_Count)
	   from dbo.cr_Group_Connectors gc
	   inner join dbo.cr_Group_Connector_Registrations gcr on gc.Group_Connector_ID = gcr.Group_Connector_ID
	   inner join dbo.cr_Registrations r on gcr.Registration_ID = r.Registration_ID
	   where gc.Project_ID = @ProjectId
    )
END


GO