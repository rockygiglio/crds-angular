USE [MinistryPlatform]
GO

SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON

INSERT INTO [dbo].[dp_Page_Views]
           ([Page_View_ID]
		   ,[View_Title]
           ,[Page_ID]
		   ,[Description]
           ,[View_Clause]
           )
     VALUES
           (2309
		   ,'Pledge Assignment Needed - Deposited'
           ,296
		   ,'These distributions are against a program that is part of a capital campaign the donor is participating in and has been deposited.  Check to see if the pledge should be credited.'
           ,'Donation_Distributions.Pledge_ID IS NULL AND EXISTS (SELECT * FROM Pledges P INNER JOIN Pledge_Campaigns PC ON PC.Pledge_Campaign_ID = P.Pledge_Campaign_ID INNER JOIN Programs PR ON PR.Pledge_Campaign_ID = PC.Pledge_Campaign_ID WHERE PR.Program_ID = Donation_Distributions.Program_ID AND P.Pledge_Status_ID IN (1,4) AND P.Donor_ID = (SELECT Donor_ID FROM Donations WHERE Donations.Donation_ID = Donation_Distributions.Donation_ID AND Donation_Status_ID = 2))'
		   );

SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
GO