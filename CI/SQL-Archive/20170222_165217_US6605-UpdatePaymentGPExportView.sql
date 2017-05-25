USE MinistryPlatform
GO

DECLARE @PageViewId int = 1112

IF EXISTS (SELECT 1 FROM dbo.dp_Page_Views WHERE Page_View_ID = @PageViewId)
BEGIN
	UPDATE dp_Page_Views
	SET Field_List = 'Payment_ID_Table_Batch_ID_Table_Deposit_ID_Table.[Deposit_ID] AS [Deposit_ID]
	, Payment_ID_Table_Batch_ID_Table.[Batch_ID] AS [Batch_ID]
	, Invoice_Detail_ID_Table_Product_ID_Table_Program_ID_Table.[Program_ID] AS [Program_ID]
	, Congregation_ID_Table.[Congregation_ID] AS [Congregation_ID]
	, (SELECT Document_Type FROM GL_Account_Mapping G WHERE G.Program_ID=Invoice_Detail_ID_Table_Product_ID_Table_Program_ID_Table.[Program_ID] AND G.Congregation_ID = Congregation_ID_Table.[Congregation_ID]) AS [Document_Type]
	, Payment_ID_Table.[Payment_ID] AS [Payment_ID]
	, Payment_ID_Table_Batch_ID_Table.[Batch_Name] AS [Batch_Name]
	, CAST (Payment_ID_Table.[Payment_Date] as date) AS [Payment_Date]
	, Payment_ID_Table_Batch_ID_Table_Deposit_ID_Table.[Deposit_Date] AS [Deposit_Date]
	, (SELECT Customer_ID FROM GL_Account_Mapping G WHERE G.Program_ID=Invoice_Detail_ID_Table_Product_ID_Table_Program_ID_Table.[Program_ID] AND G.Congregation_ID = Congregation_ID_Table.[Congregation_ID]) AS [Customer_ID]
	, Payment_ID_Table.[Payment_Total] AS [Payment_Total]
	, (SELECT Checkbook_ID FROM GL_Account_Mapping G WHERE G.Program_ID=Invoice_Detail_ID_Table_Product_ID_Table_Program_ID_Table.[Program_ID] AND G.Congregation_ID = Congregation_ID_Table.[Congregation_ID]) AS [Checkbook_ID]
	, (SELECT Cash_Account FROM GL_Account_Mapping G WHERE G.Program_ID=Invoice_Detail_ID_Table_Product_ID_Table_Program_ID_Table.[Program_ID] AND G.Congregation_ID = Congregation_ID_Table.[Congregation_ID]) AS [Cash_Account]
	, (SELECT Receivable_Account FROM GL_Account_Mapping G WHERE G.Program_ID=Invoice_Detail_ID_Table_Product_ID_Table_Program_ID_Table.[Program_ID] AND G.Congregation_ID = Congregation_ID_Table.[Congregation_ID]) AS [Receivable_Account]
	, (SELECT Distribution_Account FROM GL_Account_Mapping G WHERE G.Program_ID=Invoice_Detail_ID_Table_Product_ID_Table_Program_ID_Table.[Program_ID] AND G.Congregation_ID = Congregation_ID_Table.[Congregation_ID]) AS [Distribution_Account]
	, (SELECT Scholarship_Expense_Account FROM GL_Account_Mapping G WHERE G.Program_ID=Invoice_Detail_ID_Table_Product_ID_Table_Program_ID_Table.[Program_ID] AND G.Congregation_ID = Congregation_ID_Table.[Congregation_ID]) AS [Scholarship_Expense_Account]
	, Payment_Detail.[Payment_Amount] AS [Payment_Amount]
	, Payment_ID_Table_Batch_ID_Table_Deposit_ID_Table.[Exported] AS [Exported]
	, Payment_ID_Table_Payment_Type_ID_Table.[Payment_Type_ID] AS [Payment_Type_ID]
	, Payment_ID_Table.[Processor_Fee_Amount] AS [Processor_Fee_Amount]
	, Payment_ID_Table_Batch_ID_Table_Deposit_ID_Table.[Deposit_Amount] AS [Deposit_Amount]'
	WHERE Page_View_ID = @PageViewId
END 