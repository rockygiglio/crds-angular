SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON

INSERT INTO [dp_Page_Views]([Page_View_ID],[View_Title],[Page_ID],[Description],[Field_List],[View_Clause],[Order_By],[User_ID],[User_Group_ID])
VALUES(2206,'My Current Groups ',461,NULL,'Groups.[Group_Name],
Groups.Group_ID,
Group_Type_ID_Table.[Group_Type_ID],
Group_Type_ID_Table_Default_Role_Table.[Group_Role_ID],
Congregation_ID_Table.[Congregation_ID],
Ministry_ID_Table.Ministry_ID,
Primary_Contact_Table.[Contact_ID] AS [Primary_Contact],
Groups.Description,
Groups.Start_Date,
Groups.End_Date,
Meeting_Day_ID_Table.[Meeting_Day_ID],
Groups.[Meeting_Time],
Groups.Available_Online,
Offsite_Meeting_Address_Table.[Address_Line_1],
Offsite_Meeting_Address_Table.[Address_Line_2],
Offsite_Meeting_Address_Table.[City],
Offsite_Meeting_Address_Table.[State/Region] AS [State],
Offsite_Meeting_Address_Table.[Postal_Code] AS [Zip_Code]','((Groups.End_Date IS NULL OR Groups.End_Date >= GetDate()) AND Groups.Group_ID in 
(Select Group_ID 
from dbo.Group_Participants
inner join dbo.Participants on dbo.Group_Participants.Participant_ID = dbo.Participants.Participant_ID
inner join dbo.Contacts on dbo.Participants.Contact_ID = dbo.Contacts.Contact_ID
inner join dbo.dp_Users on dbo.Contacts.Contact_ID = dbo.dp_Users.Contact_ID
where dbo.dp_Users.User_ID = dp_UserID)
)',NULL,NULL,NULL)

SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF