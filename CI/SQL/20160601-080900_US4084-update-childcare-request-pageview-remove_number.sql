use MinistryPlatform
go

DECLARE @VIEW_ID int = 0
DECLARE @VIEW_TITLE nvarchar(255) = N'Pending Childcare Requests'



DECLARE @FIELD_LIST nvarchar(1000) = N'Requester_ID_Table.[Display_Name] AS [Requester], Group_ID_Table.[Group_Name] AS [Group], Ministry_ID_Table.[Ministry_Name] AS [Ministry]
, Congregation_ID_Table.[Congregation_Name] AS [Congregation], cr_Childcare_Requests.[Start_Date], cr_Childcare_Requests.[End_Date]
, cr_Childcare_Requests.[Frequency], cr_Childcare_Requests.[Childcare_Session], cr_Childcare_Requests.[Notes] AS [Notes], Request_Status_ID_Table.[Request_Status]'

SELECT @VIEW_ID = [Page_View_ID] FROM [dbo].[dp_Page_Views] WHERE [View_Title] = @VIEW_TITLE;

IF @VIEW_ID != 0
BEGIN
 UPDATE [dbo].[dp_Page_Views] SET Field_List = @FIELD_LIST WHERE Page_View_ID = @VIEW_ID;
END
