USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jon Horner
-- Create date: 2017-01-19
-- Description:	Finds all of the duplicate Home and Mobile phone numbers
-- =============================================

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[report_CRDS_Duplicate_Phone_Numbers]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[report_CRDS_Duplicate_Phone_Numbers] AS' 
END
GO

ALTER PROCEDURE dbo.report_CRDS_Duplicate_Phone_Numbers
	@Congregation_ID int = NULL
AS
BEGIN
	SET NOCOUNT ON;

    CREATE TABLE #household_phones (
		Household_ID int
		, Congregation_ID int
		, Household_Name nvarchar(75)
		, Contact_ID int
		, Display_Name nvarchar(75)
		, Home_Phone nvarchar(25)
		, Mobile_Phone nvarchar(25)
		, PRIMARY KEY (Household_ID, Contact_ID)
	);

	INSERT INTO #household_phones
		SELECT DISTINCT h.Household_ID
			, h.Congregation_ID
			, h.Household_Name
			, ch.Contact_ID
			, c.Display_Name
			, replace(h.Home_Phone, '-', '')
			, replace(c.Mobile_Phone, '-', '')
		FROM dbo.Households h
			, dbo.Contact_Households ch
			, dbo.Contacts c
		WHERE h.Household_ID = ch.Household_ID
			AND ch.Contact_ID = c.Contact_ID;

	SELECT main.Household_ID
		, main.Household_Name
		, main.Contact_ID
		, main.Display_Name
		, main.Home_Phone
		, main.Mobile_Phone
		, dup.Household_ID AS Duplicate_Household_ID
		, dup.Household_Name AS Duplicate_Household_Name
		, dup.Contact_ID AS Duplicate_Contact_ID
		, dup.Display_Name AS Duplicate_Display_Name
		, dup.Home_Phone AS Duplicate_Home_Phone
		, dup.Mobile_Phone AS Duplicate_Mobile_Phone
	FROM #household_phones main
		inner join #household_phones dup ON
		(
			main.Household_ID <> dup.Household_ID
			AND main.Home_Phone = dup.Home_Phone
		)
		OR
		(
			main.Contact_ID <> dup.Contact_ID
			AND main.Mobile_Phone IS NOT NULL
			AND main.Mobile_Phone = dup.Mobile_Phone
		)
	WHERE ISNULL(@Congregation_ID, 0) = 0
		OR @Congregation_ID = main.Congregation_ID;

	DROP TABLE #household_phones;
END
GO
