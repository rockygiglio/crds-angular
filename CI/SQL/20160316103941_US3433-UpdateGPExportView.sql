USE [MinistryPlatform]
GO

SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON

UPDATE [dbo].[dp_Page_Views]
   SET [Field_List] = 'Donation_ID_Table_Batch_ID_Table_Deposit_ID_Table.[Deposit_ID] AS [Deposit_ID]
, Donation_ID_Table_Batch_ID_Table.[Batch_ID] AS [Batch_ID]
, Program_ID_Table.[Program_ID] AS [Program_ID]
, Congregation_ID_Table.[Congregation_ID] AS [Congregation_ID]
, (select Document_Type from GL_Account_Mapping G where G.Program_ID=Program_ID_Table.Program_ID and G.Congregation_ID = Congregation_ID_Table.Congregation_ID) AS [Document_Type]
, Donation_ID_Table.[Donation_ID] AS [Donation_ID]
, Donation_ID_Table_Batch_ID_Table.[Batch_Name] AS [Batch_Name]
, Donation_ID_Table.[Donation_Date] AS [Donation_Date]
, Donation_ID_Table_Batch_ID_Table_Deposit_ID_Table.[Deposit_Date] AS [Deposit_Date]
, (select Customer_ID from GL_Account_Mapping G where G.Program_ID=Program_ID_Table.Program_ID and G.Congregation_ID = Congregation_ID_Table.Congregation_ID) AS [Customer_ID]
, Donation_ID_Table.[Donation_Amount] AS [Donation_Amount]
, (select Checkbook_ID from GL_Account_Mapping G where G.Program_ID=Program_ID_Table.Program_ID and G.Congregation_ID = Congregation_ID_Table.Congregation_ID) AS [Checkbook_ID]
, (select Cash_Account from GL_Account_Mapping G where G.Program_ID=Program_ID_Table.Program_ID and G.Congregation_ID = Congregation_ID_Table.Congregation_ID) AS [Cash_Account]
, (select Receivable_Account from GL_Account_Mapping G where G.Program_ID=Program_ID_Table.Program_ID and G.Congregation_ID = Congregation_ID_Table.Congregation_ID) AS [Receivable_Account]
, (select Distribution_Account from GL_Account_Mapping G where G.Program_ID=Program_ID_Table.Program_ID and G.Congregation_ID = Congregation_ID_Table.Congregation_ID) AS [Distribution_Account]
, (select Scholarship_Expense_Account from GL_Account_Mapping G where G.Program_ID=Program_ID_Table.Program_ID and G.Congregation_ID = Congregation_ID_Table.Congregation_ID) AS [Scholarship_Expense_Account]
, Donation_Distributions.[Amount] AS [Amount]
, Donation_ID_Table_Batch_ID_Table_Deposit_ID_Table.[Exported] AS [Exported]
, Donation_ID_Table_Payment_Type_ID_Table.[Payment_Type_ID] AS [Payment_Type_ID]
,Donation_ID_Table.[Processor_Fee_Amount] AS [Processor_Fee_Amount],
Donation_ID_Table_Batch_ID_Table_Deposit_ID_Table.[Deposit_Amount] AS [Deposit_Amount]'
      ,[View_Clause] = 'Donation_ID_Table.[Donation_ID] IS NOT NULL
 AND Donation_ID_Table_Batch_ID_Table_Deposit_ID_Table.[Exported] = 0'
 WHERE Page_View_ID = 92198

SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF

GO
