using PipefittersAccounting.SharedKernel.Interfaces;

namespace PipefittersAccounting.Core.Financing.CashAccountAggregate.Events
{
    public class CashAccountCreated : IDomainEvent
    {
        public Guid CashAccountId { get; set; }
        public CashAccountTypeEnum AccountType { get; set; }
        public string? BankName { get; set; }
        public string? CashAccountName { get; set; }
        public string? CashAccountNumber { get; set; }
        public string? RoutingTransitNumber { get; set; }
        public DateTime DateOpened { get; set; }
        public Guid UserId { get; set; }
    }
}
