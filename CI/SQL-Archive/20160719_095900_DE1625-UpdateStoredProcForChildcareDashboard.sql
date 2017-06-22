USE [MinistryPlatform]
GO

/****** Object:  StoredProcedure [dbo].[api_crds_GetChildcareDashboard]    Script Date: 7/19/2016 9:03:45 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[api_crds_GetChildcareDashboard]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[api_crds_GetChildcareDashboard]
GO

/****** Object:  StoredProcedure [dbo].[api_crds_GetChildcareDashboard]    Script Date: 7/19/2016 9:03:45 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[api_crds_GetChildcareDashboard]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[api_crds_GetChildcareDashboard] AS' 
END
GO


ALTER PROCEDURE [dbo].[api_crds_GetChildcareDashboard]
	-- Add the parameters for the stored procedure here
	@Domain_ID int,
	@Contact_ID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @Household_ID int;
	DECLARE @ChildcareEventType int = 243;
	DECLARE @Childcare_Group_Type int = 27;
	
	SELECT @Household_ID = Household_ID FROM dbo.Contacts
	WHERE Contact_ID = @Contact_ID

	SELECT c.Nickname, c.Last_Name, gp.Group_ID, gp.Group_Participant_ID, g.Group_Name, g.Group_Type_ID, e.Event_ID, e.Event_Title, e.Cancelled, e.Event_Start_Date, e.Event_End_Date, e.Congregation_ID, childcareGroup.Group_ID as Childcare_Group_ID, childcareGroup.Maximum_Age as Childcare_Group_Max_Age FROM dbo.Group_Participants gp
	JOIN dbo.Groups g on g.Group_ID = gp.Group_ID and g.Domain_ID = gp.Domain_ID and g.Domain_ID = @Domain_ID
	JOIN dbo.Participants p on gp.Participant_ID = p.Participant_ID and gp.Domain_ID = p.Domain_ID
	JOIN dbo.Contacts c on c.Contact_ID = p.Contact_ID AND c.Household_ID = @Household_ID and c.Domain_ID = p.Domain_ID
	JOIN dbo.Event_Groups eg on eg.Group_ID = g.Group_ID and eg.Domain_ID = g.Domain_ID
	JOIN dbo.Events e on e.Event_ID = eg.Event_ID AND Event_Type_ID = @ChildcareEventType and eg.Domain_ID = e.Domain_ID
	JOIN dbo.Event_Groups nextEventGroup on nextEventGroup.Event_ID = e.Event_ID
	JOIN dbo.Groups childcareGroup on nextEventGroup.Group_ID = childcareGroup.Group_ID
	WHERE (gp.End_Date IS NULL OR gp.End_Date > GetDate()) AND e.Event_End_Date > GetDate()
	AND c.Household_Position_ID IN (1, 7)
	AND childcareGroup.Group_Type_ID = @Childcare_Group_Type
END
GO


