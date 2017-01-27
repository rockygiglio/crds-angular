USE [MinistryPlatform]
GO

SET IDENTITY_INSERT dp_Sub_Pages ON
DECLARE @SubPageId int = 613

IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_Sub_Pages] WHERE Sub_Page_ID = @SubPageId)
BEGIN
	INSERT INTO [dbo].[dp_Sub_Pages]
			   ([Sub_Page_ID]
			   ,[Display_Name]
			   ,[Singular_Name]
			   ,[Page_ID]
			   ,[View_Order]
			   ,[Link_To_Page_ID]
			   ,[Link_From_Field_Name]
			   ,[Primary_Table]
			   ,[Default_Field_List]
			   ,[Selected_Record_Expression]
			   ,[Filter_Key]
			   ,[Relation_Type_ID]
			   ,[Display_Copy])
		 VALUES
			   (@SubPageId
			   ,'Security Roles'
			   ,'Security Role'
			   ,353
			   ,5
			   ,387
			   ,'Role_ID'
			   ,'dp_Role_Pages'
			   ,'Role_ID_Table.Role_Name'
			   ,'Role_ID_Table.Role_Name'
			   ,'Page_ID'
			   ,1
			   ,0)

	SET IDENTITY_INSERT dp_Sub_Pages OFF
END
