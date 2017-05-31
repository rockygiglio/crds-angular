USE [MinistryPlatform]
GO
/****** Object:  StoredProcedure [dbo].[report_CRDS_Deposit_Export_Selected]    Script Date: 11/30/2016 2:32:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[report_CRDS_Deposit_Export_Selected]') AND type in (N'P', N'PC'))
BEGIN
  EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[report_CRDS_Deposit_Export_Selected] AS'
END
GO


ALTER PROCEDURE [dbo].[report_CRDS_Deposit_Export_Selected]

	@DomainID varchar(40)
	,@UserID varchar(40)
	,@PageID Int
	,@SelectionID Int


AS
BEGIN
	   DECLARE @DomainGUID uniqueidentifier 
	   SET @DomainGUID = CONVERT(uniqueidentifier, @DomainID)
	   DECLARE @UserGUID uniqueidentifier 
	   SET @UserGUID = CONVERT(uniqueidentifier, @UserID)

	   SELECT De.[Deposit_ID]
			 ,De.[Deposit_Total]
			 ,De.[Deposit_Date]
			 ,De.[Account_Number] AS Deposit_Account
			 ,De.[Batch_Count]
			 ,De.[Exported]
			 ,BDSum.Batch_Name
			 ,BDSum.Batch_ID
			 ,BDSum.[Program_Name]
			 ,BDSum.Account_Number AS Income_Account
			 ,BDSum.Statement_Title
			 ,BDSum.DistrSum
			 ,BDSum.DistrItems

	   FROM Deposits De
	   INNER JOIN dp_Domains Dom ON Dom.Domain_ID = De.Domain_ID
	   INNER JOIN (SELECT B.Batch_ID, B.Deposit_ID, B.Batch_Name, P.[Program_Name], P.Statement_Title, P.Account_Number, SUM(DD.Amount) AS DistrSum, Count(*) AS DistrItems
				 FROM dbo.Batches B 
				 INNER JOIN dbo.Donations D ON D.Batch_ID = B.Batch_ID		 
				 INNER JOIN dbo.Donation_Distributions DD ON DD.Donation_ID = D.Donation_ID		 
				 INNER JOIN dbo.Programs P ON P.Program_ID = DD.Program_ID
				 GROUP BY P.[Program_Name], P.Statement_Title, P.Account_Number, B.Batch_Name, B.Batch_ID, B.Deposit_ID
				) BDSum ON BDSum.Deposit_ID = De.Deposit_ID
	   WHERE Dom.Domain_GUID = @DomainID
		  AND De.Deposit_ID IN (SELECT Record_ID FROM dp_Selected_Records SR 
							 INNER JOIN dp_Selections S ON S.Selection_ID = SR.Selection_ID  
							 INNER JOIN dbo.dp_Users U ON U.[User_ID] = S.[User_ID] AND U.User_GUID = @UserID 
						   WHERE S.Page_ID = @PageID 
							 AND ((S.Selection_ID = @SelectionID AND @SelectionID > 0) OR (S.Selection_Name = 'dp_DEFAULT' AND @SelectionID < 1 AND Sub_Page_ID IS NULL)))
	   UNION ALL

	   SELECT De.[Deposit_ID]
			 ,De.[Deposit_Total]
			 ,De.[Deposit_Date]
			 ,De.[Account_Number] AS Deposit_Account
			 ,De.[Batch_Count]
			 ,De.[Exported]
			 ,BPSum.Batch_Name
			 ,BPSum.Batch_ID
			 ,BPSum.[Program_Name]
			 ,BPSum.Account_Number AS Income_Account
			 ,BPSum.Statement_Title
			 ,BPSum.DistrSum
			 ,BPSum.DistrItems
		FROM Deposits De
		INNER JOIN dp_Domains Dom ON Dom.Domain_ID = De.Domain_ID
		INNER JOIN (SELECT B.Batch_ID, B.Deposit_ID, B.Batch_Name, P.[Program_Name], P.Statement_Title, P.Account_Number, SUM(PD.Payment_Amount) AS DistrSum, Count(*) AS DistrItems
				 FROM dbo.Batches B 
				 INNER JOIN dbo.Payments PY ON PY.Batch_ID = B.Batch_ID
				 INNER JOIN dbo.Payment_Detail PD ON PD.Payment_ID = PY.Payment_ID
				 INNER JOIN dbo.Invoice_Detail ID ON ID.Invoice_Detail_ID = PD.Invoice_Detail_ID
				 INNER JOIN dbo.Products PR ON PR.Product_ID = ID.Product_ID
				 INNER JOIN dbo.Programs P ON P.Program_ID = PR.Program_ID
				 GROUP BY P.[Program_Name], P.Statement_Title, P.Account_Number, B.Batch_Name, B.Batch_ID, B.Deposit_ID
				 ) BPSum ON BPSum.Deposit_ID = De.Deposit_ID

	   WHERE Dom.Domain_GUID = @DomainID
	   AND De.Deposit_ID IN (SELECT Record_ID FROM dp_Selected_Records SR 
							   INNER JOIN dp_Selections S ON S.Selection_ID = SR.Selection_ID  
							   INNER JOIN dbo.dp_Users U ON U.[User_ID] = S.[User_ID] 
								  AND U.User_GUID = @UserID WHERE S.Page_ID = @PageID 
								  AND ((S.Selection_ID = @SelectionID AND @SelectionID > 0) 
								  OR (S.Selection_Name = 'dp_DEFAULT' AND @SelectionID < 1 AND Sub_Page_ID IS NULL)))

	   ORDER BY Deposit_ID, BDSum.Batch_ID, BDSum.Statement_Title

END

GO
