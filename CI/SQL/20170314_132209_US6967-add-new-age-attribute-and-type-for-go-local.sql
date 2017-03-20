USE MinistryPlatform
GO

DECLARE @ATTRIBUTE_TYPE_ID int = 106;
DECLARE @ATTRIBUTE_ID int = 9041;

IF NOT EXISTS( SELECT 1 FROM [dbo].[Attribute_Types] WHERE [Attribute_Type_ID] = @ATTRIBUTE_TYPE_ID)
BEGIN
	SET IDENTITY_INSERT [dbo].[Attribute_Types] ON;
	
	INSERT INTO [dbo].[Attribute_Types] (
		 [Attribute_Type_ID]
		,[Attribute_Type]
		,[Description]
		,[Domain_ID]
		,[Prevent_Multiple_Selection]
	) VALUES (
		 @ATTRIBUTE_TYPE_ID
		,N'GO Local - Registration Children'
		,N'This was created for use on the Go Local - Anywhere application because we don''t ask for, nor do we care to know, specific age ranges. '
		,1
		,1
	)
	SET IDENTITY_INSERT [dbo].[Attribute_Types] OFF;
END

IF NOT EXISTS( SELECT 1 FROM [dbo].[Attributes] WHERE [Attribute_ID] = @ATTRIBUTE_ID)
BEGIN
	SET IDENTITY_INSERT [dbo].[Attributes] ON;

	INSERT INTO [dbo].[Attributes] (
		 [Attribute_ID]
		,[Attribute_Name]
		,[Description]
		,[Domain_ID]
		,[Attribute_Type_ID]
	) VALUES (
		 @ATTRIBUTE_ID
		,N'0-17'
		,N'Ages 0-17'
		,1
		,@ATTRIBUTE_TYPE_ID
	)

	SET IDENTITY_INSERT [dbo].[Attributes] OFF;
END