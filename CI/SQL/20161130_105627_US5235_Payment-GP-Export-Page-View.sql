USE [MinistryPlatform]
GO

DECLARE @PageViewID int = 1112;

IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_Page_Views] WHERE Page_View_ID = @PageViewID)
BEGIN
	SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON
	INSERT INTO [dbo].[dp_Page_Views]
			   ([Page_View_ID]
			   ,[View_Title]
			   ,[Page_ID]
			   ,[Field_List]
			   ,[View_Clause])
		 VALUES
			   (@PageViewID
			   ,'GP Export'
			   ,273 --All Payment Detail
			   ,'Payment_ID_Table_Batch_ID_Table_Deposit_ID_Table.[Deposit_ID]
	, Payment_ID_Table_Batch_ID_Table.[Batch_ID]
	, Invoice_Detail_ID_Table_Product_ID_Table_Program_ID_Table.[Program_ID]
	, Invoice_Detail_ID_Table_Recipient_Contact_ID_Table_Household_ID_Table_Congregation_ID_Table.[Congregation_ID]
	, (SELECT Document_Type FROM GL_Account_Mapping G WHERE G.Program_ID=Invoice_Detail_ID_Table_Product_ID_Table_Program_ID_Table.[Program_ID] AND G.Congregation_ID = Invoice_Detail_ID_Table_Recipient_Contact_ID_Table_Household_ID_Table_Congregation_ID_Table.[Congregation_ID]) 
	, Payment_ID_Table.[Payment_ID]
	, Payment_ID_Table_Batch_ID_Table.[Batch_Name]
	, Payment_ID_Table.[Payment_Date]
	, Payment_ID_Table_Batch_ID_Table_Deposit_ID_Table.[Deposit_Date]
	, (SELECT Customer_ID FROM GL_Account_Mapping G WHERE G.Program_ID=Invoice_Detail_ID_Table_Product_ID_Table_Program_ID_Table.[Program_ID] AND G.Congregation_ID = Invoice_Detail_ID_Table_Recipient_Contact_ID_Table_Household_ID_Table_Congregation_ID_Table.[Congregation_ID])
	, Payment_ID_Table.[Payment_Total]
	, (SELECT Checkbook_ID FROM GL_Account_Mapping G WHERE G.Program_ID=Invoice_Detail_ID_Table_Product_ID_Table_Program_ID_Table.[Program_ID] AND G.Congregation_ID = Invoice_Detail_ID_Table_Recipient_Contact_ID_Table_Household_ID_Table_Congregation_ID_Table.[Congregation_ID])
	, (SELECT Cash_Account FROM GL_Account_Mapping G WHERE G.Program_ID=Invoice_Detail_ID_Table_Product_ID_Table_Program_ID_Table.[Program_ID] AND G.Congregation_ID = Invoice_Detail_ID_Table_Recipient_Contact_ID_Table_Household_ID_Table_Congregation_ID_Table.[Congregation_ID])
	, (SELECT Receivable_Account FROM GL_Account_Mapping G WHERE G.Program_ID=Invoice_Detail_ID_Table_Product_ID_Table_Program_ID_Table.[Program_ID] AND G.Congregation_ID = Invoice_Detail_ID_Table_Recipient_Contact_ID_Table_Household_ID_Table_Congregation_ID_Table.[Congregation_ID])
	, (SELECT Distribution_Account FROM GL_Account_Mapping G WHERE G.Program_ID=Invoice_Detail_ID_Table_Product_ID_Table_Program_ID_Table.[Program_ID] AND G.Congregation_ID = Invoice_Detail_ID_Table_Recipient_Contact_ID_Table_Household_ID_Table_Congregation_ID_Table.[Congregation_ID])
	, (SELECT Scholarship_Expense_Account FROM GL_Account_Mapping G WHERE G.Program_ID=Invoice_Detail_ID_Table_Product_ID_Table_Program_ID_Table.[Program_ID] AND G.Congregation_ID = Invoice_Detail_ID_Table_Recipient_Contact_ID_Table_Household_ID_Table_Congregation_ID_Table.[Congregation_ID])
	, Payment_Detail.[Payment_Amount]
	, Payment_ID_Table_Batch_ID_Table_Deposit_ID_Table.[Exported]
	, Payment_ID_Table_Payment_Type_ID_Table.[Payment_Type_ID]
	, Payment_ID_Table.[Processor_Fee_Amount]
	, Payment_ID_Table_Batch_ID_Table_Deposit_ID_Table.[Deposit_Amount]'
			   ,'Payment_ID_Table_Batch_ID_Table_Deposit_ID_Table.[Exported] = 0'
	)
	SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
END

