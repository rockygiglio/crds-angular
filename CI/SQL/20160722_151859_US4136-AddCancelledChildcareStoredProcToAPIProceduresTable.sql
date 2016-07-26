USE [MinistryPlatform]
GO

INSERT INTO [dbo].[dp_API_Procedures]
           ([Procedure_Name]
           ,[Description])
     VALUES
           ('api_crds_CancelledChildcareNotification'
           ,'Gets data needed for childcare cancellation emails')
GO