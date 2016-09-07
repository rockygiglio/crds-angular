USE [MinistryPlatform]
GO

DECLARE @MEDICAL_RESTRICTIONS_TYPE_ID int = 100;
DECLARE @MEDICATIONS_TAKING_TYPE_ID int = 101;

DECLARE @MEDICATIONS_TAKING_ATTR_ID int = 9000;
DECLARE @MEDICAL_RESTRICTIONS_ATTR_ID int = 9001;

IF NOT EXISTS( SELECT 1 FROM [dbo].[Attribute_Types] WHERE [Attribute_Type_ID] = @MEDICAL_RESTRICTIONS_TYPE_ID)
BEGIN
	SET IDENTITY_INSERT [dbo].[Attribute_Types] ON
	INSERT INTO [dbo].[Attribute_Types]
           ([Attribute_Type_ID]
		   ,[Attribute_Type]
           ,[Description]
           ,[Domain_ID]
           ,[Available_Online]
           ,[Prevent_Multiple_Selection])
     VALUES
           (@MEDICAL_RESTRICTIONS_TYPE_ID
		   ,N'Medical Restrictions'
           ,N'Describe any medical restrictions you may have'
           ,1
           ,1
           ,1)
	
	SET IDENTITY_INSERT [dbo].[Attribute_Types] OFF
END


IF NOT EXISTS( SELECT 1 FROM [dbo].[Attribute_Types] WHERE [Attribute_Type_ID] = @MEDICATIONS_TAKING_TYPE_ID)
BEGIN
	SET IDENTITY_INSERT [dbo].[Attribute_Types] ON
	INSERT INTO [dbo].[Attribute_Types]
           ([Attribute_Type_ID]
		   ,[Attribute_Type]
           ,[Description]
           ,[Domain_ID]
		   ,[Available_Online]
           ,[Prevent_Multiple_Selection])
     VALUES
           (@MEDICATIONS_TAKING_TYPE_ID
		   ,N'Medications Taking'
           ,N'Describe any medications you are taking'
		   ,1
		   ,1
		   ,1)
	SET IDENTITY_INSERT [dbo].[Attribute_Types] OFF
END

IF NOT EXISTS (SELECT 1 from [dbo].[Attributes] WHERE [Attribute_ID] = @MEDICATIONS_TAKING_ATTR_ID)
BEGIN
	SET IDENTITY_INSERT [dbo].[Attributes] ON

	INSERT INTO [dbo].[Attributes]
           ([Attribute_ID]
		   ,[Attribute_Name]
           ,[Description]
           ,[Attribute_Type_ID]
           ,[Domain_ID]
           ,[Sort_Order])
     VALUES
           (@MEDICATIONS_TAKING_ATTR_ID
		   ,N'Medications Taking'
           ,N'What medications are you currently taking'
           ,@MEDICATIONS_TAKING_TYPE_ID
           ,1
           ,0)
	SET IDENTITY_INSERT [dbo].[Attributes] OFF
END

IF NOT EXISTS (SELECT 1 from [dbo].[Attributes] WHERE [Attribute_ID] = @MEDICAL_RESTRICTIONS_ATTR_ID)
BEGIN
	SET IDENTITY_INSERT [dbo].[Attributes] ON
	INSERT INTO [dbo].[Attributes]
           ([Attribute_ID]
		   ,[Attribute_Name]
           ,[Description]
           ,[Attribute_Type_ID]
           ,[Domain_ID]
           ,[Sort_Order])
     VALUES
           (@MEDICAL_RESTRICTIONS_ATTR_ID
		   ,N'Medications Taking'
           ,N'What medications are you currently taking'
           ,@MEDICAL_RESTRICTIONS_TYPE_ID
           ,1
           ,0)
	SET IDENTITY_INSERT [dbo].[Attributes] OFF
END