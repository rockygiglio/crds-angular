Use [MinistryPlatform];

INSERT INTO [dbo].groups 
(group_name                 ,group_type_id,Ministry_id,Congregation_id,Primary_contact,description                                                                ,Start_date                ,domain_id,[Secure_Check-in],Suppress_Nametag,promotion_information,Promote_to_group,Age_in_Months_to_promote,Promote_Weekly) VALUES
('Kids Club 11-12 Month Old',4            ,2          ,5              ,7622354        ,'Kids Club Age Group 11-12 Months. This promotes Manually to a Yearly Pod.','2016-03-24 00:00:00',1        ,1             ,1            ,null                 ,null            ,null                    ,0         )
GO
INSERT INTO [dbo].groups 
(group_name                 ,group_type_id,Ministry_id,Congregation_id,Primary_contact,description                                                          ,Start_date                ,domain_id,[Secure_Check-in],Suppress_Nametag,promotion_information,Promote_to_group                                                            ,Age_in_Months_to_promote,Promote_Weekly) VALUES
('Kids Club 10-11 Month Old',4            ,2          ,5              ,7622354        ,'Kids Club Age Group 10-11 Months. This promotes to 11-12 Month Old.','2016-03-24 00:00:00',1        ,1             ,1            ,null                 ,(select group_id from groups where group_name = 'Kids Club 11-12 Month Old'),11                      ,1          )
GO
INSERT INTO [dbo].groups 
(group_name                ,group_type_id,Ministry_id,Congregation_id,Primary_contact,description                                                         ,Start_date                ,domain_id,[Secure_Check-in],Suppress_Nametag,promotion_information,Promote_to_group                                                            ,Age_in_Months_to_promote,Promote_Weekly) VALUES
('Kids Club 9-10 Month Old',4            ,2          ,5              ,7622354        ,'Kids Club Age Group 9-10 Months. This promotes to 10-11 Month Old.','2016-03-24 00:00:00',1        ,1             ,1            ,null                 ,(select group_id from groups where group_name = 'Kids Club 10-11 Month Old'),10                      ,1          )
GO
INSERT INTO [dbo].groups 
(group_name               ,group_type_id,Ministry_id,Congregation_id,Primary_contact,description                                                       ,Start_date                ,domain_id,[Secure_Check-in],Suppress_Nametag,promotion_information,Promote_to_group                                                            ,Age_in_Months_to_promote,Promote_Weekly) VALUES
('Kids Club 8-9 Month Old',4            ,2          ,5              ,7622354        ,'Kids Club Age Group 8-9 Months. This promotes to 9-10 Month Old.','2016-03-24 00:00:00',1        ,1             ,1            ,null                 ,(select group_id from groups where group_name = 'Kids Club 9-10 Month Old'),9                       ,1          )
GO
INSERT INTO [dbo].groups 
(group_name               ,group_type_id,Ministry_id,Congregation_id,Primary_contact,description                                                      ,Start_date                ,domain_id,[Secure_Check-in],Suppress_Nametag,promotion_information,Promote_to_group                                                          ,Age_in_Months_to_promote,Promote_Weekly) VALUES
('Kids Club 7-8 Month Old',4            ,2          ,5              ,7622354        ,'Kids Club Age Group 7-8 Months. This promotes to 8-9 Month Old.','2016-03-24 00:00:00',1        ,1             ,1            ,null                 ,(select group_id from groups where group_name = 'Kids Club 8-9 Month Old'),8                       ,1          )
GO
INSERT INTO [dbo].groups 
(group_name               ,group_type_id,Ministry_id,Congregation_id,Primary_contact,description                                                      ,Start_date                ,domain_id,[Secure_Check-in],Suppress_Nametag,promotion_information,Promote_to_group                                                          ,Age_in_Months_to_promote,Promote_Weekly) VALUES
('Kids Club 6-7 Month Old',4            ,2          ,5              ,7622354        ,'Kids Club Age Group 6-7 Months. This promotes to 7-8 Month Old.','2016-03-24 00:00:00',1        ,1             ,1            ,null                 ,(select group_id from groups where group_name = 'Kids Club 7-8 Month Old'),7                       ,1          )
GO
INSERT INTO [dbo].groups 
(group_name               ,group_type_id,Ministry_id,Congregation_id,Primary_contact,description                                                      ,Start_date                ,domain_id,[Secure_Check-in],Suppress_Nametag,promotion_information,Promote_to_group                                                          ,Age_in_Months_to_promote,Promote_Weekly) VALUES
('Kids Club 5-6 Month Old',4            ,2          ,5              ,7622354        ,'Kids Club Age Group 5-6 Months. This promotes to 6-7 Month Old.','2016-03-24 00:00:00',1        ,1             ,1            ,null                 ,(select group_id from groups where group_name = 'Kids Club 6-7 Month Old'),6                       ,1          )
GO
INSERT INTO [dbo].groups 
(group_name               ,group_type_id,Ministry_id,Congregation_id,Primary_contact,description                                                      ,Start_date                ,domain_id,[Secure_Check-in],Suppress_Nametag,promotion_information,Promote_to_group                                                          ,Age_in_Months_to_promote,Promote_Weekly) VALUES
('Kids Club 4-5 Month Old',4            ,2          ,5              ,7622354        ,'Kids Club Age Group 4-5 Months. This promotes to 5-6 Month Old.','2016-03-24 00:00:00',1        ,1             ,1            ,null                 ,(select group_id from groups where group_name = 'Kids Club 5-6 Month Old'),5                       ,1          )
GO
INSERT INTO [dbo].groups 
(group_name               ,group_type_id,Ministry_id,Congregation_id,Primary_contact,description                                                      ,Start_date                ,domain_id,[Secure_Check-in],Suppress_Nametag,promotion_information,Promote_to_group                                                          ,Age_in_Months_to_promote,Promote_Weekly) VALUES
('Kids Club 3-4 Month Old',4            ,2          ,5              ,7622354        ,'Kids Club Age Group 3-4 Months. This promotes to 4-5 Month Old.','2016-03-24 00:00:00',1        ,1             ,1            ,null                 ,(select group_id from groups where group_name = 'Kids Club 4-5 Month Old'),4                       ,1          )
GO
INSERT INTO [dbo].groups 
(group_name               ,group_type_id,Ministry_id,Congregation_id,Primary_contact,description                                                    ,Start_date                ,domain_id,[Secure_Check-in],Suppress_Nametag,promotion_information,Promote_to_group                                                          ,Age_in_Months_to_promote,Promote_Weekly) VALUES
('Kids Club 2-3 Month Old',4            ,2          ,5              ,7622354        ,'Kids Club Age Group 2-3 Months. This promotes to 4 Month Old.','2016-03-24 00:00:00',1        ,1             ,1            ,null                 ,(select group_id from groups where group_name = 'Kids Club 3-4 Month Old'),3                       ,1          )
GO
INSERT INTO [dbo].groups 
(group_name               ,group_type_id,Ministry_id,Congregation_id,Primary_contact,description                                                      ,Start_date                ,domain_id,[Secure_Check-in],Suppress_Nametag,promotion_information,Promote_to_group                                                          ,Age_in_Months_to_promote,Promote_Weekly) VALUES
('Kids Club 1-2 Month Old',4            ,2          ,5              ,7622354        ,'Kids Club Age Group 1-2 Months. This promotes to 2-3 Month Old.','2016-03-24 00:00:00',1        ,1             ,1            ,null                 ,(select group_id from groups where group_name = 'Kids Club 2-3 Month Old'),2                       ,1          )
GO
INSERT INTO [dbo].groups 
(group_name               ,group_type_id,Ministry_id,Congregation_id,Primary_contact,description                                                      ,Start_date                ,domain_id,[Secure_Check-in],Suppress_Nametag,promotion_information,Promote_to_group                                                          ,Age_in_Months_to_promote,Promote_Weekly) VALUES
('Kids Club 0-1 Month Old',4            ,2          ,5              ,7622354        ,'Kids Club Age Group 0-1 Months. This promotes to 1-2 Month Old.','2016-03-24 00:00:00',1        ,1             ,1            ,null                 ,(select group_id from groups where group_name = 'Kids Club 1-2 Month Old'),1                       ,1          )
GO