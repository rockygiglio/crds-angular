USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================================
-- Author:		Phil Lachmann
-- Create date: 3/22/2016
-- Description:	SP to get all Pin data to load into AWS cloudsearch
-- ===================================================================================
IF NOT EXISTS
 (SELECT * FROM sys.objects WHERE object_id = object_id(N'[dbo].[api_crds_Get_Connect_AWS_Data]')
    AND TYPE IN (N'P', N'PC')) BEGIN EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[api_crds_Get_Connect_AWS_Data] AS';

END 
GO

ALTER PROCEDURE [dbo].[api_crds_Get_Connect_AWS_Data] 
AS 

BEGIN
SET nocount ON;

DECLARE @anywhereGroupTypeId INTEGER = 30; 
DECLARE @participantPinType INTEGER = 1;
DECLARE @groupPinType       INTEGER = 2;
DECLARE @sitePinType        INTEGER = 3

-- PARTICIPANTS
SELECT
    C.Nickname AS firstName,
	C.Last_Name AS lastname,
	null AS siteName,
	C.Email_Address AS emailAddress,
	C.Contact_ID AS contactId,
	P.Participant_ID AS participantId,
	A.Address_ID AS addressId,
	A.City AS city,
	A.[State/Region] AS state,
	A.Postal_Code AS zip,
	A.Latitude AS latitude,
	A.Longitude AS longitude,
	P.Host_Status_ID AS hostStatus,
	null AS groupId,
	null AS groupName,
	null AS groupDescription,
	null AS groupTypeName,
	null AS ministryId,
	null AS congregationId,
	null AS congregationName,
	null AS primarycontactId,
	null AS primaryContactEmail,
	null AS startDate,
	null AS endDate,
	null AS availableOnline,
	null AS groupFullInd,
	null AS childcareInd,
	null AS meetingDayId,
	null AS meetingTime,
	null AS meetingFrequencyId,
	null AS targetSize,
	null AS kidsWelcome,
	null AS participantList,
	null AS groupTypeId,
	H.Household_ID AS householdId,
	@participantPinType AS pinType
FROM Participants P 
JOIN Contacts C ON C.Contact_ID = P.Contact_ID
LEFT JOIN Households H ON H.Household_ID = C.Household_ID
LEFT JOIN Addresses A ON A.Address_ID = H.Address_ID
WHERE P.Show_On_Map = 1
UNION
--GATHERINGS
SELECT
    null AS firstName,
	null AS lastname,
	null As siteName,
	null AS emailAddress,
	null AS contactId,
	null AS participantId,
	G.Offsite_Meeting_Address AS addressId,
	A.City AS city,
	A.[State/Region] AS state,
	A.Postal_Code AS zip,
	A.Latitude AS latitude,
	A.Longitude AS longitude,
	null AS hostStatus,
	G.Group_ID AS groupId,
	G.Group_Name AS groupName,
	G.Description AS groupDescription,
	'Anywhere Gathering' AS groupTypeName,
	G.Ministry_ID AS ministryId,
	G.Congregation_ID AS congregationId,
	null AS congregationName,
	G.Primary_Contact as primarycontactId,
	null AS primaryContactEmail,
	G.Start_Date AS startDate,
	G.End_Date AS endDate,
	G.Available_Online AS availableOnline,
	G.Group_Is_Full AS groupFullInd,
	G.Child_Care_Available AS childcareInd,
	G.Meeting_Day_ID AS meetingDayId,
	G.Meeting_Time AS meetingTime,
	G.Meeting_Frequency_ID AS meetingFrequencyId,
	G.Target_Size AS targetSize,
	G.Kids_Welcome AS kidsWelcome,
	(SELECT STUFF((SELECT ',' + CAST(t1.Participant_ID AS varchar(16)) FROM Group_Participants t1 WHERE t1.Group_ID = t2.Group_ID FOR XML PATH ('')) , 1, 1, '') FROM Groups t2 WHERE t2.Group_ID = G.Group_ID GROUP BY t2.Group_ID) AS participantList, -- stuff list of active participant ids here
	G.Group_Type_ID AS groupTypeId,
	null AS householdId,
	@groupPinType AS pinType
FROM Groups G
LEFT JOIN Addresses A ON A.Address_ID = G.Offsite_Meeting_Address
WHERE G.Group_Type_ID = @anywhereGroupTypeId
UNION
--SITES
SELECT
    null AS firstName,
	null AS lastname,
	L.Location_Name AS siteName,
	null AS emailAddress,
	null AS contactId,
	null AS participantId,
	A.Address_ID AS addressId,
	A.City AS city,
	A.[State/Region] AS state,
	A.Postal_Code AS zip,
	A.Latitude AS latitude,
	A.Longitude AS longitude,
	null AS hostStatus,
	null AS groupId,
	null AS groupName,
	null AS groupDescription,
	null AS groupTypeName,
	null AS ministryId,
	null AS congregationId,
	null AS congregationName,
	null AS primarycontactId,
	null AS primaryContactEmail,
	null AS startDate,
	null AS endDate,
	null AS availableOnline,
	null AS groupFullInd,
	null AS childcareInd,
	null AS meetingDayId,
	null AS meetingTime,
	null AS meetingFrequencyId,
	null AS targetSize,
	null AS kidsWelcome,
	null AS participantList, 
	null AS groupTypeId,
	null AS householdId,
	@sitePinType AS pinType
FROM Congregations C
JOIN Locations L ON L.Location_ID = C.Location_ID
LEFT JOIN Addresses A ON A.Address_ID = L.Address_ID
WHERE C.Congregation_ID NOT IN (2,5,15) AND C.End_Date IS NULL

END
GO
