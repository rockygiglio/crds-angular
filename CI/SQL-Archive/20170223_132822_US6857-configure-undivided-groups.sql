use MinistryPlatform
go

declare @PARENT_TEMPLATE as nvarchar(255) = N'Undivided | {site} | Spring 2017';
declare @UNDIVIDED_GROUP_TYPE as int = 26;    --Undivided
declare @UNDIVIDED_GROUP_MINISTRY as int = 8; --Spiritual Growth
declare @PRIMARY_CONTACT as int = 7592977;    --Kristie Dierig

declare @UNDIVIDED_PARENT_GROUP_START_DATE as datetime = '2017-03-03'; -- length of registration
declare @UNDIVIDED_PARENT_GROUP_END_DATE as datetime = '2017-08-01';

declare @UNDIVIDED_SUB_GROUP_START_DATE as Date = '2017-03-03'; -- start date same as registration start date
declare @UNDIVIDED_SUB_GROUP_END_DATE as Date = '2017-08-01'; --end date, when undivided sessions end


-- These are currently the only sites we are worried about. 
-- If more sites are needed, add them to the this temp table AND add them to case statement below
declare @sites table(idx int identity(1,1), siteId int, siteName varchar(255), groupDescription varchar(255));
insert into @sites (siteId, siteName, groupDescription) 
		values (7, 'Florence', '6:30 - 8:30pm, Every Thursday from April 20 to May 25')
		     , (6, 'Mason', '6:30 - 8:30pm, Every Wednesday from April 19 to May 24')
			 , (1, 'Oakley', '6:30 - 8:30pm,  Every Monday from April 17 to May 22')
			 , (16, 'Oxford', '6:30 - 8:30pm,  Every Thursday from April 20 to May 25')
			 , (11, 'Uptown', '6:30 - 8:30pm,  Every Tuesday from April 18 to May 23')
			 , (15, 'Anywhere', '(Flexible days and times from April 17-May 26');

declare @i int;
declare @cnt int;

select @i = min(idx) - 1, @cnt = max(idx) from @sites;

while @i < @cnt
begin
	select @i = @i + 1;
	
	declare @siteId as int
	declare @siteName as varchar(255)
	declare @groupDescription as varchar(255)
	declare @groupName as varchar(255)
	declare @groupId as int = 0
	select @siteId = siteId, @siteName = siteName, @groupDescription = groupDescription from @sites where idx = @i;
	
	set @groupName = REPLACE(@PARENT_TEMPLATE, '{site}', @siteName);
	select @groupId = Group_ID from [dbo].[Groups] where Group_Name = @groupName
	
	-- MAKE SURE THE PARENT GROUP EXISTS
	IF NOT (@groupId > 0)
	BEGIN
		DECLARE @temp_groupId table (id int)
		PRINT 'Inserting new group ' + @groupName;
		INSERT INTO [dbo].[Groups] (
			 [Group_Name]
			,[Group_Type_ID]
			,[Ministry_ID]
			,[Congregation_ID]
			,[Primary_Contact]
			,[Description]
			,[Start_Date]
			,[End_Date]
			,[Domain_ID])
		OUTPUT Inserted.Group_ID into @temp_groupId
	    VALUES (
			 @groupName
			,@UNDIVIDED_GROUP_TYPE
			,@UNDIVIDED_GROUP_MINISTRY
			,@siteId
			,@PRIMARY_CONTACT
			,@groupDescription
			,@UNDIVIDED_PARENT_GROUP_START_DATE
			,@UNDIVIDED_PARENT_GROUP_END_DATE
			,1
			);
		SELECT @groupId = id from @temp_groupId;
	END

--	 CREATE THE SUBGROUPS
	DECLARE @subgroup_cnt int
	DECLARE @subgroup_idx int = 0
	set @subgroup_cnt = CASE @siteName
		WHEN 'Florence' THEN 4
		WHEN 'Oakley' THEN 60
		WHEN 'Mason' THEN 10
		WHEN 'Oxford' THEN 4
		WHEN 'Uptown' THEN 4
		ELSE 0
	END

	while @subgroup_idx < @subgroup_cnt
	BEGIN
		select @subgroup_idx = @subgroup_idx + 1;
		DECLARE @subgroup_name nvarchar(255)
		DECLARE @subgroup_id int = 0

		SET @subgroup_name = REPLACE(@PARENT_TEMPLATE, '{site}', @siteName) + ' | #' + RIGHT('0' + CAST(@subgroup_idx as varchar), 2)
		PRINT 'Looking for group ' + @subgroup_name
		
		select @subgroup_id = Group_ID from [dbo].[Groups] where Group_Name = @subgroup_name
		IF NOT (@subgroup_id > 0)
		BEGIN
			PRINT 'Creating group ' + @subgroup_name
			INSERT INTO [dbo].[Groups] (
			 [Group_Name]
			,[Parent_Group]
			,[Group_Type_ID]
			,[Ministry_ID]
			,[Congregation_ID]
			,[Primary_Contact]
			,[Description]
			,[Start_Date]
			,[End_Date]
			,[Domain_ID])
		OUTPUT Inserted.Group_ID into @temp_groupId
	    VALUES (
			 @subgroup_name
			,@groupId
			,@UNDIVIDED_GROUP_TYPE
			,@UNDIVIDED_GROUP_MINISTRY
			,@siteId
			,@PRIMARY_CONTACT
			,@groupDescription
			,@UNDIVIDED_SUB_GROUP_START_DATE
			,@UNDIVIDED_SUB_GROUP_END_DATE
			,1
			);
		END
		
	END
end
