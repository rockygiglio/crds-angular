use MinistryPlatform
GO

DECLARE @subpageID int = 609;
DECLARE @batchPageID int = 282;
DECLARE @paymentPageID int = 359;
DECLARE @relationTypeID int = 1;

IF NOT EXISTS (SELECT 1 FROM dp_Sub_Pages WHERE Sub_Page_ID = @subpageID) 
BEGIN
	SET IDENTITY_INSERT [dbo].[dp_Sub_Pages] ON;
	INSERT INTO dp_Sub_Pages (
		 [Sub_Page_ID]
		,[Display_Name]
		,[Singular_Name]
		,[Page_ID]
		,[View_Order]
		,[Link_To_Page_ID]
		,[Link_From_Field_Name]
		,[Select_To_Page_ID]
		,[Select_From_Field_Name]
		,[Primary_Table]
		,[Primary_Key]
		,[Default_Field_List]
		,[Selected_Record_Expression]
		,[Filter_Key]
		,[Relation_Type_ID]
		,[Contact_ID_Field]
		,[Start_Date_Field]
		,[Display_Copy]
	) VALUES (
		 @subpageID
		,N'Payments'
		,N'Payment'
		,@batchPageID
		,2
		,@paymentPageID
		,N'Payment_ID'
		,@paymentPageID
		,N'Payments.Payment_ID'
		,N'Payments'
		,N'Payment_ID'
		,N'Contact_ID_Table.Display_Name,Payments.Payment_Total,(SELECT SUM(Payment_Amount) FROM Payment_Detail DD WHERE DD.Payment_ID = Payments.Payment_ID) AS Payment_Details_Amount,Payment_Type_ID_Table.Payment_Type,Payments.Payment_Date'
		,N'Payments.Payment_Total'
		,N'Batch_ID'
		,@relationTypeID
		,N'Contact_ID_Table.Contact_ID'
		,N'Payment_Date'
		,1
	)

	SET IDENTITY_INSERT [dbo].[dp_Sub_Pages] OFF;
END