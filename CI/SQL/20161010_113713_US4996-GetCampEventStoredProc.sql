USE [MinistryPlatform]
GO

/****** Object:  StoredProcedure [dbo].[api_crds_GetCampEvent]    Script Date: 10/10/2016 11:23:37 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[api_crds_GetCampEvent]
	-- Add the parameters for the stored procedure here
	@Domain_ID int,
	@Event_ID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	
	SELECT e.Event_ID
	       , e.Event_Type_ID
		   , e.Event_Start_Date
		   , e.Event_End_Date
		   , e.Online_Registration_Product
		   , e.Program_ID
		   , e.Registration_Start
		   , e.Registration_End

    FROM Events e
	WHERE e.Event_ID = @Event_ID 

END
GO
