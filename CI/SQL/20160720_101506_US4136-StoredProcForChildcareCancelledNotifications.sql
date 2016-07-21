SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Andy Canterbury <andrew.canterbury@ingagepartners.com>
-- Create date: 7/20/2016
-- Description:	Get data for sending notifications when a childcare session is cancelled. 
-- =============================================
CREATE PROCEDURE api_crds_CancelledChildcareNotification 
	@ChildcareGroupType int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @Events Table (
		Event_ID int NOT NULL
	)
	INSERT INTO @Events
	SELECT e.Event_ID FROM dbo.Events e
	JOIN dbo.Event_Groups eg ON eg.Event_ID = e.Event_ID
	JOIN dbo.Groups g ON g.Group_ID = eg.Group_ID
	WHERE e.Cancelled = 1 AND Event_Type_ID = 243 AND g.Group_Type_ID = 27 AND e.Event_Start_Date > GETDATE()
	AND g.Group_ID IN (SELECT Group_ID FROM dbo.Group_Participants WHERE End_Date IS NULL GROUP BY Group_ID  HAVING Count(*) > 0)

	SELECT gp.Group_Participant_ID, gp.Group_ID, c.Contact_ID, c.Nickname, c.Last_Name, e.Event_Start_Date, e.Congregation_ID, l.Congregation_Name, l.Childcare_Contact, cc.Email_Address AS [Childcare_Email], ec.Contact_ID, ec.Email_Address, ec.Nickname AS [Enroller_Nickname], ebg.Group_Name FROM dbo.Group_Participants gp
	JOIN dbo.Event_Groups eg ON gp.Group_ID = eg.Group_ID
	JOIN dbo.Events e on e.Event_ID = eg.Event_ID
	JOIN dbo.Congregations l on l.Congregation_ID = e.Congregation_ID
	JOIN dbo.Groups g ON g.Group_ID = eg.Group_ID AND g.Group_Type_ID = @ChildcareGroupType
	JOIN dbo.Contacts c ON c.Participant_Record = gp.Participant_ID
	JOIN dbo.Group_Participants eb ON eb.Group_Participant_ID = gp.Enrolled_By
	JOIN dbo.Contacts ec ON ec.Participant_Record = eb.Participant_ID
	JOIN dbo.Groups ebg ON ebg.Group_ID = eb.Group_ID
	JOIN dbo.Contacts cc on cc.Contact_ID = L.Childcare_Contact
	WHERE eg.Event_ID IN (SELECT Event_ID FROM @Events)
END
GO