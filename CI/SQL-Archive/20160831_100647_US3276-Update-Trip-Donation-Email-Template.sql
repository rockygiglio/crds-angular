USE [MinistryPlatform]
GO

UPDATE [dbo].[dp_Communications]
   SET [Body] = '<p>Thank you for your investment in [Program_Name]!</p><p></p><p></p><p><b>You gave to</b>: [Program_Name] towards [Pledge_Donor]<br /></p><p>
			<b>Amount</b>: $[Donation_Amount]</p><p>
			<b>Date</b>: [Donation_Date]</p>
			<p>	<b>Payment Method</b>: [Payment_Method]</p>
			<p>If at any point you have questions, please contact our Finance team at <a href="mailto:finance@crossroads.net"><span style="color:#1155CC; font-family:Arial;">finance@crossroads.net</span></a>.</p>
			<p>No goods or services were exchanged for this gift. </p>
			<p>Note:
			If you logged in to give, this will be recorded as part of your regular giving and will be included on your quarterly statement for tax purposes. If you do not currently receive a quarterly statement, you will begin to receive one now.  For guest givers, this is your receipt but you can register at <a href="http://www.crossroads.net" target="_self">crossroads.net</a> to receive quarterly statements on future giving.</p>

		<p>Thanks again for being part of the team!<br /></p>
			<p>Crossroads </p>'
 WHERE Communication_ID = 121611
GO