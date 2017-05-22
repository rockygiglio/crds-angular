USE [MinistryPlatform]
GO

/****** Object:  StoredProcedure [dbo].[report_summer_camp_med_info_crossroads]    Script Date: 5/12/2017 10:10:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[report_summer_camp_med_info_crossroads]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[report_summer_camp_med_info_crossroads] AS' 
END
GO


ALTER PROCEDURE [dbo].[report_summer_camp_med_info_crossroads]

	@DomainID varchar(40)
	,@UserID varchar(40)
	,@PageID int
	,@EventID int
	,@ParticipantID int

AS
BEGIN

	DECLARE @Domain_ID int = (SELECT Domain_ID FROM dp_Domains WHERE CAST(Domain_GUID as varchar(40)) = @DomainID)

	SELECT 
		C.Contact_ID
		,MI.MedicalInformation_ID
		,C.First_Name
		,C.NickName
		,C.Last_Name
		,C.Display_Name
		,C.Date_of_Birth

		,(SELECT G.Gender FROM Genders G WHERE G.Gender_ID = C.Gender_ID) AS Gender

		,(SELECT TOP 1 G.Group_Name FROM Groups G 
			JOIN Group_Participants GP ON GP.Group_ID = G.Group_ID 
			WHERE G.Group_Type_ID = 4 AND GP.Participant_ID = C.Participant_Record 
				AND ISNULL(GP.End_Date,GETDATE()+1) > GETDATE() 
			ORDER BY GP.Start_Date DESC) AS Current_Grade

		,(SELECT FA.Response FROM Form_Response_Answers FA 
			JOIN Form_Responses FR ON FR.Form_Response_ID = FA.Form_Response_ID
			JOIN Event_Participants EP ON FA.Event_Participant_ID = EP.Event_Participant_ID
			WHERE FR.Contact_ID = C.Contact_ID 
				AND FR.Form_ID = 28 
				AND EP.Event_ID = @EventID AND EP.Participant_ID = ISNULL(@ParticipantID,C.Participant_Record)
				AND FA.Form_Field_ID = 1506) AS Emergency_Contact_First

		,(SELECT FA.Response FROM Form_Response_Answers FA 
			JOIN Form_Responses FR ON FR.Form_Response_ID = FA.Form_Response_ID
			JOIN Event_Participants EP ON FA.Event_Participant_ID = EP.Event_Participant_ID
			WHERE FR.Contact_ID = C.Contact_ID 
				AND FR.Form_ID = 28 
				AND EP.Event_ID = @EventID AND EP.Participant_ID = ISNULL(@ParticipantID,C.Participant_Record)
				AND FA.Form_Field_ID = 1507) AS Emergency_Contact_Last

		,(SELECT FA.Response FROM Form_Response_Answers FA 
			JOIN Form_Responses FR ON FR.Form_Response_ID = FA.Form_Response_ID
			JOIN Event_Participants EP ON FA.Event_Participant_ID = EP.Event_Participant_ID
			WHERE FR.Contact_ID = C.Contact_ID 
				AND FR.Form_ID = 28 
				AND EP.Event_ID = @EventID AND EP.Participant_ID = ISNULL(@ParticipantID,C.Participant_Record)
				AND FA.Form_Field_ID = 1508) AS Emergency_Contact_Mobile

		,(SELECT FA.Response FROM Form_Response_Answers FA 
			JOIN Form_Responses FR ON FR.Form_Response_ID = FA.Form_Response_ID
			JOIN Event_Participants EP ON FA.Event_Participant_ID = EP.Event_Participant_ID
			WHERE FR.Contact_ID = C.Contact_ID 
				AND FR.Form_ID = 28 
				AND EP.Event_ID = @EventID AND EP.Participant_ID = ISNULL(@ParticipantID,C.Participant_Record)
				AND FA.Form_Field_ID = 1509) AS Emergency_Contact_Email

		,(SELECT FA.Response FROM Form_Response_Answers FA 
			JOIN Form_Responses FR ON FR.Form_Response_ID = FA.Form_Response_ID
			JOIN Event_Participants EP ON FA.Event_Participant_ID = EP.Event_Participant_ID
			WHERE FR.Contact_ID = C.Contact_ID 
				AND FR.Form_ID = 28  
				AND EP.Event_ID = @EventID AND EP.Participant_ID = ISNULL(@ParticipantID,C.Participant_Record)
				AND FA.Form_Field_ID = 1510) AS Emergency_Contact_Relationship

		,(SELECT FA.Response FROM Form_Response_Answers FA 
			JOIN Form_Responses FR ON FR.Form_Response_ID = FA.Form_Response_ID
			JOIN Event_Participants EP ON FA.Event_Participant_ID = EP.Event_Participant_ID
			WHERE FR.Contact_ID = C.Contact_ID 
				AND FR.Form_ID = 28 
				AND EP.Event_ID = @EventID AND EP.Participant_ID = ISNULL(@ParticipantID,C.Participant_Record)
				AND FA.Form_Field_ID = 1512) AS Emergency_Contact2_First

		,(SELECT FA.Response FROM Form_Response_Answers FA 
			JOIN Form_Responses FR ON FR.Form_Response_ID = FA.Form_Response_ID
			JOIN Event_Participants EP ON FA.Event_Participant_ID = EP.Event_Participant_ID
			WHERE FR.Contact_ID = C.Contact_ID 
				AND FR.Form_ID = 28 
				AND EP.Event_ID = @EventID AND EP.Participant_ID = ISNULL(@ParticipantID,C.Participant_Record)
				AND FA.Form_Field_ID = 1513) AS Emergency_Contact2_Last

		,(SELECT FA.Response FROM Form_Response_Answers FA 
			JOIN Form_Responses FR ON FR.Form_Response_ID = FA.Form_Response_ID
			JOIN Event_Participants EP ON FA.Event_Participant_ID = EP.Event_Participant_ID
			WHERE FR.Contact_ID = C.Contact_ID 
				AND FR.Form_ID = 28 
				AND EP.Event_ID = @EventID AND EP.Participant_ID = ISNULL(@ParticipantID,C.Participant_Record)
				AND FA.Form_Field_ID = 1514) AS Emergency_Contact2_Mobile

		,(SELECT FA.Response FROM Form_Response_Answers FA 
			JOIN Form_Responses FR ON FR.Form_Response_ID = FA.Form_Response_ID
			JOIN Event_Participants EP ON FA.Event_Participant_ID = EP.Event_Participant_ID
			WHERE FR.Contact_ID = C.Contact_ID 
				AND FR.Form_ID = 28 
				AND EP.Event_ID = @EventID AND EP.Participant_ID = ISNULL(@ParticipantID,C.Participant_Record)
				AND FA.Form_Field_ID = 1515) AS Emergency_Contact2_Email

		,(SELECT FA.Response FROM Form_Response_Answers FA 
			JOIN Form_Responses FR ON FR.Form_Response_ID = FA.Form_Response_ID
			JOIN Event_Participants EP ON FA.Event_Participant_ID = EP.Event_Participant_ID
			WHERE FR.Contact_ID = C.Contact_ID 
				AND FR.Form_ID = 28 
				AND EP.Event_ID = @EventID AND EP.Participant_ID = ISNULL(@ParticipantID,C.Participant_Record)
				AND FA.Form_Field_ID = 1516) AS Emergency_Contact2_Relationship
		
		,MI.Insurance_Company
		,MI.Policy_Holder_Name
		,MI.Physician_Name
		,MI.Physician_Phone
		,MI.Allowed_To_Administer_Medications

		,STUFF((SELECT ', ' + A.Description 
			FROM cr_Medical_Information_Allergies MIA JOIN cr_Allergy A ON A.Allergy_ID = MIA.Allergy_ID
			WHERE MIA.Medical_Information_ID = MI.MedicalInformation_ID AND A.Allergy_Type_ID = 1 
			FOR XML PATH('')),1,2,'') AS Medicine_Allergies

		,STUFF((SELECT ', ' + A.Description 
			FROM cr_Medical_Information_Allergies MIA JOIN cr_Allergy A ON A.Allergy_ID = MIA.Allergy_ID
			WHERE MIA.Medical_Information_ID = MI.MedicalInformation_ID AND A.Allergy_Type_ID = 2 
			FOR XML PATH('')),1,2,'') AS Food_Allergies

		,STUFF((SELECT ', ' + A.Description 
			FROM cr_Medical_Information_Allergies MIA JOIN cr_Allergy A ON A.Allergy_ID = MIA.Allergy_ID
			WHERE MIA.Medical_Information_ID = MI.MedicalInformation_ID AND A.Allergy_Type_ID = 3 
			FOR XML PATH('')),1,2,'') AS Environmental_Allergies

		,STUFF((SELECT ', ' + A.Description 
			FROM cr_Medical_Information_Allergies MIA JOIN cr_Allergy A ON A.Allergy_ID = MIA.Allergy_ID
			WHERE MIA.Medical_Information_ID = MI.MedicalInformation_ID AND A.Allergy_Type_ID = 4 
			FOR XML PATH('')),1,2,'') AS Other_Allergies

		,(SELECT MAX(AL.Date_Time) FROM dp_Audit_Log AL WHERE AL.Record_ID = C.Contact_ID AND AL.Table_Name = 'Contacts') AS Last_Update

		,(SELECT E.Event_Title FROM Events E WHERE E.Event_ID = @EventID) AS Event_Title

		,MIM.MedicalInformationMedication_ID
		,MIM.Medication_Name
		,(SELECT MT.Medication_Type FROM cr_Medication_Types MT WHERE MT.Medication_Type_ID = MIM.Medication_Type_ID) AS [Medication_Type]
		,MIM.Dosage_Time
		,MIM.Dosage_Amount 
	
	FROM 
		Contacts C 
		JOIN cr_Medical_Information MI ON MI.Contact_ID = C.Contact_ID -- what if there is more than one?
		LEFT JOIN cr_Medical_Information_Medications MIM ON MIM.MedicalInformation_ID = MI.MedicalInformation_ID

	WHERE C.Domain_ID = @Domain_ID 
		AND C.Participant_Record = ISNULL(@ParticipantID,C.Participant_Record)
		AND EXISTS (SELECT 1 FROM Event_Participants EP 
			WHERE EP.Event_ID = @EventID AND EP.Participant_ID = C.Participant_Record
			AND EP.Participation_Status_ID = 2)

	ORDER BY Display_Name

END


GO


