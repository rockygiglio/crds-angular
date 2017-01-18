USE MinistryPlatform
GO

DECLARE @VIEW_ID int = 1114;

IF EXISTS (SELECT 1 FROM [dbo].[dp_Page_Views] WHERE [Page_View_ID] = @VIEW_ID) 
BEGIN
	UPDATE [dbo].[dp_Page_Views] 
	SET [View_Clause] = N'Batch_ID_Table.[Batch_ID] IS NULL AND Payments.[Payment_Date] <= GETDATE()' 
	WHERE [Page_View_ID] = @VIEW_ID
END