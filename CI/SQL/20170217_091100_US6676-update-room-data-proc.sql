-- =============================================
-- Author:      John Cleaver
-- Create date: 2017-2-13
-- Description:	Update event groups on an event
-- =============================================

USE [MinistryPlatform]
GO

/****** Object:  StoredProcedure [dbo].[api_crds_Update_Single_Room_Checkin_Data]    Script Date: 1/6/2017 12:06:45 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[api_crds_Update_Single_Room_Checkin_Data]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[api_crds_Update_Single_Room_Checkin_Data] AS' 
END
GO

ALTER PROCEDURE [dbo].[api_crds_Update_Single_Room_Checkin_Data]
	@EventId INT,
	@RoomId INT,
	@GroupsXml XML
AS
BEGIN

-- Default values for a new event room
DECLARE @Allow_Checkin INT = 0
DECLARE @Capacity INT = 0
DECLARE @Volunteers INT = 0

-- Decalred here, set below for the moment - not sure this needs to come in from the proc call
DECLARE @EventRoomId INT

DECLARE @AdventureClubEventTypeID INT = 20

-- hardcoded until I'm sure this actually works right
SET @EventID = 4534870
SET @RoomID = 1972

-- These fields are for the event room, and will eventually come down as params
DECLARE @EventRoomAdventureClub BIT = 0

-- 1. Get the event data for the Event ID - either the parent or the ac event
DECLARE @Events TABLE
(
	Event_ID int,
	Event_Type_ID int,
	Parent_Event_ID int
)

INSERT INTO @Events (Event_ID, Event_Type_ID, Parent_Event_ID)
	SELECT DISTINCT Event_ID, Event_Type_ID, Parent_Event_ID
	FROM [dbo].[Events]
	WHERE Event_ID = @EventID OR Parent_Event_ID = @EventID

-- delete data on the inactive event
DECLARE @InactiveEventRoomId INT
IF (SELECT Event_Type_ID FROM @Events WHERE Event_ID = @EventID) <> @AdventureClubEventTypeID
	BEGIN
		-- If the event is not an AC event - delete AC event room
		SET @InactiveEventRoomId = (SELECT Event_Room_ID FROM Event_Rooms WHERE Event_ID = (SELECT TOP(1)
			Event_ID FROM @Events WHERE Event_Type_ID = @AdventureClubEventTypeID) AND Room_ID = @RoomID)

		-- delete from event groups
		DELETE FROM Event_Groups WHERE Event_Room_ID = @EventRoomId

		-- delete the event room
		DELETE FROM Event_Rooms WHERE Event_Room_ID = @EventRoomId
	END
ELSE
	BEGIN
		-- If the event is an AC event - delete parent event room
		-- This will have to be tweaked if we go to using subevents for everything, not just
		-- Adventure Club events
		SET @InactiveEventRoomId = (SELECT Event_Room_ID FROM Event_Rooms WHERE Event_ID = (SELECT TOP(1)
			Event_ID FROM @Events WHERE Event_Type_ID <> @AdventureClubEventTypeID) AND Room_ID = @RoomID)

		-- delete from event groups
		DELETE FROM Event_Groups WHERE Event_Room_ID = @EventRoomId

		-- delete the event room
		DELETE FROM Event_Rooms WHERE Event_Room_ID = @EventRoomId
	END


-- Create the Event Room, if needed, or just get the event room ID
IF NOT EXISTS (SELECT * FROM Event_Rooms WHERE Event_ID=@EventId AND Room_ID=@RoomID)
BEGIN
	INSERT INTO Event_Rooms
		([Event_ID], [Room_ID], [Domain_ID], [Hidden], [Allow_Checkin], [Capacity], [Volunteers])
	VALUES 
		(@EventId, @RoomId, 1, 0, @Allow_Checkin, @Capacity, @Volunteers)

	-- double check that this is working as expected
	SELECT @EventRoomId = Event_Room_ID FROM Event_Rooms WHERE Event_Room_ID = SCOPE_IDENTITY()
	PRINT 'Inserted event room ID: ' + @EventRoomId
END
ELSE
	BEGIN
		SELECT @EventRoomId = Event_Room_ID FROM Event_Rooms WHERE Event_ID = @EventId AND Room_ID = @RoomId
	END

-- delete all groups on the event room before re-adding
DELETE FROM Event_Groups WHERE Event_Room_ID = @EventRoomId AND Event_ID = @EventId AND Room_ID = @RoomId

-- all group ids are going into this table
DECLARE @AllGroupIds TABLE
(
	Group_ID INT
)

-- groups already on the event
DECLARE @ExtantGroupIds TABLE
(
	GroupId INT
)

INSERT INTO @ExtantGroupIds 
SELECT Group_ID
FROM Event_Groups WHERE Event_ID = @EventId AND Room_ID = @RoomId



-----------------------------------
---- 6. Create Nursery Event Groups
-----------------------------------
DECLARE @NurseryGroups TABLE
(
	Id INT,
	TypeId INT,
	Selected BIT
)

INSERT INTO @NurseryGroups (Id, TypeId, Selected)
SELECT DISTINCT
	'Id' = x.v.value('Id[1]', 'int'),
	'TypeId' = x.v.value('TypeId[1]', 'int'),
	'Selected' = x.v.value('Selected[1]', 'bit')
FROM 
	@GroupsXml.nodes('Groups/NurseryGroupXml/Attribute') x(v)

-- Get the nursery group ids for selected groups that are not already on the event
INSERT INTO @AllGroupIds SELECT Group_ID FROM Attributes a INNER JOIN Group_Attributes ga ON a.Attribute_ID = ga.Attribute_ID
WHERE a.Attribute_ID IN (SELECT Id FROM @NurseryGroups ng WHERE Selected=1) AND ga.Group_ID NOT IN (SELECT * FROM @ExtantGroupIds)

-------------------------------
---- 7. Create Age Event Groups
-------------------------------
--select g1.group_id
--from group_attributes g1, group_attributes g2 where g1.group_id = g2.group_id and g1.attribute_id=9002 and g2.attribute_id=9015

DECLARE @AgeGroups TABLE
(
	YearId INT,
	YearTypeId INT,
	MonthId INT,
	MonthTypeId INT,
	Selected BIT
)

INSERT INTO @AgeGroups (YearId, YearTypeId, MonthId, MonthTypeId, Selected)
SELECT DISTINCT
	'YearId' = x.v.value('YearId[1]', 'int'),
	'YearTypeId' = x.v.value('YearTypeId[1]', 'int'),
	'MonthId' = x.v.value('MonthId[1]', 'int'),
	'MonthTypeId' = x.v.value('MonthTypeId[1]', 'int'),
	'Selected' = x.v.value('Selected[1]', 'bit')
FROM 
	@GroupsXml.nodes('Groups/YearGroupXml/Attribute') x(v)

---- Get the nursery group ids for selected groups that are not already on the event
--INSERT INTO @AllGroupIds SELECT Group_ID FROM Attributes a INNER JOIN Group_Attributes ga ON a.Attribute_ID = ga.Attribute_ID
--WHERE a.Attribute_ID IN (SELECT Id FROM @NurseryGroups ng WHERE Selected=1) AND ga.Group_ID NOT IN (SELECT * FROM @ExtantGroupIds)


-- Add the new event groups for all group types here
DECLARE @YearId INT
DECLARE @MonthId INT
DECLARE @Selected IGT

DECLARE id_cursor CURSOR FOR SELECT YearId, MonthId FROM @AgeGroups WHERE @Selected=1

OPEN id_cursor
FETCH NEXT FROM id_cursor INTO @YearId, @MonthId

WHILE @@FETCH_STATUS = 0
BEGIN

	INSERT INTO @AllGroupIds
	SELECT g1.Group_ID
	FROM Group_Attributes g1, Group_Attributes g2 WHERE g1.Group_ID = g2.Group_ID and g1.Attribute_ID=@YearId and g2.Attribute_ID=@MonthId

	FETCH NEXT FROM id_cursor INTO @YearId, @MonthId
END

CLOSE id_cursor
DEALLOCATE id_cursor

-- need to add in a cursor to populate the data
--select g1.group_id
--from group_attributes g1, group_attributes g2 where g1.group_id = g2.group_id and g1.attribute_id=9002 and g2.attribute_id=9015


----------------------------
---- 7.5 Create Grade Groups
----------------------------








-- Add the new event groups for all group types here
DECLARE @Group_ID INT
DECLARE group_cursor CURSOR FOR SELECT Group_ID FROM @AllGroupIds

OPEN group_cursor
FETCH NEXT FROM group_cursor INTO @Group_ID

WHILE @@FETCH_STATUS = 0
BEGIN

	INSERT INTO Event_Groups (Group_ID, Event_ID, Room_ID, Domain_ID, Event_Room_ID)
	VALUES (@Group_ID, @EventId, @RoomId, 1, @EventRoomId)

	FETCH NEXT FROM group_cursor INTO @Group_ID
END

CLOSE group_cursor
DEALLOCATE group_cursor







-- 8. Correctly set adventure club status - if there are existing event rooms on the AC event, uncancel the AC event,
-- otherwise cancel it
IF EXISTS (SELECT * FROM Event_Rooms WHERE @EventId = (SELECT Event_ID FROM @Events WHERE Event_Type_ID = @AdventureClubEventTypeID))
BEGIN
	UPDATE [Events] SET Cancelled = 0 WHERE Event_ID = (SELECT Event_ID FROM @Events WHERE Event_Type_ID = @AdventureClubEventTypeID)
END
ELSE
BEGIN
	UPDATE [Events] SET Cancelled = 1 WHERE Event_ID = (SELECT Event_ID FROM @Events WHERE Event_Type_ID = @AdventureClubEventTypeID)
END

-- testing to make sure the right groups are getting set
SELECT * FROM @NurseryGroups

--SELECT @EventRoomId
SELECT * FROM @AllGroupIds

SELECT * FROM @AgeGroups

END
GO
