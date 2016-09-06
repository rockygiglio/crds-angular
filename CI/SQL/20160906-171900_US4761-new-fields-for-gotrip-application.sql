USE [MinistryPlatform]
GO

DECLARE @MEDICAL_RESTRICTIONS_TYPE_ID int = 100;
DECLARE @MEDICATIONS_TAKING_TYPE_ID int = 101;

IF NOT EXISTS( SELECT 1 FROM [dbo].[Attribute_Types] WHERE [Attribute_Type_ID] = @MEDICAL_RESTRICTIONS_TYPE_ID)
BEGIN
	SET IDENTITY_INSERT [dbo].[Attribute_Types] ON
	INSERT INTO [dbo].[Attribute_Types]
           ([Attribute_Type_ID]
		   ,[Attribute_Type]
           ,[Description]
           ,[Domain_ID]
           ,[Available_Online]
           ,[__ExternalAttributeTypeID]
           ,[Prevent_Multiple_Selection]
           ,[Online_Sort_Order])
     VALUES
           (@MEDICAL_RESTRICTIONS_TYPE_ID
		   ,<Attribute_Type, nvarchar(50),>
           ,<Description, nvarchar(255),>
           ,<Domain_ID, int,>
           ,<Available_Online, bit,>
           ,<__ExternalAttributeTypeID, int,>
           ,<Prevent_Multiple_Selection, bit,>
           ,<Online_Sort_Order, int,>)
	
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
           ,[__ExternalAttributeTypeID]
           ,[Prevent_Multiple_Selection]
           ,[Online_Sort_Order])
     VALUES
           (@MEDICATIONS_TAKING_TYPE_ID
		   ,<Attribute_Type, nvarchar(50),>
           ,<Description, nvarchar(255),>
           ,<Domain_ID, int,>
           ,<Available_Online, bit,>
           ,<__ExternalAttributeTypeID, int,>
           ,<Prevent_Multiple_Selection, bit,>
           ,<Online_Sort_Order, int,>)
	SET IDENTITY_INSERT [dbo].[Attribute_Types] OFF
END

