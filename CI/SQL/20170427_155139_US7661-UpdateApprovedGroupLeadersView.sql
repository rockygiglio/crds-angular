USE MinistryPlatform
GO

DECLARE @PageViewId int = 93041
IF EXISTS(SELECT 1 FROM [dbo].[dp_Page_Views] WHERE Page_View_ID = @PageViewId)
BEGIN
	UPDATE [dbo].[dp_Page_Views]
	SET View_Clause = 'Participants.Group_Leader_Status_ID=4'
	WHERE Page_View_ID = @PageViewId
END