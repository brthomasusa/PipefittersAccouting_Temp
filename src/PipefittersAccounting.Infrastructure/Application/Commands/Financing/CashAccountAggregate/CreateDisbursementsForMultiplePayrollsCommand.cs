using PipefittersAccounting.Core.Financing.CashAccountAggregate;
using PipefittersAccounting.Core.Financing.CashAccountAggregate.ValueObjects;
using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;

namespace PipefittersAccounting.Infrastructure.Application.Commands.Financing.CashAccountAggregate
{
    public class CreateDisbursementsForMultiplePayrollsCommand
        : WriteCommandCollectionHandler<List<PayrollRegisterWriteModel>,
                                        ICashAccountAggregateRepository,
                                        ICashAccountAggregateValidationService,
                                        CashAccount>
    {
        public CreateDisbursementsForMultiplePayrollsCommand
        (
            List<PayrollRegisterWriteModel> writeModelCollection,
            ICashAccountAggregateRepository repo,
            ICashAccountAggregateValidationService validationService,
            IUnitOfWork unitOfWork
        ) : base(writeModelCollection, repo, validationService, unitOfWork)
        {

        }

        protected async override Task<ValidationResult> Validate()
        {
            return await ValidationService.IsValidPayrollRegister(WriteModels);
        }

        protected override async Task<OperationResult<bool>> ProcessCommand()
        {
            try
            {
                //TODO Fix this! it's a mess.
                OperationResult<CashAccount> result = await Repository.GetByIdAsync(WriteModels[0].CashAccountId);

                if (result.Success)
                {
                    CashAccount cashAccount = result.Result;

                    int checkNumber = 5015;

                    WriteModels.ForEach(model => cashAccount.DisburseCash
                    (
                        new CashTransaction
                        (
                        CashTransactionTypeEnum.CashDisbursementTimeCardPayment,
                        EntityGuidID.Create(model.CashAccountId),
                        CashTransactionDate.Create(new DateTime(2022, 2, 28)),
                        CashTransactionAmount.Create(model.NetPay),
                        EntityGuidID.Create(model.EmployeeId),
                        EntityGuidID.Create(model.TimeCardId),
                        CheckNumber.Create(checkNumber++.ToString()),
                        RemittanceAdvice.Create(model.EmployeeName ?? model.EmployeeId.ToString()),
                        EntityGuidID.Create(new Guid("660bb318-649e-470d-9d2b-693bfb0b2744"))
                        )
                    ));

                    OperationResult<bool> updateResult = await Repository.UpdateCashAccountAsync(cashAccount);

                    return OperationResult<bool>.CreateSuccessResult(true);
                }
                else
                {
                    return OperationResult<bool>.CreateFailure(result.NonSuccessMessage);
                }

            }
            catch (Exception ex)
            {
                return OperationResult<bool>.CreateFailure(ex.Message);
            }
        }
    }
}
