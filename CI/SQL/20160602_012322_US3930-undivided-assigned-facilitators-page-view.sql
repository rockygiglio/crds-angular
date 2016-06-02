USE [MinistryPlatform]	
GO

DECLARE @VIEW_ID int = 0
DECLARE @VIEW_TITLE nvarchar(255) = N'Undivided - Assigned Facilitators'
DECLARE @PAGE_ID int = 316
DECLARE @DESCRIPTION nvarchar(255) = N'Staff members can view the users that have been assigned to a group as a facilitator.'

DECLARE @FIELD_LIST nvarchar(2000) = N'Group_Participants.[Start_Date] AS [Registration Date] 
	,Group_ID_Table.[Group_Name]
	,Participant_ID_Table_Contact_ID_Table.[First_Name]
	,Participant_ID_Table_Contact_ID_Table.[Last_Name]
	,Participant_ID_Table_Contact_ID_Table_Gender_ID_Table.[Gender]
	,STUFF((SELECT '', '' + Attribute_Name 
      FROM Contact_Attributes CA
      INNER JOIN Attributes A ON A.Attribute_ID = CA.Attribute_ID	
	  INNER JOIN Contacts C ON C.Contact_ID = CA.Contact_ID	
      WHERE CA.Contact_ID = Participant_ID_Table_Contact_ID_Table.[Contact_ID]
	  AND A.Attribute_Type_ID = 20
	  AND GETDATE() BETWEEN CA.Start_Date 
      AND ISNULL(CA.End_Date,GETDATE()) 
	  FOR XML PATH('''')), 1, 1, '''') AS [Ethnicity]
	,DATEDIFF(hour,Participant_ID_Table_Contact_ID_Table.[Date_of_Birth],GETDATE())/8766 AS [AGE]
	,(SELECT TOP 1 Attribute_Name 
	  FROM Group_Participant_Attributes GPA, Attributes A 
	  WHERE GPA.Attribute_ID = A.Attribute_ID
	  AND GPA.Group_Participant_ID = Group_Participants.Group_Participant_ID
	  AND GETDATE() BETWEEN GPA.Start_Date AND ISNULL(GPA.End_Date,GETDATE()) 
	  AND Attribute_Type_ID = 85) AS [Facilitator Training]
	,(SELECT Notes 
	  FROM Group_Participant_Attributes GPA, Attributes A 
	  WHERE GPA.Attribute_ID = A.Attribute_ID and 
	  GPA.Group_Participant_ID = Group_Participants.Group_Participant_ID and
	  Attribute_Type_ID = 87) AS [Preferred Co-Facilitator]		
	,Group_Participants.[Child_Care_Requested] AS [Requested Child Care]'

DECLARE @VIEW_CLAUSE nvarchar(1000) = N'Group_ID_Table_Group_Type_ID_Table.Group_Type_ID = 26 AND Group_Role_ID_Table.Group_Role_ID = 22 AND (Group_Participants.End_Date > GetDate() OR Group_Participants.End_Date IS NULL) AND (Group_ID_Table.End_Date > GetDate() OR Group_ID_Table.End_Date IS NULL) AND Group_ID_Table_Parent_Group_Table.Group_ID IS NOT NULL'
DECLARE @ORDER_BY nvarchar(1000) = N'Group_ID_Table.[Group_Name], Group_Participants.[Start_Date]'
SELECT @VIEW_ID = [Page_View_ID] FROM [dbo].[dp_Page_Views] WHERE [Page_View_ID] = 92144;

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
	    92144
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