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
	null AS primarycontactId,
	null AS primaryContactEmail,
	null AS participantCount,
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
	G.Primary_Contact as primarycontactId,
	null AS primaryContactEmail,
	123 as participantCount,
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
	null AS primarycontactId,
	null AS primaryContactEmail,
	null AS participantCount, 
	null AS groupTypeId,
	null AS householdId,
	@sitePinType AS pinType
FROM Congregations C
JOIN Locations L ON L.Location_ID = C.Location_ID
LEFT JOIN Addresses A ON A.Address_ID = L.Address_ID
WHERE C.Congregation_ID NOT IN (2,5,15) AND C.End_Date IS NULL

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_API_Procedures] WHERE [Procedure_Name] = N'api_crds_Get_Connect_AWS_Data')
BEGIN
	INSERT INTO [dbo].[dp_API_Procedures] (
		 Procedure_Name
		,Description
	) VALUES (
		 N'api_crds_Get_Connect_AWS_Data'
		,N'Get all connect records to upload to AWS Cloudsearch'
	)
END
GO

DECLARE @API_ROLE_ID int = 62;
DECLARE @API_ID int;

SELECT @API_ID = API_Procedure_ID FROM [dbo].[dp_API_Procedures] WHERE [Procedure_Name] = N'api_crds_Get_Connect_AWS_Data';

IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_Role_API_Procedures] WHERE [Role_ID] = @API_ROLE_ID AND [API_Procedure_ID] = @API_ID)
BEGIN
	INSERT INTO [dbo].[dp_Role_API_Procedures] (
		 [Role_ID]
		,[API_Procedure_ID]
		,[Domain_ID]
	) VALUES (
		 @API_ROLE_ID
		,@API_ID
		,1
	)
END
GO
