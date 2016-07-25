USE [MinistryPlatform]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[api_crds_ChildcareReminderEmails]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[api_crds_ChildcareReminderEmails]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[api_crds_ChildcareReminderEmails]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[api_crds_ChildcareReminderEmails] AS' 
END
GO



ALTER PROCEDURE [dbo].[api_crds_ChildcareReminderEmails]
	-- Add the parameters for the stored procedure here
	@DaysOut int = 3
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @today_plus DateTime;

	SELECT @today_plus = CONVERT(date, DATEADD(day, @DaysOut, getDate()));

	SELECT Distinct(c.Email_Address) FROM dbo.[Events] e
	JOIN dbo.[Event_Groups] eg on eg.Event_ID = e.Event_ID
	JOIN dbo.[Groups] g on g.Group_ID = eg.Group_ID
	JOIN dbo.[Group_Participants] gp1 on gp1.Group_ID = g.Group_ID
	JOIN dbo.[Group_Participants] gp2 on gp2.Group_Participant_ID = gp1.Enrolled_By
	JOIN dbo.[Participants] p on p.Participant_ID = gp2.Participant_ID
	JOIN dbo.[Contacts] c on p.Contact_ID = c.Contact_ID
	WHERE Convert(date, e.Event_Start_Date) = @today_plus
		AND e.Event_Type_ID = 243
		AND e.Event_End_Date > GETDATE()
		AND g.Group_Type_ID = 27
		AND (gp1.[End_Date] IS Null OR gp1.[End_Date] > GETDATE())
	END
GO