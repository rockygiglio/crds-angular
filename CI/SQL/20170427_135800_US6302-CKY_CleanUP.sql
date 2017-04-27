USE [MinistryPlatform]
GO

--DELETE STORED PROCS
IF OBJECT_ID('api_sync_GetChanges', 'P') IS NOT NULL
DROP PROC api_sync_GetChanges
GO
IF OBJECT_ID('api_sync_GetContactByID', 'P') IS NOT NULL
DROP PROC api_sync_GetContactByID
GO
IF OBJECT_ID('api_sync_GetContacts', 'P') IS NOT NULL
DROP PROC api_sync_GetContacts
GO
IF OBJECT_ID('api_sync_GetHouseholdByID', 'P') IS NOT NULL
DROP PROC api_sync_GetHouseholdByID
GO
IF OBJECT_ID('api_sync_GetHouseholds', 'P') IS NOT NULL
DROP PROC api_sync_GetHouseholds
GO
IF OBJECT_ID('api_sync_GetLastAuditLog', 'P') IS NOT NULL
DROP PROC api_sync_GetLastAuditLog
GO
IF OBJECT_ID('api_sync_GetUserByContactID', 'P') IS NOT NULL
DROP PROC api_sync_GetUserByContactID
GO
IF OBJECT_ID('api_sync_GetUserByID', 'P') IS NOT NULL
DROP PROC api_sync_GetUserByID
GO
IF OBJECT_ID('api_sync_GetUsers', 'P') IS NOT NULL
DROP PROC api_sync_GetUsers
GO


--PAGES Same ids in all envs
--Sync Events, Sync Event Types, Sync Event Operations(script referenced had as sub page but this is a page), Sync Sources. 

DELETE FROM dp_Pages 
WHERE page_id in (1002, 1003, 1004, 1005) 

DELETE FROM dp_Page_Section_Pages where Page_ID in (1002, 1003, 1004, 1005)

DELETE FROM dp_Role_Pages WHERE Page_ID in (1002, 1003, 1004, 1005)

--TABLES
DROP TABLE Sync_Sources;
DROP TABLE Sync_Event_Types;
DROP TABLE Sync_Events;
DROP TABLE Sync_Event_Operations;

--Adam: I think that these are yours. Ok to clean up?
DROP TABLE temp_CKY_batches;
DROP TABLE temp_CKY_DeactiveUser;
DROP TABLE temp_CKY_donation;
DROP TABLE temp_CKY_Downtown;
DROP TABLE temp_CKY_Fund_To_Pledge_Campaign;
DROP TABLE temp_CKY_Fund_To_Program;
DROP TABLE temp_CKY_funds;
DROP TABLE temp_CKY_pledges;
DROP TABLE temp_CKY_users;
DROP TABLE temp_CKY_UserSites;

