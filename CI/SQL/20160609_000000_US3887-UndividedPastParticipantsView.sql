USE [MinistryPlatform]	
GO

DECLARE @VIEW_ID int = 0
DECLARE @VIEW_TITLE nvarchar(255) = N'Undivided - Past Participants'
DECLARE @PAGE_ID int = 355
DECLARE @DESCRIPTION nvarchar(255) = N'Staff members can view the facilitators from past Undivided sessions.'

DECLARE @FIELD_LIST nvarchar(2000) = '(SELECT COUNT(gp.Participant_ID) 
FROM Group_Participants gp   
	INNER JOIN Groups g ON g.Group_ID = gp.group_id                      
WHERE gp.Group_Role_Id = 16 AND g.Group_Type_ID = 26  AND g.End_Date < getdate() AND gp.Participant_Id = Participants.[Participant_ID]             
GROUP BY gp.Participant_ID) AS Num_Sessions_Participated
,Contact_ID_Table_Household_ID_Table_Congregation_ID_Table.[Congregation_Name]
,Contact_ID_Table.[First_Name] 
,Contact_ID_Table.[Last_Name] 
,Contact_ID_Table.[Email_Address] 	 
,Contact_ID_Table_Gender_ID_Table.[Gender]
,STUFF((SELECT '', '' + Attribute_Name 
      FROM Contact_Attributes CA
      INNER JOIN Attributes A ON A.Attribute_ID = CA.Attribute_ID	
	  INNER JOIN Contacts C ON C.Contact_ID = CA.Contact_ID	
      WHERE CA.Contact_ID = Contact_ID_Table.[Contact_ID]
	  AND A.Attribute_Type_ID = 20
	  AND GETDATE() BETWEEN CA.Start_Date 
      AND ISNULL(CA.End_Date,GETDATE()) 
	  FOR XML PATH('''')), 1, 1, '''') AS [Ethnicity]
,DATEDIFF(hour,Contact_ID_Table.[Date_of_Birth],GETDATE())/8766 AS [AGE]'

DECLARE @VIEW_CLAUSE nvarchar(1000) = N'Participants.[Participant_ID] IN (
	SELECT gp.Participant_ID
			FROM 
			Groups g,
			Group_Participants gp
			WHERE gp.Participant_id = Participants.[Participant_ID] AND gp.group_id = g.group_id AND gp.Group_Role_Id = 16 AND 
g.Group_Type_ID = 26  AND g.End_Date < getdate() AND g.Parent_Group IS NULL)'
DECLARE @ORDER_BY nvarchar(1000) = N'Contact_ID_Table_Household_ID_Table_Congregation_ID_Table.[Congregation_Name],Contact_ID_Table.[Last_Name] '
SELECT @VIEW_ID = [Page_View_ID] FROM [dbo].[dp_Page_Views] WHERE [Page_View_ID] = 2304;

IF @VIEW_ID = 0
BEGIN
    SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON
	PRINT 'Inserting new page view'
	INSERT INTO [dbo].[dp_Page_Views] (
	    [Page_View_ID]
		,[View_Title]
		,[Page_ID]
		,[Description]
		,[Field_List]
		,[View_Clause]
		,[Order_By])	
	VALUES (
	    2304
		,@VIEW_TITLE
		,@PAGE_ID
		,@DESCRIPTION
		,@FIELD_LIST
		,@VIEW_CLAUSE
		,@ORDER_BY
	)	
	SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
END
ELSE
BEGIN
	PRINT 'Update viewId ' + Convert(varchar, @VIEW_ID)
	UPDATE [dbo].[dp_Page_Views] SET 
		[View_Title] = @VIEW_TITLE
		,[Page_ID] = @PAGE_ID
		,[Description] = @DESCRIPTION
		,[Field_List] = @FIELD_LIST
		,[View_Clause] = @VIEW_CLAUSE
		,[Order_By] = @ORDER_BY
	WHERE Page_View_ID = @VIEW_ID
END