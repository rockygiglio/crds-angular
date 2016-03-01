USE MinistryPlatform
GO

SET IDENTITY_INSERT Groups ON

INSERT INTO [Groups]([Group_ID],[Group_Name],[Group_Type_ID],[Ministry_ID],[Congregation_ID],[Primary_Contact],[Description],[Start_Date],[End_Date],[Target_Size],[Parent_Group],[Priority_ID],[Enable_Waiting_List],[Small_Group_Information],[Offsite_Meeting_Address],[Group_Is_Full],[Available_Online],[Life_Stage_ID],[Group_Focus_ID],[Meeting_Time],[Meeting_Day_ID],[Descended_From],[Reason_Ended],[Domain_ID],[Check_in_Information],[Secure_Check-in],[Suppress_Nametag],[Suppress_Care_Note],[On_Classroom_Manager],[Promotion_Information],[Promote_to_Group],[Age_in_Months_to_Promote],[Promote_Weekly],[__ExternalGroupID],[__ExternalParentGroupID],[__IsPublic],[__ISBlogEnabled],[__ISWebEnabled],[Group_Notes],[Sign_Up_To_Serve],[Deadline_Passed_Message_ID],[Notifications],[Send_Attendance_Notification],[Send_Service_Notification],[Child_Care_Available],[Meeting_Frequency_ID],[Meeting_Duration_ID],[Required_Book],[Online_RSVP_Minimum_Age],[Enable_Discussion],[Small_Group_Information_2],[Remaining_Capacity])
VALUES(166574,'2016 Brave - Anywhere',2,20,15,7622376,'This is the Crossroads Anywhere Brave holdin Group, which contains community members who need to be placed in an Anywhere Journey Group',CAST('20160201 00:00:00.000' as DATETIME),CAST('20160530 00:00:00.000' as DATETIME),10,NULL,NULL,0,NULL,NULL,0,0,NULL,NULL,NULL,NULL,NULL,NULL,1,NULL,0,NULL,0,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,0,0,0,NULL,NULL,NULL,NULL,NULL,NULL,NULL)

SET IDENTITY_INSERT Groups OFF