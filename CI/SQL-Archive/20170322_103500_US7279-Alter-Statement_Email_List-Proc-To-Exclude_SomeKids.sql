USE [MinistryPlatform]
GO

/****** Object:  StoredProcedure [dbo].[report_CRDS_Statement_Email_List]    Script Date: 3/22/2017 10:35:19 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[report_CRDS_Statement_Email_List]
    @DomainID varchar(40)
    ,@FromDate Date 
    ,@ToDate Date 

  AS
  BEGIN

    SET NOCOUNT ON
    SET FMTONLY OFF

    IF OBJECT_ID('tempdb..#Statement_Email_List') IS NOT NULL DROP TABLE #Statement_Email_List
    CREATE TABLE #Statement_Email_List (Donor_ID INT, Contact_ID INT, Display_Name NVARCHAR(75),
                                  Email_Address VARCHAR(100), Statement_Method NVARCHAR(50),
                                  Congregation_Name NVARCHAR(50))

    IF OBJECT_ID('tempdb..#Donor_Email_List') IS NOT NULL DROP TABLE #Donor_Email_List
    CREATE TABLE #Donor_Email_List (Donor_ID INT, Contact_ID INT, Display_Name NVARCHAR(75), Household_ID INT,
                              Household_Position_ID INT, Email_Address VARCHAR(100), Statement_Method NVARCHAR(50),
                              Statement_Type_ID INT, Congregation_Name NVARCHAR(50), isKid bit)

    IF OBJECT_ID('tempdb..#Co_Giver_Email_List') is NOT NULL DROP TABLE #Co_Giver_Email_List
    CREATE TABLE #Co_Giver_Email_List (Donor_ID INT, Contact_ID INT, Display_Name NVARCHAR(75), Household_ID INT,
                              Household_Position_ID INT, Email_Address VARCHAR(100), Statement_Method NVARCHAR(50),
                              Statement_Type_ID INT, Congregation_Name NVARCHAR(50))

    /**
     ** Givers
     **/
    INSERT INTO #Donor_Email_List (
      Donor_ID,
      Contact_ID,
      Display_Name,
      Household_ID,
      Household_Position_ID,
      Email_Address,
      Statement_Method,
      Statement_Type_ID,
      Congregation_Name,
	  isKid
    )
    SELECT DISTINCT
          Donors.Donor_ID,
          Donors.Contact_ID,
          Contacts.Display_Name,
          Contacts.Household_ID,
          Contacts.Household_Position_ID,
          Contacts.Email_Address,
          Statement_Method,
          Statement_Type_ID,
          Congregation_Name,
		  (CASE 
		     WHEN Contacts.Household_Position_ID = 1 THEN 0 --if they are HoH then they are not a kid
			 WHEN ISNULL(Contacts.__Age, 0 ) >= 13 THEN 0 --if they have an age >= 13 then they are not a kid (if the age is missing then fall thru to the next one)
			 WHEN ISNULL(Contacts.__Age, 18 ) <  13 THEN 1 --if they have an age < 13 then they are a kid (if the age is missing then fall thru to the next one)
			 WHEN Contacts.Household_Position_ID <> 2 THEN 0 --household position anything other than minor child then they are not a kid
			 ELSE 1 --otherwise they are a kid
		   END )
        FROM Donors
          INNER JOIN Donations
            ON Donors.Donor_ID = Donations.Donor_ID
		  INNER JOIN Donation_Distributions --We want to filter soft credits out of this list
            ON Donation_Distributions.Donation_ID = Donations.Donation_ID
          INNER JOIN Statement_Methods
            ON Statement_Methods.Statement_Method_ID = Donors.Statement_Method_ID
          INNER JOIN Contacts
            ON Donors.Contact_ID = Contacts.Contact_ID
          INNER JOIN Households
            ON Contacts.Household_ID = Households.Household_ID
          LEFT OUTER JOIN Congregations
            ON Households.Congregation_ID = Congregations.Congregation_ID
        WHERE
          Donations.Donation_Status_ID = (SELECT Donation_Status_ID  -- 'Deposited' status
                                          FROM Donation_Statuses
                                          WHERE Donation_Statuses.Donation_Status = 'Deposited')
          AND
          CONVERT(DATE, Donations.Donation_Date) BETWEEN @FromDate AND @ToDate
		  AND Contacts.Display_Name <> 'Guest Giver' --DE2824 KD: Don't show Guest Giver
		  AND Donation_Distributions.Soft_Credit_Donor IS NULL --Don't include the Donor for Soft Credit Donations


	/**
     **   Add Soft Credit Givers
     **/
    INSERT INTO #Donor_Email_List (
      Donor_ID,
      Contact_ID,
      Display_Name,
      Household_ID,
      Household_Position_ID,
      Email_Address,
      Statement_Method,
      Statement_Type_ID,
      Congregation_Name,
	  isKid
    )
    SELECT DISTINCT
          Donors.Donor_ID,
          Donors.Contact_ID,
          Contacts.Display_Name,
          Contacts.Household_ID,
          Contacts.Household_Position_ID,
          Contacts.Email_Address,
          Statement_Method,
          Statement_Type_ID,
          Congregation_Name,
		  (CASE 
		     WHEN Contacts.Household_Position_ID = 1 THEN 0 --if they are HoH then they are not a kid
			 WHEN ISNULL(Contacts.__Age, 0 ) >= 13 THEN 0 --if they have an age >= 13 then they are not a kid (if the age is missing then fall thru to the next one)
			 WHEN ISNULL(Contacts.__Age, 18 ) <  13 THEN 1 --if they have an age < 13 then they are a kid (if the age is missing then fall thru to the next one)
			 WHEN Contacts.Household_Position_ID <> 2 THEN 0 --household position anything other than minor child then they are not a kid
			 ELSE 1 --otherwise they are a kid
		   END 
		  )
        FROM Donors
		  INNER JOIN Donation_Distributions
		    ON Donors.Donor_ID = Donation_Distributions.Soft_Credit_Donor  
          INNER JOIN Donations
            ON Donation_Distributions.Donation_ID = Donations.Donation_ID
          INNER JOIN Statement_Methods
            ON Statement_Methods.Statement_Method_ID = Donors.Statement_Method_ID
          INNER JOIN Contacts
            ON Donors.Contact_ID = Contacts.Contact_ID
          INNER JOIN Households
            ON Contacts.Household_ID = Households.Household_ID
          LEFT OUTER JOIN Congregations
            ON Households.Congregation_ID = Congregations.Congregation_ID
        WHERE
          Donations.Donation_Status_ID = (SELECT Donation_Status_ID  -- 'Deposited' status
                                          FROM Donation_Statuses
                                          WHERE Donation_Statuses.Donation_Status = 'Deposited')
          AND
          CONVERT(DATE, Donations.Donation_Date) BETWEEN @FromDate AND @ToDate
		  AND Contacts.Display_Name <> 'Guest Giver' --DE2824 KD: Don't show Guest Giver
		  AND NOT EXISTS ( SELECT 1 
						   FROM #Donor_Email_List  
						   WHERE #Donor_Email_List.Donor_ID = Donors.Donor_ID) --We don't want to have duplicates if someone gave normally and as a soft credit donor

    
	CREATE INDEX IX_Donor_Email ON #Donor_Email_List(Donor_ID)


    /**
     ** Co-givers (who didn't make any donation or made donation which are not Deposited status)
     **/
    INSERT INTO #Co_Giver_Email_List (
      Donor_ID,
      Contact_ID,
      Display_Name,
      Household_ID,
      Household_Position_ID,
      Email_Address,
      Statement_Method,
      Statement_Type_ID,
      Congregation_Name
    )
    SELECT DISTINCT
      Donors.Donor_ID,
      Donors.Contact_ID, /*, Donation_ID, Donation_Status_ID*/
      Contacts.Display_Name,
      Contacts.Household_ID,
      Contacts.Household_Position_ID,
      Contacts.Email_Address,
      Statement_Methods.Statement_Method,
      Donors.Statement_Type_ID,
      Congregations.Congregation_Name
    FROM Donors
      LEFT OUTER JOIN Donations
        ON Donors.Donor_ID = Donations.Donor_ID AND CONVERT(DATE, Donations.Donation_Date) BETWEEN @FromDate AND @ToDate
      INNER JOIN Statement_Methods
        ON Donors.Statement_Method_ID = Statement_Methods.Statement_Method_ID
      INNER JOIN Contacts
        ON Donors.Contact_ID = Contacts.Contact_ID
      INNER JOIN Households
        ON Contacts.Household_ID = Households.Household_ID
      LEFT OUTER JOIN Congregations
        ON Households.Congregation_ID = Congregations.Congregation_ID
      INNER JOIN #Donor_Email_List
        ON Contacts.Household_ID = #Donor_Email_List.Household_ID
    WHERE
      (Donation_ID IS NULL
        OR
      (Donation_ID IS NOT NULL
        AND
      Donation_Status_ID != (SELECT Donation_Status_ID
                             FROM Donation_Statuses
                             WHERE Donation_Status = 'Deposited')))
        AND
      Donors.Contact_ID != #Donor_Email_List.Contact_ID
        AND
      Contacts.Household_Position_ID = 1                                       -- Co-giver is Head of Household
        AND
	  (#Donor_Email_List.Household_Position_ID = 1                              -- Donor is Head of Household
	    OR
	  #Donor_Email_List.isKid = 1)												-- OR Donor is a kid
        AND
      Donors.Statement_Type_ID = (SELECT Statement_Type_ID
                                  FROM Statement_Types
                                  WHERE Statement_Type = 'Family')             -- Receive 'Family' type statement
        AND
      #Donor_Email_List.Statement_Type_ID = (SELECT Statement_Type_ID
                                            FROM Statement_Types
                                            WHERE Statement_Type = 'Family')

    CREATE INDEX IX_Co_Giver_Email ON #Co_Giver_Email_List(Donor_ID)


    INSERT INTO #Statement_Email_List(
      Donor_ID,
      Contact_ID,
      Display_Name,
      Email_Address,
      Statement_Method,
      Congregation_Name
    )
    SELECT Donor_ID, Contact_ID, Display_Name, Email_Address, Statement_Method, Congregation_Name
    FROM #Donor_Email_List
	WHERE isKid = 0

    CREATE INDEX IX_Statement_Email ON #Statement_Email_List(Donor_ID)

    INSERT INTO #Statement_Email_List(
      Donor_ID,
      Contact_ID,
      Display_Name,
      Email_Address,
      Statement_Method,
      Congregation_Name
    )
    SELECT Donor_ID, Contact_ID, Display_Name, Email_Address, Statement_Method, Congregation_Name
    FROM #Co_Giver_Email_List

    /**
     ** Statement Email List
     **/
    SELECT DISTINCT Email_Address, Display_Name, Statement_Method, Congregation_Name
    FROM #Statement_Email_List
    ORDER BY Display_Name

    DROP TABLE #Donor_Email_List
    DROP TABLE #Statement_Email_List
    DROP TABLE #Co_Giver_Email_List

    END


GO

