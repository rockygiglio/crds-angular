Use [MinistryPlatform];

INSERT INTO [dbo].groups 
(group_name            ,group_type_id,Ministry_id,Congregation_id,Primary_contact,description                                               ,Start_date                ,domain_id,[Secure-Check-in],Suppress_Nametag,promotion_information,Promote_to_group,Age_in_Months_to_promote,Promote_Weekly) VALUES
('High School Grade 12',4            ,34         ,5              ,7622354        ,'Participants should be cleared at the end of school year','2016-03-24 00:00:00',1        ,1             ,1            ,null                 ,null            ,null                    ,0         );
GO

INSERT INTO [dbo].groups 
(group_name            ,group_type_id,Ministry_id,Congregation_id,Primary_contact,description                                                 ,Start_date                ,domain_id,[Secure-Check-in],Suppress_Nametag,promotion_information,Promote_to_group                                                       ,Age_in_Months_to_promote,Promote_Weekly) VALUES
('High School Grade 11',4            ,34         ,5              ,7622354        ,'Promotes to High School Grade 12 at the end of school year','2016-03-24 00:00:00',1        ,1             ,1            ,null                 ,(select group_id from groups where group_name = 'High School Grade 12'),null                    ,0         );
GO

INSERT INTO [dbo].groups 
(group_name            ,group_type_id,Ministry_id,Congregation_id,Primary_contact,description                                                 ,Start_date                ,domain_id,[Secure-Check-in],Suppress_Nametag,promotion_information,Promote_to_group                                                       ,Age_in_Months_to_promote,Promote_Weekly) VALUES
('High School Grade 10',4            ,34         ,5              ,7622354        ,'Promotes to High School Grade 11 at the end of school year','2016-03-24 00:00:00',1        ,1             ,1            ,null                 ,(select group_id from groups where group_name = 'High School Grade 11'),null                    ,0         );
GO

INSERT INTO [dbo].groups 
(group_name           ,group_type_id,Ministry_id,Congregation_id,Primary_contact,description                                                 ,Start_date                ,domain_id,[Secure-Check-in],Suppress_Nametag,promotion_information,Promote_to_group                                                       ,Age_in_Months_to_promote,Promote_Weekly) VALUES
('High School Grade 9',4            ,34         ,5              ,7622354        ,'Promotes to High School Grade 10 at the end of school year','2016-03-24 00:00:00',1        ,1             ,1            ,null                 ,(select group_id from groups where group_name = 'High School Grade 10'),null                    ,0         );
GO

INSERT INTO [dbo].groups 
(group_name                ,group_type_id,Ministry_id,Congregation_id,Primary_contact,description                                                ,Start_date                ,domain_id,[Secure-Check-in],Suppress_Nametag,promotion_information,Promote_to_group                                                      ,Age_in_Months_to_promote,Promote_Weekly) VALUES
('Student Ministry Grade 8',4            ,34         ,5              ,7622354        ,'Promotes to High School Grade 9 at the end of school year','2016-03-24 00:00:00',1        ,1             ,1            ,null                 ,(select group_id from groups where group_name = 'High School Grade 9'),null                    ,0         );
GO

INSERT INTO [dbo].groups 
(group_name                ,group_type_id,Ministry_id,Congregation_id,Primary_contact,description                                                     ,Start_date                ,domain_id,[Secure-Check-in],Suppress_Nametag,promotion_information,Promote_to_group                                                           ,Age_in_Months_to_promote,Promote_Weekly) VALUES
('Student Ministry Grade 7',4            ,34         ,5              ,7622354        ,'Promotes to Student Ministry Grade 8 at the end of school year','2016-03-24 00:00:00',1        ,1             ,1            ,null                 ,(select group_id from groups where group_name = 'Student Ministry Grade 8'),null                    ,0         );
GO

INSERT INTO [dbo].groups 
(group_name                ,group_type_id,Ministry_id,Congregation_id,Primary_contact,description                                                     ,Start_date                ,domain_id,[Secure-Check-in],Suppress_Nametag,promotion_information,Promote_to_group                                                           ,Age_in_Months_to_promote,Promote_Weekly) VALUES
('Student Ministry Grade 6',4            ,34         ,5              ,7622354        ,'Promotes to Student Ministry Grade 7 at the end of school year','2016-03-24 00:00:00',1        ,1             ,1            ,null                 ,(select group_id from groups where group_name = 'Student Ministry Grade 7'),null                    ,0         );
GO

INSERT INTO [dbo].groups 
(group_name         ,group_type_id,Ministry_id,Congregation_id,Primary_contact,description                                                     ,Start_date                ,domain_id,[Secure-Check-in],Suppress_Nametag,promotion_information,Promote_to_group                                                           ,Age_in_Months_to_promote,Promote_Weekly) VALUES
('Kids Club Grade 5',4            ,2          ,5              ,7622354        ,'Promotes to Student Ministry Grade 6 at the end of school year','2016-03-24 00:00:00',1        ,1             ,1            ,null                 ,(select group_id from groups where group_name = 'Student Ministry Grade 6'),null                    ,0         );
GO

INSERT INTO [dbo].groups 
(group_name         ,group_type_id,Ministry_id,Congregation_id,Primary_contact,description                                              ,Start_date                ,domain_id,[Secure-Check-in],Suppress_Nametag,promotion_information,Promote_to_group                                                    ,Age_in_Months_to_promote,Promote_Weekly) VALUES
('Kids Club Grade 4',4            ,2          ,5              ,7622354        ,'Promotes to Kids Club Grade 5 at the end of school year','2016-03-24 00:00:00',1        ,1             ,1            ,null                 ,(select group_id from groups where group_name = 'Kids Club Grade 5'),null                    ,0         );
GO

INSERT INTO [dbo].groups 
(group_name         ,group_type_id,Ministry_id,Congregation_id,Primary_contact,description                                              ,Start_date                ,domain_id,[Secure-Check-in],Suppress_Nametag,promotion_information,Promote_to_group                                                    ,Age_in_Months_to_promote,Promote_Weekly) VALUES
('Kids Club Grade 3',4            ,2          ,5              ,7622354        ,'Promotes to Kids Club Grade 4 at the end of school year','2016-03-24 00:00:00',1        ,1             ,1            ,null                 ,(select group_id from groups where group_name = 'Kids Club Grade 4'),null                    ,0         );
GO

INSERT INTO [dbo].groups 
(group_name         ,group_type_id,Ministry_id,Congregation_id,Primary_contact,description                                              ,Start_date                ,domain_id,[Secure-Check-in],Suppress_Nametag,promotion_information,Promote_to_group                                                    ,Age_in_Months_to_promote,Promote_Weekly) VALUES
('Kids Club Grade 2',4            ,2          ,5              ,7622354        ,'Promotes to Kids Club Grade 3 at the end of school year','2016-03-24 00:00:00',1        ,1             ,1            ,null                 ,(select group_id from groups where group_name = 'Kids Club Grade 3'),null                    ,0         );
GO

INSERT INTO [dbo].groups 
(group_name         ,group_type_id,Ministry_id,Congregation_id,Primary_contact,description                                              ,Start_date                ,domain_id,[Secure-Check-in],Suppress_Nametag,promotion_information,Promote_to_group                                                    ,Age_in_Months_to_promote,Promote_Weekly) VALUES
('Kids Club Grade 1',4            ,2          ,5              ,7622354        ,'Promotes to Kids Club Grade 2 at the end of school year','2016-03-24 00:00:00',1        ,1             ,1            ,null                 ,(select group_id from groups where group_name = 'Kids Club Grade 2'),null                    ,0         );
GO

INSERT INTO [dbo].groups 
(group_name              ,group_type_id,Ministry_id,Congregation_id,Primary_contact,description                                              ,Start_date                ,domain_id,[Secure-Check-in],Suppress_Nametag,promotion_information,Promote_to_group                                                    ,Age_in_Months_to_promote,Promote_Weekly) VALUES
('Kids Club Kindergarten',4            ,2          ,5              ,7622354        ,'Promotes to Kids Club Grade 1 at the end of school year','2016-03-24 00:00:00',1        ,1             ,1            ,null                 ,(select group_id from groups where group_name = 'Kids Club Grade 1'),null                    ,0         );
GO