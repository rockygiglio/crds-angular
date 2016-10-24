USE [MinistryPlatform]
GO
/****** Object:  StoredProcedure [dbo].[api_crds_SearchGroups]    Script Date: 10/21/2016 9:43:53 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[api_crds_SearchGroups]
	@GroupTypeId INT = 1, -- Search small group type by default
	@SearchString VARCHAR(MAX) = NULL
AS
BEGIN

	-- split keyword string - we search in each column for these
	CREATE TABLE #Keywords (Keyword NVARCHAR(MAX));
	INSERT INTO #Keywords SELECT UPPER(Item) FROM dp_Split(@SearchString, ',');

	SELECT 
	Group_ID, 
	Group_Name,
	Group_Type_ID,
	Description AS [GroupDescription],
	Child_Care_Available AS [ChildCareAvailable],
	Kids_Welcome AS [KidsWelcome],
	Primary_Contact AS [ContactId],
	'[ ' + STUFF((SELECT ' { Nickname: "' + Nickname + '", Last_name: "' + Last_Name + '", Congregation: "'+ ISNULL(cong.Congregation_Name, '') +'", Group_Role_ID: ' + CONVERT(VARCHAR(10), gp.Group_Role_ID) + ' },'
			FROM Groups g INNER JOIN Group_Participants gp ON g.Group_ID = gp.Group_ID 
			INNER JOIN Participants p ON gp.Participant_ID = p.Participant_ID
			INNER JOIN Contacts c ON p.Contact_ID = c.Contact_ID
			INNER JOIN Households h on c.Household_ID = h.Household_ID
			INNER JOIN Congregations cong ON cong.Congregation_ID = h.Congregation_ID
			WHERE gp.Group_Role_ID=22 AND g.Group_ID = gr.Group_ID   
			FOR XML PATH('')), 1, 1, '') + ' ]' AS [GroupParticipants],
	'[' + 
	STUFF((SELECT ' { "AttributeId": ' + CONVERT(VARCHAR, A.Attribute_ID) + ', "AttributeTypeId": ' + CONVERT(VARCHAR, A.Attribute_Type_ID) + ', "AttributeTypeName": "' + AT.Attribute_Type + '", "Name": "' + REPLACE(REPLACE(A.Attribute_Name, '\', '\\'), '"', '\"') + '", "Category": "' + COALESCE(AC.Attribute_Category, '') + '", "CategoryId": ' + CONVERT(VARCHAR, COALESCE(AC.Attribute_Category_ID, 0)) + ', "Description": "' + REPLACE(REPLACE(COALESCE(A.Description, ''), '\', '\\'), '"', '\"') + '"}, '
			FROM Group_Attributes GA, Attribute_Types AT, Attributes A LEFT OUTER JOIN Attribute_Categories AC ON A.Attribute_Category_ID = AC.Attribute_Category_ID
			WHERE GA.Attribute_ID = A.Attribute_ID
			AND A.Attribute_Type_ID = AT.Attribute_Type_ID
			AND AT.Prevent_Multiple_Selection = 0
			AND GA.Group_ID = gr.Group_ID
			AND GETDATE() BETWEEN GA.Start_Date 
			AND ISNULL(GA.End_Date,GETDATE())   
			FOR XML PATH('')), 1, 1, '') +
	']' AS [MultiSelectAttributes],
	'[' + 
	STUFF((SELECT ' { "AttributeId": ' + CONVERT(VARCHAR, A.Attribute_ID) + ', "AttributeTypeId": ' + CONVERT(VARCHAR, A.Attribute_Type_ID) + ', "AttributeTypeName": "' + AT.Attribute_Type + '", "Name": "' + REPLACE(REPLACE(A.Attribute_Name, '\', '\\'), '"', '\"') + '", "Category": "' + COALESCE(AC.Attribute_Category, '') + '", "CategoryId": ' + CONVERT(VARCHAR, COALESCE(AC.Attribute_Category_ID, 0)) + ', "Description": "' + REPLACE(REPLACE(COALESCE(A.Description, ''), '\', '\\'), '"', '\"') + '"}, '
			FROM Group_Attributes GA, Attribute_Types AT, Attributes A LEFT OUTER JOIN Attribute_Categories AC ON A.Attribute_Category_ID = AC.Attribute_Category_ID
			WHERE GA.Attribute_ID = A.Attribute_ID
			AND A.Attribute_Type_ID = AT.Attribute_Type_ID
			AND AT.Prevent_Multiple_Selection = 1
			AND GA.Group_ID = gr.Group_ID
			AND GETDATE() BETWEEN GA.Start_Date 
			AND ISNULL(GA.End_Date,GETDATE())   
			FOR XML PATH('')), 1, 1, '') +
	']' AS [SingleSelectAttributes],
	(SELECT TOP(1) Congregation_Name FROM Groups g INNER JOIN Congregations c ON g.Congregation_ID = c.Congregation_ID 
								WHERE g.Group_ID = gr.Group_ID) AS [Congregation],
	(SELECT TOP(1) Address_Line_1 FROM Groups g INNER JOIN Addresses a ON g.Offsite_Meeting_Address = a.Address_ID
								WHERE g.Group_ID = gr.Group_ID) AS [Address_Line_1],
	(SELECT TOP(1) City FROM Groups g INNER JOIN Addresses a ON g.Offsite_Meeting_Address = a.Address_ID
								WHERE g.Group_ID = gr.Group_ID) AS [City],
	(SELECT TOP(1) [State/Region] FROM Groups g INNER JOIN Addresses a ON g.Offsite_Meeting_Address = a.Address_ID
								WHERE g.Group_ID = gr.Group_ID) AS [State],
	(SELECT TOP(1) Postal_Code FROM Groups g INNER JOIN Addresses a ON g.Offsite_Meeting_Address = a.Address_ID
								WHERE g.Group_ID = gr.Group_ID) AS [Postal_Code],
	(SELECT TOP(1) Longitude FROM Groups g INNER JOIN Addresses a ON g.Offsite_Meeting_Address = a.Address_ID
								WHERE g.Group_ID = gr.Group_ID) AS [Longitude],
	(SELECT TOP(1) Latitude FROM Groups g INNER JOIN Addresses a ON g.Offsite_Meeting_Address = a.Address_ID
								WHERE g.Group_ID = gr.Group_ID) AS [Latitude],
	(SELECT TOP(1) Meeting_Day FROM Groups g INNER JOIN Meeting_Days md ON g.Meeting_Day_ID = md.Meeting_Day_ID
								WHERE g.Group_ID = gr.Group_ID) AS [MeetingDay],
	Meeting_Time as [MeetingTime],
	LTRIM(RIGHT(CONVERT(VARCHAR(20), Meeting_Time, 100), 7)) AS [MeetingTwelveHourTime], -- convert, so we search on 12 hour time
	(SELECT TOP(1) Meeting_Frequency FROM Groups g INNER JOIN Meeting_Frequencies mf ON g.Meeting_Frequency_ID = mf.Meeting_Frequency_ID
								WHERE g.Group_ID = gr.Group_ID) AS [MeetingFrequency],
	Meeting_Frequency_ID AS [MeetingFrequencyID]
	INTO #AllGroups
	FROM Groups gr 
	WHERE gr.Group_Type_ID = @GroupTypeId AND gr.Available_Online = 1 AND gr.End_Date IS NULL

	-- if we have an empty search string, return all groups
	-- (#Keywords is defined at start of proc)
	IF ((SELECT COUNT(*) FROM #Keywords) = 0)
	BEGIN
		PRINT ('No Keywords, return all results')
		SELECT * FROM #AllGroups
	END
	ELSE
	BEGIN

		DECLARE @DynamicQuery VARCHAR(MAX)
		SET @DynamicQuery = 'SELECT * FROM #AllGroups WHERE '

		DECLARE @Group_Description_Subquery NVARCHAR(MAX)
		SELECT @Group_Description_Subquery = STUFF((
			select ' UPPER(GroupDescription) LIKE ''%' + Keyword + '%'' OR '
			from #Keywords
			FOR XML PATH('')
			)
			,1,1,'')

		DECLARE @Group_Name_Subquery NVARCHAR(MAX)
		SELECT @Group_Name_Subquery = STUFF((
			select ' UPPER(Group_Name) LIKE ''%' + Keyword + '%'' OR '
			from #Keywords
			FOR XML PATH('')
			)
			,1,1,'')

		DECLARE @Group_Participants_Subquery NVARCHAR(MAX)
		SELECT @Group_Participants_Subquery = STUFF((
			select ' UPPER(GroupParticipants) LIKE ''%' + Keyword + '%'' OR '
			from #Keywords
			FOR XML PATH('')
			)
			,1,1,'')

		DECLARE @MultiSelectAttributes_Subquery NVARCHAR(MAX)
		SELECT @MultiSelectAttributes_Subquery = STUFF((
			select ' UPPER(MultiSelectAttributes) LIKE ''%' + Keyword + '%'' OR '
			from #Keywords
			FOR XML PATH('')
			)
			,1,1,'')

		DECLARE @SingleSelectAttributes_Subquery NVARCHAR(MAX)
		SELECT @SingleSelectAttributes_Subquery = STUFF((
			select ' UPPER(SingleSelectAttributes) LIKE ''%' + Keyword + '%'' OR '
			from #Keywords
			FOR XML PATH('')
			)
			,1,1,'')

		DECLARE @Site_Subquery NVARCHAR(MAX)
		SELECT @Site_Subquery = STUFF((
			select ' UPPER(Congregation) LIKE ''%' + Keyword + '%'' OR '
			from #Keywords
			FOR XML PATH('')
			)
			,1,1,'')

		DECLARE @Address_Subquery NVARCHAR(MAX)
		SELECT @Address_Subquery = STUFF((
			select ' UPPER(Address_Line_1) LIKE ''%' + Keyword + '%'' OR '
			from #Keywords
			FOR XML PATH('')
			)
			,1,1,'')

		DECLARE @City_Subquery NVARCHAR(MAX)
		SELECT @City_Subquery = STUFF((
			select ' UPPER(City) LIKE ''%' + Keyword + '%'' OR '
			from #Keywords
			FOR XML PATH('')
			)
			,1,1,'')

		DECLARE @State_Subquery NVARCHAR(MAX)
		SELECT @State_Subquery = STUFF((
			select ' UPPER(State) LIKE ''%' + Keyword + '%'' OR '
			from #Keywords
			FOR XML PATH('')
			)
			,1,1,'')

		DECLARE @Zip_Subquery NVARCHAR(MAX)
		SELECT @Zip_Subquery = STUFF((
			select ' UPPER(Postal_Code) LIKE ''%' + Keyword + '%'' OR '
			from #Keywords
			FOR XML PATH('')
			)
			,1,1,'')

		DECLARE @Meeting_Day_Subquery NVARCHAR(MAX)
		SELECT @Meeting_Day_Subquery = STUFF((
			select ' UPPER(MeetingDay) LIKE ''%' + Keyword + '%'' OR '
			from #Keywords
			FOR XML PATH('')
			)
			,1,1,'')

		DECLARE @Meeting_Time_Subquery NVARCHAR(MAX)
		SELECT @Meeting_Time_Subquery = STUFF((
			select ' UPPER(MeetingTwelveHourTime) LIKE ''%' + Keyword + '%'' OR '
			from #Keywords
			FOR XML PATH('')
			)
			,1,1,'')

		DECLARE @Meeting_Frequency_Subquery NVARCHAR(MAX)
		SELECT @Meeting_Frequency_Subquery = STUFF((
			select ' UPPER(MeetingFrequency) LIKE ''%' + Keyword + '%'' OR '
			from #Keywords
			FOR XML PATH('')
			)
			,1,1,'')

		SET @DynamicQuery = CONCAT(@DynamicQuery, @Group_Name_Subquery, @Group_Description_Subquery, @Group_Participants_Subquery, @MultiSelectAttributes_Subquery, @SingleSelectAttributes_Subquery,
			@Site_Subquery, @Address_Subquery, @City_Subquery, @State_Subquery, @Zip_Subquery, @Meeting_Day_Subquery, @Meeting_Time_Subquery, @Meeting_Frequency_Subquery)

		-- need to figure out a better way to handle the dangling OR
		SET @DynamicQuery += 'Group_Name = ''ThisIsAHack'''

		EXEC(@DynamicQuery)

	END

END
