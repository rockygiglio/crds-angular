USE MinistryPlatform
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[api_crds_get_Pins_Within_Range]') AND type in (N'P', N'PC'))

BEGIN
    
	EXEC('CREATE PROCEDURE [dbo].[api_crds_get_Pins_Within_Range] @Latitude nvarchar(15), @Longitude nvarchar(15), @RadiusInKilometers int
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
ACOS( SIN( RADIANS( CAST(A.[Latitude] AS float) ) ) * SIN( RADIANS( @CenterLatFloat ) ) + COS( RADIANS( CAST(A.[Latitude] AS float) ) )
* COS( RADIANS( @CenterLatFloat  )) * COS( RADIANS( CAST(A.[Longitude] AS float) ) - RADIANS( @CenterLngFloat )) ) * 6380 < @RadiusInKilometers
UNION 
SELECT NULL as First_Name, NULL as Last_Name, NULL as Email_Address, NULL AS Contact_ID, NULL AS Participant_ID,
    NULL AS Host_Status_ID, NULL as Gathering, NULL AS Household_ID, L.[Location_Name],
	A.[Address_Line_1], A.[Address_Line_2], A.[City], A.[State/Region] AS State, A.[Postal_Code], 
	A.[Latitude], A.[Longitude], 3 AS Pin_Type  
FROM [Locations] AS L
LEFT JOIN [Addresses] AS A on L.[Address_ID] = A.[Address_ID]
WHERE Location_Name != ''Not Site Specific'' AND Location_Type_ID = 1')

END


IF NOT EXISTS(SELECT * FROM [dbo].[dp_API_Procedures] WHERE [procedure_name] = 'api_crds_get_Pins_Within_Range')
BEGIN
	DECLARE @ID INT

	INSERT INTO [dbo].[dp_API_Procedures] ([procedure_name]) 
	VALUES('api_crds_get_Pins_Within_Range')

	SET @ID = SCOPE_IDENTITY();

	IF NOT EXISTS(select * from [dbo].[dp_Role_API_Procedures] WHERE API_Procedure_ID =@ID AND Role_ID = 62)
	BEGIN
		INSERT INTO [dbo].[dp_Role_API_Procedures]
		([Role_ID],
		[API_Procedure_ID],
		[Domain_ID])
		VALUES
		(62,
		@ID,
		1)
	END
END