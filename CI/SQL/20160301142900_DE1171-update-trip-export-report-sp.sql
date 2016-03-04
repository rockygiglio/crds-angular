USE [ministryplatform] 
GO 
/****** Object:  StoredProcedure [dbo].[report_CRDS_Event_Selected_Campaign_Donations]    Script Date: 10/1/2015 4:18:16 PM ******/
SET ansi_nulls ON
GO

SET quoted_identifier ON 
GO
 
IF NOT EXISTS
  (SELECT *
   FROM sys.objects
   WHERE object_id = object_id(N'[dbo].[report_CRDS_Trip_Export]')
     AND TYPE IN (N'P',
                   N'PC')) BEGIN EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[report_CRDS_Trip_Export] AS';

END 
GO

ALTER PROCEDURE [dbo].[report_crds_trip_export] 
  @eventid INT 
AS 
BEGIN
SET nocount ON;

DECLARE @missiontripeventtype INT = 6;

DECLARE @pledgediscontinued INT = 3;

-- MAPPING VALUES
DECLARE @validpassport INT = 1464;

DECLARE @tshirtsize INT = 21;

DECLARE @scrubsizetop INT = 22;

DECLARE @scrubsizebottom INT = 23;

DECLARE @dietaryrestrictions INT = 65;

DECLARE @allergies INT = 67;

DECLARE @sprituallife INT = 60;

DECLARE @previoustripexperience INT = 62;

DECLARE @profession INT = 61;

DECLARE @frequentflyerdelta INT = 3958;

DECLARE @frequentflyersouthafrica INT = 3959;

DECLARE @frequentflyerunited INT = 3960;

DECLARE @frequentflyerusair INT = 3980;

DECLARE @internationaltravelexperience INT = 66;

DECLARE @experienceservingabroad INT = 68;

DECLARE @abusevictim INT = 69;

DECLARE @why INT = 1434;

DECLARE @emergencycontactfirstname INT = 1439;

DECLARE @emergencycontactlastname INT = 1440;

DECLARE @emergencycontactprimaryphone INT = 1442;

DECLARE @emergencycontactsecondaryphone INT = 1443;

DECLARE @emergencycontactemailaddress INT = 1441;

DECLARE @lotterypreference INT = 1444;

DECLARE @commonname INT = 1445;

DECLARE @requestedroommate1 INT = 1446;

DECLARE @requestedroommate2 INT = 1447;

DECLARE @supportpersonemail INT = 1448;

DECLARE @gogroupleaderinterest INT = 1449;

DECLARE @tripguardianfirstname INT = 1426;

DECLARE @tripguardianlastname INT = 1427;

DECLARE @howdidyouhearabouttrip INT = 1433;

DECLARE @medicalconditions INT = 1432;

DECLARE @workteampreference1 INT = 1423;

DECLARE @workteampreference2 INT = 1425;

DECLARE @workteamexperience INT = 1424;

DECLARE @stewardsofchildren INT = 9;

DECLARE @visa INT = 11;

DECLARE @diploma INT = 1;

DECLARE @medicallicense INT = 2;

DECLARE @letterverification INT = 3;

DECLARE @goodfaithletter INT = 4;

DECLARE @goodstandingletter INT = 5;

DECLARE @form10 INT = 6;

DECLARE @medicalapplication INT = 7;

DECLARE @orchardafricawaiver INT = 8;

DECLARE @copyofpassport INT = 10;

DECLARE @ipromise INT = 12;

DECLARE @waiver INT = 13;

DECLARE @proofofguardianship INT = 16;

DECLARE @countrydocumentation INT = 17;


SELECT c.first_name,
       c.middle_name,
       c.last_name,
       c.maiden_name,
       c.nickname,
       c.email_address,
       c.date_of_birth,
       ms.marital_status,
       g.gender,
       c.employer_name,
       p.attendance_start_date AS first_attendance_ever,
       c.mobile_phone,
       a.address_line_1,
       a.address_line_2,
       a.city,
       a.[state/region],
       a.postal_code,
       a.county,
       a.foreign_country,
       h.home_phone,
       cn.congregation_name,

  (SELECT response
   FROM [ministryplatform].[dbo].[form_response_answers] r
   WHERE r.form_field_id = @validpassport
     AND r.form_response_id = fr.form_response_id) AS validpassport,

       c.passport_firstname,
       c.passport_middlename,
       c.passport_lastname,
       c.passport_number,
       c.passport_country,
       c.passport_expiration,

  (SELECT response
   FROM [ministryplatform].[dbo].[form_response_answers] r
   WHERE r.form_field_id = @tripguardianfirstname
     AND r.form_response_id = fr.form_response_id) AS tripguardianfirstname,

  (SELECT response
   FROM [ministryplatform].[dbo].[form_response_answers] r
   WHERE r.form_field_id = @tripguardianlastname
     AND r.form_response_id = fr.form_response_id) AS tripguardianlastname,

  (SELECT attribute_name
   FROM vw_crds_contact_attributes
   WHERE contact_id = c.contact_id
     AND attribute_type_id = @tshirtsize) AS tshirtsize,

  (SELECT attribute_name
   FROM vw_crds_contact_attributes
   WHERE contact_id = c.contact_id
     AND attribute_type_id = @scrubsizetop) AS scrubsizetop,

  (SELECT attribute_name
   FROM vw_crds_contact_attributes
   WHERE contact_id = c.contact_id
     AND attribute_type_id = @scrubsizebottom) AS scrubsizebottom,

  (SELECT STUFF(
                  (SELECT '|' + attribute_name
                   FROM dbo.vw_crds_contact_attributes AS ca
                   WHERE ca.contact_id = c.contact_id
                     AND ca.attribute_type_id = @dietaryrestrictions
                     FOR xml path(''), TYPE).value('.', 'nvarchar(max)'), 1, 1, '')) AS dietary_restrictions,

  (SELECT notes
   FROM vw_crds_contact_attributes
   WHERE contact_id = c.contact_id
     AND attribute_type_id = @allergies) AS allergies,

  (SELECT STUFF(
                  (SELECT '|' + attribute_name
                   FROM dbo.vw_crds_contact_attributes AS ca
                   WHERE ca.contact_id = c.contact_id
                     AND ca.attribute_type_id = @sprituallife
                     FOR xml path(''), TYPE).value('.', 'nvarchar(max)'), 1, 1, '')) AS spritual_life,

  (SELECT response
   FROM [ministryplatform].[dbo].[form_response_answers] r
   WHERE r.form_field_id = @why
     AND r.form_response_id = fr.form_response_id) AS whygoontrip,

  (SELECT response
   FROM [ministryplatform].[dbo].[form_response_answers] r
   WHERE r.form_field_id = @emergencycontactfirstname
     AND r.form_response_id = fr.form_response_id) AS emergencycontactfirstname,

  (SELECT response
   FROM [ministryplatform].[dbo].[form_response_answers] r
   WHERE r.form_field_id = @emergencycontactlastname
     AND r.form_response_id = fr.form_response_id) AS emergencycontactlastname,

  (SELECT response
   FROM [ministryplatform].[dbo].[form_response_answers] r
   WHERE r.form_field_id = @emergencycontactprimaryphone
     AND r.form_response_id = fr.form_response_id) AS emergencycontactprimaryphone,

  (SELECT response
   FROM [ministryplatform].[dbo].[form_response_answers] r
   WHERE r.form_field_id = @emergencycontactsecondaryphone
     AND r.form_response_id = fr.form_response_id) AS emergencycontactsecondaryphone,

  (SELECT response
   FROM [ministryplatform].[dbo].[form_response_answers] r
   WHERE r.form_field_id = @emergencycontactemailaddress
     AND r.form_response_id = fr.form_response_id) AS emergencycontactemailaddress,

  (SELECT response
   FROM [ministryplatform].[dbo].[form_response_answers] r
   WHERE r.form_field_id = @lotterypreference
     AND r.form_response_id = fr.form_response_id) AS lotterypreference,

  (SELECT response
   FROM [ministryplatform].[dbo].[form_response_answers] r
   WHERE r.form_field_id = @commonname
     AND r.form_response_id = fr.form_response_id) AS commonname,

  (SELECT response
   FROM [ministryplatform].[dbo].[form_response_answers] r
   WHERE r.form_field_id = @requestedroommate1
     AND r.form_response_id = fr.form_response_id) AS requestedroommate1,

  (SELECT response
   FROM [ministryplatform].[dbo].[form_response_answers] r
   WHERE r.form_field_id = @requestedroommate2
     AND r.form_response_id = fr.form_response_id) AS requestedroommate2,

  (SELECT response
   FROM [ministryplatform].[dbo].[form_response_answers] r
   WHERE r.form_field_id = @supportpersonemail
     AND r.form_response_id = fr.form_response_id) AS supportpersonemail,

  (SELECT response
   FROM [ministryplatform].[dbo].[form_response_answers] r
   WHERE r.form_field_id = @gogroupleaderinterest
     AND r.form_response_id = fr.form_response_id) AS gogroupleaderinterest,

       sponsoredchild.sponsoredchild,
       sponsoredchild.sponsoredchildfirstname,
       sponsoredchild.sponsoredchildlastname,
       sponsoredchild.sponsoredchildidnumber,

  (SELECT notes
   FROM vw_crds_contact_attributes
   WHERE contact_id = c.contact_id
     AND attribute_type_id = @previoustripexperience) AS previous_trip_experience,

  (SELECT STUFF(
                  (SELECT '|' + attribute_name
                   FROM dbo.vw_crds_contact_attributes AS ca
                   WHERE ca.contact_id = c.contact_id
                     AND ca.attribute_type_id = @profession
                     FOR xml path(''), TYPE).value('.', 'nvarchar(max)'), 1, 1, '')) AS profession,

  (SELECT notes
   FROM vw_crds_contact_attributes
   WHERE contact_id = c.contact_id
     AND attribute_id = @frequentflyerdelta) AS ff_delta,

  (SELECT notes
   FROM vw_crds_contact_attributes
   WHERE contact_id = c.contact_id
     AND attribute_id = @frequentflyersouthafrica) AS ff_south_african_airlines,

  (SELECT notes
   FROM vw_crds_contact_attributes
   WHERE contact_id = c.contact_id
     AND attribute_id = @frequentflyerunited) AS ff_united,

  (SELECT notes
   FROM vw_crds_contact_attributes
   WHERE contact_id = c.contact_id
     AND attribute_id = @frequentflyerusair) AS ff_usair,

  (SELECT attribute_name
   FROM vw_crds_contact_attributes
   WHERE contact_id = c.contact_id
     AND attribute_type_id = @internationaltravelexperience) AS international_travel_experience,

  (SELECT notes
   FROM vw_crds_contact_attributes
   WHERE contact_id = c.contact_id
     AND attribute_type_id = @experienceservingabroad) AS experience_serving_abroad,
       (convert(varchar(10), fr.response_date, 101)) signupdate,
       spouse.spouseontrip,
       spouse.spousename,
       dep.deposit,
       dep.payment_type,
       dep.donation_date,
       dep.pledge_status,
       dep.amount depositamount,

  (SELECT attribute_name
   FROM vw_crds_contact_attributes
   WHERE contact_id = c.contact_id
     AND attribute_type_id = @abusevictim) AS abuse_victim,

  (SELECT(CASE d.received WHEN 0 THEN 'No' WHEN 1 THEN 'Yes' END) AS docreceived
   FROM dbo.cr_eventparticipant_documents d
   WHERE d.event_participant_id = ep.event_participant_id
     AND d.document_id = @stewardsofchildren) AS stewardsofchildren,

  (SELECT(CASE d.received WHEN 0 THEN 'No' WHEN 1 THEN 'Yes' END) AS docreceived
   FROM dbo.cr_eventparticipant_documents d
   WHERE d.event_participant_id = ep.event_participant_id
     AND d.document_id = @visa) AS visa,

  (SELECT(CASE d.received WHEN 0 THEN 'No' WHEN 1 THEN 'Yes' END) AS docreceived
   FROM dbo.cr_eventparticipant_documents d
   WHERE d.event_participant_id = ep.event_participant_id
     AND d.document_id = @diploma) AS diploma,

  (SELECT(CASE d.received WHEN 0 THEN 'No' WHEN 1 THEN 'Yes' END) AS docreceived
   FROM dbo.cr_eventparticipant_documents d
   WHERE d.event_participant_id = ep.event_participant_id
     AND d.document_id = @medicallicense) AS medicallicense,

  (SELECT(CASE d.received WHEN 0 THEN 'No' WHEN 1 THEN 'Yes' END) AS docreceived
   FROM dbo.cr_eventparticipant_documents d
   WHERE d.event_participant_id = ep.event_participant_id
     AND d.document_id = @letterverification) AS letterverification,

  (SELECT(CASE d.received WHEN 0 THEN 'No' WHEN 1 THEN 'Yes' END) AS docreceived
   FROM dbo.cr_eventparticipant_documents d
   WHERE d.event_participant_id = ep.event_participant_id
     AND d.document_id = @goodfaithletter) AS goodfaithletter,

  (SELECT(CASE d.received WHEN 0 THEN 'No' WHEN 1 THEN 'Yes' END) AS docreceived
   FROM dbo.cr_eventparticipant_documents d
   WHERE d.event_participant_id = ep.event_participant_id
     AND d.document_id = @goodstandingletter) AS goodstandingletter,

  (SELECT(CASE d.received WHEN 0 THEN 'No' WHEN 1 THEN 'Yes' END) AS docreceived
   FROM dbo.cr_eventparticipant_documents d
   WHERE d.event_participant_id = ep.event_participant_id
     AND d.document_id = @form10) AS form10,

  (SELECT(CASE d.received WHEN 0 THEN 'No' WHEN 1 THEN 'Yes' END) AS docreceived
   FROM dbo.cr_eventparticipant_documents d
   WHERE d.event_participant_id = ep.event_participant_id
     AND d.document_id = @medicalapplication) AS medicalapplication,

  (SELECT(CASE d.received WHEN 0 THEN 'No' WHEN 1 THEN 'Yes' END) AS docreceived
   FROM dbo.cr_eventparticipant_documents d
   WHERE d.event_participant_id = ep.event_participant_id
     AND d.document_id = @orchardafricawaiver) AS orchardafricawaiver,

  (SELECT(CASE d.received WHEN 0 THEN 'No' WHEN 1 THEN 'Yes' END) AS docreceived
   FROM dbo.cr_eventparticipant_documents d
   WHERE d.event_participant_id = ep.event_participant_id
     AND d.document_id = @copyofpassport) AS copyofpassport,

  (SELECT(CASE d.received WHEN 0 THEN 'No' WHEN 1 THEN 'Yes' END) AS docreceived
   FROM dbo.cr_eventparticipant_documents d
   WHERE d.event_participant_id = ep.event_participant_id
     AND d.document_id = @ipromise) AS ipromise,

  (SELECT(CASE d.received WHEN 0 THEN 'No' WHEN 1 THEN 'Yes' END) AS docreceived
   FROM dbo.cr_eventparticipant_documents d
   WHERE d.event_participant_id = ep.event_participant_id
     AND d.document_id = @waiver) AS waiver,

  (SELECT(CASE d.received WHEN 0 THEN 'No' WHEN 1 THEN 'Yes' END) AS docreceived
   FROM dbo.cr_eventparticipant_documents d
   WHERE d.event_participant_id = ep.event_participant_id
     AND d.document_id = @proofofguardianship) AS proofofguardianship,

  (SELECT(CASE d.received WHEN 0 THEN 'No' WHEN 1 THEN 'Yes' END) AS docreceived
   FROM dbo.cr_eventparticipant_documents d
   WHERE d.event_participant_id = ep.event_participant_id
     AND d.document_id = @countrydocumentation) AS countrydocumentation,

  (SELECT stuff(
                  (SELECT '|' + e.event_title
                   FROM ministryplatform.dbo.event_participants trip
                   INNER JOIN dbo.events e ON trip.event_id = e.event_id
                   AND e.event_type_id = @missiontripeventtype
                   AND e.event_id != @eventid
                   WHERE trip.participant_id = ep.participant_id
                     FOR xml path(''), TYPE).value('.', 'nvarchar(max)'), 1, 1, '')) AS previoustrips,

  (SELECT response
   FROM [ministryplatform].[dbo].[form_response_answers] r
   WHERE r.form_field_id = @workteampreference1
     AND r.form_response_id = fr.form_response_id) AS workteampreference1,

  (SELECT response
   FROM [ministryplatform].[dbo].[form_response_answers] r
   WHERE r.form_field_id = @workteampreference2
     AND r.form_response_id = fr.form_response_id) AS workteampreference2,

  (SELECT response
   FROM [ministryplatform].[dbo].[form_response_answers] r
   WHERE r.form_field_id = @workteamexperience
     AND r.form_response_id = fr.form_response_id) AS workteamexperience
     
FROM [ministryplatform].[dbo].[event_participants] ep
INNER JOIN dbo.participants p ON ep.participant_id = p.participant_id
INNER JOIN dbo.pledge_campaigns pc ON ep.event_id = pc.event_id
INNER JOIN dbo.form_responses fr ON p.contact_id = fr.contact_id
                                 AND pc.pledge_campaign_id = fr.pledge_campaign_id
INNER JOIN dbo.contacts c ON p.contact_id = c.contact_id
LEFT JOIN dbo.pledges pg ON pc.pledge_campaign_id = pg.pledge_campaign_id
                         AND c.donor_record = pg.donor_id
                         AND pg.pledge_status_id != @pledgediscontinued
LEFT JOIN households h ON c.household_id = h.household_id
LEFT JOIN addresses a ON h.address_id = a.address_id
LEFT JOIN congregations cn ON h.congregation_id = cn.congregation_id
LEFT JOIN genders g ON c.gender_id = g.gender_id
LEFT JOIN marital_statuses ms ON c.marital_status_id = ms.marital_status_id 
CROSS APPLY dbo.crds_tripdeposit(c.contact_id, ep.event_id, pc.pledge_campaign_id) dep 
CROSS APPLY dbo.crds_spouseontrip(c.contact_id, ep.event_id) spouse 
CROSS APPLY dbo.crds_sponsoredchild(c.contact_id) sponsoredchild
WHERE ep.event_id = @eventid;

END;
