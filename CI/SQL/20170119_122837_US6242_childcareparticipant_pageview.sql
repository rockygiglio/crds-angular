USE [MinistryPlatform]
GO

SET IDENTITY_INSERT dp_Page_Views ON

IF NOT EXISTS (SELECT * FROM dp_Page_Views WHERE Page_View_ID = 1118)
BEGIN
   INSERT INTO [dbo].[dp_Page_Views]
           ([Page_View_ID]
		   ,[View_Title]
           ,[Page_ID]
           ,[Description]
           ,[Field_List]
           ,[View_Clause])
        VALUES
           (1118
		   , 'Childcare Participants'
           ,316
           ,'Helps to find childcare group participants to edit/cancel their RSVPS'
           ,'Participant_ID_Table_Contact_ID_Table.[Display_Name] AS [Child Name]
              , Group_Participants.[Start_Date] AS [Child Participant Start Date]
              , Group_ID_Table.[Group_Name] AS [Childcare Group]
              , Group_ID_Table_Group_Type_ID_Table.[Group_Type] AS [Childcare Group Type]
              , Enrolled_By_Table_Participant_ID_Table_Contact_ID_Table.[Display_Name] AS [Parent/Guardian Name]
              , Enrolled_By_Table_Group_ID_Table.[Group_Name] AS [Parent/Guardian Group]'
           ,'Group_ID_Table.Group_Type_ID = ''27'' AND Group_Participants.[End_Date] IS NULL')
END
SET IDENTITY_INSERT dp_Page_Views OFF
GO


