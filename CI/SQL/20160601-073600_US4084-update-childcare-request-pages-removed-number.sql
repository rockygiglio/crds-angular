USE MinistryPlatform
GO

-- update My Childcare requests
UPDATE dp_pages SET Default_Field_List='Group_ID_Table.[Group_Name] AS [Group]  ,cr_Childcare_Requests.[Start_Date] AS [Start Date]  ,cr_Childcare_Requests.[End_Date] AS [End Date] ,Request_Status_ID_Table.[Request_Status] AS [Request Status]'
       WHERE page_id=555;

-- update childcare requests
UPDATE dp_pages SET Default_Field_List='Requester_ID_Table.[Display_Name] AS [Requester],
                                        Group_ID_Table.[Group_Name] AS [Group], 
										Ministry_ID_Table.[Ministry_Name] AS [Ministry],
										Congregation_ID_Table.[Congregation_Name] AS [Congregation],
										cr_Childcare_Requests.[Start_Date] AS [Start Date],
										cr_Childcare_Requests.[End_Date] AS [End Date],
										cr_Childcare_Requests.[Frequency] AS [Frequency],
										cr_Childcare_Requests.[Childcare_Session] AS [Childcare Session],
										cr_Childcare_Requests.[Notes] AS [Notes],
										Request_Status_ID_Table.[Request_Status] AS [Request Status]' WHERE page_id=36;
