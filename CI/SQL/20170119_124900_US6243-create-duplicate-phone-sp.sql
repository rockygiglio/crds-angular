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

    DECLARE @Minor_Child int = 2

    CREATE TABLE #phones (
		ID int identity
		, Household_ID int
		, Congregation_ID int
		, Household_Name nvarchar(75)
		, Contact_ID int
		, Display_Name nvarchar(75) NULL
		, Home_Phone nvarchar(25) NULL
		, Mobile_Phone nvarchar(25) NULL
		, PRIMARY KEY (ID)
	);

	INSERT INTO #phones
		SELECT DISTINCT h.Household_ID
			, h.Congregation_ID
			, h.Household_Name
			, c.Contact_ID
			, c.Display_Name
			, replace(replace(replace(replace(h.Home_Phone, '-', ''), ')', ''), '(', ''), ' ', '')
			, replace(replace(replace(replace(c.Mobile_Phone, '-', ''), ')', ''), '(', ''), ' ', '')
		FROM dbo.Households h
			JOIN Contacts c ON (c.Household_ID = h.Household_ID)
		WHERE EXISTS
		(
			SELECT TOP 1 *
			FROM Contacts c2
			WHERE c2.Household_ID = h.Household_ID
				AND c2.Household_Position_ID = @Minor_Child
		);

	SELECT TOP 500 main.Household_ID
		, main.Household_Name
		, main.Contact_ID
		, main.Display_Name
		, main.Home_Phone
		, main.Mobile_Phone
		, dup.Household_ID AS dup_Household_ID
		, dup.Household_Name AS dup_Household_Name
		, dup.Contact_ID AS dup_Contact_ID
		, dup.Display_Name AS dup_Display_Name
		, dup.Home_Phone AS dup_Home_Phone
		, dup.Mobile_Phone AS dup_Mobile_Phone
	FROM #phones main
		inner join #phones dup ON
		(
			main.Household_ID <> dup.Household_ID
			AND main.Home_Phone = dup.Home_Phone
		)
		OR
		(
			main.Household_ID <> dup.Household_ID
			AND main.Contact_ID <> dup.Contact_ID
			AND main.Mobile_Phone IS NOT NULL
			AND main.Mobile_Phone = dup.Mobile_Phone
		)
	WHERE ISNULL(@Congregation_ID, 0) = 0
		OR @Congregation_ID = main.Congregation_ID;

	DROP TABLE #phones;
END
GO
