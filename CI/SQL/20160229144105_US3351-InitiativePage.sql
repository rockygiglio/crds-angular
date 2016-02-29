USE [MinistryPlatform]
GO

SET IDENTITY_INSERT [dbo].[dp_Pages] ON
GO

INSERT INTO [dbo].[dp_Pages]
			([Page_ID]
           ,[Display_Name]
           ,[Singular_Name]
           ,[Description]
           ,[View_Order]
           ,[Table_Name]
           ,[Primary_Key]
           ,[Default_Field_List]
           ,[Selected_Record_Expression]
           ,[Display_Copy])
     VALUES
		(11,
           'Initiatives',
           'Initiative',
           'An occurance of a volunteer event.',
           100,
           'cr_Initiatives',
           'Initiative_ID',
           'Initiative_Name, Program_ID_Table.Program_Name, Start_Date, End_Date, Leader_Signup_Start_Date, Leader_Signup_End_Date, Volunteer_Signup_Start_Date, Volunteer_Signup_End_Date',
           'Initiative_Name',
           1)

SET IDENTITY_INSERT [dbo].[dp_Pages] OFF
GO