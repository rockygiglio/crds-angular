USE [MinistryPlatform]
GO

IF NOT EXISTS(SELECT * FROM [dbo].[dp_Pages] WHERE [Display_Name] = 'Sequence Records')
BEGIN
SET IDENTITY_INSERT dp_Pages ON
INSERT INTO [dbo].[dp_Pages]
           ([Page_ID]
           ,[Display_Name]
           ,[Singular_Name]
           ,[Description]
           ,[View_Order]
           ,[Table_Name]
           ,[Primary_Key]
           ,[Display_Search]
           ,[Default_Field_List]
           ,[Selected_Record_Expression]
           ,[Display_Copy])
     VALUES
           (624
           ,'Sequence Records'
           ,'Sequence Record'
           ,'Sequences'
           ,100
           ,'dp_sequence_records'
           ,'Record_ID'
           ,1
           ,'Record_ID, Table_Name, Domain_ID, Sequence_ID'
           ,'Record_ID'
           ,0)

SET IDENTITY_INSERT dp_Pages OFF
END