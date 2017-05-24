USE [MinistryPlatform]
GO

/****** Object:  UserDefinedFunction [dbo].[crds_GoVolunteerFamilyCount]    Script Date: 3/18/2016 2:47:02 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[crds_GoVolunteerProjectMemberCount](@ProjectId INT)
RETURNS INT
AS
BEGIN
    RETURN(
	   select SUM(r._Family_Count)
	   from dbo.cr_GroupConnectors gc
	   inner join dbo.cr_GroupConnectorRegistrations gcr on gc.GroupConnector_ID = gcr.GroupConnector_ID
	   inner join dbo.cr_Registrations r on gcr.Registration_ID = r.Registration_ID
	   where gc.Project_ID = @ProjectId
    )
END

GO


