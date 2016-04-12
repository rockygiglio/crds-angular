USE [MinistryPlatform]
GO

SET IDENTITY_INSERT [dbo].[Attribute_Types] ON

DECLARE @ATTRIBUTE_TYPE_ID int = 2;
DECLARE @ATTRIBUTE_TYPE_VALUE varchar(50) = N'GO Cincinnati - Equipment';
DECLARE @ATTRIBUTE_TYPE_DESCRIPTION varchar(255) = N'Equipment that a volunteer has';

INSERT INTO [dbo].[Attribute_Types]
		  ( [Attribute_Type_ID]
		  ,[Attribute_Type]
		  ,[Description]
		  ,[Domain_ID]
		  ,[Available_Online])
    VALUES
		  (@ATTRIBUTE_TYPE_ID
		  ,@ATTRIBUTE_TYPE_VALUE
		  ,@ATTRIBUTE_TYPE_DESCRIPTION
		  ,1
		  ,1);

SELECT @ATTRIBUTE_TYPE_ID = 3;
SELECT @ATTRIBUTE_TYPE_VALUE = N'GO Cincinnati - Other Equipment';
SELECT @ATTRIBUTE_TYPE_DESCRIPTION = N'Other Equipment that a volunteer has';

INSERT INTO [dbo].[Attribute_Types]
		  ( [Attribute_Type_ID]
		  ,[Attribute_Type]
		  ,[Description]
		  ,[Domain_ID]
		  ,[Available_Online])
    VALUES
		  (@ATTRIBUTE_TYPE_ID
		  ,@ATTRIBUTE_TYPE_VALUE
		  ,@ATTRIBUTE_TYPE_DESCRIPTION
		  ,1
		  ,1)

SET IDENTITY_INSERT [dbo].[Attribute_Types] OFF

SET IDENTITY_INSERT [dbo].[Attributes] ON
INSERT INTO [dbo].[Attributes]
		  ( [Attribute_ID]
		  ,[Attribute_Name]
		  ,[Description]
		  ,Sort_Order
		  ,[Domain_ID]
		  ,[Attribute_Type_ID])
    VALUES
		  (154
		  ,N'Other Equipment'
		  ,N'User defined equipment'
		  ,9999
		  ,1
		  ,@ATTRIBUTE_TYPE_ID)
SET IDENTITY_INSERT [dbo].[Attributes] OFF