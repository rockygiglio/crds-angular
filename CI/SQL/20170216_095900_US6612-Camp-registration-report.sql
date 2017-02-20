USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Jon Horner
-- Create date: 2/16/2017
-- Description:	Gets data for camp registration report
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[report_CRDS_Camp_Registration]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[report_CRDS_Camp_Registration] AS' 
END
GO

ALTER PROCEDURE report_CRDS_Camp_Registration
	@campId int = 0
AS
BEGIN
	SET NOCOUNT ON;

    DECLARE @ageGradeGroup INT = 4;
	DECLARE @tshirtSizeAttrId INT = 21;

	DECLARE @schoolAttendingNextYear INT = 1504;
	DECLARE @crdsSite INT = 1517;
	DECLARE @roommateFirstLast INT = 1505;
	DECLARE @emerContactFirst INT = 1506;
	DECLARE @emerContactLast INT = 1507;
	DECLARE @emerContactMobile INT = 1508;
	DECLARE @emerContactEmail INT = 1509;
	DECLARE @emerContactRelationship INT = 1510;
	DECLARE @addlEmerContactFirst INT = 1512;
	DECLARE @addlEmerContactLast INT = 1513;
	DECLARE @addlEmerContactMobile INT = 1514;
	DECLARE @addlEmerContactEmail INT = 1515;
	DECLARE @addlEmerContactRelationship INT = 1516;
	DECLARE @financialAssistance INT = 1511;

	DECLARE @campFormFieldResponses TABLE
	(
		ID int identity
		, Event_Participant_ID int
		, Field_ID int
		, Label nvarchar(128)
		, Response nvarchar(max)
	);
	INSERT INTO @campFormFieldResponses
		SELECT Event_Participant_ID 
			, fra.Form_Field_ID
			--, REPLACE(Field_Label, ' ', '_')
			, Field_Label
			, Response
		FROM Form_Response_Answers fra
			JOIN Form_Fields ff ON (ff.Form_Field_ID = fra.Form_Field_ID)
		WHERE fra.Form_Field_ID IN (
			@schoolAttendingNextYear,			@crdsSite
			, @roommateFirstLast,				@emerContactFirst
			, @emerContactLast,					@emerContactMobile
			, @emerContactEmail,				@emerContactRelationship
			, @addlEmerContactFirst,			@addlEmerContactLast
			, @addlEmerContactMobile,			@addlEmerContactEmail
			, @addlEmerContactRelationship,		@financialAssistance
		);

	WITH campFormResponses AS (
		SELECT Event_Participant_ID
			, MIN(CASE Field_ID WHEN @schoolAttendingNextYear THEN Response END)		School_Attending_Next_Year
			, MIN(CASE Field_ID WHEN @crdsSite THEN Response END)						Students_Crossroads_Site
			, MIN(CASE Field_ID WHEN @roommateFirstLast THEN Response END)				Preferred_Roommate_First_and_Last_Name
			, MIN(CASE Field_ID WHEN @emerContactFirst THEN Response END)				Emergency_Contact_First_Name
			, MIN(CASE Field_ID WHEN @emerContactLast THEN Response END)				Emergency_Contact_Last_Name
			, MIN(CASE Field_ID WHEN @emerContactMobile THEN Response END)				Emergency_Contact_Mobile_Number
			, MIN(CASE Field_ID WHEN @emerContactEmail THEN Response END)				Emergency_Contact_Email
			, MIN(CASE Field_ID WHEN @emerContactRelationship THEN Response END)		Emergency_Contact_Relationship
			, MIN(CASE Field_ID WHEN @addlEmerContactFirst THEN Response END)			Addl_Emergency_Contact_First_Name
			, MIN(CASE Field_ID WHEN @addlEmerContactLast THEN Response END)			Addl_Emergency_Contact_Last_Name
			, MIN(CASE Field_ID WHEN @addlEmerContactMobile THEN Response END)			Addl_Emergency_Contact_Mobile_Number
			, MIN(CASE Field_ID WHEN @addlEmerContactEmail THEN Response END)			Addl_Emergency_Contact_Email
			, MIN(CASE Field_ID WHEN @addlEmerContactRelationship THEN Response END)	Addl_Emergency_Contact_Relationship
			, MIN(CASE Field_ID WHEN @financialAssistance THEN Response END)			Financial_Assistance_Requested
		FROM @campFormFieldResponses cffr
		GROUP BY Event_Participant_ID
	)
	SELECT ep.Event_Participant_ID
		, applicant.Nickname AS Applicant_First_Name
		, applicant.Last_Name AS Applicant_Last_Name
		, camper.First_Name AS Camper_First_Name
		, camper.Last_Name AS Camper_Last_Name
		, camper.Nickname AS Camper_Preferred_Name
		, camper.Mobile_Phone AS Student_Mobile_Number
		, a.Attribute_Name AS 'T-Shirt_Size'
		, ps.Participation_Status
		, genders.Gender AS Camper_Gender
		, camper.Date_of_Birth AS Camper_Birth_Date
		, g.Group_Name AS Current_Grade_Group
		, camper.Current_School AS School_Currently_Attending
		, cfr.School_Attending_Next_Year
		, cfr.Students_Crossroads_Site
		, cfr.Preferred_Roommate_First_and_Last_Name
		, cfr.Emergency_Contact_First_Name
		, cfr.Emergency_Contact_Last_Name
		, cfr.Emergency_Contact_Mobile_Number
		, cfr.Emergency_Contact_Email
		, cfr.Emergency_Contact_Relationship
		, cfr.Addl_Emergency_Contact_First_Name
		, cfr.Addl_Emergency_Contact_Last_Name
		, cfr.Addl_Emergency_Contact_Mobile_Number
		, cfr.Addl_Emergency_Contact_Email
		, cfr.Addl_Emergency_Contact_Relationship
		, MIN(CAST(ISNULL(epw.Accepted, 0) AS int)) Waiver_Accepted
		, MIN(signee.Display_Name) Waiver_Signee
		, MIN(signee.__Age) Waiver_Signee_Age
		, i.Invoice_Total AS Total_Cost
		, SUM(pay.Payment_Total) Amount_Paid
		, i.Invoice_Total - SUM(pay.Payment_Total) AS Remaining_Balance
	FROM [Events] e
		JOIN Event_Participants ep ON (ep.Event_ID = e.Event_ID)
		JOIN Participants p ON (p.Participant_ID = ep.Participant_ID)
		JOIN Contacts camper ON (camper.Contact_ID = p.Contact_ID)
		JOIN Participation_Statuses ps ON (ps.Participation_Status_ID = ep.Participation_Status_ID)
		JOIN Invoice_Detail id ON (id.Event_Participant_ID = ep.Event_Participant_ID)
		JOIN Invoices i ON (i.Invoice_ID = id.Invoice_ID)
		JOIN Payment_Detail pd ON (pd.Invoice_Detail_ID = id.Invoice_Detail_ID)
		JOIN Payments pay ON (pay.Payment_ID = pd.Payment_ID)
		JOIN Contacts applicant ON (applicant.Contact_ID = i.Purchaser_Contact_ID)
		JOIN Genders genders ON (genders.Gender_ID = camper.Gender_ID)
		JOIN Group_Participants gp ON (
			gp.Participant_ID = ep.Participant_ID
			AND (
				gp.End_Date IS NULL
				OR gp.End_Date > GETDATE()
			)
		)
		JOIN Groups g ON (g.Group_ID = gp.Group_ID AND g.Group_Type_ID = @ageGradeGroup)
		JOIN Contact_Attributes ca ON (ca.Contact_ID = camper.Contact_ID)
		JOIN Attributes a ON (a.Attribute_ID = ca.Attribute_ID AND a.Attribute_Type_ID = @tshirtSizeAttrId)
		JOIN cr_Event_Waivers ew ON (ew.Event_ID = ep.Event_ID)
		JOIN cr_Event_Participant_Waivers epw ON (epw.Event_Participant_ID = ep.Event_Participant_ID AND epw.Waiver_ID = ew.Waiver_ID)
		JOIN Contacts signee ON (signee.Contact_ID = epw.Signee_Contact_ID)
		JOIN campFormResponses cfr ON (cfr.Event_Participant_ID = ep.Event_Participant_ID)
	WHERE e.Event_ID = @campId
	GROUP BY ep.Event_Participant_ID
		, applicant.Nickname
		, applicant.Last_Name
		, camper.First_Name
		, camper.Last_Name
		, camper.Nickname
		, ps.Participation_Status
		, genders.Gender
		, camper.Date_of_Birth
		, g.Group_Name
		, camper.Current_School
		, cfr.School_Attending_Next_Year
		, cfr.Students_Crossroads_Site
		, cfr.Preferred_Roommate_First_and_Last_Name
		, cfr.Emergency_Contact_First_Name
		, cfr.Emergency_Contact_Last_Name
		, cfr.Emergency_Contact_Mobile_Number
		, cfr.Emergency_Contact_Email
		, cfr.Emergency_Contact_Relationship
		, cfr.Addl_Emergency_Contact_First_Name
		, cfr.Addl_Emergency_Contact_Last_Name
		, cfr.Addl_Emergency_Contact_Mobile_Number
		, cfr.Addl_Emergency_Contact_Email
		, cfr.Addl_Emergency_Contact_Relationship
		, camper.Mobile_Phone
		, a.Attribute_Name
		, i.Invoice_Total
END
GO
