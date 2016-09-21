USE [MinistryPlatform]
GO

DECLARE @FORM_ID int = 20;
DECLARE @FIELD_ID int = 1272;
DECLARE @FIELD_NAME nvarchar(1000) = N'Emergency Contact Relationship';
DECLARE @FIELD_TYPE int = 1;
DECLARE @CROSSROADS_ID int = 652;

IF NOT EXISTS (SELECT 1 FROM [dbo].[Form_Fields] WHERE [Form_ID] = @FORM_ID AND [Field_Label] = @FIELD_NAME)
BEGIN
	SET IDENTITY_INSERT [dbo].[Form_Fields] ON;
	INSERT INTO [dbo].[Form_Fields]
           ([Form_Field_ID]
		   ,[Field_Order]
           ,[Field_Label]
           ,[Field_Type_ID]
           ,[Required]
           ,[Form_ID]
           ,[Domain_ID]
           ,[Placement_Required]
           ,[CrossroadsId])
     VALUES
           (@FIElD_ID
		   ,165
           ,@FIELD_NAME
           ,@FIELD_TYPE
           ,0
           ,@FORM_ID
           ,1
           ,0
           ,@CROSSROADS_ID)
		   SET IDENTITY_INSERT [dbo].[Form_Fields] OFF;
END