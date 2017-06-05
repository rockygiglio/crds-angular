USE [MinistryPlatform]
GO

UPDATE [dbo].[dp_Communications]
   SET [Body] = '<div>Welcome [Nickname]! You have successfully registered for [Group_Name] which will be held at the [Congregation_Name] site.</div><div><br />If you need childcare, sign up <a href="https://[Base_Url]/childcare" target="_self">here</a> so we can plan!<br /></div>'      
   WHERE [Communication_ID] = 5386;
GO


