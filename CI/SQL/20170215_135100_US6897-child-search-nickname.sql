USE [MinistryPlatform]
GO

/****** Object:  StoredProcedure [dbo].[api_crds_Child_Signin_Search]    Script Date: 12/6/2016 12:50:46 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[api_crds_Child_Signin_Search]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[api_crds_Child_Signin_Search] AS'
END
GO
-- =============================================
-- Author:      Tim Giblin
-- Create date: 2017-2-15
-- Description:	Add nickname to child search
-- =============================================
ALTER PROCEDURE [dbo].[api_crds_Child_Signin_Search]
	@Phone_Number nvarchar(25)
  , @Include_Other_Household bit = 1
AS
BEGIN
  DECLARE @Phone_Number_Without_Dashes nvarchar(25) = REPLACE(@Phone_Number, '-', '');

  DECLARE @Primary_Household_ID int;

  DECLARE @PrimaryKids TABLE (
    Participant_ID int
    , Contact_ID int
    , Household_ID int
    , Household_Position_ID int
    , First_Name nvarchar(50)
    , Last_Name nvarchar(50)
    , Nickname nvarchar(50)
    , Date_Of_Birth date
    , Group_ID int
  );

  DECLARE @OtherKids TABLE (
    Participant_ID int
    , Contact_ID int
    , Household_ID int
    , Household_Position_ID int
    , First_Name nvarchar(50)
    , Last_Name nvarchar(50)
    , Nickname nvarchar(50)
    , Date_Of_Birth date
    , Group_ID int
  );

  -- Get Primary Household ID
  SELECT TOP 1
    @Primary_Household_ID = H.Household_ID
  FROM
    Contacts C
    , Households H
  WHERE
    C.Household_ID = H.Household_ID
    AND C.Household_Position_ID IN (1, 3, 4) -- Head of Household, Other Adult, Adult Child
    AND (
      C.Mobile_Phone = @Phone_Number
      OR C.Mobile_Phone = @Phone_Number_Without_Dashes
      OR H.Home_Phone = @Phone_Number
      OR H.Home_Phone = @Phone_Number_Without_Dashes
    );

  IF @@ROWCOUNT > 0
  BEGIN
    -- Get all Primary Household kids, with Group ID
    INSERT INTO @PrimaryKids
      SELECT
          P.Participant_ID
          , Child.Contact_ID
          , H.Household_ID
          , Child.Household_Position_ID
          , Child.First_Name
          , Child.Last_Name
          , Child.Nickname
          , Child.Date_of_Birth
          , GP.Group_ID
        FROM
          Contacts Child
          , Households H
          , Participants P
          LEFT OUTER JOIN Group_Participants GP ON (GP.Participant_ID = P.Participant_ID)
          LEFT OUTER JOIN Groups G ON (G.Group_ID = GP.Group_ID)
        WHERE
          P.Contact_ID = Child.Contact_ID
          AND Child.Household_ID = H.Household_ID
          AND H.Household_ID = @Primary_Household_ID
          AND Child.Household_Position_ID = 2 -- Minor Child
          AND G.Congregation_ID = 5 -- Not Site Specific
          AND G.Group_Type_ID = 4 -- Age or Grade Group
          AND G.Ministry_ID = 2 -- Kids Club
		  AND GP.End_Date IS NULL;

    IF @Include_Other_Household > 0
    BEGIN
      -- Get all Other Household kids, with Group ID
      INSERT INTO @OtherKids
        SELECT
            P.Participant_ID
            , Child.Contact_ID
            , OtherHousehold.Household_ID
            , OtherHousehold.Household_Position_ID
            , Child.First_Name
            , Child.Last_Name
            , Child.Nickname
            , Child.Date_of_Birth
            , GP.Group_ID
          FROM
            Contacts Child
            , Contact_Households OtherHousehold
            , Participants P
            LEFT OUTER JOIN Group_Participants GP ON (GP.Participant_ID = P.Participant_ID)
            LEFT OUTER JOIN Groups G ON (G.Group_ID = GP.Group_ID)
          WHERE
            P.Contact_ID = Child.Contact_ID
            AND OtherHousehold.Contact_ID = Child.Contact_ID
            AND OtherHousehold.Household_ID = @Primary_Household_ID
            AND OtherHousehold.Household_Position_ID = 2 -- Minor Child
            AND G.Congregation_ID = 5 -- Not Site Specific
            AND G.Group_Type_ID = 4 -- Age or Grade Group
            AND G.Ministry_ID = 2 -- Kids Club
			AND GP.End_Date IS NULL;
    END;
  END;

  -- Return two result sets - one containing the primary household id, the other containing the kids
  SELECT @Primary_Household_ID AS Household_ID;

  SELECT * FROM @PrimaryKids
  UNION
  SELECT * FROM @OtherKids;
END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_API_Procedures] WHERE [Procedure_Name] = N'api_crds_Child_Signin_Search')
BEGIN
	INSERT INTO [dbo].[dp_API_Procedures] (
		 Procedure_Name
		,Description
	) VALUES (
		 N'api_crds_Child_Signin_Search'
		,N'Gets all children matching a phone number for signing in to Kids Club'
	)
END

DECLARE @API_ROLE_ID int = 62;
DECLARE @API_ID int;

SELECT @API_ID = API_Procedure_ID FROM [dbo].[dp_API_Procedures] WHERE [Procedure_Name] = N'api_crds_Child_Signin_Search';

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
