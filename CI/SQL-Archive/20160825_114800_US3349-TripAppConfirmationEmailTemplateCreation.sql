USE MinistryPlatform
GO

DECLARE @CommunicationId INT = 2001
DECLARE @Subject NVARCHAR(256) = 'Trip Application Received'
DECLARE @Body NVARCHAR(MAX) = '<p dir="ltr" style="line-height:1.38;margin-top:0pt;margin-bottom:0pt;">Welcome to GO [Destination_Name]! We are super excited to have you on this trip. Now that you have signed up, we need to get you caught up on some important next steps. </p><p dir="ltr" style="line-height:1.38;margin-top:0pt;margin-bottom:0pt;"><b id="docs-internal-guid-fdc837ab-c27d-6d23-1d7b-2dc78c29ec0c" style="font-weight:normal;"><br /></b></p><p dir="ltr" style="line-height:1.38;margin-top:0pt;margin-bottom:0pt;">Depending on your trip''s departure will determine immediate next steps. All of the trip preparation will begin 20 weeks prior to departure. This means that you will start to hear from some important people (like your trip leader) as it relates to getting prepared. So, don’t panic if you don’t hear from anyone right away. </p><p dir="ltr" style="line-height:1.38;margin-top:0pt;margin-bottom:0pt;"><b style="font-weight:normal;"><br /></b></p><p dir="ltr" style="line-height:1.38;margin-top:0pt;margin-bottom:0pt;">In the meantime, feel free to begin support raising and sharing your excitement with others (heck, invite some friends as well!) Start this journey by downloading the new Crossroads GO app on Google Play or the iTunes App store. It includes an extensive Q & A and provides suggestions on how to kick start your support raising. </p><p dir="ltr" style="line-height:1.38;margin-top:0pt;margin-bottom:0pt;"><br />If you have any questions along the way, email <a href="mailto:gotrips@crossroads.net" style="text-decoration:none;">gotrips@crossroads.net</a>.<br /></p><p dir="ltr" style="line-height:1.38;margin-top:0pt;margin-bottom:0pt;"><br /></p><p dir="ltr" style="line-height:1.38;margin-top:0pt;margin-bottom:0pt;">Thank you for your contribution. Please use this as your receipt.</p><p dir="ltr" style="line-height:1.38;margin-top:0pt;margin-bottom:0pt;"><br /></p><p dir="ltr" style="line-height:1.38;margin-top:0pt;margin-bottom:0pt;">You gave to: [Program_Name]</p><p dir="ltr" style="line-height:1.38;margin-top:0pt;margin-bottom:0pt;">Amount: $[Donation_Amount]</p><p dir="ltr" style="line-height:1.38;margin-top:0pt;margin-bottom:0pt;">Date: [Donation_Date]</p><p dir="ltr" style="line-height:1.38;margin-top:0pt;margin-bottom:0pt;">Payment Method: [Payment_Method]</p><p dir="ltr" style="line-height:1.38;margin-top:0pt;margin-bottom:0pt;"><b id="docs-internal-guid-fdc837ab-c27e-780c-946d-b51c37506156" style="font-weight:normal;"><br /></b></p><p dir="ltr" style="line-height:1.38;margin-top:0pt;margin-bottom:0pt;">If at any point you have questions, please contact our Finance team at finance@crossroads.net.</p><p dir="ltr" style="line-height:1.38;margin-top:0pt;margin-bottom:0pt;"><br /></p><p dir="ltr" style="line-height:1.38;margin-top:0pt;margin-bottom:0pt;">No goods or services were exchanged for this gift.</p>'

SET IDENTITY_INSERT [dbo].[dp_Communications] ON

IF NOT EXISTS(SELECT * FROM [dbo].[dp_Communications] WHERE Communication_ID = @CommunicationId)
BEGIN
 INSERT INTO [dbo].[dp_Communications](Communication_ID,Author_User_ID,Subject,Body,Domain_ID,Start_Date,From_Contact,Reply_to_Contact,Template,Active)
	VALUES(@CommunicationId, 5, @Subject, @Body, 1, GETDATE(),1519180,1519180,1,1)
END

SET IDENTITY_INSERT [dbo].[dp_Communications] OFF

GO
