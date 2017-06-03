USE [MinistryPlatform]
GO

/****** Object:  StoredProcedure [dbo].[service_church_specific]    Script Date: 4/6/2017 2:56:43 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



ALTER PROCEDURE [dbo].[service_church_specific]

	@DomainID INT	

AS
BEGIN

EXEC crds_service_assign_pledges_nightly @DomainID
EXEC crds_service_update_email_nightly

END

GO


