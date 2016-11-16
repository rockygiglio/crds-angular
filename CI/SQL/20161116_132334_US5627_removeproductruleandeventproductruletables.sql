USE [MinistryPlatform]
GO
-- remove cr_Product_Rules table
ALTER TABLE [dbo].[cr_Product_Rules] DROP CONSTRAINT [FK_cr_Product_Rules_Congregations]
ALTER TABLE [dbo].[cr_Product_Rules] DROP CONSTRAINT [FK_cr_Product_Rules_dp_Domains]
ALTER TABLE [dbo].[cr_Product_Rules] DROP CONSTRAINT [FK_cr_Product_Rules_Genders]
ALTER TABLE [dbo].[cr_Product_Rules] DROP CONSTRAINT [FK_cr_Product_Rules_Products]
ALTER TABLE [dbo].[cr_Event_Product_Rules] DROP CONSTRAINT [FK_cr_Event_Product_Rules_Product_Rules]
ALTER TABLE [dbo].[cr_Product_Rules] DROP CONSTRAINT [PK_cr_Product_Rules]

GO

DROP TABLE [dbo].[cr_Product_Rules]
GO
-- remove cr_event_Product_Ruules table
ALTER TABLE [dbo].[cr_Event_Product_Rules] DROP CONSTRAINT [FK_cr_Event_Product_Rules_Events]
ALTER TABLE [dbo].[cr_Event_Product_Rules] DROP CONSTRAINT [FK_cr_Event_Product_Rules_dp_Domains]
ALTER TABLE [dbo].[cr_Event_Product_Rules] DROP CONSTRAINT [PK_cr_Event_Product_Rules]

GO

DROP TABLE [dbo].[cr_Event_Product_Rules]
GO


