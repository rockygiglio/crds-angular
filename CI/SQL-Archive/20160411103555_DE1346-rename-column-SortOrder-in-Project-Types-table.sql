USE MinistryPlatform; 
GO

EXEC sp_rename 'dbo.cr_Project_Types.SortOrder', 'Sort_Order', 'COLUMN';

