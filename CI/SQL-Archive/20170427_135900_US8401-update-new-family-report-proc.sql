USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[report_CRDS_Checkin_New_Families]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[report_CRDS_Checkin_New_Families] AS' 
END
GO
-- =============================================
-- Author:      John Cleaver
-- Create date: 2017-04-27
-- Description:	Alter report_CRDS_Checkin_New_Families to add address so new
-- families can get a sweet, sweet mailer
-- =============================================
ALTER PROCEDURE [dbo].report_CRDS_Checkin_New_Families 
	@StartDate DATETIME
  , @EndDate DATETIME
  , @EventCongregations NVARCHAR(1000)
AS
BEGIN
	SELECT 
			E.Event_Title AS "Event Title",
			E.Event_Start_Date,
			H.Household_Name AS "Household Last Name"
			, H.Home_Phone AS "Household Phone"
			, COUNT(EP.Participant_ID) AS "Total Household Children"
			, CONVERT(time, MIN(P.Participant_Start_Date)) AS "Time Created"
			, COUNT(DISTINCT H.Household_Name) AS "Total Households"
			,(SELECT CONCAT(
					s_A.Address_Line_1 + ' ',
					s_A.Address_Line_2 + ' ',
					s_A.City + ', ', 
					s_A.[State/Region] + ' ',
					s_A.Postal_Code + ' ',
					s_A.Foreign_Country)
			FROM  
					Addresses s_A 
			WHERE s_A.Address_ID = H.Address_ID)
			AS "Address"
	FROM
			Events E INNER JOIN Event_Participants EP ON E.Event_ID = EP.Event_ID
			INNER JOIN Participants P ON EP.Participant_ID = P.Participant_ID
			INNER JOIN Contacts C ON C.Contact_ID = P.Contact_ID
			INNER JOIN Households H ON H.Household_ID = C.Household_ID
	WHERE
			CONVERT(date, P.Participant_Start_Date) = CONVERT(DATE, E.Event_Start_Date)
			AND H.Household_Source_ID = 48 -- Kids Club Registration
			AND E.Congregation_ID IN (SELECT * FROM dbo.fnSplitString(@EventCongregations,','))
			AND CONVERT(date, E.Event_Start_Date) >= @StartDate
			AND CONVERT(date, E.Event_Start_Date) <= @EndDate
	GROUP BY
			E.Event_ID,
			E.Event_Title,
			E.Event_Start_Date,
			H.Household_Name
			, H.Home_Phone
			, H.Address_ID
END