use MinistryPlatform;
GO

Update [dbo].[dp_Pages] 
	SET [Display_Name] = 'Email Whitelist', [Singular_Name] = 'Email Whitelist'
	WHERE [Display_Name] = N'Override Email Prevention'

GO