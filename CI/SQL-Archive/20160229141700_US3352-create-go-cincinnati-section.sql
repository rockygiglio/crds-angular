USE MinistryPlatform;
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_Page_Sections] WHERE Page_Section = N'GO Cincinnati')
BEGIN
	SET IDENTITY_INSERT [dbo].[dp_Page_Sections] ON
	INSERT INTO [dbo].[dp_Page_Sections] (
		 [Page_Section_ID]
		,[Page_Section]
		,[View_Order]
	) VALUES (
		 21
		,N'GO Cincinnati'
		,83
	)
	SET IDENTITY_INSERT [dbo].[dp_Page_Sections] OFF
END