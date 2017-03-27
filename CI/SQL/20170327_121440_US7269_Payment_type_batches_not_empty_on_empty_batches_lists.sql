USE [MinistryPlatform]
GO

UPDATE [dbo].[dp_Page_Views]
SET View_Clause = N'Batches.Setup_Date >= (SELECT top 1 CONVERT(DateTime,Value) FROM dp_Configuration_Settings CS WHERE ISDATE(CS.Value) = 1 AND CS.Key_Name = \'CutOverDate\' AND CS.Application_Code = \'COMMON\' AND CS.domain_ID = Batches.Domain_ID) AND NOT EXISTS (SELECT 1 FROM Donations WHERE Donations.Batch_ID = Batches.Batch_ID) AND NOT EXISTS (SELECT 1 FROM Payments WHERE Payments.Batch_ID = Batches.Batch_ID)'
GO
