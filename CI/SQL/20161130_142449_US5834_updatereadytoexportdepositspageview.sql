USE [MinistryPlatform]
GO
IF EXISTS (SELECT * FROM [dbo].[dp_Page_Views] WHERE [Page_View_ID] = 559)
BEGIN
    UPDATE [dbo].[dp_Page_Views]
       SET [Field_List] = 'Deposits.Deposit_Date 
	                      ,Deposits.Deposit_Name 
						  ,Deposits.Deposit_ID AS DepositID 
						  ,(SELECT Max(Batch_Entry_Type) FROM dbo.Batches B INNER JOIN Batch_Entry_Types BET ON BET.Batch_Entry_Type_ID = B.Batch_Entry_Type_ID WHERE B.Deposit_ID = Deposits.Deposit_ID) AS Entry_Type 
						  ,Deposits.Deposit_Total 
						  ,Deposits.Batch_Count 
						  ,(SELECT SUM(Batch_Total) FROM Batches B WHERE B.Deposit_ID = Deposits.Deposit_ID) AS [Batch Total] 
						  ,(SELECT SUM(Donation_Amount) FROM Donations D INNER JOIN Batches B ON B.Batch_ID = D.Batch_ID WHERE B.Deposit_ID = Deposits.Deposit_ID) AS [Donation Total] 
						  ,(SELECT SUM(Payment_Total) FROM Payments P INNER JOIN Batches B ON B.Batch_ID = P.Batch_ID WHERE B.Deposit_ID = Deposits.Deposit_ID) AS [Payment Total] 
						  ,(SELECT SUM(Amount) FROM Donation_Distributions DD INNER JOIN Donations D ON D.Donation_ID = DD.Donation_ID INNER JOIN dbo.Batches B ON B.Batch_ID = D.Batch_ID WHERE B.Deposit_ID = Deposits.Deposit_ID) AS [Distribution Total] 
						  ,(SELECT SUM(Donation_Amount) FROM Donations D INNER JOIN Batches B ON B.Batch_ID = D.Batch_ID WHERE B.Deposit_ID = Deposits.Deposit_ID AND D.Payment_Type_ID = 1)  AS [Check Total] 
						  ,(SELECT SUM(Donation_Amount) FROM Donations D INNER JOIN Batches B ON B.Batch_ID = D.Batch_ID WHERE B.Deposit_ID = Deposits.Deposit_ID AND D.Payment_Type_ID = 2) AS [Cash Total]
						  ,(SELECT SUM(Donation_Amount) FROM Donations D INNER JOIN Batches B ON B.Batch_ID = D.Batch_ID WHERE B.Deposit_ID = Deposits.Deposit_ID AND D.Payment_Type_ID = 3) AS [Coin Total] 
						  ,(SELECT SUM(Donation_Amount) FROM Donations D INNER JOIN Batches B ON B.Batch_ID = D.Batch_ID WHERE B.Deposit_ID = Deposits.Deposit_ID AND D.Payment_Type_ID = 4) AS [Credit Total]
						  ,(SELECT SUM(Donation_Amount) FROM Donations D INNER JOIN Batches B ON B.Batch_ID = D.Batch_ID WHERE B.Deposit_ID = Deposits.Deposit_ID AND D.Payment_Type_ID = 5) AS [ACH Total]
						  ,(SELECT SUM(Donation_Amount) FROM Donations D INNER JOIN Batches B ON B.Batch_ID = D.Batch_ID WHERE B.Deposit_ID = Deposits.Deposit_ID AND D.Payment_Type_ID = 6) AS [Non Cash Total]'
	  WHERE [Page_View_ID] = 559
END
GO


