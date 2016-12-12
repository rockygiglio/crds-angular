USE [MinistryPlatform]
GO
IF EXISTS (SELECT * FROM [dbo].[dp_Page_Views] WHERE [Page_View_ID] = 361)
BEGIN
    UPDATE [dbo].[dp_Page_Views]
       SET [Field_List] = 'Batches.Setup_Date
                          ,Batches.Batch_Name
                          ,Batch_Type_ID_Table.Batch_Type
                          ,Batches.Batch_Total
                          ,(SELECT SUM(Donation_Amount) FROM Donations WHERE Donations.Batch_ID = Batches.Batch_ID) AS [Donation Total]
                          ,(SELECT SUM(Payment_Total) FROM Payments WHERE Payments.Batch_ID = Batches.Batch_ID) AS [Payment Total]
                          ,(SELECT SUM(Donation_Amount) FROM Donations WHERE Donations.Batch_ID = Batches.Batch_ID AND Payment_Type_ID = 1) AS [Check Total]
                          ,(SELECT SUM(Donation_Amount) FROM Donations WHERE Donations.Batch_ID = Batches.Batch_ID AND Payment_Type_ID = 2) AS [Cash Total]
                          ,(SELECT SUM(Donation_Amount) FROM Donations WHERE Donations.Batch_ID = Batches.Batch_ID AND Payment_Type_ID = 3) AS [Coin Total]
                          ,(SELECT SUM(Donation_Amount) FROM Donations WHERE Donations.Batch_ID = Batches.Batch_ID AND Payment_Type_ID = 4) AS [Credit Donation Total]
                          ,(SELECT SUM(Donation_Amount) FROM Donations WHERE Donations.Batch_ID = Batches.Batch_ID AND Payment_Type_ID = 5) AS [ACH Donation Total]
                          ,(SELECT SUM(Payment_Total) FROM Payments WHERE Payments.Batch_ID = Batches.Batch_ID AND Payment_Type_ID = 4) AS [Credit Payments Total]
                          ,(SELECT SUM(Payment_Total) FROM Payments WHERE Payments.Batch_ID = Batches.Batch_ID AND Payment_Type_ID = 5) AS [ACH Payments Total]
                          ,(SELECT SUM(Donation_Amount) FROM Donations WHERE Donations.Batch_ID = Batches.Batch_ID AND Payment_Type_ID = 6) AS [Non Cash Total]'
         WHERE [Page_View_ID] = 361
END
GO


