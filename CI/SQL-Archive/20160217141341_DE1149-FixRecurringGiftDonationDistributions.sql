USE [MinistryPlatform]
GO 

UPDATE [dbo].[Donation_Distributions]
SET Amount = R.[Recurring Gift Amount]
FROM 
(SELECT
	  g.[Recurring_Gift_ID] as "Recurring Gift ID"
	  , g.[Amount] as "Recurring Gift Amount"
	  , d.[Donation_ID] as "Donation ID"
	  , d.[Donation_Amount] as "Donation Amount"
	  , dd.[Donation_Distribution_ID] as "Donation Distribution ID"
	  , dd.[Amount] as "Donation Distribution Amount"
	FROM 
	  [dbo].[Recurring_Gifts] g
	  , [dbo].[Donations] d
	  , [dbo].[Donation_Distributions] dd
	WHERE 
	  g.[Amount] % 1 <> 0
	  and LEN(LTRIM(RTRIM(COALESCE(g.[Subscription_ID], '')))) > 0
	  and d.[Recurring_Gift_ID] = g.[Recurring_Gift_ID]
	  and d.[Donation_ID] = dd.[Donation_ID]
	  and g.[Amount] <> dd.[Amount]) R
WHERE Donation_ID = R.[Donation ID]