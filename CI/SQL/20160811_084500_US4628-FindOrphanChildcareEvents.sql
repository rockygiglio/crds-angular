USE MinistryPlatform
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ===============================================================
-- Author: Phil Lachmann
-- Create date: 8/11/2016
-- Description:	Find Childcare Events with no childcare group
-- ===============================================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[api_crds_GetOrphanChildcareEvents]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[api_crds_GetOrphanChildcareEvents] AS'
END
GO

ALTER PROCEDURE [dbo].[api_crds_GetOrphanChildcareEvents]
AS
BEGIN
		DECLARE @ChildcareEvents TABLE
		(
			EventID int
		)

		INSERT INTO @ChildcareEvents
		SELECT e.Event_ID 
		from events e
		join event_types et on et.event_type_id = e.event_type_id
		where et.Event_Type = 'Childcare'

		DECLARE @ChildcareGroups TABLE
		(
			GroupId int
		)

		insert into @ChildcareGroups
		select g.group_id
		from groups g
		join group_types gt on gt.group_type_id = g.Group_Type_ID
		where gt.Group_Type = 'Childcare'

		select distinct EventID as Event_ID from @ChildcareEvents
		EXCEPT
		select eg.Event_ID from event_groups eg
		join events e on e.event_id = eg.Event_ID
		join Event_Types et on et.event_type_id = e.event_type_id
		where et.Event_Type = 'Childcare'
		and   eg.Event_ID in (select EventID from @ChildcareEvents)
		and   eg.Group_ID in (select GroupID from @ChildcareGroups)

END 
GO
