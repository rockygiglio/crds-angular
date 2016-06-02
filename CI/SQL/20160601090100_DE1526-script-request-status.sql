use MinistryPlatform
go

DELETE FROM [dbo].[cr_Childcare_Requests]

DELETE FROM [dbo].[cr_Childcare_Request_Statuses]

SET IDENTITY_INSERT [dbo].[cr_Childcare_Request_Statuses] ON

INSERT INTO [dbo].[cr_Childcare_Request_Statuses] ( 
	 [Childcare_Request_Status_ID]
	,[Domain_ID]
	,[Request_Status]
	) VALUES (
		 1
		,1
		,N'Approved'
	),
	(
		 2
		,1
		,N'Conditionally Approved'
	),
	(
		 3
		,1
		,N'Pending'
	) ,
	(
		 4
		,1
		,N'Rejected'
	)

SET IDENTITY_INSERT [dbo].[cr_Childcare_Request_Statuses] OFF