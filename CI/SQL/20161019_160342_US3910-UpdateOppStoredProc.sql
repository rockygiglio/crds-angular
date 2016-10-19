USE MinistryPlatform
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ===============================================================
-- Author: Ken Baum <ken.baum@ingagepartners.com>
-- Create date: 10/12/2016
-- Description:	Returns an opportunity id and title for a particular team event
-- ===============================================================

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[api_crds_Get_Opportunities_For_Team]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[api_crds_Get_Opportunities_For_Team] AS' 
END
GO

ALTER PROCEDURE [dbo].[api_crds_Get_Opportunities_For_Team]
	 @GroupID Int
	,@EventID Int
AS
BEGIN

	select opportunity_id, 
		opportunity_title,
		group_role_id,
		shift_start,
		shift_end,
		room,
		minimum_needed,
		maximum_needed
	from opportunities 
	where Add_to_Group = @GroupID 
		and Event_Type_ID = (select event_type_id from events where event_id = @EventID);


END




