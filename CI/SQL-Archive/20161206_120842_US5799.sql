USE [MinistryPlatform]
GO

DECLARE @ToolId INT = 376;

UPDATE dp_Tools SET Tool_Name = 'Create/Edit Event' where Tool_ID = @ToolId;
GO

