USE Ministryplatform
GO

UPDATE dp_Pages
SET Default_Field_List = 'Group_ID_Table.[Group_Name] AS [Group] ,cr_Childcare_Requests.[Start_Date] AS [Start Date] ,cr_Childcare_Requests.[End_Date] AS [End Date] ,cr_Childcare_Requests.[No_of_Children_Attending] AS [Childcare Session] ,Request_Status_ID_Table.[Request_Status] AS [Request Status]'
WHERE Display_Name = 'My childcare Requests';