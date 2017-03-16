USE [MinistryPlatform]

IF OBJECT_ID('dbo.report_CRDS_Donor_Contribution_Statement') IS NULL -- Check if SP Exists
 EXEC('CREATE PROCEDURE dbo.report_CRDS_Donor_Contribution_Statement AS SET NOCOUNT ON;') -- Create dummy/empty SP
GO

-- Created 3/14/2017 for Paper Statements
ALTER PROCEDURE report_CRDS_Donor_Contribution_Statement (
	@FromDate DateTime,
	@ToDate DateTime,
	@AccountingCompanyID Int
)
AS
BEGIN
	DECLARE @DomainID INT = 1

	DECLARE @RowsPerPage FLOAT = 16
	DECLARE @AddSoftCredit BIT = 0

	SET NOCOUNT ON
	SET FMTONLY OFF

	-- Normalize dates to remove time and ensure end date is inclusive
	SET @FromDate = CONVERT(DATE, @FromDate)
	SET @ToDate = CONVERT(DATE, @ToDate + 1)

	CREATE TABLE #D (
		Donor_ID INT
		, Statement_ID VARCHAR(15)
		, Contact_Or_Household_ID INT
	)

	INSERT INTO #D (
		Donor_ID
		, Statement_ID
		, Contact_Or_Household_ID
	)
	SELECT
		Do.Donor_ID
		, Statement_ID = CASE WHEN C.Household_ID IS NULL OR Do.Statement_Type_ID = 1 THEN 'C' + CONVERT(VARCHAR(10),C.Contact_ID) ELSE 'F' + CONVERT(Varchar(10),C.Household_ID) END
		, Contact_Or_Household_ID = CASE WHEN C.Household_ID IS NULL OR Do.Statement_Type_ID = 1 THEN C.Contact_ID ELSE C.Household_ID END
	FROM
		Donors Do
		INNER JOIN Contacts C ON C.Contact_ID = Do.Contact_ID
	WHERE
		EXISTS (
			SELECT 1
			FROM Donations D
			WHERE D.Donor_ID = Do.Donor_ID AND (Donation_Date >= @FromDate AND Donation_Date < @ToDate)
		)
		AND Do.Statement_Type_ID = 1		-- TODO: Individual only for now; remove this when adding Family
	GROUP BY
		Do.Donor_ID
		, CASE WHEN C.Household_ID IS NULL OR Do.Statement_Type_ID = 1 THEN 'C' + CONVERT(VARCHAR(10),C.Contact_ID) ELSE 'F' + CONVERT(Varchar(10),C.Household_ID) END
		, CASE WHEN C.Household_ID IS NULL OR Do.Statement_Type_ID = 1 THEN C.Contact_ID ELSE C.Household_ID END		

	CREATE INDEX IX_D_DonorID ON #D(Donor_ID)

	CREATE TABLE #DONORS (Donor_ID INT, Statement_ID Varchar(11));
	INSERT INTO #DONORS SELECT DISTINCT Donor_ID, Statement_ID FROM #D;

	CREATE INDEX IX_DONORS_DonorID ON #DONORS(Donor_ID)

	CREATE TABLE #Gifts(
		Donation_ID INT
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
		, Donor_Name VARCHAR(1000)
		, Contact_ID INT
		, Event_Name VARCHAR(1000)
		, Mail_Name VARCHAR(1000)
		, Address_Line_1 VARCHAR(1000)
		, Apt_or_Suite  VARCHAR(1000)
		, City VARCHAR(50)
		, [State] VARCHAR(50)
		, Postal_Code VARCHAR(15)
		, Statement_ID VARCHAR(15)
		, Contact_Or_Household_ID INT
		, Donor_ID INT
		, Gender_ID INT
		, Household_Position_ID INT
		, Is_Recurring_Gift BIT
		, Section_Title VARCHAR(1000)
		, Header_Title VARCHAR(1000)
		, Section_Sort INT
	)

	--Tax Deductible and Stock Giving
	INSERT INTO #Gifts(
		Donation_ID
		, Donation_Distribution_ID
		, Donation_Date
		, Amount
		, Non_Deductible_Amount
		, Deductible_Amount
		, Payment_Type
		, Payment_Type_ID
		, Item_Number
		, Fund_Name
		, Donation_Detail
		, Donor_Name
		, Contact_ID
		, Event_Name
		, Mail_Name
		, Address_Line_1
		, Apt_or_Suite 
		, City
		, [State]
		, Postal_Code
		, Statement_ID
		, Contact_Or_Household_ID
		, Donor_ID
		, Gender_ID
		, Household_Position_ID
		, Is_Recurring_Gift
		, Section_Title
		, Header_Title
		, Section_Sort
	)
	SELECT TOP 100 PERCENT
		D.Donation_ID
		, DD.Donation_Distribution_ID
		, D.Donation_Date
		, Amount = CASE WHEN D.Payment_Type_ID <> 6 AND Prog.Tax_Deductible_Donations = 1 THEN DD.Amount ELSE 0 END
		, Non_Deductible_Amount = CASE WHEN D.Payment_Type_ID = 6 OR Prog.Tax_Deductible_Donations = 0 THEN DD.Amount ELSE 0 END
		, Deductible_Amount = CASE WHEN D.Payment_Type_ID <> 6 AND Prog.Tax_Deductible_Donations = 1 THEN DD.Amount ELSE 0 END
		, PT.Payment_Type
		, D.Payment_Type_ID
		, D.Item_Number
		, Prog.Statement_Title AS Fund_Name
		, Donation_Detail = CASE
							WHEN D.Payment_Type_ID = 6  THEN ISNULL(D.Notes, 'No Description Given')
							WHEN PLDo.Donor_ID > 2 AND PLDo.Donor_ID <> Do.Donor_ID THEN PT.Payment_Type + ISNULL(' ' + D.Item_Number,'') + ': ' + COALESCE(PC.Campaign_Name + ': ' + PLDoC.Display_Name, Event_Title,Prog.Statement_Title)
							ELSE  PT.Payment_Type + ISNULL(' ' + D.Item_Number,'') + ': ' + ISNULL(E.Event_Title,Prog.Statement_Title)   END
		, COALESCE(C.Last_Name,C.Company_Name,C.Display_Name) + ISNULL(', ' + C.First_Name,'') + ISNULL(' & ' + SP.First_Name,'') AS Donor_Name
		, C.Contact_ID
		, E.Event_Title AS Event_Name
		, Mail_Name = CASE WHEN C.Company = 1 THEN C.Display_Name
						WHEN SP.Last_Name <> C.Last_Name THEN C.First_Name + SPACE(1) + C.Last_Name + ' & ' + SP.First_Name + SPACE(1) + Sp.Last_Name
						WHEN SP.Gender_ID < C.Gender_ID THEN ISNULL(SP.First_Name + ' & ','') + C.First_Name + Space(1) + C.Last_Name
						ELSE ISNULL(C.First_Name + ISNULL(' & ' + SP.First_Name,'') + SPACE(1) + C.Last_Name, C.Display_Name) END
		, A.Address_Line_1
		, A.Address_Line_2 Apt_or_Suite
		, A.City
		, A.[State/Region] AS [State]
		, A.Postal_Code
		, #DONORS.Statement_ID
		, #D.Contact_Or_Household_ID
		, D.Donor_ID
		, ISNULL(C.Gender_ID, 9) AS Gender_ID
		, ISNULL(C.Household_Position_ID, 9) AS Household_Position_ID
		, Is_Recurring_Gift
		, Section_Title = CAST(CASE WHEN D.Payment_Type_ID <> 6 AND Prog.Tax_Deductible_Donations = 1 THEN 'Tax-Deductible Information:' ELSE 'Stock Giving' END AS VARCHAR(100))
		, Header_Title = CAST(CASE WHEN D.Payment_Type_ID = 6 THEN 'Stock Giving' ELSE 'Giving' END AS VARCHAR(50))
		, Section_Sort = CASE WHEN D.Payment_Type_ID = 6 THEN 2 ELSE 1 END
	FROM Donations D
		INNER JOIN #DONORS ON #DONORS.Donor_ID = D.Donor_ID
		INNER JOIN dp_Domains Dom ON Dom.Domain_ID = D.Domain_ID
		INNER JOIN Donors Do ON Do.Donor_ID = D.Donor_ID
		INNER JOIN Contacts C ON C.Contact_ID = Do.Contact_ID
		OUTER APPLY (SELECT Top 1 Contact_ID, Household_ID, Last_Name, Nickname, First_Name, Gender_ID FROM Contacts S WHERE Do.Statement_Type_ID = 2 AND S.Household_ID = C.Household_ID AND C.Contact_ID <> S.Contact_ID AND S.Household_Position_ID = 1 AND C.Household_Position_ID = 1) SP
		LEFT OUTER JOIN Households H ON H.Household_ID = C.Household_ID
		LEFT OUTER JOIN Addresses A ON A.Address_ID = H.Address_ID
		INNER JOIN Donation_Distributions DD ON DD.Donation_ID = D.Donation_ID AND (@AddSoftCredit = 0 OR DD.Soft_Credit_Donor IS NULL)
		INNER JOIN Programs Prog ON Prog.Program_ID = DD.Program_ID
		INNER JOIN Congregations Cong ON Cong.Congregation_ID = Prog.Congregation_ID AND Cong.Accounting_Company_ID = ISNULL(@AccountingCompanyID, Cong.Accounting_Company_ID)
		INNER JOIN Payment_Types PT ON PT.Payment_Type_ID = D.Payment_Type_ID
		INNER JOIN Donation_Statuses DS ON DS.Donation_Status_ID  = D.Donation_Status_ID
		LEFT OUTER JOIN dbo.[Events] E ON E.Event_ID = DD.Target_Event
		LEFT OUTER JOIN Pledges Pl ON PL.Pledge_ID = DD.Pledge_ID
		LEFT OUTER JOIN Pledge_Campaigns PC ON PC.Pledge_Campaign_ID = PL.Pledge_Campaign_ID
		INNER JOIN #D ON #D.Donor_ID = D.Donor_ID --AND PL.Pledge_ID = #D.Pledge_ID
		LEFT OUTER JOIN Donors PLDo ON PLDo.Donor_ID = PL.Donor_ID
		LEFT OUTER JOIN Contacts PLDoC ON PLDoC.Contact_ID = PLDo.Contact_ID
	WHERE
		(Donation_Date >= @FromDate AND Donation_Date < @ToDate)
		AND DS.Display_On_Statements = 1
		AND @DomainID = Dom.Domain_ID
		AND ISNULL(Prog.Tax_Deductible_Donations,0) = 1 --Omit Non Deductible
		AND Do.Statement_Method_ID <> 4		-- No Statement Needed
		AND Do.Statement_Frequency_ID <> 3		-- Never
		AND D.Donation_Status_ID = 2	-- Deposited

	CREATE INDEX IX_Gifts_DonorID ON #Gifts(Donor_ID)

	SELECT
		Statement_ID
		, Row_No = DENSE_RANK() OVER (PARTITION BY Statement_ID ORDER BY Section_Sort, Donation_Date, Donation_Distribution_ID)
		, Donation_ID
	--	, Donation_Distribution_ID
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
	--	, Contact_ID
	--	, Event_Name
		, Mail_Name
		, Address_Line_1
		, Apt_or_Suite
		, City
		, [State]
		, Postal_Code
	--	, Donor_Row_No = DENSE_Rank() OVER (Partition By Donor_ID ORDER BY Section_Sort, Donation_Date, Donation_ID)
	--	, Page_No = Ceiling(Dense_Rank() OVER (Partition By Statement_ID ORDER BY Section_Sort, Donation_Date, Donation_Distribution_ID)/@RowsPerPage)
	--	, Household_Position_ID
		, Section_Title
		, Header_Title
		, Section_Sort
	FROM #Gifts

	DROP TABLE #D
	DROP TABLE #DONORS
	DROP TABLE #Gifts

END
