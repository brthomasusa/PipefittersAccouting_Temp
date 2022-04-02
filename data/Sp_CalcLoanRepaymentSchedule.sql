SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROC [Finance].[Sp_CalcLoanRepaymentSchedule]   
@LoanAmount decimal(18,2),   
@InterestRate decimal(18,2),   
@LoanPeriod Int,   
@StartPaymentDate DATETIME  
AS  
BEGIN  
SET NOCOUNT ON  
  
DECLARE   
  
@Payment decimal(12,2),   
@InstallmentNumber FLOAT,   
@Payment2 decimal(12,2),  
@TotalPayment decimal(12,2),  
@FinanceCharges FLOAT,  
@CompoundingPeriod FLOAT,  
@CompoundingInterest FLOAT,  
@CurrentBalance decimal(12,2),  
@Principal FLOAT,  
@Interest FLOAT,  
@LoanPaymentEndDate DATETIME,  
@LoanPayDate DATETIME,  
@LoanDueDate DATETIME   
  
  
  
SET @InterestRate = @InterestRate/100   
  
SET @CompoundingPeriod = 12   
  
  
/*** END USER VARIABLES ***/   
  
SET @CompoundingInterest = @InterestRate/@CompoundingPeriod   
  
SET @Payment = ROUND((((@InterestRate/12) * @LoanAmount)/(1- ( POWER( (1 + (@InterestRate/12)),(-1 * @LoanPeriod) )))),2)   
  
SET @TotalPayment = @Payment * @LoanPeriod   
  
SET @FinanceCharges = @TotalPayment - @LoanAmount   
  
IF EXISTS(SELECT object_id FROM tempdb.sys.objects WHERE name LIKE '#EMI%')   
  
BEGIN   
  
DROP TABLE #EMI   
  
END   
  
/*** IT'S A TEMPORERY TABLE ***/   
  
CREATE TABLE #EMI(   
  
Installment INT   
  
,DueDate SMALLDATETIME   
  
,Payment decimal(12,2)   
  
,BalanceRemaining decimal(12,2)
  
,Interest decimal(12,2)   
  
,Principal decimal(12,2)   
  
)   
  
SET @InstallmentNumber = 1   
  
SET @LoanPaymentEndDate = DATEADD(month,@LoanPeriod,@StartPaymentDate)   
  
SET @LoanPayDate = @StartPaymentDate  
  
BEGIN   
  
WHILE (@InstallmentNumber < = @LoanPeriod)   
  
BEGIN   
  
SET @CurrentBalance = ROUND (@LoanAmount * POWER( (1+ @CompoundingInterest) , @InstallmentNumber ) - ( (ROUND(@Payment,2)/@CompoundingInterest) * (POWER((1 + @CompoundingInterest),@InstallmentNumber ) - 1)),0)   
  
SET @Principal =   
CASE   
WHEN @InstallmentNumber = 1   
THEN   
ROUND((ROUND(@LoanAmount,0) - ROUND(@CurrentBalance,0)),0)   
ELSE   
ROUND ((SELECT ABS(ROUND(BalanceRemaining,0) - ROUND(@CurrentBalance,0))   
FROM #EMI   
WHERE Installment = @InstallmentNumber -1),2)   
END   
  
SET @Interest = ROUND(ABS(ROUND(@Payment,2) - ROUND(@Principal,2)),2)   
  
SET @LoanDueDate = @LoanPayDate   
  
INSERT   
#EMI  
  
SELECT   
  
@InstallmentNumber,   
@LoanDueDate,   
@Payment,   
@CurrentBalance,   
@Interest,   
@Principal   
  
SET @InstallmentNumber = @InstallmentNumber + 1   
  
SET @LoanPayDate = DATEADD(MM,1,@LoanPayDate)   
  
END   
  
END   
  
SELECT Installment, DueDate AS 'Payment Due Date', Payment, Principal, Interest, BalanceRemaining AS 'Balance Remaining' FROM #EMI  
END  
GO
