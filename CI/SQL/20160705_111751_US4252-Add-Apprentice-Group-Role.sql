Use [MinistryPlatform]
GO

IF NOT EXISTS(SELECT * FROM Group_Roles WHERE Role_Title = 'Apprentice')
BEGIN

SET IDENTITY_INSERT [dbo].[Group_Roles] ON;

INSERT INTO Group_Roles 
(Group_Role_ID,Role_Title  ,Description,Group_Role_Type_ID,Group_Role_Direction_ID,Group_Role_Intensity_ID,Ministry_ID,Domain_ID,Background_Check_Required,Omit_From_Portal) VALUES
(66           ,'Apprentice',NULL       ,2                 ,NULL                   ,2                      ,NULL       ,1        ,0                        ,NULL            )

SET IDENTITY_INSERT [dbo].[Group_Roles] OFF;

END

