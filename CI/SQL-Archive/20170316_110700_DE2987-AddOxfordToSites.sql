USE [MinistryPlatform]
GO
/****** Object:  StoredProcedure [dbo].[api_crds_get_Pins_Within_Range]    Script Date: 3/16/2017 10:46:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[api_crds_get_Pins_Within_Range] @Latitude nvarchar(15), @Longitude nvarchar(15), @RadiusInKilometers int
AS

DECLARE @CenterLatFloat float(24);
DECLARE @CenterLngFloat float(24);
SET @CenterLatFloat = CAST(@Latitude AS float);
SET @CenterLngFloat = CAST(@Longitude AS float);

SELECT C.[First_Name], C.[Last_Name], C.[Email_Address], P.[Contact_ID], P.[Participant_ID],
       P.[Host_Status_ID], NULL as Gathering, C.[Household_ID], NULL as Site_Name,
	   A.[Address_Line_1], A.[Address_Line_2], A.[City], A.[State/Region] AS State, A.[Postal_Code], 
	   A.[Latitude], A.[Longitude], 1 AS Pin_Type
FROM [Participants] AS P
LEFT JOIN [Contacts] AS C on P.[Contact_ID] = C.[Contact_ID]
LEFT JOIN [Households] AS H on C.[Household_ID] = H. [Household_ID]
LEFT JOIN [Addresses] AS A on H.[Address_ID] = A.[Address_ID]
WHERE
P.SHOW_ON_MAP = 1 AND
ACOS( SIN( RADIANS( CAST(A.[Latitude] AS float) ) ) * SIN( RADIANS( @CenterLatFloat ) ) + COS( RADIANS( CAST(A.[Latitude] AS float) ) )
* COS( RADIANS( @CenterLatFloat  )) * COS( RADIANS( CAST(A.[Longitude] AS float) ) - RADIANS( @CenterLngFloat )) ) * 6380 < @RadiusInKilometers
UNION 
SELECT NULL as First_Name, NULL as Last_Name, NULL as Email_Address, NULL AS Contact_ID, NULL AS Participant_ID,
    NULL AS Host_Status_ID, NULL as Gathering, NULL AS Household_ID, L.[Location_Name],
	A.[Address_Line_1], A.[Address_Line_2], A.[City], A.[State/Region] AS State, A.[Postal_Code], 
	A.[Latitude], A.[Longitude], 3 AS Pin_Type
FROM [Congregations] AS C, [Locations] AS L
LEFT JOIN [Addresses] AS A on L.[Address_ID] = A.[Address_ID]
WHERE L.[Location_ID] = C.[Location_ID]
  AND C.Available_Online = 1
  AND C.Congregation_ID not in (2, 15)  -- 'I do not attend Crossroads' and Anywhere
