USE MinistryPlatform
GO

--Remove Trip Participants Tool from Form Response Page

DECLARE @ToolID INT

SET @ToolID = (SELECT tool_ID FROM dp_tools WHERE Tool_Name = 'Trip Participants')

IF(@ToolID IS NOT NULL)
BEGIN
	DELETE dp_tool_pages WHERE tool_id = @ToolID
END

