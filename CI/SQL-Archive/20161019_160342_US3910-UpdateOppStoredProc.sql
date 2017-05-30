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

	select o.opportunity_id, 
		o.opportunity_title,
		o.group_role_id,
		o.shift_start,
		o.shift_end,
		o.room,
		o.minimum_needed,
		o.maximum_needed, 
		gr.Role_Title
	from opportunities o, group_roles gr
	where o.Add_to_Group = @GroupID 
		and o.Event_Type_ID = (select event_type_id from events where event_id = @EventID)
		and o.group_role_id = gr.group_role_id 
		Order By o.group_role_id desc;
END




