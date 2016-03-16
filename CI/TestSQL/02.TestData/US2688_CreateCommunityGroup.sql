--Community Group Setup
USE [MinistryPlatform]
GO

--Declaring variables
DECLARE @groupName as varchar(75)
set @groupName = '(t) Life Group Oakley';

DECLARE @groupStartDate as varchar(19)
set @groupStartDate = {CONVERT(VARCHAR(19), getdate());

DECLARE @eventTitle as varchar(75)
set @eventTitle = '(t) Life Group Oakley';

DECLARE @eventStartDate as varchar(19)
set @eventStartDate = CONVERT(VARCHAR(19), getdate());

DECLARE @eventEndDate as varchar(19)
set @eventEndDate = CONVERT(VARCHAR(19), getdate());

DECLARE @contactEmail as varchar(255)
set @contactEmail = 'mpcrds+6@gmail.com';

DECLARE @participantStartDate as varchar(19)
set @participantStartDate = CONVERT(VARCHAR(19), getdate());

DECLARE @participantEndDate as varchar(19)
set @eventEndDate = CONVERT(VARCHAR(19), getdate());

--Allow specification of Group_ID
SET IDENTITY_INSERT [dbo].[Groups] ON;

--Store the current group_ID value so we can reset it.
DECLARE @group_id as int
set @group_id = IDENT_CURRENT('Groups');

--Add record to dbo.Groups table
INSERT INTO [dbo].Groups
(Group_ID,Group_Name,Group_Type_ID,Ministry_ID,Congregation_ID,Primary_Contact,Description,Start_Date                            ,End_Date,Target_Size,Parent_Group,Priority_ID,Enable_Waiting_List,Small_Group_Information,Offsite_Meeting_Address,Group_Is_Full,Available_Online,Life_Stage_ID,Group_Focus_ID,Meeting_Time,Meeting_Day_ID,Descended_From,Reason_Ended,Domain_ID,Check_in_Information,[Secure_Check-in],Suppress_Nametag,Suppress_Care_Note,On_Classroom_Manager,Promotion_Information,Promote_to_Group,Age_in_Months_to_Promote,Promote_Weekly,__ExternalGroupID,__ExternalParentGroupID,__IsPublic,__ISBlogEnabled,__ISWebEnabled,Group_Notes,Sign_Up_To_Serve,Deadline_Passed_Message_ID,Notifications,Send_Attendance_Notification,Send_Service_Notification,Child_Care_Available,Meeting_Frequency_ID,Meeting_Duration_ID,Required_Book,Online_RSVP_Minimum_Age,Enable_Discussion,Small_Group_Information_2,Remaining_Capacity) VALUES
(10000002,@groupName,8            ,8          ,1              ,2              ,null       ,CAST(@groupStartDate as smalldatetime),null    ,0          ,null        ,null       ,0                  ,null                   ,null                   ,0            ,1               ,null         ,null          ,null        ,null          ,null          ,null        ,1        ,null                ,0                ,0               ,0                 ,0                   ,null                 ,null            ,null                    ,0             ,null             ,null                   ,null      ,null           ,null          ,null       ,null            ,null                      ,null         ,0                           ,0                        ,1                   ,null                ,null               ,null         ,null                   ,null             ,null                     ,null              );

SET IDENTITY_INSERT [dbo].[Groups] OFF;

--This command resets the Group_ID value so that if someone adds Groups through the UI it won't use a big ID. 
DBCC CHECKIDENT (Groups, reseed, @group_id);

--Allow specification of Group_ID
SET IDENTITY_INSERT [dbo].[Events] ON;

--Store the current group_ID value so we can reset it.
DECLARE @event_id as int
set @event_id = IDENT_CURRENT('Events');

--Add record to dbo.Events table
INSERT INTO [dbo].Events
(Event_ID,Event_Title            ,Event_Type_ID,Congregation_ID,Location_ID,Meeting_Instructions,Description,Program_ID,Primary_Contact,Participants_Expected,Minutes_for_Setup,Event_Start_Date                      ,Event_End_Date                      ,Minutes_for_Cleanup,Cancelled,_Approved,Public_Website_Settings,Visibility_Level_ID,Featured_On_Calendar,Online_Registration_Product,Registration_Form,Registration_Start,Registration_End,Registration_Active,Register_Into_Series,External_Registration_URL,_Web_Approved,[Check-in_Information],[Allow_Check-in],Ignore_Program_Groups,Prohibit_Guests,[Early_Check-in_Period],[Late_Check-in_Period],Notification_Settings,Registrant_Message,Days_Out_to_Remind,Optional_Reminder_Message,Participant_Reminder_Settings,Send_Reminder,Reminder_Sent,Reminder_Days_Prior_ID,Other_Event_Information,Parent_Event_ID,Priority_ID,Domain_ID,On_Connection_Card,Accounting_Information,On_Donation_Batch_Tool,Project_Code,__Reservation_Start                   ,__Reservation_End                   ,__ExternalTripID,__ExternalTripLegID,__ExternalEventID,__ExternalOrganizerUserID,__ExternalGroupID,__ExternalRoomID,__ExternalContactUserID,__ExternalSignupFormRoldDateID) VALUES
(10000000,@eventTitle            ,80           ,1              ,3          ,null                ,null       ,111       ,2              ,null                 ,0                ,CAST(@eventStartDate as smalldatetime),CAST(@eventEndDate as smalldatetime),0                  ,0        ,1        ,null                   ,4                  ,0                   ,null                       ,null             ,null              ,null            ,0                  ,0                   ,null                     ,0            ,null                  ,0               ,0                    ,0              ,null                   ,null                  ,null                 ,null              ,null              ,null                     ,null                         ,0            ,0            ,2                     ,null                   ,null           ,null       ,1        ,0                 ,null                  ,0                     ,null        ,CAST(@eventStartDate as smalldatetime),CAST(@eventEndDate as smalldatetime),null            ,null               ,null             ,null                     ,null             ,null            ,null                   ,null                          );

SET IDENTITY_INSERT [dbo].[Events] OFF;

--This command resets the Event_ID value so that if someone adds Events through the UI it won't use a big ID. 
DBCC CHECKIDENT (Events, reseed, @event_id);

--Add Group to Event (dbo.Event_Groups)
INSERT INTO [dbo].Event_Groups
(Event_ID                                                     ,Group_ID                                                                ,Room_ID,Domain_ID,[__Secure_Check-in],Closed) VALUES
((Select Event_ID from Events where Event_Title = @eventTitle),(Select Group_ID from Groups where Group_Name = @groupName),null   ,1        ,null               ,null  );

--Add Room reservation
INSERT INTO [dbo].Event_Rooms
(Event_ID                                                                 ,Room_ID,Room_Layout_ID,Chairs,Tables,Notes  ,Domain_ID,_Approved,__Start_Time_Offset,__End_Time_Offset,Cancelled,__ExternalStartTime,__ExternalFinishTime,__ExternalSetupTime,__ExternalTeardownTime,__ExternalAudioVisual,Hidden) VALUES
((Select Event_ID from Events where Event_Title = @eventTitle),31         ,1             ,null  ,null  ,null   ,1        ,null     ,null               ,null             ,0        ,null               ,null                ,null               ,null                  ,null                 ,1     );

--Add Equipment
INSERT INTO [dbo].Event_Equipment
(Event_ID                                                                 ,Equipment_ID,Notes,Domain_ID,_Approved,__Event_Room_ID,__Start_Time_Offset,__End_Time_Offset,Desired_Placement_or_Location,Cancelled,Quantity_Requested) VALUES
((Select Event_ID from Events where Event_Title = @eventTitle),11         ,null ,1        ,null     ,null           ,null               ,null             ,null                         ,0        ,null              );

--Add a Participant
INSERT INTO [dbo].Group_Participants
(Group_ID                                                   ,Participant_ID                                                                                                                                                      ,Group_Role_ID,Domain_ID,Start_Date           ,End_Date           ,Employee_Role,Hours_Per_Week,Notes,__ExternalPersonGroupRoleID,__ExternalGroupRoleID,__CanManageEvents,__CanMANageMembers,__EmailOptOut,__ISAnonymous,__ServiceTimeID,_First_Attendance,_Second_Attendance,_Third_Attendance,_Last_Attendance,Child_Care_Requested,Share_With_Group,Email_Opt_Out,Need_Book) VALUES
((Select Group_ID from Groups where Group_Name = @groupName),(Select Participant_ID from Participants where Contact_ID = (Select Contact_ID from Contacts where Email_Address = @contactEmail and Participant_Record is not null),58           ,1        ,@participantStartDate,@participantEndDate,0            ,null          ,null ,null                       ,null                 ,null             ,null              ,null         ,null         ,null           ,null             ,null              ,null             ,null            ,0                   ,null            ,null         ,0        );

--Add recurring events (is this necessary?)

--Go through approval process (is this necessary? I don't think it is, data setup is what I need it to be without this.)

--Create signup page in CMS (need to figure out how to script adding data into CMS)

GO