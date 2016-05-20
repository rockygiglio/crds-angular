USE MinistryPlatform
GO

UPDATE dp_Pages 
SET Default_Field_List = 'Requester_ID_Table.[Display_Name] AS [Requester] 
                          , Group_ID_Table.[Group_Name] AS [Group] 
						  , Ministry_ID_Table.[Ministry_Name] AS [Ministry] 
						  , Congregation_ID_Table.[Congregation_Name] AS [Congregation] 
						  , cr_Childcare_Requests.[Start_Date] AS [Start Date] 
						  , cr_Childcare_Requests.[End_Date] AS [End Date] 
						  , cr_Childcare_Requests.[Frequency] AS [Frequency] 
						  , cr_Childcare_Requests.[Time_Frame] AS [Childcare Session] 
						  , cr_Childcare_Requests.[No_of_Children_Attending] AS [Est. No of Children Attending] 
						  , cr_Childcare_Requests.[Notes] AS [Notes] 
						  , Request_Status_ID_Table.[Request_Status] AS [Request Status]'
WHERE Display_Name = 'Childcare Requests';