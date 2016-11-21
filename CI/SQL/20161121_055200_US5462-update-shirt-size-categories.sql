-- noinspection SqlNoDataSourceInspectionForFile
USE [MinistryPlatform]
GO

DECLARE @adultCat int = 1006;
DECLARE @childCat int = 1007;

DECLARE @adultXS int = 6845;
DECLARE @adultS int = 6846;
DECLARE @adultM int = 6847;
DECLARE @adultL int = 6848;
DECLARE @adultXL int = 6849;
DECLARE @adultXXL int = 6850;
DECLARE @adultXXXL int = 6851;
DECLARE @childS int = 6852;
DECLARE @childM int = 6853;
DECLARE @childL int = 6854;

IF NOT EXISTS(SELECT * FROM [dbo].[Attribute_Categories] WHERE Attribute_Category = 'Adult')
  BEGIN
    SET IDENTITY_INSERT [dbo].[Attribute_Categories] ON;

    INSERT INTO Attribute_Categories
    (Attribute_Category_ID,
     Attribute_Category,
     Available_Online,
     Domain_ID)
    VALUES
      (@adultCat,
       'Adult',
       0,
       1)

    SET IDENTITY_INSERT [dbo].[Attribute_Categories] OFF;
  END

IF NOT EXISTS(SELECT * FROM [dbo].[Attribute_Categories] WHERE Attribute_Category = 'Child')
  BEGIN
    SET IDENTITY_INSERT [dbo].[Attribute_Categories] ON;

    INSERT INTO Attribute_Categories
    (Attribute_Category_ID,
     Attribute_Category,
     Available_Online,
     Domain_ID)
    VALUES
      (@childCat,
       'Child',
       0,
       1)

    SET IDENTITY_INSERT [dbo].[Attribute_Categories] OFF;
  END

UPDATE [dbo].[Attributes]
SET [Attribute_Category] = @adultCat
WHERE [Attribute_Id] in (@adultXS, @adultS, @adultM, @adultL, @adultXL, @adultXXL, @adultXXXL);

UPDATE [dbo].[Attributes]
SET [Attribute_Category] = @childCat
WHERE [Attribute_Id] in (@childS, @childM, @childL);
