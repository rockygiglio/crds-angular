USE MinistryPlatform
GO

-- Update attribute page view
UPDATE dbo.dp_Page_Views 
	SET 
		Field_List = 'Attributes.[Attribute_ID], Attributes.[Attribute_Name], Attributes.[Description] AS Attribute_Description, Attribute_Type_ID_Table.[Attribute_Type_ID], Attribute_Type_ID_Table.[Attribute_Type], Attribute_Type_ID_Table.[Prevent_Multiple_Selection], Attribute_Category_ID_Table.[Attribute_Category_ID], Attribute_Category_ID_Table.[Attribute_Category], Attributes.[Sort_Order], Attribute_Category_ID_Table.[Description] AS Attribute_Category_Description'
	WHERE 
		Page_View_ID = 2185

GO