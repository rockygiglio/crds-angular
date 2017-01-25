SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Phil Lachmann
-- Create date: 1/25/2017
-- Description:	Get All the Pages a User Can See and What Role lets them see that. 
-- =============================================
CREATE PROCEDURE report_CRDS_PagesAccessByUser 
	@UserId int
AS
BEGIN
	DECLARE @SecurityList TABLE (Page_ID int, Sub_Page_ID int, Name varchar(256), Security_Role varchar(1024))

	INSERT INTO @SecurityList (Page_ID, Sub_Page_ID, Name, Security_Role)
	(SELECT DISTINCT P.Page_ID, '', P.Display_Name, 
       STUFF((SELECT ','+ Role_Name FROM[dbo].[dp_Role_Pages] RP
                                    JOIN [dp_Pages] PGS on RP.Page_ID = PGS.Page_ID
                                    JOIN [dp_Roles] R on R.Role_ID = RP.Role_ID
                                    JOIN [dp_User_Roles] UR on UR.Role_ID = R.Role_ID WHERE UR.User_ID = @UserId and PGS.Page_ID = P.Page_ID FOR XML Path('')),1,1,'') AS Security_Roles
       
       FROM [dbo].[dp_Role_Pages] RP
       JOIN [dp_Pages] P on RP.Page_ID = P.Page_ID
       JOIN [dp_Roles] R on R.Role_ID = RP.Role_ID
       JOIN [dp_User_Roles] UR on UR.Role_ID = R.Role_ID
       WHERE UR.User_ID = @UserId)

	INSERT INTO @SecurityList (Page_ID, Sub_Page_ID, Name, Security_Role)
	(SELECT SP.Page_ID, SP.Sub_Page_ID, SP.Display_Name, 
       STUFF((SELECT ','+ Role_Name FROM[dbo].[dp_Role_Sub_Pages] RP
                                    JOIN [dp_Sub_Pages] SPGS on RP.Sub_Page_ID = SPGS.Sub_Page_ID
                                    JOIN [dp_Roles] R on R.Role_ID = RP.Role_ID
                                    JOIN [dp_User_Roles] UR on UR.Role_ID = R.Role_ID WHERE UR.User_ID = @UserId and SPGS.Sub_Page_ID = SP.Sub_Page_ID FOR XML Path('')),1,1,'') AS Security_Roles
       
       FROM [dbo].[dp_Role_Sub_Pages] RP
       JOIN [dp_Sub_Pages] SP on RP.Sub_Page_ID = SP.Sub_Page_ID
       JOIN [dp_Roles] R on R.Role_ID = RP.Role_ID
       JOIN [dp_User_Roles] UR on UR.Role_ID = R.Role_ID
       WHERE UR.User_ID = @UserId)

	SELECT * FROM @SecurityList
	END
GO
