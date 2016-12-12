USE [MinistryPlatform]
GO

/****** Object:  StoredProcedure [dbo].[api_crds_All_Family_Members]    Script Date: 12/6/2016 12:50:46 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[api_crds_All_Family_Members]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[api_crds_All_Family_Members] AS' 
END
GO

ALTER PROCEDURE [dbo].[api_crds_All_Family_Members] 
	@Contact_ID int	
AS
BEGIN
	SELECT c1.*, hp.Household_Position
	FROM [MinistryPlatform].[dbo].[Contacts] c
	JOIN [MinistryPlatform].[dbo].[Contacts] c1 on c.Household_ID = c1.Household_ID  
	JOIN Household_Positions hp on c1.Household_Position_ID = hp.Household_Position_ID
	WHERE c.[Contact_ID] = @Contact_ID

	UNION

	SELECT c.*,  hp.Household_Position 
	FROM Contact_Households ch
	JOIN Contacts c on c.Household_ID = ch.Household_ID
	JOIN Household_Positions hp on c.Household_Position_ID = hp.Household_Position_ID
	WHERE ch.Contact_ID = @Contact_ID	
END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_API_Procedures] WHERE [Procedure_Name] = N'api_crds_All_Family_Members')
BEGIN 
	INSERT INTO [dbo].[dp_API_Procedures] (
		 Procedure_Name
		,Description
	) VALUES (
		 N'api_crds_All_Family_Members'
		,N'Gets all Contacts in the same family as the passed in Contact ID'
	)
END

DECLARE @API_ROLE_ID int = 62;
DECLARE @API_ID int;

SELECT @API_ID = API_Procedure_ID FROM [dbo].[dp_API_Procedures] WHERE [Procedure_Name] = N'api_crds_All_Family_Members';

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