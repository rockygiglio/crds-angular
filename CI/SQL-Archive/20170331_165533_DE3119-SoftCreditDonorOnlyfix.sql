USE [MinistryPlatform]
GO
/****** Object:  StoredProcedure [dbo].[report_CRDS_Donor_Contribution_Statement]    Script Date: 3/31/2017 3:54:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- Created 3/14/2017 for Paper Statements
ALTER PROCEDURE [dbo].[report_CRDS_Donor_Contribution_Statement] (
	@FromDate DateTime,
	@ToDate DateTime,
	@AccountingCompanyID Int
)
AS
BEGIN
	DECLARE @DomainID INT = 1

	SET NOCOUNT ON
	SET FMTONLY OFF

	-- Normalize dates to remove time and ensure end date is inclusive
	SET @FromDate = CONVERT(DATE, @FromDate)
	SET @ToDate = CONVERT(DATE, @ToDate + 1)

	CREATE TABLE #D (
		Donor_ID INT
		, Statement_ID VARCHAR(15)
		, Contact_Or_Household_ID INT
	);

	INSERT INTO #D (
		Donor_ID
		, Statement_ID
		, Contact_Or_Household_ID
	)
	SELECT
		Do.Donor_ID
		, Statement_ID = CASE WHEN C.Household_ID IS NULL OR Do.Statement_Type_ID = 1 THEN 'C' + CONVERT(VARCHAR(10),C.Contact_ID) ELSE 'F' + CONVERT(VARCHAR(10),C.Household_ID) END
		, Contact_Or_Household_ID = CASE WHEN C.Household_ID IS NULL OR Do.Statement_Type_ID = 1 THEN C.Contact_ID ELSE C.Household_ID END
	FROM
		Donors Do
		INNER JOIN Contacts C ON C.Contact_ID = Do.Contact_ID
	WHERE
		EXISTS (
			SELECT 1
			FROM Donations D
			WHERE D.Donor_ID = Do.Donor_ID AND (Donation_Date >= @FromDate AND Donation_Date < @ToDate)
		);

    -- Adding soft credit donors
	INSERT INTO #D (
		Donor_ID
		, Statement_ID
		, Contact_Or_Household_ID
	)
	SELECT
		DD.Soft_Credit_Donor
		, Statement_ID = CASE WHEN C.Household_ID IS NULL OR Do.Statement_Type_ID = 1 THEN 'C' + CONVERT(VARCHAR(10),C.Contact_ID) ELSE 'F' + CONVERT(VARCHAR(10),C.Household_ID) END
		, Contact_Or_Household_ID = CASE WHEN C.Household_ID IS NULL OR Do.Statement_Type_ID = 1 THEN C.Contact_ID ELSE C.Household_ID END
	FROM
		Donation_Distributions DD
		INNER JOIN Donors Do ON Do.Donor_ID = DD.Soft_Credit_Donor
		INNER JOIN Contacts C ON C.Contact_ID = Do.Contact_ID
	WHERE
		EXISTS (
			SELECT 1
			FROM Donations D
			join Donation_Distributions DD ON DD.Donation_ID = D.Donation_ID
			WHERE DD.Soft_Credit_Donor IS NOT NULL AND (D.Donation_Date >= @FromDate AND D.Donation_Date < @ToDate)
		);

	CREATE INDEX IX_D_DonorID ON #D(Donor_ID);

	CREATE TABLE #DONORS (Donor_ID INT, Statement_ID VARCHAR(15));
	INSERT INTO #DONORS SELECT DISTINCT Donor_ID, Statement_ID FROM #D;

	CREATE INDEX IX_DONORS_DonorID ON #DONORS(Donor_ID);

	CREATE TABLE #Gifts(
	    Donor_ID INT
		, Donation_ID INT
		, Donation_Distribution_ID INT
		, Donation_Date DATETIME
		, Amount MONEY
		, Non_Deductible_Amount MONEY
		, Deductible_Amount MONEY
		, Payment_Type VARCHAR(50)
		, Payment_Type_ID INT	
		, Item_Number VARCHAR(15)
		, Fund_Name VARCHAR(50)
		, Donation_Detail VARCHAR(1000)
		, Mail_Name VARCHAR(1000)
		, Address_Line_1 VARCHAR(1000)
		, Apt_or_Suite  VARCHAR(1000)
		, City VARCHAR(50)
		, [State] VARCHAR(50)
		, Postal_Code VARCHAR(15)
		, Statement_ID VARCHAR(15)
		, Is_Recurring_Gift BIT
		, Section_Title VARCHAR(1000)
		, Header_Title VARCHAR(1000)
		, Section_Sort INT
	);

	--Tax Deductible and Stock Giving
	INSERT INTO #Gifts(
		Donor_ID
		, Donation_ID
		, Donation_Distribution_ID
		, Donation_Date
		, Amount
		, Non_Deductible_Amount
		, Deductible_Amount
		, Payment_Type
		, Payment_Type_ID	
		, Item_Number 
		, Fund_Name
		, Mail_Name
		, Address_Line_1
		, Apt_or_Suite 
		, City
		, [State]
		, Postal_Code
		, Statement_ID
		, Is_Recurring_Gift
		, Section_Title
		, Header_Title
		, Section_Sort
	)
	SELECT 
	    #DONORS.Donor_ID
		, D.Donation_ID
		, DD.Donation_Distribution_ID
		, D.Donation_Date
		, Amount = CASE WHEN D.Payment_Type_ID <> 6 AND Prog.Tax_Deductible_Donations = 1 THEN DD.Amount ELSE 0 END
		, Non_Deductible_Amount = CASE WHEN D.Payment_Type_ID = 6 OR Prog.Tax_Deductible_Donations = 0 THEN DD.Amount ELSE 0 END
		, Deductible_Amount = CASE WHEN D.Payment_Type_ID <> 6 AND Prog.Tax_Deductible_Donations = 1 THEN DD.Amount ELSE 0 END
		, PT.Payment_Type
		, D.Payment_Type_ID
		, D.Item_Number
		, Prog.Statement_Title AS Fund_Name
		, Mail_Name = CASE WHEN C.Company = 1 THEN C.Display_Name
						WHEN SP.Last_Name <> C.Last_Name AND CNT.num < 2 THEN C.First_Name + SPACE(1) + C.Last_Name + ' & ' + SP.First_Name + SPACE(1) + Sp.Last_Name
						WHEN SP.Gender_ID < C.Gender_ID AND CNT.num < 2 THEN ISNULL(SP.First_Name + ' & ','') + C.First_Name + Space(1) + C.Last_Name
						WHEN CNT.num >= 2 AND C.Household_Position_ID = 1 THEN STUFF(REPLACE((SELECT '#!' + LTRIM(RTRIM(co.first_name + ' '+ co.last_name)) AS 'data()' FROM contacts co WHERE Do.Statement_Type_ID = 2 AND C.Household_ID = co.Household_ID AND co.Household_Position_ID =1 FOR XML PATH('')),' #!',' & '), 1, 2, '')
	    				WHEN CNT.num = 2 AND CountLast.sameLast = 2 AND C.Household_Position_id = 2 THEN CONCAT((STUFF(REPLACE((SELECT '#!' + LTRIM(RTRIM(co.first_name)) AS 'data()' FROM contacts co WHERE Do.Statement_Type_ID = 2 AND C.Household_ID = co.Household_ID AND co.Household_Position_ID =1 FOR XML PATH('')),' #!',' & '), 1, 2, '')),' ',(SELECT TOP 1 Last_Name FROM Contacts con WHERE Do.Statement_Type_ID = 2 AND C.Household_ID = con.Household_ID AND con.Household_Position_ID =1))
						ELSE ISNULL(C.First_Name + ISNULL(' & ' + SP.First_Name,'') + SPACE(1) + C.Last_Name, C.Display_Name) END
		, A.Address_Line_1
		, A.Address_Line_2 Apt_or_Suite
		, A.City
		, A.[State/Region] AS [State]
		, A.Postal_Code
		, #DONORS.Statement_ID
		, D.Is_Recurring_Gift
		, Section_Title = CAST(CASE WHEN D.Payment_Type_ID <> 6 AND Prog.Tax_Deductible_Donations = 1 THEN 'Tax-Deductible Information:' ELSE 'Stock Giving' END AS VARCHAR(100))
		, Header_Title = CAST(CASE WHEN D.Payment_Type_ID = 6 THEN 'Stock Giving' ELSE 'Giving' END AS VARCHAR(50))
		, Section_Sort = CASE WHEN D.Payment_Type_ID = 6 THEN 2 ELSE 1 END
	FROM Donations D
		INNER JOIN #DONORS ON #DONORS.Donor_ID = D.Donor_ID
		INNER JOIN Donors Do ON Do.Donor_ID = D.Donor_ID
		INNER JOIN Contacts C ON C.Contact_ID = Do.Contact_ID
		OUTER APPLY (SELECT Count(*) as num FROM Contacts S WHERE Do.Statement_Type_ID = 2 AND S.Household_ID = C.Household_ID AND C.Contact_ID <> S.Contact_ID AND S.Household_Position_ID = 1) CNT
		OUTER APPLY (SELECT Top 1 Contact_ID, Household_ID, Last_Name, Nickname, First_Name, Gender_ID FROM Contacts S WHERE Do.Statement_Type_ID = 2 AND S.Household_ID = C.Household_ID AND C.Contact_ID <> S.Contact_ID AND S.Household_Position_ID = 1) SP
		OUTER APPLY (SELECT Count(Last_Name) as sameLast From Contacts CTS WHERE CTS.Last_Name in (SELECT TS.Last_Name from Contacts TS group by TS.Last_Name having count(TS.Last_Name ) > 1) AND CTS.Household_ID = C.Household_ID AND  CTS.household_position_id = 1) CountLast
		LEFT OUTER JOIN Households H ON H.Household_ID = C.Household_ID
		LEFT OUTER JOIN Addresses A ON A.Address_ID = H.Address_ID
		INNER JOIN Donation_Distributions DD ON DD.Donation_ID = D.Donation_ID AND DD.Soft_Credit_Donor IS NULL
		INNER JOIN Programs Prog ON Prog.Program_ID = DD.Program_ID
		INNER JOIN Congregations Cong ON Cong.Congregation_ID = Prog.Congregation_ID AND Cong.Accounting_Company_ID = ISNULL(@AccountingCompanyID, Cong.Accounting_Company_ID)
		INNER JOIN Payment_Types PT ON PT.Payment_Type_ID = D.Payment_Type_ID
		INNER JOIN Donation_Statuses DS ON DS.Donation_Status_ID  = D.Donation_Status_ID
	WHERE
		(D.Donation_Date >= @FromDate AND D.Donation_Date < @ToDate)
		AND D.Domain_ID = @DomainID
		AND DS.Display_On_Statements = 1
		AND ISNULL(Prog.Tax_Deductible_Donations, 0) = 1 --Omit Non Deductible
		AND Do.Statement_Method_ID <> 4		-- No Statement Needed
		AND Do.Statement_Frequency_ID <> 3		-- Never
		AND D.Donation_Status_ID = 2	-- Deposited
	;

	--ADD SOFT CREDITS
	INSERT INTO #Gifts
		(Donation_ID
		, Donor_ID
		, Donation_Distribution_ID
		, Donation_Date
		, Amount
		, Non_Deductible_Amount
		, Deductible_Amount
		, Payment_Type
		, Payment_Type_ID
		, Item_Number
		, Fund_Name
		, Mail_Name
		, Address_Line_1
		, Apt_or_Suite 
		, City
		, [State]
		, Postal_Code
		, Statement_ID
		, Is_Recurring_Gift
		, Section_Title
		, Header_Title
		, Section_Sort
	)
	SELECT #DONORS.Donor_ID
	    , D.Donation_ID
		, DD.Donation_Distribution_ID
		, D.Donation_Date
		, DD.Amount
		, DD.Amount AS Non_Deductible_Amount
		, 0 AS Deductible_Amount--Even though it is a Non-Deductible, it needs to be in this field to show on the report
		, PT.Payment_Type
		, D.Payment_Type_ID
		, D.Item_Number
		, Prog.Statement_Title AS Fund_Namey
		, Mail_Name = CASE WHEN C.Company = 1 THEN C.Display_Name
						WHEN SP.Last_Name <> C.Last_Name THEN C.First_Name + SPACE(1) + C.Last_Name + ' & ' + SP.First_Name + SPACE(1) + Sp.Last_Name
						WHEN SP.Gender_ID < C.Gender_ID THEN ISNULL(SP.First_Name + ' & ','') + C.First_Name + Space(1) + C.Last_Name
						ELSE C.First_Name + ISNULL(' & ' + SP.First_Name,'') + SPACE(1) + C.Last_Name END
		, A.Address_Line_1
		, A.Address_Line_2 Apt_or_Suite
		, A.City
		, A.[State/Region] AS [State]
		, A.Postal_Code
		, #DONORS.Statement_ID
		, D.Is_Recurring_Gift
		, Section_Title = 'Trust and Foundation Giving' 
		, Header_Title = 'Non Tax-Deductible Information through Crossroads: (You will receive a tax-deductible statement from the trust or foundation.)' 
		, Section_Sort = 3
		FROM Donations D
			 INNER JOIN Donation_Distributions DD ON DD.Donation_ID = D.Donation_ID
			 INNER JOIN #DONORS ON #DONORS.Donor_ID = DD.Soft_Credit_Donor
			 INNER JOIN Donors Do ON Do.Donor_ID = DD.Soft_Credit_Donor
			 INNER JOIN Contacts C ON C.Contact_ID = Do.Contact_ID
			 OUTER APPLY (SELECT Top 1 Contact_ID, Household_ID, Last_Name, Nickname, First_Name, Gender_ID FROM Contacts S WHERE Do.Statement_Type_ID = 2 AND S.Household_ID = C.Household_ID AND C.Contact_ID <> S.Contact_ID AND S.Household_Position_ID = 1 AND C.Household_Position_ID = 1) SP
			 LEFT OUTER JOIN Households H ON H.Household_ID = C.Household_ID
			 LEFT OUTER JOIN Addresses A ON A.Address_ID = H.Address_ID
			 INNER JOIN Programs Prog ON Prog.Program_ID = DD.Program_ID
			 INNER JOIN Congregations Cong ON Cong.Congregation_ID = Prog.Congregation_ID AND Cong.Accounting_Company_ID = ISNULL(@AccountingCompanyID, Cong.Accounting_Company_ID)
			 INNER JOIN Statement_Headers SH ON SH.Statement_Header_ID = Prog.Statement_Header_ID
			 INNER JOIN Payment_Types PT ON PT.Payment_Type_ID = D.Payment_Type_ID
			 INNER JOIN Donation_Statuses DS ON DS.Donation_Status_ID  = D.Donation_Status_ID
		WHERE 
			 (D.Donation_Date >= @FromDate AND D.Donation_Date < @ToDate)
			 AND D.Domain_ID = @DomainID
			 AND ISNULL(Prog.Tax_Deductible_Donations,0) = 1 --Omit Non Deductible
			 AND Do.Statement_Method_ID <> 4
			 AND Do.Statement_Frequency_ID <> 3
			 AND D.Donation_Status_ID = 2 -- Deposited 
			 ;

	CREATE INDEX IX_Gifts_DonorID ON #Gifts(Donor_ID)


	SELECT
		Statement_ID
		, Row_No = DENSE_RANK() OVER (PARTITION BY Statement_ID ORDER BY Section_Sort, Donation_Date, Donation_Distribution_ID)
		, Donation_ID
		, Donation_Date
		, Amount
		, Non_Deductible_Amount
		, Deductible_Amount
		, Payment_Type = CASE
							WHEN Payment_Type_ID = 0 THEN Donation_Detail
							WHEN Payment_Type_ID = 1 THEN CONCAT('Check #', Item_Number)
							WHEN Payment_Type_ID = 2 THEN 'Cash'
							WHEN Payment_Type_ID = 4 AND Is_Recurring_Gift = 1 THEN 'Recurring Credit Card'
							WHEN Payment_Type_ID = 4 THEN 'One-time Credit Card'
							WHEN Payment_Type_ID = 5 AND Is_Recurring_Gift = 1 THEN 'Recurring ACH'
							WHEN Payment_Type_ID = 5 THEN 'One-time ACH'
							WHEN Payment_Type_ID = 6 THEN ISNULL(Donation_Detail, 'Non-cash')
							ELSE ISNULL(Item_Number, Payment_Type)
						END
		, Fund_Name AS [Donation_Description]
		, Mail_Name
		, Address_Line_1
		, Apt_or_Suite
		, City
		, [State]
		, Postal_Code
		, Section_Title
		, Header_Title
		, Section_Sort
	FROM #Gifts
	;

	DROP TABLE #D;
	DROP TABLE #DONORS;
	DROP TABLE #Gifts;

END

