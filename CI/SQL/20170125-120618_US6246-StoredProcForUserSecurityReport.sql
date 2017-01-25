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
	SELECT DISTINCT P.Page_ID, P.Display_Name, 
       STUFF((SELECT ','+ Role_Name FROM[dbo].[dp_Role_Pages] RP
                                    JOIN [dp_Pages] PGS on RP.Page_ID = PGS.Page_ID
                                    JOIN [dp_Roles] R on R.Role_ID = RP.Role_ID
                                    JOIN [dp_User_Roles] UR on UR.Role_ID = R.Role_ID WHERE UR.User_ID = @UserId and PGS.Page_ID = P.Page_ID FOR XML Path('')),1,1,'') AS Security_Roles
       
       FROM [dbo].[dp_Role_Pages] RP
       JOIN [dp_Pages] P on RP.Page_ID = P.Page_ID
       JOIN [dp_Roles] R on R.Role_ID = RP.Role_ID
       JOIN [dp_User_Roles] UR on UR.Role_ID = R.Role_ID
       WHERE UR.User_ID = @UserId
	END
GO
