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
		DECLARE @ChildcareEventTypeId INT = 243
		DECLARE @ChildcareGroupTypeId INT = 27

		DECLARE @ChildcareEvents TABLE
		(
			EventID INT
		)

		INSERT INTO @ChildcareEvents
		SELECT e.Event_ID 
		FROM events e
		WHERE e.Event_Type_ID = @ChildcareEventTypeId

		DECLARE @ChildcareGroups TABLE
		(
			GroupId INT
		)

		INSERT INTO @ChildcareGroups
		SELECT g.group_id
		FROM groups g
		WHERE g.Group_Type_ID = @ChildcareGroupTypeId

		SELECT DISTINCT EventID AS Event_ID FROM @ChildcareEvents
		EXCEPT
		SELECT eg.Event_ID FROM event_groups eg
		JOIN events e ON e.event_id = eg.Event_ID
		WHERE e.Event_Type_ID = @ChildcareEventTypeId
		AND   eg.Event_ID IN (SELECT EventID FROM @ChildcareEvents)
		AND   eg.Group_ID IN (SELECT GroupID FROM @ChildcareGroups)

END 
GO
