USE MinistryPlatform
GO

DECLARE @Group_Type_ID AS INT = 26

--New Group Type for Undivided
SET IDENTITY_INSERT Group_Types ON

INSERT INTO Group_Types ([Group_Type_ID],[Group_Type],[Description],[Domain_ID],[Default_Role],[Activity_Log_Start_Date],[Show_On_Group_Finder],[Show_On_Sign_Up_to_Serve])
VALUES(@Group_Type_ID, 'Undivided', 'Group type for UNDIVIDED for meaningful conversation to break down the racial divide in our city', 1, 16, NULL, 0, 0)

SET IDENTITY_INSERT Group_Types OFF


--New Group for Undivided Facilitators
SET IDENTITY_INSERT Groups ON

INSERT INTO [Groups]([Group_ID],[Group_Name],[Group_Type_ID],[Ministry_ID],[Congregation_ID],[Primary_Contact],[Description],[Start_Date],[End_Date],[Target_Size],[Parent_Group],[Priority_ID],[Enable_Waiting_List],[Small_Group_Information],[Offsite_Meeting_Address],[Group_Is_Full],[Available_Online],[Life_Stage_ID],[Group_Focus_ID],[Meeting_Time],[Meeting_Day_ID],[Descended_From],[Reason_Ended],[Domain_ID],[Check_in_Information],[Secure_Check-in],[Suppress_Nametag],[Suppress_Care_Note],[On_Classroom_Manager],[Promotion_Information],[Promote_to_Group],[Age_in_Months_to_Promote],[Promote_Weekly],[__ExternalGroupID],[__ExternalParentGroupID],[__IsPublic],[__ISBlogEnabled],[__ISWebEnabled],[Group_Notes],[Sign_Up_To_Serve],[Deadline_Passed_Message_ID],[Notifications],[Send_Attendance_Notification],[Send_Service_Notification],[Child_Care_Available],[Meeting_Frequency_ID],[Meeting_Duration_ID],[Required_Book],[Online_RSVP_Minimum_Age],[Enable_Discussion],[Small_Group_Information_2],[Remaining_Capacity])
VALUES(166572,'Undivided - Registered Facilitators',@Group_Type_ID,8,5,7592977,'This is the Crossroads Undivided Facilitators holding group, which contains community members who need to be placed into Undivided Facilitators Group',CAST('20160615 00:00:00.000' as DATETIME),CAST('20160915 00:00:00.000' as DATETIME),NULL,NULL,NULL,0,NULL,NULL,0,0,NULL,NULL,NULL,NULL,NULL,NULL,1,NULL,0,NULL,0,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,0,0,0,NULL,NULL,NULL,NULL,NULL,NULL,NULL)

SET IDENTITY_INSERT Groups OFF


--New Group for Undivided Participants
SET IDENTITY_INSERT Groups ON

INSERT INTO [Groups]([Group_ID],[Group_Name],[Group_Type_ID],[Ministry_ID],[Congregation_ID],[Primary_Contact],[Description],[Start_Date],[End_Date],[Target_Size],[Parent_Group],[Priority_ID],[Enable_Waiting_List],[Small_Group_Information],[Offsite_Meeting_Address],[Group_Is_Full],[Available_Online],[Life_Stage_ID],[Group_Focus_ID],[Meeting_Time],[Meeting_Day_ID],[Descended_From],[Reason_Ended],[Domain_ID],[Check_in_Information],[Secure_Check-in],[Suppress_Nametag],[Suppress_Care_Note],[On_Classroom_Manager],[Promotion_Information],[Promote_to_Group],[Age_in_Months_to_Promote],[Promote_Weekly],[__ExternalGroupID],[__ExternalParentGroupID],[__IsPublic],[__ISBlogEnabled],[__ISWebEnabled],[Group_Notes],[Sign_Up_To_Serve],[Deadline_Passed_Message_ID],[Notifications],[Send_Attendance_Notification],[Send_Service_Notification],[Child_Care_Available],[Meeting_Frequency_ID],[Meeting_Duration_ID],[Required_Book],[Online_RSVP_Minimum_Age],[Enable_Discussion],[Small_Group_Information_2],[Remaining_Capacity])
VALUES(166571,'Undivided - Registered Participants',@Group_Type_ID,8,5,7592977,'This is the Crossroads Undivided Participants holding group, which contains community members who need to be placed into Undivided Facilitators Group',CAST('20160615 00:00:00.000' as DATETIME),CAST('20160915 00:00:00.000' as DATETIME),NULL,NULL,NULL,0,NULL,NULL,0,0,NULL,NULL,NULL,NULL,NULL,NULL,1,NULL,0,NULL,0,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,0,0,0,NULL,NULL,NULL,NULL,NULL,NULL,NULL)

SET IDENTITY_INSERT Groups OFF