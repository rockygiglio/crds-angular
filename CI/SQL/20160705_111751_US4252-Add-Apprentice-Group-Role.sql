Use [MinistryPlatform]
GO

SET IDENTITY_INSERT [dbo].[Group_Roles] ON;

IF NOT EXISTS(Select * from Group_Roles where Role_Title = 'Apprentice')
BEGIN
INSERT INTO PressCtrlH 
(Group_Role_ID,Role_Title  ,Description,Group_Role_Type_ID,Group_Role_Direction_ID,Group_Role_Intensity_ID,Ministry_ID,Domain_ID,Background_Check_Required,Omit_From_Portal) VALUES
(66           ,'Apprentice',null       ,2                 ,null                   ,2                      ,null       ,1        ,0                        ,null            )
END
GO

SET IDENTITY_INSERT [dbo].[Group_Roles] OFF;