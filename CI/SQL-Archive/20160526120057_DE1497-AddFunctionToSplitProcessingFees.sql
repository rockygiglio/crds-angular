USE [MinistryPlatform]
GO

/****** Object:  UserDefinedFunction [dbo].[crds_udfGetOrdinalNumber]    Script Date: 5/26/2016 11:40:04 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      Dustin Kocher
-- Create date: 05/26/2016
-- Description: Split Processing Fees on Donation Distributions.  This will assign
--				the processing fee to the first of each distribution of a donation
--				and assign 0 to the others.  This is based on first donation
--				distribution id.
-- =============================================
CREATE FUNCTION [dbo].[crds_udfSplitDDProcessorFees](@Donation_ID INT, @Donation_Distribution_ID INT)
RETURNS MONEY
AS
BEGIN
	DECLARE @First_Donation_Distribution_ID INT;
	DECLARE @Processor_Fee MONEY;
  
	SET @Processor_Fee = 0;

	-- Get the first Donation Distribution ID for a Donation
	SELECT TOP 1 @First_Donation_Distribution_ID = Donation_Distribution_ID
		FROM [dbo].Donation_Distributions
		where Donation_ID = @Donation_ID
		ORDER BY Donation_Distribution_ID ASC;

	IF @First_Donation_Distribution_ID = @Donation_Distribution_ID
	BEGIN
		SELECT @Processor_Fee = PROCESSOR_FEE_AMOUNT
		FROM [dbo].Donations
		WHERE Donation_ID = @Donation_ID
	END


	RETURN @Processor_Fee
END

GO
