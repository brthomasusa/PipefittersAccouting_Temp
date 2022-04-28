#pragma warning disable CS8618

using PipefittersAccounting.SharedKernel;

namespace PipefittersAccounting.Core.Financing.CashAccountAggregate.ValueObjects
{
    public class AccountBalanceInformation : ValueObject
    {
        private AccountBalanceInformation
        (
            CashTransactionAmount cashInflow,
            CashTransactionAmount cashOutflow,
            CashTransactionAmount currentBalance
        ) 
        {
            CashInflow = cashInflow;
            CashOutflow = cashOutflow;
            CurrentBalance = currentBalance;
        }

        public CashTransactionAmount CashInflow { get; }

        public CashTransactionAmount CashOutflow { get; }

        public CashTransactionAmount CurrentBalance { get; }

        public static AccountBalanceInformation Create
        (
            CashTransactionAmount cashInflow,
            CashTransactionAmount cashOutflow,
            CashTransactionAmount currentBalance            
        )
        {
            return new AccountBalanceInformation(cashInflow, cashOutflow, currentBalance);
        }        
    }
}