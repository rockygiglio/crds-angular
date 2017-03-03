USE [MinistryPlatform]
GO

INSERT INTO Groups 
(Group_Name           ,Group_Type_ID,Ministry_ID ,Congregation_ID,Primary_Contact,Description,Start_Date                 ,Target_Size,Group_Is_Full,Available_Online,Domain_ID,[Secure_Check-in],Suppress_Nametag,Suppress_Care_Note,On_Classroom_Manager,Promote_Weekly,__IsPublic,__ISBlogEnabled,__ISWebEnabled,Group_Notes,Sign_Up_To_Serve,Deadline_Passed_Message_ID,Send_Attendance_Notification,Send_Service_Notification,Child_Care_Available,Kids_Welcome) VALUES
('(t+auto) Avalanche',9            ,11          ,1              ,7654100         ,'Test data' ,{ts '2015-11-21 00:00:00'},0          ,0            ,1               ,1        ,0                ,0               ,0                 ,0                   ,0             ,'N'       ,'Y'            ,'Y'           ,''         ,null            ,null                      ,0                           ,0                        ,0                   ,0           );

-- getting the ID for the group
declare @GroupID as int
set @GroupID = (select group_id from Groups where Group_Name = '(t+auto) Avalanche');

--Saturday Attack Shinra Mako reactor 1
--Setup
INSERT INTO Opportunities 
(Opportunity_Title                      ,Description,Group_Role_ID,Program_ID,Visibility_Level_ID,Contact_Person,Publish_Date              ,Add_to_Group ,Minimum_Needed,Maximum_Needed,[Domain_ID],On_Connection_Card,Shift_Start   ,Shift_End     ,Event_Type_ID,Sign_Up_Deadline_ID,Room           ,Send_Reminder,Reminder_Template,Reminder_Days_Prior) VALUES
('(t+auto) Attack Shinra Mako reactor 1','Test Data',16           ,106       ,4                  ,7654100       ,{ts '2015-11-27 14:55:00'},@GroupID     ,1             ,250           ,1          ,0                 ,{t '14:00:00'},{t '16:00:00'},95           ,7                  ,'Oakley Atrium',1            ,108700           ,3                  );

--Attack Shinra Mako reactor 5
INSERT INTO Opportunities 
(Opportunity_Title                      ,Description,Group_Role_ID,Program_ID,Visibility_Level_ID,Contact_Person,Publish_Date              ,Add_to_Group ,Minimum_Needed,Maximum_Needed,[Domain_ID],On_Connection_Card,Shift_Start   ,Shift_End     ,Event_Type_ID,Sign_Up_Deadline_ID,Room                       ,Send_Reminder,Reminder_Template,Reminder_Days_Prior) VALUES
('(t+auto) Attack Shinra Mako reactor 5','Test Data',16           ,106       ,4                  ,7654100       ,{ts '2015-11-27 14:55:00'},@GroupID     ,1             ,250           ,1          ,0                 ,{t '16:00:00'},{t '18:00:00'},94           ,7                  ,'Main Coffee Station Alpha',1            ,108700           ,3                  );

