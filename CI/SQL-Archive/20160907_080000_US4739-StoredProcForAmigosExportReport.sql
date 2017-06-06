USE MinistryPlatform
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[report_CRDS_Amigos_Export]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[report_CRDS_Amigos_Export] AS' 
END
GO

ALTER procedure [dbo].[report_CRDS_Amigos_Export]
	@pledgecampaignid INT

AS
BEGIN

DECLARE @emergencycontactfirstname INT = 1439;
DECLARE @emergencycontactlastname INT = 1440;
DECLARE @emergencycontactrelationship INT = 1272;  
DECLARE @emergencycontactprimaryphone INT = 1442;
DECLARE @emergencycontactemailaddress INT = 1441;
DECLARE @dietaryrestrictions INT = 65;
DECLARE @allergies INT = 67;

DECLARE @medicationrestrictions INT = 9001; 
DECLARE @medicationstaking INT = 9000; 
DECLARE @medicalconditions INT = 1432;

SELECT 
	cmi.email_address
	,cmi.First_Name
	,cmi.Middle_Name
	,cmi.Last_Name
	,cmi.Nickname
	,cmi.Address_Line_1
	,cmi.City
	,cmi.[State/Region]
	,cmi.Postal_Code
	,cmi.Home_Phone
	,cmi.Mobile_Phone
	,cmi.Date_of_Birth
	,cmi.Gender
	,(SELECT TOP 1 response
	   FROM [ministryplatform].[dbo].[form_response_answers] r
	   JOIN (SELECT fra.Form_Response_Answer_ID AS max_id 
				FROM dbo.Form_Response_Answers fra )m
				ON m.max_id = r.Form_Response_Answer_ID
		WHERE r.form_field_id = @emergencycontactfirstname
		AND r.form_response_id = fr.form_response_id
		ORDER BY r.Form_Response_Answer_ID DESC) AS emergencycontactfirstname

	,(SELECT TOP 1 response
	   FROM [ministryplatform].[dbo].[form_response_answers] r
	   JOIN (SELECT fra.Form_Response_Answer_ID AS max_id 
				FROM dbo.Form_Response_Answers fra )m
				ON m.max_id = r.Form_Response_Answer_ID
		WHERE r.form_field_id = @emergencycontactlastname
		AND r.form_response_id = fr.form_response_id
		ORDER BY r.Form_Response_Answer_ID DESC) AS emergencycontactlastname

	,(SELECT TOP 1 response
	   FROM [ministryplatform].[dbo].[form_response_answers] r
	   JOIN (SELECT fra.Form_Response_Answer_ID as max_id 
				FROM dbo.Form_Response_Answers fra )m
				ON m.max_id = r.Form_Response_Answer_ID
	   WHERE r.form_field_id = @emergencycontactrelationship
		 AND r.form_response_id = fr.Form_Response_ID
		 ORDER BY r.Form_Response_Answer_ID DESC) AS emergencycontactrelationship

	,(SELECT TOP 1 response
	   FROM [ministryplatform].[dbo].[form_response_answers] r
	   JOIN (SELECT fra.Form_Response_Answer_ID as max_id 
				FROM dbo.Form_Response_Answers fra )m
				ON m.max_id = r.Form_Response_Answer_ID
	   WHERE r.form_field_id = @emergencycontactprimaryphone
		 AND r.form_response_id = fr.Form_Response_ID
		 ORDER BY r.Form_Response_Answer_ID DESC) AS emergencycontactprimaryphone

	,(SELECT TOP 1  response
		FROM [ministryplatform].[dbo].[form_response_answers] r
		JOIN (SELECT fra.Form_Response_Answer_ID AS max_id 
				FROM dbo.Form_Response_Answers fra )m
				ON m.max_id = r.Form_Response_Answer_ID
		WHERE r.form_field_id = @emergencycontactemailaddress
		AND r.form_response_id = fr.form_response_id
		ORDER BY r.Form_Response_Answer_ID DESC) AS emergencycontactemailaddress

	,(SELECT STUFF(
            (SELECT '|' + attribute_name
            FROM dbo.vw_crds_contact_attributes AS ca
            WHERE ca.contact_id = c.contact_id
                AND ca.attribute_type_id = @dietaryrestrictions
                FOR xml path(''), TYPE).value('.', 'nvarchar(max)'), 1, 1, '')) AS Dietary_Restrictions

	,(SELECT TOP 1 notes
	   FROM vw_crds_contact_attributes
	   WHERE contact_id = c.contact_id
		 AND attribute_type_id = @allergies 
		ORDER BY Start_Date DESC) AS allergies

	,(SELECT TOP 1 notes
	   FROM vw_crds_contact_attributes
	   WHERE contact_id = c.contact_id
		 AND attribute_type_id = @medicationrestrictions 
		ORDER BY Start_Date DESC) AS medicationrestrictions

	,(SELECT TOP 1 notes
	   FROM vw_crds_contact_attributes
	   WHERE contact_id = c.contact_id
		 AND attribute_type_id = @medicationstaking 
		ORDER BY Start_Date DESC) AS medicationstaking

	,(SELECT top 1 response
	   FROM [ministryplatform].[dbo].[form_response_answers] r
	   join (SELECT fra.Form_Response_Answer_ID AS max_id 
				FROM dbo.Form_Response_Answers fra )m
				ON m.max_id = r.Form_Response_Answer_ID
		WHERE r.form_field_id = @medicalconditions
		AND r.form_response_id = fr.form_response_id
		ORDER BY r.Form_Response_Answer_ID DESC) AS medical_conditions

FROM pledges p
JOIN contacts c ON c.Donor_Record = p.Donor_ID
JOIN Pledge_Campaigns pc ON pc.Pledge_Campaign_ID=p.pledge_campaign_id
JOIN mp_vw_Contact_Mail_Info cmi ON c.contact_id = cmi.contact_id
JOIN dbo.form_responses fr ON c.contact_id = fr.contact_id
                                 AND pc.pledge_campaign_id = fr.pledge_campaign_id
WHERE 
p.pledge_status_id=1
AND pc.Pledge_Campaign_ID = @pledgecampaignid

END

GO
