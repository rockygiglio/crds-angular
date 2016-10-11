USE MinistryPlatform
GO

DECLARE @APIId INT = 341
DECLARE @RoleId INT = 62

SET IDENTITY_INSERT dbo.dp_API_Procedures ON
INSERT INTO dbo.dp_API_Procedures(API_Procedure_ID,Procedure_Name,Description) VALUES(@APIId,'api_crds_GetCampEvent','Get Event information for Camp')
SET IDENTITY_INSERT dbo.dp_API_Procedures OFF


INSERT INTO dbo.dp_Role_API_Procedures
        ( Role_ID ,
          API_Procedure_ID ,
          Domain_ID
        )
VALUES  ( @RoleId , -- Role_ID - int
          @APIId , -- API_Procedure_ID - int
          1  -- Domain_ID - int
        )

		
GO

