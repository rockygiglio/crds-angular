USE [MinistryPlatform]
GO

/****** Object:  StoredProcedure [dbo].[report_CRDS_Payment_Selected_Create_Batch]    Script Date: 1/6/2017 11:51:53 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[report_CRDS_Payment_Selected_Create_Batch]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[report_CRDS_Payment_Selected_Create_Batch] AS' 
END
GO



ALTER PROCEDURE [dbo].[report_CRDS_Payment_Selected_Create_Batch]

	@DomainID varchar(40)
	,@UserID varchar(40)
	,@PageID Int
	,@SelectionID Int
	,@CongregationID Int
	,@BatchName Varchar(50)
	,@DepositDate DateTime
	,@OutputString Varchar(50) OUTPUT
	,@BatchEntryTypeID Int
	,@FinalizeAndDeposit BIT = 1

AS
BEGIN

CREATE Table #TD (Payment_ID INT)
INSERT INTO #TD (Payment_ID)
SELECT Payment_ID 
FROM dbo.Payments
WHERE Payment_ID IN (SELECT Record_ID FROM dp_Selected_Records SR INNER JOIN dp_Selections S ON S.Selection_ID = SR.Selection_ID  INNER JOIN dbo.dp_Users U ON U.[User_ID] = S.[User_ID] AND U.User_GUID = @UserID WHERE S.Page_ID = @PageID AND ((S.Selection_ID = @SelectionID AND @SelectionID > 0) OR (S.Selection_Name = 'dp_DEFAULT' AND @SelectionID < 1 AND Sub_Page_ID IS NULL)))

DECLARE @FinalizeDate DateTime
SET @FinalizeDate = CASE WHEN @FinalizeAndDeposit = 1 THEN GETDATE() ELSE NULL END

--Create Batch Record from selected donations

INSERT INTO [dbo].[Batches]
           ([Batch_Name]
           ,[Setup_Date]
           ,[Batch_Total]
           ,[Item_Count]
           ,[Batch_Entry_Type_ID]
           ,[Finalize_Date]
			,Congregation_ID
           ,[Domain_ID])

SELECT [Batch_Name] = @BatchName
           ,[Setup_Date] = GetDate()
           ,[Batch_Total] = SUM(Payment_Total)
           ,[Item_Count] = Count(*)
           ,[Batch_Entry_Type_ID]= @BatchEntryTypeID
           ,[Finalize_Date] = @FinalizeDate
			,Congregation_ID = @CongregationID
           ,[Domain_ID] = D.Domain_ID

FROM Payments D
 INNER JOIN dp_Domains Dom ON Dom.Domain_ID = D.Domain_ID
WHERE D.Batch_ID IS NULL AND Dom.Domain_GUID = @DomainID
 AND D.Payment_ID IN (SELECT Payment_ID FROM #TD)

GROUP BY D.Domain_ID

--Get @BatchID from batch created

DECLARE @User_ID INT
DECLARE @UserName Varchar(50)

SELECT @UserName = [Display_Name], @User_ID = [User_ID] FROM dp_Users WHERE User_GUID = @UserID

DECLARE @BatchID INT
SET @BatchID = Scope_Identity()


INSERT INTO dp_Audit_Log (Table_Name,Record_ID,Audit_Description,[User_Name],[User_ID],Date_Time)
	VALUES ('Batches'
	,@BatchID
	,'Created'
	,@UserName
	,@User_ID
	,GETDATE())
	

--Update Payments With Batch ID

UPDATE dbo.Payments 
SET Batch_ID = @BatchID
WHERE Batch_ID IS NULL
 AND Payment_ID IN  (SELECT Payment_ID FROM #TD)

--Audit Log Change to Each Donation
	DECLARE @AuditItemID INT
	DECLARE @RecordID INT	

	DECLARE ALDonations CURSOR FAST_FORWARD FOR
	SELECT Payment_ID FROM #TD
	
	OPEN ALDonations
	FETCH NEXT FROM ALDonations INTO @RecordID
	WHILE @@FETCH_STATUS = 0
	BEGIN

	INSERT INTO dp_Audit_Log (Table_Name,Record_ID,Audit_Description,[User_Name],[User_ID],Date_Time)
	VALUES ('Payments'
	,@RecordID
	,'Mass Updated'
	,@UserName
	,@User_ID
	,GetDate())
	
	SET @AuditItemID = Scope_Identity()

	INSERT INTO dp_Audit_Detail (Audit_Item_ID,Field_Name,Field_Label,Previous_Value,New_Value,Previous_ID,New_ID)
	VALUES (@AuditItemID,'Batch_ID','Batch','',@BatchName,NULL,@BatchID)


	FETCH NEXT FROM ALDonations INTO @RecordID
	END

	CLOSE ALDonations
	DEALLOCATE ALDonations

	IF @FinalizeAndDeposit = 1
	BEGIN
	--Create Deposit Record

	INSERT INTO [dbo].[Deposits]
			   ([Deposit_Total]
			   ,[Deposit_Date]
			   ,[Account_Number]
			   ,[Batch_Count]
			   ,[Domain_ID]
			   ,[Deposit_Name])

	SELECT [Batch_Total]
			   ,[Deposit_Date] = @DepositDate
			   ,[Account_Number] = '0'--Parameterize
			   ,[Batch_Count] = 1
			   ,[Domain_ID] = B.Domain_ID
			   ,[Deposit_Name] = @BatchName

	FROM dbo.[Batches] B
	WHERE B.Deposit_ID IS NULL AND B.Batch_ID = @BatchID

	--Get @DepositID from recently created deposit

	DECLARE @DepositID INT
	SET @DepositID = Scope_Identity()

	INSERT INTO dp_Audit_Log (Table_Name,Record_ID,Audit_Description,[User_Name],[User_ID],Date_Time)
		VALUES ('Deposits'
		,@DepositID
		,'Created'
		,@UserName
		,@User_ID
		,GetDate())

	--Update Batch Record with DepositID

	UPDATE dbo.Batches
	SET Deposit_ID = @DepositID
	WHERE Batch_ID = @BatchID AND Deposit_ID IS NULL
	
	----Update Donation Distributions with default target event and pledges

	--CREATE TABLE #D (Donation_Distribution_ID Int)
	--INSERT INTO #D (Donation_Distribution_ID)
	--SELECT Donation_Distribution_ID 
	--FROM Deposits De
	-- INNER JOIN Batches Ba ON Ba.Deposit_ID = DE.Deposit_ID 
	-- INNER JOIN Donations D ON D.Batch_ID = Ba.Batch_ID
	-- INNER JOIN Donation_Distributions DD ON DD.Donation_ID = D.Donation_ID
	--WHERE De.Deposit_ID = @DepositID 

	--DECLARE @DonationDistributionID INT, @ProgramID INT, @BestPledgeID INT, @DefaultEventID INT, @CurrentEventID INT, @CurrentPledgeID INT

	--	DECLARE CursorD CURSOR FAST_FORWARD FOR
	--		SELECT Donation_Distribution_ID 
	--		FROM #D
			
	--	OPEN CursorD
	--	FETCH NEXT FROM CursorD INTO @DonationDistributionID
	--		WHILE @@FETCH_STATUS = 0
	--		BEGIN
	--		SELECT Top 1 @ProgramID = DD.Program_ID
	--		,@DefaultEventID = Prog.Default_Target_Event
	--		,@CurrentEventID = DD.Target_Event
	--		,@BestPledgeID = PL.Pledge_ID
	--		,@CurrentPledgeID = DD.Pledge_ID 
	--		FROM Donation_Distributions DD
	--		 INNER JOIN Programs Prog ON Prog.Program_ID = DD.Program_ID
	--		 INNER JOIN Donations D ON D.Donation_ID = DD.Donation_ID 
	--		 LEFT OUTER JOIN Pledges PL ON PL.Donor_ID = D.Donor_ID AND PL.Pledge_Campaign_ID = Prog.Pledge_Campaign_ID AND Pledge_Status_ID  = 1-- AND GETDATE() BETWEEN PL.First_Installment_Date AND ISNULL(PL._Last_Installment_Date, GETDATE())
	--		WHERE DD.Donation_Distribution_ID  = @DonationDistributionID

	--		--Update Target Event if null and default not null
	--		IF @CurrentEventID IS NULL AND @DefaultEventID IS NOT NULL
	--		BEGIN
	--		UPDATE Donation_Distributions SET Target_Event = @DefaultEventID WHERE Donation_Distribution_ID = @DonationDistributionID 
	--		END

	--		--Update Pledge if null and best not null
	--		IF @CurrentPledgeID IS NULL AND @BestPledgeID IS NOT NULL
	--		BEGIN
	--		UPDATE Donation_Distributions SET Pledge_ID = @BestPledgeID WHERE Donation_Distribution_ID = @DonationDistributionID 
	--		END

	--	FETCH NEXT FROM CursorD INTO @DonationDistributionID
	--	END

	--	CLOSE CursorD
	--	DEALLOCATE CursorD

		
	--	DROP Table #D

	
	END


DELETE FROM dp_Selected_Records WHERE SELECTION_ID IN (SELECT S.Selection_ID FROM dp_Selections S INNER JOIN dp_Users U ON U.[User_ID] = S.[User_ID] WHERE @PageID = S.Page_ID AND @UserID = U.[User_GUID] AND ((S.Selection_ID = @SelectionID AND @SelectionID > 0) OR (S.Selection_Name = 'dp_DEFAULT' AND @SelectionID < 1 AND S.Sub_Page_ID IS NULL)))
DELETE FROM dp_Selected_Contacts WHERE SELECTION_ID IN (SELECT S.Selection_ID FROM dp_Selections S  INNER JOIN dp_Users U ON U.[User_ID] = S.[User_ID] WHERE @PageID = S.Page_ID AND @UserID = U.[User_GUID] AND ((S.Selection_ID = @SelectionID AND @SelectionID > 0) OR (S.Selection_Name = 'dp_DEFAULT' AND @SelectionID < 1 AND S.Sub_Page_ID IS NULL)))
--DELETE FROM dp_Selections WHERE @PageID = Page_ID AND [User_ID] IN (SELECT U.[User_ID] FROM dp_Users U WHERE U.User_GUID = @UserID) AND ((Selection_ID = @SelectionID AND @SelectionID > 0) OR (Selection_Name = 'dp_DEFAULT' AND @SelectionID < 1 AND Sub_Page_ID IS NULL))

--Output

SELECT CASE WHEN @BatchID > 0 AND @DepositID > 0 THEN @OutputString + '. Batch ID: ' + CONVERT(Varchar(9),@BatchID) + ' & Deposit ID: '  + CONVERT(Varchar(9),@DepositID) WHEN @BatchID > 0 THEN @OutputString + '. Batch ID: ' + Convert(Varchar(20), @BatchID)  ELSE 'The Batch Was Not Created' END AS Result

DROP TABLE #TD

END


GO


