USE [MinistryPlatform]
GO

-- remove cr_Product_Rules table
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[FK_cr_Product_Rules_Congregations]') AND type in (N'F'))
BEGIN
ALTER TABLE [dbo].[cr_Product_Rules] DROP CONSTRAINT [FK_cr_Product_Rules_Congregations]
END
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[FK_cr_Product_Rules_dp_Domains]') AND type in (N'F'))
BEGIN
ALTER TABLE [dbo].[cr_Product_Rules] DROP CONSTRAINT [FK_cr_Product_Rules_dp_Domains]
END
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[FK_cr_Product_Rules_Genders]') AND type in (N'F'))
BEGIN
ALTER TABLE [dbo].[cr_Product_Rules] DROP CONSTRAINT [FK_cr_Product_Rules_Genders]
END
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[FK_cr_Product_Rules_Products]') AND type in (N'F'))
BEGIN
ALTER TABLE [dbo].[cr_Product_Rules] DROP CONSTRAINT [FK_cr_Product_Rules_Products]
END
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[FK_cr_Event_Product_Rules_Product_Rules]') AND type in (N'F'))
BEGIN
ALTER TABLE [dbo].[cr_Event_Product_Rules] DROP CONSTRAINT [FK_cr_Event_Product_Rules_Product_Rules]
END
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[PK_cr_Product_Rules]') AND type in (N'PK'))
BEGIN
ALTER TABLE [dbo].[cr_Product_Rules] DROP CONSTRAINT [PK_cr_Product_Rules]
END
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cr_Product_Rules]') AND type in (N'U'))
BEGIN
DROP TABLE [dbo].[cr_Product_Rules]
END
GO
-- remove cr_event_Product_Ruules table

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[FK_cr_Event_Product_Rules_Events]') AND type in (N'F'))
BEGIN
ALTER TABLE [dbo].[cr_Event_Product_Rules] DROP CONSTRAINT [FK_cr_Event_Product_Rules_Events]
END
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[FK_cr_Event_Product_Rules_dp_Domains]') AND type in (N'F'))
BEGIN
ALTER TABLE [dbo].[cr_Event_Product_Rules] DROP CONSTRAINT [FK_cr_Event_Product_Rules_dp_Domains]
END
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[PK_cr_Event_Product_Rules]') AND type in (N'PK'))
BEGIN
ALTER TABLE [dbo].[cr_Event_Product_Rules] DROP CONSTRAINT [PK_cr_Event_Product_Rules]
END
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cr_Event_Product_Rules]') AND type in (N'U'))
BEGIN
DROP TABLE [dbo].[cr_Event_Product_Rules]
END
GO


