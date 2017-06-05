USE [MinistryPlatform]
GO

/****** Object:  StoredProcedure [dbo].[report_CRDS_Payment_Selected_For_Batch]    Script Date: 1/6/2017 12:06:45 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[report_CRDS_Payment_Selected_For_Batch]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[report_CRDS_Payment_Selected_For_Batch] AS' 
END
GO


ALTER PROCEDURE [dbo].[report_CRDS_Payment_Selected_For_Batch]

	@DomainID varchar(40)
	,@UserID varchar(40)
	,@PageID Int
	,@SelectionID Int
	,@CongregationID Int
	,@BatchName Varchar(50)
	,@DepositDate DateTime
	,@BatchEntryTypeID Int
	,@FinalizeAndDeposit BIT = 1


AS
BEGIN

SELECT [Batch_Name] = ISNULL(@BatchName, 'Selected Payment Batch')
           ,[Setup_Date] = GetDate()
           ,[Batch_Total] = SUM(Payment_Total)
			,Batch_Status = CASE WHEN D.Batch_ID IS NULL THEN 'To Be Batched' ELSE 'Already Batched' END
           ,[Item_Count] = Count(*)
			,[Batched_Count] = SUM(CASE WHEN D.Batch_ID IS NULL THEN 0 ELSE 1 END)
           ,[Batch_Entry_Type_ID]= @BatchEntryTypeID
           ,[Finalize_Date] = Getdate()
			,Congregation_ID = @CongregationID
           ,[Domain_ID] = D.Domain_ID

FROM Payments D
 INNER JOIN dp_Domains Dom ON Dom.Domain_ID = D.Domain_ID

WHERE Dom.Domain_GUID = @DomainID
 AND D.Payment_ID IN (SELECT Record_ID FROM dp_Selected_Records SR INNER JOIN dp_Selections S ON S.Selection_ID = SR.Selection_ID  INNER JOIN dbo.dp_Users U ON U.[User_ID] = S.[User_ID] AND U.User_GUID = @UserID WHERE S.Page_ID = @PageID AND ((S.Selection_ID = @SelectionID AND @SelectionID > 0) OR (S.Selection_Name = 'dp_DEFAULT' AND @SelectionID < 1 AND S.Sub_Page_ID IS NULL)))

GROUP BY D.Domain_ID, D.Batch_ID

END



GO


