USE [MinistryPlatform]
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[Attribute_Types] WHERE [Attribute_Type] = 'GO Cincinnati - Registration Children')
BEGIN	
	INSERT INTO [dbo].[Attribute_Types]
			  ( [Attribute_Type]
			   ,[Description]
			   ,[Domain_ID]
			   ,[Prevent_Multiple_Selection])
		 VALUES
			   ('GO Cincinnati - Registration Children'
			    ,0, 1, 0)	
END
