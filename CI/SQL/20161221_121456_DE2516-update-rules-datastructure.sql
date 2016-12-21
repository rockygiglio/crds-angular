use MinistryPlatform
GO

DECLARE @Rule_Registration int = 622;
DECLARE @Product_Rulese int = 623;
DECLARE @Gender_Rules int = 621;

DECLARE @Rule_Page_Section int;

SELECT @Rule_Page_Section = [Page_Section_ID] from [dbo].[dp_Page_Sections] where [Page_Section] = 'Rules';

IF EXISTS (SELECT 1 FROM [dbo].[dp_Pages] WHERE [Page_ID] = @Rule_Registration)
BEGIN
	Update [dbo].[dp_Pages] SET
	[Default_Field_List] = N'cr_Rule_Registrations.Maximum_Registrants, cr_Rule_Registrations.Rule_Start_Date, cr_Rule_Registrations.Rule_End_Date'
	WHERE [Page_ID] = @Rule_Registration;
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_Page_Section_Pages] WHERE [Page_Section_ID] = @Rule_Page_Section AND [Page_ID] = @Rule_Registration) 
BEGIN
	INSERT INTO [dbo].[dp_Page_Section_Pages] (
		 [Page_ID]
		,[Page_Section_ID]
	) VALUES (
		 @Rule_Registration
		,@Rule_Page_Section
	)
	
END


IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_Page_Section_Pages] WHERE [Page_Section_ID] = @Rule_Page_Section AND [Page_ID] = @Product_Rulese) 
BEGIN
	INSERT INTO [dbo].[dp_Page_Section_Pages] (
		 [Page_ID]
		,[Page_Section_ID]
	) VALUES (
		 @Product_Rulese
		,@Rule_Page_Section
	)	
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_Page_Section_Pages] WHERE [Page_Section_ID] = @Rule_Page_Section AND [Page_ID] = @Gender_Rules) 
BEGIN
	INSERT INTO [dbo].[dp_Page_Section_Pages] (
		 [Page_ID]
		,[Page_Section_ID]
	) VALUES (
		 @Gender_Rules
		,@Rule_Page_Section
	)	
END