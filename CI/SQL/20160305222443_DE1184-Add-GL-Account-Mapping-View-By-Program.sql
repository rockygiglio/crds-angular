USE MinistryPlatform
GO

SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON

INSERT INTO [dp_Page_Views](
  [Page_View_ID],
  [View_Title],
  [Page_ID],
  [Description],
  [Field_List],
  [View_Clause],
  [Order_By],
  [User_ID],
  [User_Group_ID])
VALUES(2213,
  'GL Account Mapping By Program',
  504,
  NULL,
  'Program_ID_Table.[Program_ID] AS [Program_ID]
, GL_Account_Mapping.[GL_Account] AS [GL_Account]
, GL_Account_Mapping.[GL_Account_Mapping_ID] AS [GL_Account_Mapping_ID]
, GL_Account_Mapping.[Checkbook_ID] AS [Checkbook_ID]
, GL_Account_Mapping.[Cash_Account] AS [Cash_Account]
, GL_Account_Mapping.[Receivable_Account] AS [Receivable_Account]
, GL_Account_Mapping.[Distribution_Account] AS [Distribution_Account]
, GL_Account_Mapping.[Document_Type] AS [Document_Type]
, GL_Account_Mapping.[Customer_ID] AS [Customer_ID]
, GL_Account_Mapping.[Scholarship_Expense_Account] AS [Scholarship_Expense_Account]',
  '1=1',
  NULL,
  NULL,
  NULL)

SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
