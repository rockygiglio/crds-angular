SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON

INSERT INTO [dp_Page_Views]([Page_View_ID],[View_Title],[Page_ID],[Description],[Field_List],[View_Clause],[Order_By],[User_ID],[User_Group_ID])
VALUES(2206,'My Current Groups ',322,NULL,'Groups.[Group_Name]
, Group_Type_ID_Table.[Group_Type_ID]
, Group_Type_ID_Table_Default_Role_Table.[Group_Role_ID]
, Group_Type_ID_Table_Default_Role_Table_Group_Role_Type_ID_Table.[Group_Role_Type_ID]
, Congregation_ID_Table.[Congregation_ID]
, Meeting_Day_ID_Table.[Meeting_Day]
, Groups.[Meeting_Time]','((Groups.End_Date IS NULL OR Groups.End_Date >= GetDate()) AND Groups.Group_ID in 
(Select Group_ID 
from dbo.Group_Participants
inner join dbo.Participants on dbo.Group_Participants.Participant_ID = dbo.Participants.Participant_ID
inner join dbo.Contacts on dbo.Participants.Contact_ID = dbo.Contacts.Contact_ID
inner join dbo.dp_Users on dbo.Contacts.Contact_ID = dbo.dp_Users.Contact_ID
where dbo.dp_Users.User_ID = dp_UserID)
)',NULL,NULL,NULL)

SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF