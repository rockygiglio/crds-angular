USE MinistryPlatform
GO

DECLARE @CommID INT = 12953

IF EXISTS(SELECT * FROM dbo.dp_Communications where Communication_ID = @CommID)
BEGIN
	UPDATE dbo.dp_Communications
	SET Body = '<p dir="ltr" style="line-height:1.38;margin-top:0pt;margin-bottom:0pt;">Welcome to your GO Trip! We are super excited to have you on this trip. Now that you have signed up, we need to get you caught up on some important next steps. </p><p dir="ltr" style="line-height:1.38;margin-top:0pt;margin-bottom:0pt;"><b id="docs-internal-guid-fdc837ab-dcc9-bfaf-f75e-e8ad4fed4984" style="font-weight:normal;"><br /></b></p><p dir="ltr" style="line-height:1.38;margin-top:0pt;margin-bottom:0pt;">Depending on your trip''s departure will determine immediate next steps. All of the trip preparation will begin 20 weeks prior to departure. This means that you will start to hear from some important people (like your trip leader) as it relates to getting prepared. So, don’t panic if you don’t hear from anyone right away. </p><p dir="ltr" style="line-height:1.38;margin-top:0pt;margin-bottom:0pt;"><b style="font-weight:normal;"><br /></b></p><p dir="ltr" style="line-height:1.38;margin-top:0pt;margin-bottom:0pt;">In the meantime, feel free to begin support raising and sharing your excitement with others (heck, invite some friends as well!) Start this journey by downloading the new Crossroads GO app on Google Play or the iTunes App store. It includes an extensive Q & A and provides suggestions on how to kick start your support raising. </p><p dir="ltr" style="line-height:1.38;margin-top:0pt;margin-bottom:0pt;"><br /></p><p dir="ltr" style="line-height:1.38;margin-top:0pt;margin-bottom:0pt;">If you have any questions along the way, email <a href="mailto:gotrips@crossroads.net" style="text-decoration:none;">gotrips@crossroads.net</a>.</p>'
	WHERE Communication_ID = @CommID
END
GO
