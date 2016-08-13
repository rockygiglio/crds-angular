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
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[api_crds_GetOrphanChildcareEvents]') AND TYPE IN (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[api_crds_GetOrphanChildcareEvents] AS'
END
GO

ALTER PROCEDURE [dbo].[api_crds_GetOrphanChildcareEvents]
AS
BEGIN
		DECLARE @ChildcareEvents TABLE
		(
			EventID INT
		)

		INSERT INTO @ChildcareEvents
		SELECT e.Event_ID 
		FROM events e
		JOIN event_types et ON et.event_type_id = e.event_type_id
		WHERE et.Event_Type = 'Childcare'

		DECLARE @ChildcareGroups TABLE
		(
			GroupId INT
		)

		INSERT INTO @ChildcareGroups
		SELECT g.group_id
		FROM groups g
		JOIN group_types gt ON gt.group_type_id = g.Group_Type_ID
		WHERE gt.Group_Type = 'Childcare'

		SELECT DISTINCT EventID AS Event_ID FROM @ChildcareEvents
		EXCEPT
		SELECT eg.Event_ID FROM event_groups eg
		JOIN events e ON e.event_id = eg.Event_ID
		JOIN Event_Types et ON et.event_type_id = e.event_type_id
		WHERE et.Event_Type = 'Childcare'
		AND   eg.Event_ID IN (SELECT EventID FROM @ChildcareEvents)
		AND   eg.Group_ID IN (SELECT GroupID FROM @ChildcareGroups)

END 
GO
