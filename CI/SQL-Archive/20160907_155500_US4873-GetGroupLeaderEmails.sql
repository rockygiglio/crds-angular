USE [MinistryPlatform]
GO

/****** Object:  UserDefinedFunction [dbo].[crds_GetGroupLeaderEmails]    Script Date: 5/26/2016 11:40:04 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[crds_GetGroupLeaderEmails]') AND type = 'FN')
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[crds_GetGroupLeaderEmails](@GroupId INT) RETURNS NVARCHAR(MAX) AS BEGIN; RETURN 1; END'
END
GO


-- =============================================
-- Author:      Jim Kriz
-- Create date: 09/07/2016
-- Description: Get all group leader emails for a given group id.
-- =============================================
ALTER FUNCTION [dbo].[crds_GetGroupLeaderEmails](@GroupId INT)
RETURNS NVARCHAR(MAX)
AS
BEGIN
	DECLARE @LeaderEmails AS NVARCHAR(MAX);
	SELECT @LeaderEmails = STUFF((
		SELECT 
			   ', ' + C.Email_Address
		FROM  
			   Group_Participants GP, Contacts C
		WHERE  
			   GP.Group_ID = @GroupId
			   AND GETDATE() BETWEEN GP.Start_Date
			   AND ISNULL(GP.End_Date,GETDATE())
         AND GP.Group_Role_ID = 22 -- Leader 
			   AND GP.Participant_ID = C.Participant_Record
        FOR XML PATH('')), 1, 2, '');
	
	RETURN @LeaderEmails;
END
GO