USE MinistryPlatform
GO

DECLARE @PAGE_VIEW_ID int = 1113;
DECLARE @PAYMENT_PAGE_ID int = 359;

IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_Page_Views] where [Page_View_ID] = @PAGE_VIEW_ID)
BEGIN
	SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON
	INSERT INTO [dbo].[dp_Page_Views] (
		 [Page_View_ID]
		,[View_Title]
		,[Page_ID]
		,[Description]
		,[Field_List]
		,[View_Clause]		
	) VALUES (
		 @PAGE_VIEW_ID
		,N'Declined Payments'
		,@PAYMENT_PAGE_ID
		,N'All Declined Payments'
		,N'Payments.[Payment_ID] AS [Payment ID]
			, Payments.[Payment_Date] AS [Payment Date]
			, Contact_ID_Table.[Display_Name] AS [Display Name]
			, Contact_ID_Table.[First_Name] AS [First Name]
			, Contact_ID_Table.[Last_Name] AS [Last Name]
			, Contact_ID_Table.[Email_Address] AS [Email Address]
			, Payments.[Payment_Total] AS [Payment Total]
			, Payment_Type_ID_Table.[Payment_Type] AS [Payment Type]
			, Payments.[Invoice_Number] AS [Invoice Number]
			, [dp_Updated].[Date_Time] AS [Decline Date]
			, Payments.[Transaction_Code] AS [Transaction Code]'
		,N'Payment_Status_ID_Table.[Donation_Status] LIKE ''%Declined%'''
	)

	SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
END