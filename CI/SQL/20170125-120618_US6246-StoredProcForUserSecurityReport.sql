USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[report_CRDS_PagesAccessByUser]') AND TYPE IN (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[report_CRDS_PagesAccessByUser] AS' 
END
GO

-- =============================================
-- Author:		Phil Lachmann
-- Create date: 1/25/2017
-- Description:	Get All the Pages a User(s) Can See and What Role lets them see that. 
-- =============================================
ALTER PROCEDURE [dbo].[report_CRDS_PagesAccessByUser] 
	@SelectionID int
AS
BEGIN

--DECLARE @SelectionID INT  = 27101
	
	--get the user ids from the MP selection
	DECLARE @UserList TABLE (UserID int)
	INSERT INTO @UserList(UserID)
	SELECT user_id from dp_Users U 
	 where U.USER_ID in
	 (SELECT Record_ID FROM dp_Selected_Records SR INNER JOIN dp_Selections S ON S.Selection_ID = SR.Selection_ID  
	                                                INNER JOIN dbo.dp_Users U ON U.User_ID = S.[User_ID]  
													WHERE ((S.Selection_ID = @SelectionID AND @SelectionID > 0) 
													      OR (S.Selection_Name = 'dp_DEFAULT' AND @SelectionID < 1 AND S.Sub_Page_ID IS NULL)))

	-- get the roles for the users
	DECLARE @UserRoles TABLE (UserID int, Role_ID int, Role_Name varchar(256))
	INSERT INTO @UserRoles (UserID, Role_ID, Role_Name)
	(SELECT UL.UserID, UR.Role_ID, Role_Name 
	FROM dbo.dp_User_Roles UR 
	JOIN dbo.dp_Roles R ON UR.Role_ID = R.Role_ID
	JOIN @UserList UL ON UL.UserID = UR.User_ID)
	
	DECLARE @SecurityList TABLE (User_ID int, Page_ID int, Sub_Page_ID int, SecuredObjectType varchar(20), Name varchar(256), Security_Role varchar(1024))

	-- Get Pages
	INSERT INTO @SecurityList (User_ID, Page_ID, Sub_Page_ID, SecuredObjectType, Name, Security_Role)
	(SELECT DISTINCT UU.UserID, P.Page_ID, '', 'Pages', P.Display_Name, 
       STUFF((SELECT ','+ UU.Role_Name FROM[dbo].[dp_Role_Pages] RP
                                    JOIN @UserRoles UU ON UU.Role_ID = RP.Role_ID
									WHERE  RP.Page_ID = P.Page_ID FOR XML Path('')),1,1,'') AS Security_Roles
       
       FROM [dbo].[dp_Role_Pages] RP
       JOIN [dp_Pages] P on RP.Page_ID = P.Page_ID
       JOIN @UserRoles UU ON UU.Role_ID = RP.Role_ID)

	-- Get Sub Pages
	INSERT INTO @SecurityList (User_ID, Page_ID, Sub_Page_ID, SecuredObjectType, Name, Security_Role)
	(SELECT  UU.UserID, SP.Page_ID, SP.Sub_Page_ID, 'Pages', SP.Display_Name, 
       STUFF((SELECT ','+ UU.Role_Name FROM[dbo].[dp_Role_Sub_Pages] RP
                                    JOIN @UserRoles UU ON UU.Role_ID = RP.Role_ID
									WHERE RP.Sub_Page_ID = SP.Sub_Page_ID FOR XML Path('')),1,1,'') AS Security_Roles
       
       FROM [dbo].[dp_Role_Sub_Pages] RP
       JOIN [dp_Sub_Pages] SP on RP.Sub_Page_ID = SP.Sub_Page_ID
       JOIN @UserRoles UU ON UU.Role_ID = RP.Role_ID)

	--Get Reports
	INSERT INTO @SecurityList (User_ID, Page_ID, Sub_Page_ID, SecuredObjectType, Name, Security_Role)
	(SELECT DISTINCT UU.UserID, R.Report_ID, '', 'Reports', R.Report_Name, 
		STUFF((SELECT ','+ UU.Role_Name FROM [dbo].[dp_Role_Reports] RR
									JOIN @UserRoles UU ON UU.Role_ID = RR.Role_ID
									WHERE RR.Report_ID = R.Report_ID FOR XML PATH('')),1,1,'') AS Security_Roles
		FROM dbo.dp_Role_Reports RP
		JOIN dbo.dp_Reports R ON RP.Report_ID = R.Report_ID
		JOIN @UserRoles UU ON RP.Role_ID = UU.Role_ID)

	--Get Tools
	INSERT INTO @SecurityList (User_ID, Page_ID, Sub_Page_ID, SecuredObjectType, Name, Security_Role)
	(SELECT DISTINCT UU.UserID, T.Tool_ID, '', 'Tools', T.Tool_Name, 
		STUFF((SELECT ','+ UU.Role_Name FROM [dbo].[dp_Role_Tools] RT
									JOIN @UserRoles UU ON UU.Role_ID = RT.Role_ID
									WHERE RT.Tool_ID = T.Tool_ID FOR XML PATH('')),1,1,'') AS Security_Roles
		FROM dbo.dp_Role_Tools RT
		JOIN dbo.dp_Tools T ON RT.Tool_ID = T.Tool_ID
		JOIN @UserRoles UU ON RT.Role_ID = UU.Role_ID)

	--Get API Procedures
	INSERT INTO @SecurityList (User_ID, Page_ID, Sub_Page_ID, SecuredObjectType, Name, Security_Role)
	(SELECT DISTINCT UU.UserID, A.API_Procedure_ID, '', 'API Procedures', A.Procedure_Name, 
		STUFF((SELECT ','+ UU.Role_Name FROM [dbo].[dp_Role_API_Procedures] RA
									JOIN @UserRoles UU ON UU.Role_ID = RA.Role_ID
									WHERE RA.API_Procedure_ID = A.API_Procedure_ID FOR XML PATH('')),1,1,'') AS Security_Roles
		FROM dbo.dp_Role_API_Procedures RA
		JOIN dbo.dp_API_Procedures A ON RA.API_Procedure_ID = A.API_Procedure_ID
		JOIN @UserRoles UU ON RA.Role_ID = UU.Role_ID)

	SELECT * FROM @SecurityList --order by user_id,page_id
	END

GO


