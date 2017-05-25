USE [MinistryPlatform]
GO

ALTER TABLE Groups
ADD KC_Sort_Order INT NOT NULL DEFAULT (999) 
GO

EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Weekend Services Report Group Sort Order', 
@level0type = N'SCHEMA', @level0name = dbo, 
@level1type = N'TABLE',  @level1name = Groups,
@level2type = N'COLUMN', @level2name = KC_Sort_Order;
GO
