using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Infrastructure.Application.Validation.Financing.CashAccountAggregate;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Application.Services.Financing.CashAccountAggregate
{
    public class CashAccountAggregateValidationService : ICashAccountAggregateValidationService
    {
        private readonly ICashAccountQueryService _cashAcctQrySvc;
        private readonly ISharedQueryService _sharedQrySvc;
        private IQueryServicesRegistry _servicesRegistry;

        public CashAccountAggregateValidationService
        (
            ICashAccountQueryService cashAcctQrySvc,
            ISharedQueryService sharedQueryService,
            IQueryServicesRegistry servicesRegistry
        )
        {
            _cashAcctQrySvc = cashAcctQrySvc;
            _sharedQrySvc = sharedQueryService;
            _servicesRegistry = servicesRegistry;

            _servicesRegistry.RegisterService("CashAccountQueryService", _cashAcctQrySvc);
            _servicesRegistry.RegisterService("SharedQueryService", _sharedQrySvc);
        }


        public virtual Task<ValidationResult> IsValidCashAccount(CashAccount cashAccount)
            => throw new NotImplementedException();

        public virtual async Task<ValidationResult> IsValidCreateCashAccountInfo(CashAccountWriteModel writeModel)
            => await CreateCashAccountInfoValidator.Validate(writeModel, _cashAcctQrySvc);

        public virtual async Task<ValidationResult> IsValidEditCashAccountInfo(CashAccountWriteModel writeModel)
            => await EditCashAccountInfoValidator.Validate(writeModel, _cashAcctQrySvc);

        public virtual async Task<ValidationResult> IsValidDeleteCashAccountInfo(CashAccountWriteModel writeModel)
            => await DeleteCashAccountInfoValidator.Validate(writeModel, _cashAcctQrySvc);

        public virtual Task<ValidationResult> IsValidCashDepositOfSalesProceeds(CashTransactionWriteModel transactionInfo)
            => throw new NotImplementedException();

        public virtual async Task<ValidationResult> IsValidCashDepositOfDebtIssueProceeds(CashTransactionWriteModel transactionInfo)
            => await ValidateDepositOfDebtIssueProceeds(transactionInfo);

        public virtual async Task<ValidationResult> IsValidCashDepositOfStockIssueProceeds(CashTransactionWriteModel transactionInfo)
        => await ValidateDepositOfStockIssueProceeds(transactionInfo);

        public virtual async Task<ValidationResult> IsValidCashDisbursementForLoanPayment(CashTransactionWriteModel transactionInfo)
            => await ValidateDisbursementForLoanPayment(transactionInfo);

        public async virtual Task<ValidationResult> IsValidCashDisbursementForDividendPayment(CashTransactionWriteModel transactionInfo)
            => await ValidateDisbursementForDividendPayment(transactionInfo);

        public async Task<ValidationResult> IsValidCreateCashAccountTransferInfo(CashAccountTransferWriteModel transferInfo)
            => await CashAccountTransferValidator.Validate(transferInfo, _cashAcctQrySvc);

        public async Task<ValidationResult> IsValidTimeCardPaymentInfo(List<CashTransactionWriteModel> writeModelCollection)
        {
            DisbursementForMultiplePayrollPymtValidator validator = new(writeModelCollection, _servicesRegistry);
            return await validator.Validate();
        }


        // private stuff
        private async Task<ValidationResult> ValidateDepositOfDebtIssueProceeds(CashTransactionWriteModel model)
        {
            DepositOfDebtIssueProceedsValidator validation = new(model, _cashAcctQrySvc, _sharedQrySvc);
            return await validation.Validate();
        }

        private async Task<ValidationResult> ValidateDepositOfStockIssueProceeds(CashTransactionWriteModel model)
        {
            DepositOfStockIssueProceedsValidator validation = new(model, _cashAcctQrySvc, _sharedQrySvc);
            return await validation.Validate();
        }

        private async Task<ValidationResult> ValidateDisbursementForLoanPayment(CashTransactionWriteModel model)
        {
            DisbursementForLoanPaymentValidator validation = new(model, _cashAcctQrySvc, _sharedQrySvc);
            return await validation.Validate();
        }

        private async Task<ValidationResult> ValidateDisbursementForDividendPayment(CashTransactionWriteModel model)
        {
            DisbursementForDividendPaymentValidator validation = new(model, _cashAcctQrySvc, _sharedQrySvc);
            return await validation.Validate();
        }
    }
}