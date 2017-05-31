use MinistryPlatform
go

DECLARE @VIEW_ID int = 0
DECLARE @VIEW_TITLE nvarchar(255) = N'Pending Childcare Requests'
DECLARE @PAGE_ID int = 36
DECLARE @DESCRIPTION nvarchar(255) = N'Show only pending childcare requests'

DECLARE @FIELD_LIST nvarchar(1000) = N'Requester_ID_Table.[Display_Name] AS [Display Name], Group_ID_Table.[Group_Name] AS [Group Name], Ministry_ID_Table.[Ministry_Name] AS [Ministry Name]
, Congregation_ID_Table.[Congregation_Name] AS [Congregation Name], cr_Childcare_Requests.[Start_Date] AS [Start Date], cr_Childcare_Requests.[End_Date] AS [End Date]
, cr_Childcare_Requests.[Frequency] AS [Frequency], cr_Childcare_Requests.[Childcare_Session] AS [Childcare Session], cr_Childcare_Requests.[Est_No_of_Children] AS [Est No of Children], cr_Childcare_Requests.[Notes] AS [Notes], Request_Status_ID_Table.[Request_Status] AS [Request Status]'

DECLARE @VIEW_CLAUSE nvarchar(255) = N'Request_Status_ID_Table.[Request_Status] = ''Pending'''
SELECT @VIEW_ID = [Page_View_ID] FROM [dbo].[dp_Page_Views] WHERE [View_Title] = @VIEW_TITLE;

IF @VIEW_ID = 0
BEGIN
	PRINT 'Inserting new page view'
	DECLARE @temp_pageviewid table (id int)
	INSERT INTO [dbo].[dp_Page_Views] (
			[View_Title]
		,[Page_ID]
		,[Description]
		,[Field_List]
		,[View_Clause])
	OUTPUT Inserted.Page_View_ID into @temp_pageviewid
	VALUES (
			@VIEW_TITLE
		,@PAGE_ID
		,@DESCRIPTION
		,@FIELD_LIST
		,@VIEW_CLAUSE
	)
	SELECT @VIEW_ID = id from @temp_pageviewid;
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
	WHERE Page_View_ID = @VIEW_ID
END

PRINT 'Adding view as the default view on Childcare Requests'
UPDATE [dbo].[dp_Pages] 
	SET [Default_View] = @VIEW_ID
	WHERE [Page_ID] = @PAGE_ID


declare @mistake table(idx int identity(1,1), pageId int, defaultView varchar(255));
insert into @mistake (pageId, defaultView) 
		values (282, 469),

(292,387),
(294,559),
(297,357),
(299,607),
(309,718),
(316,343),
(322,347),
(330,744),
(331,696),
(338,699),
(344,605),
(345,492),
(346,682),
(348,420),
(358,92148),
(359,747),
(363,529),
(366,729),
(373,621),
(375,369),
(377,692),
(382,423),
(384,448),
(397,629),
(400,555),
(408,791),
(412,741),
(417,798),
(436,850),
(437,854),
(438,857),
(450,888),
(455,389),
(461,880),
(474,389),
(475,389),
(481,387),
(490,374),
(536,347),
(537,92304),
(538,92357)
