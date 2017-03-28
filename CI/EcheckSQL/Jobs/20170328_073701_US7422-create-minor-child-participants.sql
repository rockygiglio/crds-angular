USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[crds_Create_Minor_Child_Participants]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[crds_Create_Minor_Child_Participants] AS' 
END
GO
-- =============================================
-- Author:      John Cleaver
-- Create date: 2017-03-28
-- Description:	Creates crds_Create_Minor_Child_Participants to add participant records for active minor children
-- that do not already have a participant record
--
-- This script must run first!
-- =============================================
ALTER PROCEDURE [dbo].[crds_Create_Minor_Child_Participants] 
AS
BEGIN
	INSERT INTO  [MinistryPlatform].[dbo].[Participants] 
	([Contact_ID],[Participant_Type_ID],[Participant_Start_Date],[Notes],[Domain_ID],[Approved_Small_Group_Leader])
	select 
	c.Contact_ID, 
	2 as Participant_Type_ID, 
	getdate() as Participant_Start_Date, 
	'Created by crds_Create_Minor_Child_Participants daily job' as Notes, --NEED TO DECIDE WHAT THIS SHOULD SAY
	1 as Domain_ID, 
	0 as Approved_Small_Group_Leader
	from [dbo].[Contacts] as c (nolock)
	left join [dbo].[Participants] as p (nolock)
	on p.contact_id = c.contact_id
	where c.[Household_Position_ID] = 2 
	and c.[Contact_Status_ID] = 1
	and p.[Participant_ID] is null
	and c.household_id is not null;
END