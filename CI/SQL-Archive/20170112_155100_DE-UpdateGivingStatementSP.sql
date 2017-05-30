USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[report_CRDS_Donor_Statement_N_Columns]') AND TYPE IN (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[report_CRDS_Donor_Statement_N_Columns] AS' 
END
GO



-- =============================================
-- Author:      Cleaver, John
-- Create date: 3/18/2016
-- Description: Update of MP report_Donor_Statement_N_Columns, customized for Crossroads
-- Update:		12/21/2016	Add Soft Credit
-- Update:      1/1/2017 Add 2 other Sections
-- =============================================

ALTER PROCEDURE [dbo].[report_CRDS_Donor_Statement_N_Columns]
	 @DomainID varchar(40)
	,@UserID varchar(40)
	,@PageID Int
	,@SelectionID Int = 0
	,@StmtYr Int
	,@ThroughDate DateTime
	,@CombineFamily BIT
	,@RowsPerPage FLOAT
	,@EnvelopeOnly BIT = NULL
	,@CampaignIDs varchar(4000) = NULL
	,@OmitStmtHeaderID INT = NULL
	,@AccountingCompanyID INT = NULL
	,@OtherColumnStartsAt INT = 5
	,@AddSoftCredit BIT = 1

AS
BEGIN

SET NOCOUNT ON
SET FMTONLY OFF



CREATE TABLE #Campaigns (ID INT);
INSERT INTO #Campaigns SELECT CONVERT(INT, Item) FROM dp_Split(@CampaignIDs, ',');

-- Remove the "None" selection from the campaign IDs.
-- "None" is added as a valid value in dbo.api_CRDS_MPP_GetPledgeCampaigns as a hack to workaround
-- the fact that SSRS requires a multi-select dropdown to have a value, but in this case, we want to
-- allow the user to not choose any campaign.
DELETE FROM #Campaigns WHERE ID < 0;

DECLARE @Campaign_Count INT;
SELECT @Campaign_Count = COUNT(*) FROM #Campaigns;

CREATE TABLE #PLAmt (Pledge_ID INT, Pledge_Campaign_ID INT, Amount Money)
INSERT INTO #PLAmt (Pledge_ID, Pledge_Campaign_ID, Amount)
SELECT PL.Pledge_ID, PL.Pledge_Campaign_ID, SUM(DD.Amount) AS Amount
FROM Donation_Distributions DD
 INNER JOIN Donations D ON D.Donation_ID = DD.Donation_ID AND D.Donation_Date < @ThroughDate + 1
 INNER JOIN Pledges PL ON PL.Pledge_ID = DD.Pledge_ID
WHERE PL.Pledge_Campaign_ID IN (SELECT ID FROM #Campaigns)
 AND D.Donation_Status_ID = 2 -- Deposited
GROUP BY PL.Pledge_ID, PL.Pledge_Campaign_ID

CREATE INDEX IX_Temp_PlAmt_PledgeID ON #PLAmt(Pledge_ID)

CREATE TABLE #PL(Pledge_ID INT, Donor_ID INT, Total_Pledge MONEY, Statement_Type_ID INT, Household_ID INT, Contact_ID INT, Given Money, Pledge_Campaign_ID INT)
INSERT INTO #PL (Pledge_ID, Donor_ID, Total_Pledge, Statement_Type_ID, Household_ID, Contact_ID, Given, Pledge_Campaign_ID)
SELECT PL.Pledge_ID
, Do.Donor_ID
, PL.Total_Pledge
, Do.Statement_Type_ID
, C.Household_ID
, C.Contact_ID
, SUM(#PLAmt.Amount) AS Given
, PL.Pledge_Campaign_ID
FROM Pledges PL
	INNER JOIN Donors Do ON Do.Donor_ID = PL.Donor_ID
	INNER JOIN Contacts C ON C.Contact_ID = Do.Contact_ID
	LEFT OUTER JOIN #PLAmt ON #PLAmt.Pledge_ID = PL.Pledge_ID
WHERE PL.Pledge_Campaign_ID IN (SELECT ID From #Campaigns)
GROUP BY PL.Pledge_ID
, Do.Donor_ID
, PL.Total_Pledge
, Do.Statement_Type_ID
, C.Household_ID
, C.Contact_ID
, PL.Pledge_Campaign_ID

CREATE INDEX IX_TempPL_DonorID ON #PL(Donor_ID)
CREATE INDEX IX_TempPL_HouseholdID ON #PL(Household_ID)
CREATE TABLE #D (Donor_ID INT, Pledge Money, Given_to_Pledge Money, Statement_ID Varchar(11), Pledge_ID INT, Pledge_Name VARCHAR(50))

	IF ISNULL(@SelectionID,0) <= 1
	BEGIN
	INSERT INTO #D (Donor_ID, Pledge, Given_to_Pledge, Statement_ID, Pledge_ID)
	SELECT Do.Donor_ID
	,SUM(Pledge) AS Pledge
	,SUM(Given) AS Given
	,Statement_ID = CASE WHEN @CombineFamily = 0 OR C.Household_ID IS NULL OR Do.Statement_Type_ID = 1  THEN 'C' + CONVERT(Varchar(10),C.Contact_ID) ELSE 'F' + CONVERT(Varchar(10),C.Household_ID) END
	,Pledge_ID
	FROM Donors Do
	 INNER JOIN Contacts C ON C.Contact_ID = Do.Contact_ID
	 OUTER APPLY (SELECT SUM(Total_Pledge) AS Pledge
				,SUM(Given) AS Given, Pledge_ID
				FROM #PL Plg
					WHERE Plg.Donor_ID = Do.Donor_ID
					 OR (PLG.Household_ID = C.Household_ID AND PLG.Statement_Type_ID = 2 AND @CombineFamily = 1 AND Do.Statement_Type_ID = 2)
					 GROUP BY Pledge_ID
					) FamPlg
	WHERE EXISTS (SELECT 1 FROM Donations D WHERE D.Donor_ID = Do.Donor_ID AND YEAR(Donation_Date) = @StmtYr)
	GROUP BY Do.Donor_ID, Pledge_ID
	,CASE WHEN @CombineFamily = 0 OR C.Household_ID IS NULL OR Do.Statement_Type_ID = 1  THEN 'C' + CONVERT(Varchar(10),C.Contact_ID) ELSE 'F' + CONVERT(Varchar(10),C.Household_ID) END
	END
	ELSE
	BEGIN
	CREATE TABLE #DD (Donor_ID INT, Household_ID INT, Statement_Type_ID INT)
	INSERT INTO #DD (Donor_ID, Household_ID, Statement_Type_ID)
	SELECT Do.Donor_ID, ISNULL(C.Household_ID,0) AS Household_ID, Do.Statement_Type_ID
	FROM Donors Do
	 INNER JOIN Contacts C ON C.Contact_ID = Do.Contact_ID
	WHERE Do.Donor_ID IN (SELECT Record_ID FROM dp_Selected_Records SR INNER JOIN dp_Selections S ON S.Selection_ID = SR.Selection_ID  INNER JOIN dbo.dp_Users U ON U.[User_ID] = S.[User_ID] AND U.User_GUID = @UserID WHERE S.Page_ID = @PageID AND ((S.Selection_ID = @SelectionID AND @SelectionID > 0) OR (S.Selection_Name = 'dp_DEFAULT' AND @SelectionID < 1 AND S.Sub_Page_ID IS NULL)))

	CREATE INDEX IX_DD_HouseholdID ON #DD(Household_ID)
	CREATE INDEX IX_DD_DonorID ON #DD(Donor_ID)

	INSERT INTO #DD (Donor_ID, Household_ID, Statement_Type_ID)
	SELECT Do.Donor_ID, ISNULL(C.Household_ID,0) AS Household_ID, Do.Statement_Type_ID
	FROM Donors Do
	 INNER JOIN Contacts C ON C.Contact_ID = Do.Contact_ID
	WHERE @CombineFamily = 1 AND Do.Statement_Type_ID = 2 AND EXISTS (SELECT 1 FROM #DD WHERE #DD.Household_ID = C.Household_ID AND #DD.Donor_ID <> Do.Donor_ID AND #DD.Statement_Type_ID = Do.Statement_Type_ID)
	
	INSERT INTO #D (Donor_ID, Pledge, Given_to_Pledge, Statement_ID, Pledge_ID)
	SELECT Do.Donor_ID
	,SUM(Pledge) AS Pledge
	,SUM(Given) AS Given
	,Statement_ID = CASE WHEN @CombineFamily = 0 OR C.Household_ID IS NULL OR Do.Statement_Type_ID = 1  THEN 'C' + CONVERT(Varchar(10),C.Contact_ID) ELSE 'F' + CONVERT(Varchar(10),C.Household_ID) END
	,Pledge_ID
	FROM Donors Do
	 INNER JOIN Contacts C ON C.Contact_ID = Do.Contact_ID
	 OUTER APPLY (SELECT SUM(Total_Pledge) AS Pledge
				,SUM(Given) AS Given, Pledge_ID
				FROM #PL Plg
					WHERE Plg.Donor_ID = Do.Donor_ID
					 OR (PLG.Household_ID = C.Household_ID AND PLG.Statement_Type_ID = 2 AND @CombineFamily = 1 AND Do.Statement_Type_ID = 2)
					 GROUP BY Pledge_ID
					) FamPlg
	WHERE Do.Donor_ID IN (SELECT Donor_ID FROM #DD)
	GROUP BY Do.Donor_ID, Pledge_ID
	,CASE WHEN @CombineFamily = 0 OR C.Household_ID IS NULL OR Do.Statement_Type_ID = 1  THEN 'C' + CONVERT(Varchar(10),C.Contact_ID) ELSE 'F' + CONVERT(Varchar(10),C.Household_ID) END

	END

CREATE INDEX IX_D_DonorID ON #D(Donor_ID)

UPDATE #D
SET Pledge_Name = Campaign_Name
FROM #D 
INNER JOIN Pledges P ON #D.Pledge_ID = P.Pledge_ID 
INNER JOIN Pledge_Campaigns PC ON P.Pledge_Campaign_ID = PC.Pledge_Campaign_ID	

CREATE TABLE #DONORS (Donor_ID INT, Statement_ID Varchar(11));
INSERT INTO #DONORS SELECT DISTINCT Donor_ID, Statement_ID FROM #D;

CREATE INDEX IX_DONORS_DonorID ON #DONORS(Donor_ID)

CREATE TABLE #Gifts(Donation_ID INT
	, Donation_Distribution_ID INT
	, Donation_Date DATETIME
	, Amount MONEY
	, Non_Deductible_Amount MONEY
	, Deductible_Amount MONEY
	, Payment_Type VARCHAR(50)
	, Payment_Type_ID INT
	, Item_Number VARCHAR(15)
	, Fund_Name VARCHAR(50)
	, Statement_Header VARCHAR(50)
	, Header_Sort INT
	, Donation_Detail VARCHAR(1000)
	, Donor_Name VARCHAR(1000)
	, Contact_ID INT
	, Event_Name VARCHAR(1000)
	, On_Pledge VARCHAR(5)
	, Mail_Name VARCHAR(1000)
	, Address_Line_1 VARCHAR(1000)
	, Apt_or_Suite  VARCHAR(1000)
	, City VARCHAR(50)
	, [State] VARCHAR(50)
	, Postal_Code VARCHAR(15)
	, Foreign_Country VARCHAR(50)
	, Statement_ID VARCHAR(15)
	, Envelope_No INT
	, PLEDGE MONEY
	, Given_To_Pledge MONEY
	, Donor_ID INT
	, SH_1 MONEY
	, SH_2 MONEY
	, SH_3 MONEY
	, SH_4 MONEY
	, SH_5 MONEY
	, SH_6 MONEY
	, SH_O MONEY
	, Other_Note VARCHAR(MAX)
	, Gender_ID INT
	, Household_Position_ID INT
	, Campaign_Name VARCHAR(75)
	, Is_Recurring_Gift BIT
	, Section_Title VARCHAR(1000)
	, Header_Title VARCHAR(1000)
	, Section_Sort INT
)

--Tax Deductible and Stock Giving
INSERT INTO #Gifts(Donation_ID
	, Donation_Distribution_ID
	, Donation_Date
	, Amount
	, Non_Deductible_Amount
	, Deductible_Amount
	, Payment_Type
	, Payment_Type_ID
	, Item_Number
	, Fund_Name
	, Statement_Header
	, Header_Sort
	, Donation_Detail
	, Donor_Name
	, Contact_ID
	, Event_Name
	, On_Pledge
	, Mail_Name
	, Address_Line_1
	, Apt_or_Suite 
	, City
	, [State]
	, Postal_Code
	, Foreign_Country
	, Statement_ID
	, Envelope_No
	, PLEDGE
	, Given_To_Pledge
	, Donor_ID
	, SH_1
	, SH_2
	, SH_3
	, SH_4
	, SH_5
	, SH_6
	, SH_O
	, Other_Note
	, Gender_ID
	, Household_Position_ID
	, Campaign_Name
	, Is_Recurring_Gift
	, Section_Title
	, Header_Title
	, Section_Sort
)
SELECT Top 100 PERCENT D.Donation_ID
	, DD.Donation_Distribution_ID
	, D.Donation_Date
	, Amount = CASE WHEN D.Payment_Type_ID <> 6 AND Prog.Tax_Deductible_Donations = 1 THEN DD.Amount ELSE 0 END
	, CASE WHEN D.Payment_Type_ID = 6 OR Prog.Tax_Deductible_Donations = 0 THEN DD.Amount ELSE 0 END AS Non_Deductible_Amount
	, CASE WHEN D.Payment_Type_ID <> 6 AND Prog.Tax_Deductible_Donations = 1 THEN DD.Amount ELSE 0 END AS Deductible_Amount
	, PT.Payment_Type
	, D.Payment_Type_ID
	, D.Item_Number
	, Prog.Statement_Title AS Fund_Name
	, ISNULL(SH.Statement_Header,'Other') AS Statement_Header
	, ISNULL(SH.Header_Sort,99) AS Header_Sort
	, Donation_Detail = CASE
						WHEN D.Payment_Type_ID = 6  THEN  ISNULL(D.Notes,'No Description Given')
						WHEN PLDo.Donor_ID > 2 AND PLDo.Donor_ID <> Do.Donor_ID THEN PT.Payment_Type + ISNULL(' ' + D.Item_Number,'') + ': ' + COALESCE(PC.Campaign_Name + ': ' + PLDoC.Display_Name, Event_Title,Prog.Statement_Title)
						ELSE  PT.Payment_Type + ISNULL(' ' + D.Item_Number,'') + ': ' + ISNULL(E.Event_Title,Prog.Statement_Title)   END
	, COALESCE(C.Last_Name,C.Company_Name,C.Display_Name) + ISNULL(', ' + C.First_Name,'') + ISNULL(' & ' + SP.First_Name,'') AS Donor_Name
	, C.Contact_ID
	, E.Event_Title AS Event_Name
	, On_Pledge = CASE WHEN ISNULL(#D.Pledge_ID,0) = 0 THEN '' ELSE 'Yes' END
	, Mail_Name = CASE WHEN C.Company = 1 THEN C.Display_Name
					WHEN SP.Last_Name <> C.Last_Name THEN C.First_Name + SPACE(1) + C.Last_Name + ' & ' + SP.First_Name + SPACE(1) + Sp.Last_Name
					WHEN SP.Gender_ID < C.Gender_ID THEN ISNULL(SP.First_Name + ' & ','') + C.First_Name + Space(1) + C.Last_Name
					ELSE C.First_Name + ISNULL(' & ' + SP.First_Name,'') + SPACE(1) + C.Last_Name END
	, A.Address_Line_1
	, A.Address_Line_2 Apt_or_Suite
	, A.City
	, A.[State/Region] AS [State]
	, A.Postal_Code
	, CASE A.Foreign_Country WHEN 'USA' THEN NULL WHEN 'US' THEN NULL WHEN 'United States' THEN NULL ELSE UPPER(A.Foreign_Country) END AS Foreign_Country
	, #DONORS.Statement_ID-- = CASE WHEN @CombineFamily = 0 OR C.Household_ID IS NULL OR Do.Statement_Type_ID = 1  THEN 'C' + CONVERT(Varchar(10),C.Contact_ID) ELSE 'F' + CONVERT(Varchar(10),H.Household_ID) END
	, Do.Envelope_No
	, ISNULL(#D.Pledge,0) AS PLEDGE
	, ISNULL(#D.Given_to_Pledge,0) AS Given_To_Pledge
	, D.Donor_ID
	, SH_1 = CASE WHEN SH.Header_Sort = 1 AND D.Payment_Type_ID <> 6 AND Prog.Tax_Deductible_Donations = 1 THEN DD.Amount ELSE 0 END
	, SH_2 = CASE WHEN SH.Header_Sort = 2 AND D.Payment_Type_ID <> 6 AND Prog.Tax_Deductible_Donations = 1 THEN DD.Amount ELSE 0 END
	, SH_3 = CASE WHEN SH.Header_Sort = 3 AND D.Payment_Type_ID <> 6 AND Prog.Tax_Deductible_Donations = 1 THEN DD.Amount ELSE 0 END
	, SH_4 = CASE WHEN SH.Header_Sort = 4 AND D.Payment_Type_ID <> 6 AND Prog.Tax_Deductible_Donations = 1 THEN DD.Amount ELSE 0 END
	, SH_5 = CASE WHEN SH.Header_Sort = 5 AND D.Payment_Type_ID <> 6 AND Prog.Tax_Deductible_Donations = 1 THEN DD.Amount ELSE 0 END
	, SH_6 = CASE WHEN SH.Header_Sort = 6 AND D.Payment_Type_ID <> 6 AND Prog.Tax_Deductible_Donations = 1 THEN DD.Amount ELSE 0 END
	, SH_O = CASE WHEN ISNULL(SH.Header_Sort,99) >= @OtherColumnStartsAt AND D.Payment_Type_ID <> 6 AND Prog.Tax_Deductible_Donations = 1 THEN DD.Amount ELSE 0 END
	, Other_Note = CASE WHEN ISNULL(SH.Header_Sort,99) >= @OtherColumnStartsAt THEN COALESCE(E.Event_Title,Prog.Statement_Title) ELSE NULL END
	, ISNULL(C.Gender_ID,9) AS Gender_ID
	, ISNULL(C.Household_Position_ID,9) AS Household_Position_ID
	, CampaignName = #D.Pledge_Name
	, Is_Recurring_Gift
	, Section_Title = CAST(CASE WHEN D.Payment_Type_ID <> 6 AND Prog.Tax_Deductible_Donations = 1 THEN 'Tax-Deductible Information:' ELSE '' END AS VARCHAR(100))
	, Header_Title = CAST(CASE WHEN D.Payment_Type_ID = 6 THEN 'Stock Giving' ELSE 'Giving' END AS VARCHAR(50))
	, Section_Sort = CASE WHEN D.Payment_Type_ID = 6 THEN 3 ELSE 2 END

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
	 INNER JOIN Statement_Headers SH ON SH.Statement_Header_ID = Prog.Statement_Header_ID
	 INNER JOIN Payment_Types PT ON PT.Payment_Type_ID = D.Payment_Type_ID
	 INNER JOIN Donation_Statuses DS ON DS.Donation_Status_ID  = D.Donation_Status_ID
	 LEFT OUTER JOIN dbo.[Events] E ON E.Event_ID = DD.Target_Event
	 LEFT OUTER JOIN Pledges Pl ON PL.Pledge_ID = DD.Pledge_ID
	 LEFT OUTER JOIN Pledge_Campaigns PC ON PC.Pledge_Campaign_ID = PL.Pledge_Campaign_ID
	 INNER JOIN #D ON #D.Donor_ID = D.Donor_ID --AND PL.Pledge_ID = #D.Pledge_ID
	 LEFT OUTER JOIN Donors PLDo ON PLDo.Donor_ID = PL.Donor_ID
	 LEFT OUTER JOIN Contacts PLDoC ON PLDoC.Contact_ID = PLDo.Contact_ID
	
	WHERE Year(Donation_Date) = @StmtYr
	 AND DS.Display_On_Statements = 1
	 AND Donation_Date < @ThroughDate+1
	 AND @DomainID = Dom.Domain_GUID
	 AND ISNULL(Prog.Tax_Deductible_Donations,0) = 1 --Omit Non Deductible
	 AND Do.Statement_Method_ID <> 4
	 AND Do.Statement_Frequency_ID <> 3
	 AND ISNULL(Prog.Statement_Header_ID,0) <> ISNULL(@OmitStmtHeaderID,-1)
	 AND ((@EnvelopeOnly = 1 AND Do.Envelope_No IS NOT NULL) OR (@EnvelopeOnly = 0 AND Do.Envelope_No IS NULL) OR @EnvelopeOnly IS NULL)
     AND D.Donation_Status_ID = 2 -- Deposited

--ADD SOFT CREDITS
INSERT INTO #Gifts(Donation_ID
	, Donation_Distribution_ID
	, Donation_Date
	, Amount
	, Non_Deductible_Amount
	, Deductible_Amount
	, Payment_Type
	, Payment_Type_ID
	, Item_Number
	, Fund_Name
	, Statement_Header
	, Header_Sort
	, Donation_Detail
	, Donor_Name
	, Contact_ID
	, Event_Name
	, On_Pledge
	, Mail_Name
	, Address_Line_1
	, Apt_or_Suite 
	, City
	, [State]
	, Postal_Code
	, Foreign_Country
	, Statement_ID
	, Envelope_No
	, PLEDGE
	, Given_To_Pledge
	, Donor_ID
	, SH_1
	, SH_2
	, SH_3
	, SH_4
	, SH_5
	, SH_6
	, SH_O
	, Other_Note
	, Gender_ID
	, Household_Position_ID
	, Campaign_Name
	, Is_Recurring_Gift
	, Section_Title
	, Header_Title
	, Section_Sort
)
SELECT Top 100 PERCENT D.Donation_ID
	, DD.Donation_Distribution_ID
	, D.Donation_Date
	, DD.Amount
	, DD.Amount AS Non_Deductible_Amount
	, 0 AS Deductible_Amount--Even though it is a Non-Deductible, it needs to be in this field to show on the report
	, PT.Payment_Type
	, 0
	, D.Item_Number
	, Prog.Statement_Title AS Fund_Name
	, ISNULL(SH.Statement_Header,'Other') AS Statement_Header
	, ISNULL(SH.Header_Sort,99) AS Header_Sort
	, Donation_Detail = COALESCE(PLDoC.Last_Name,PLDoC.Company_Name,PLDoC.Display_Name) + ISNULL(', ' + PLDoC.First_Name,'') --Donated by
	, COALESCE(C.Last_Name,C.Company_Name,C.Display_Name) + ISNULL(', ' + C.First_Name,'') + ISNULL(' & ' + SP.First_Name,'') AS Donor_Name
	, C.Contact_ID
	, E.Event_Title AS Event_Name
	, On_Pledge = CASE WHEN ISNULL(#D.Pledge_ID,0) = 0 THEN '' ELSE 'Yes' END
	, Mail_Name = CASE WHEN C.Company = 1 THEN C.Display_Name
					WHEN SP.Last_Name <> C.Last_Name THEN C.First_Name + SPACE(1) + C.Last_Name + ' & ' + SP.First_Name + SPACE(1) + Sp.Last_Name
					WHEN SP.Gender_ID < C.Gender_ID THEN ISNULL(SP.First_Name + ' & ','') + C.First_Name + Space(1) + C.Last_Name
					ELSE C.First_Name + ISNULL(' & ' + SP.First_Name,'') + SPACE(1) + C.Last_Name END
	, A.Address_Line_1
	, A.Address_Line_2 Apt_or_Suite
	, A.City
	, A.[State/Region] AS [State]
	, A.Postal_Code
	, CASE A.Foreign_Country WHEN 'USA' THEN NULL WHEN 'US' THEN NULL WHEN 'United States' THEN NULL ELSE UPPER(A.Foreign_Country) END AS Foreign_Country
	, #DONORS.Statement_ID-- = CASE WHEN @CombineFamily = 0 OR C.Household_ID IS NULL OR Do.Statement_Type_ID = 1  THEN 'C' + CONVERT(Varchar(10),C.Contact_ID) ELSE 'F' + CONVERT(Varchar(10),H.Household_ID) END
	, Do.Envelope_No
	, ISNULL(#D.Pledge,0) AS PLEDGE
	, ISNULL(#D.Given_to_Pledge,0) AS Given_To_Pledge
	, Donor_ID = DD.Soft_Credit_Donor
	, SH_1 = CASE WHEN SH.Header_Sort = 1 AND D.Payment_Type_ID <> 6 AND Prog.Tax_Deductible_Donations = 1 THEN DD.Amount ELSE 0 END
	, SH_2 = CASE WHEN SH.Header_Sort = 2 AND D.Payment_Type_ID <> 6 AND Prog.Tax_Deductible_Donations = 1 THEN DD.Amount ELSE 0 END
	, SH_3 = CASE WHEN SH.Header_Sort = 3 AND D.Payment_Type_ID <> 6 AND Prog.Tax_Deductible_Donations = 1 THEN DD.Amount ELSE 0 END
	, SH_4 = CASE WHEN SH.Header_Sort = 4 AND D.Payment_Type_ID <> 6 AND Prog.Tax_Deductible_Donations = 1 THEN DD.Amount ELSE 0 END
	, SH_5 = CASE WHEN SH.Header_Sort = 5 AND D.Payment_Type_ID <> 6 AND Prog.Tax_Deductible_Donations = 1 THEN DD.Amount ELSE 0 END
	, SH_6 = CASE WHEN SH.Header_Sort = 6 AND D.Payment_Type_ID <> 6 AND Prog.Tax_Deductible_Donations = 1 THEN DD.Amount ELSE 0 END
	, SH_O = CASE WHEN ISNULL(SH.Header_Sort,99) >= @OtherColumnStartsAt AND D.Payment_Type_ID <> 6 AND Prog.Tax_Deductible_Donations = 1 THEN DD.Amount ELSE 0 END
	, Other_Note = CASE WHEN ISNULL(SH.Header_Sort,99) >= @OtherColumnStartsAt THEN COALESCE(E.Event_Title,Prog.Statement_Title) ELSE NULL END
	, ISNULL(C.Gender_ID,9) AS Gender_ID
	, ISNULL(C.Household_Position_ID,9) AS Household_Position_ID
	, CampaignName = #D.Pledge_Name
	, Is_Recurring_Gift
	, Section_Title = 'Non Tax-Deductible Information through Crossroads: (You will receive a tax-deductible statement from the trust or foundation.)' 
	, Header_Title = 'Trust and Foundation Giving' 
	, Section_Sort = 1
	FROM Donations D
	 INNER JOIN dp_Domains Dom ON Dom.Domain_ID = D.Domain_ID
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
	 LEFT OUTER JOIN dbo.[Events] E ON E.Event_ID = DD.Target_Event
	 LEFT OUTER JOIN Pledges Pl ON PL.Pledge_ID = DD.Pledge_ID
	 LEFT OUTER JOIN Pledge_Campaigns PC ON PC.Pledge_Campaign_ID = PL.Pledge_Campaign_ID
	 INNER JOIN #D ON #D.Donor_ID = DD.Soft_Credit_Donor --AND PL.Pledge_ID = #D.Pledge_ID
	 LEFT OUTER JOIN Donors PLDo ON PLDo.Donor_ID = D.Donor_ID
	 LEFT OUTER JOIN Contacts PLDoC ON PLDoC.Contact_ID = PLDo.Contact_ID

	WHERE Year(Donation_Date) = @StmtYr
	 AND DS.Display_On_Statements = 1
	 AND Donation_Date < @ThroughDate+1
	 AND @DomainID = Dom.Domain_GUID
	 AND ISNULL(Prog.Tax_Deductible_Donations,0) = 1 --Omit Non Deductible
	 AND Do.Statement_Method_ID <> 4
	 AND Do.Statement_Frequency_ID <> 3
	 AND ISNULL(Prog.Statement_Header_ID,0) <> ISNULL(@OmitStmtHeaderID,-1)
	 AND ((@EnvelopeOnly = 1 AND Do.Envelope_No IS NOT NULL) OR (@EnvelopeOnly = 0 AND Do.Envelope_No IS NULL) OR @EnvelopeOnly IS NULL)
     AND D.Donation_Status_ID = 2 -- Deposited
	 AND @AddSoftCredit  = 1

CREATE INDEX IX_Gifts_DonorID ON #Gifts(Donor_ID)

SELECT Donation_ID
	, Donation_Distribution_ID
	, Donation_Date
	, Amount
	, Non_Deductible_Amount
	, Deductible_Amount
	, Payment_Type
	, Payment_Type_ID
	, Item_Number = CASE
						WHEN Payment_Type_ID = 6 THEN ISNULL(Donation_Detail,'Non-cash')
						WHEN Payment_Type_ID = 0 THEN Donation_Detail
						WHEN Payment_Type_ID = 2 THEN 'Cash'
						WHEN Payment_Type_ID = 1 THEN CONCAT('Check #', Item_Number)
						WHEN Payment_Type_ID = 5 AND Is_Recurring_Gift = 1 THEN 'Recurring ACH'
						WHEN Payment_Type_ID = 5 THEN 'One-time ACH'
						WHEN Payment_Type_ID = 4 AND Is_Recurring_Gift = 1 THEN 'Recurring Credit Card'
						WHEN Payment_Type_ID = 4 THEN 'One-time Credit Card'
						ELSE ISNULL(Item_Number,Payment_Type)
					END
	, Fund_Name
	, Donation_Detail
	, Donor_Name
	, Contact_ID
	, Event_Name
	, On_Pledge
	, Mail_Name
	, Address_Line_1
	, Apt_or_Suite
	, City
	, [State]
	, Postal_Code
	, Foreign_Country
	, Statement_ID
	, Row_No = DENSE_RANK() OVER (Partition By Statement_ID ORDER BY Donation_Date,Donation_Distribution_ID)
	, Donor_Row_No = DENSE_Rank() OVER (Partition By Donor_ID ORDER BY Donation_Date,Donation_ID)
	, Page_No = Ceiling(Dense_Rank() OVER (Partition By Statement_ID ORDER BY Donation_Date,Donation_Distribution_ID)/@RowsPerPage)
	, Envelope_No
	, Statement_Header
	, Header_Sort
	, Pledge
	, Given_to_Pledge
	, SH_1
	, SH_2
	, SH_3
	, SH_4
	, SH_5
	, SH_6
	, SH_O
	, CASE WHEN Payment_Type_ID = 6 THEN Donation_Detail ELSE Other_Note END AS Other_Note
	, Gender_ID
	, Household_Position_ID
	, Campaign_Name
	, Section_Title
	, Header_Title
	, Section_Sort
FROM #Gifts

DROP TABLE #Campaigns
DROP TABLE #D
IF OBJECT_ID('tempdb..#DD') IS NOT NULL DROP TABLE #DD
DROP TABLE #DONORS
DROP TABLE #Gifts
DROP TABLE #PL
DROP TABLE #PLAmt


END

GO
