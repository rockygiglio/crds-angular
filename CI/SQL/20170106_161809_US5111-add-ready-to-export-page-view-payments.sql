USE MinistryPlatform
GO

DECLARE @PAGE_VIEW_ID int = 1114;
DECLARE @PAGE_ID int = 359;

IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_Page_Views] WHERE [Page_View_ID] = @PAGE_VIEW_ID) 
BEGIN
	SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON;
	INSERT INTO [dbo].[dp_Page_Views] (
		 [Page_View_ID]
		,[View_Title]
		,[Page_ID]
		,[Field_List]
		,[View_Clause]
	) VALUES (
		 @PAGE_VIEW_ID
		,N'3. Ready For a Batch'
		,@PAGE_ID
		,N'Payments.[Payment_Date] AS [Payment Date]
, Contact_ID_Table.[Display_Name] AS [Display Name]
, Contact_ID_Table.[Nickname] AS [Nickname]
, Contact_ID_Table.[First_Name] AS [First Name]
, Payments.[Payment_Total] AS [Payment Total]
, Payment_Type_ID_Table.[Payment_Type] AS [Payment Type]
, Payments.[Item_Number] AS [Item Number]
, Payments.[Transaction_Code] AS [Transaction Code]
, Batch_ID_Table.[Batch_ID] AS [Batch ID]
, Batch_ID_Table.[Setup_Date] AS [Setup Date]'
		,N'Batch_ID_Table.[Batch_ID] IS NULL
 AND Batch_ID_Table.[Setup_Date] <= GETDATE()'
	)
	SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF;
END