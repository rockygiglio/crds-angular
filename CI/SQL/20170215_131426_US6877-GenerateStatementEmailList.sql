USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[report_CRDS_Statement_Email_List]
    @DomainID varchar(40)
    ,@FromDate Date = '1-1-2016'
    ,@ToDate Date = '12-31-2016'

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
                              Statement_Type_ID INT, Congregation_Name NVARCHAR(50))

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
      Congregation_Name
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
          Congregation_Name
        FROM Donors
          INNER JOIN Donations
            ON Donors.Donor_ID = Donations.Donor_ID
          INNER JOIN Statement_Methods
            ON Statement_Methods.Statement_Method_ID = Donors.Statement_Method_ID
          INNER JOIN Contacts
            ON Donors.Contact_ID = Contacts.Contact_ID
          INNER JOIN Households
            ON Contacts.Household_ID = Households.Household_ID
          INNER JOIN Congregations
            ON Households.Congregation_ID = Congregations.Congregation_ID
        WHERE
          Statement_Methods.Statement_Method_ID != 4                 -- Exclude 'no statement needed' method
          AND
          Donations.Payment_Type_ID IN (SELECT Payment_Type_ID       -- regular/soft credit/non-cash donations
                                        FROM Payment_Types
                                        WHERE
                                          Payment_Type IN ('Check', 'Cash', 'Credit Card', 'Bank', 'Non-Cash/Asset'))
          AND
          Donations.Donation_Status_ID = (SELECT Donation_Status_ID  -- 'Deposited' status
                                          FROM Donation_Statuses
                                          WHERE Donation_Statuses.Donation_Status = 'Deposited')
          AND
          CONVERT(DATE, Donations.Donation_Date) BETWEEN @FromDate AND @ToDate

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
        ON Donors.Donor_ID = Donations.Donor_ID
      INNER JOIN Statement_Methods
        ON Donors.Statement_Method_ID = Statement_Methods.Statement_Method_ID
      INNER JOIN Contacts
        ON Donors.Contact_ID = Contacts.Contact_ID
      INNER JOIN Households
        ON Contacts.Household_ID = Households.Household_ID
      INNER JOIN Congregations
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
      Contacts.Household_Position_ID = 1                                       -- Head of Household
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
    WHERE Email_Address IS NOT NULL
    ORDER BY Display_Name

    DROP TABLE #Donor_Email_List
    DROP TABLE #Statement_Email_List
    DROP TABLE #Co_Giver_Email_List

  END
GO
