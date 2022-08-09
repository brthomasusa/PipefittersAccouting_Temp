using PipefittersAccounting.Core.CashManagement.CashAccountAggregate;
using PipefittersAccounting.Core.CashManagement.CashAccountAggregate.Events;
using PipefittersAccounting.Core.Interfaces;
using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Application.Validation.Financing;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.Interfaces;

namespace PipefittersAccounting.Infrastructure.Application.EventHandlers.Financing
{
    public class CreateCashAccountEventHandler : IDomainEventHandler<CashAccountCreatedEvent>
    {
        private readonly ICashAccountAggregateValidationService _validationService;

        public CreateCashAccountEventHandler(ICashAccountAggregateValidationService validationService)
            => _validationService = validationService;

        public async void Handle(CashAccountCreatedEvent domainEvent)
        {
            ValidationResult result = await _validationService.IsValidCashAccount(domainEvent.CashAccount);
        }
    }
}