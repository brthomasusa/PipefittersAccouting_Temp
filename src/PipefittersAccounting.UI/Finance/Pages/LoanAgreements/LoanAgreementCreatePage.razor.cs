using Microsoft.AspNetCore.Components;
using Blazorise;
using PipefittersAccounting.SharedModel;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.UI.Finance.Components;
using PipefittersAccounting.UI.Interfaces;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Finance.Pages.LoanAgreements
{
    public partial class LoanAgreementCreatePage
    {
        private bool _showEditDialog;
        private string _selectedTab = "loanInfo";
        private const string _returnUri = "Finance/Pages/LoanAgreements/LoanAgreementsPage";
        private const string _formTitle = "Create Loan Agreement Info";
        private string? _snackBarMessage;
        private LoanAgreementDataEntryState _state = new();

        [Inject] public ILoanAgreementRepository? LoanAgreementService { get; set; }
        [Inject] public IMessageService? MessageService { get; set; }

        protected async override Task OnInitializedAsync()
        {
            await GetFinanciers();
            _state.LoanWriteModel = new()
            {
                LoanId = Guid.NewGuid(),
                UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
            };
        }

        private async Task GetFinanciers()
        {
            OperationResult<List<FinancierLookup>> result = await LoanAgreementService!.GetFinanciersLookup(new GetFinanciersLookup() { });

            if (result.Success)
            {
                _state.Financiers = result.Result;
            }
            else
            {
                await MessageService!.Error($"Error while retrieving list of financiers: {result.NonSuccessMessage}", "Error");
            }
        }

        private async Task<OperationResult<bool>> Save()
        {
            if (_state.LoanWriteModel.AmortizationSchedule.Count > 0)
            {
                OperationResult<LoanAgreementReadModel> result = await LoanAgreementService!.CreateLoanAgreement(_state.LoanWriteModel!);

                if (result.Success)
                {
                    _snackBarMessage = $"Information for loan number {result.Result.LoanNumber} was successfully created.";
                    _state.LoanWriteModel = result.Result.Map();
                    await InvokeAsync(StateHasChanged);
                    return OperationResult<bool>.CreateSuccessResult(true);
                }
                else
                {
                    await MessageService!.Error($"Error while creating loan agreement: {result.NonSuccessMessage}", "Error");
                    return OperationResult<bool>.CreateFailure(result.NonSuccessMessage);
                }
            }
            else
            {
                string msg = "A loan agreement must have an amortization schedule.";
                await MessageService!.Error($"Error while creating loan agreement: {msg}", "Error");
                return OperationResult<bool>.CreateFailure(msg);
            }

        }

        private Task OnSelectedTabChanged(string name)
        {
            _selectedTab = name;

            return Task.CompletedTask;
        }

        private void OnActionItemClicked(string action, Guid installmentId)
        {
            // LoanInstallments on second tab
        }

        private async Task CreateAmortizationSchedule(string creationMethod)
        {
            bool isValid = _state.LoanWriteModel!.LoanId != default &&
                           _state.LoanWriteModel!.LoanAmount > 0 &&
                           _state.LoanWriteModel!.LoanDate != default &&
                           _state.LoanWriteModel!.MaturityDate != default &&
                           _state.LoanWriteModel!.MaturityDate > _state.LoanWriteModel!.LoanDate;

            if (isValid)
            {
                switch (creationMethod)
                {
                    case "auto":
                        await GenerateAmortizationSchedule();
                        break;
                    case "manual":
                        await ShowDataEntryDialog();
                        break;
                    case "import":
                        break;
                };
            }
            else
            {
                string msg = "It seems that one or more of the following is missing: loan id, interest rate, loan amount, loan date, or maturity date.";
                await MessageService!.Warning(msg, "Missing loan agreement info!");
            }
        }

        private async Task ShowDataEntryDialog()
        {
            _showEditDialog = true;
            await InvokeAsync(StateHasChanged);
        }

        private async Task OnEditDialogClosed(string action)
        {
            if (action.Equals("saved"))
            {
                await Task.CompletedTask;
            }

            _showEditDialog = false;
        }

        private async Task GenerateAmortizationSchedule()
        {
            LoanAmortizationCalculator schedule = LoanAmortizationCalculator.Create
            (
                _state.LoanWriteModel!.InterestRate,
                _state.LoanWriteModel!.LoanAmount,
                _state.LoanWriteModel!.LoanDate.AddMonths(1),
                _state.LoanWriteModel!.MaturityDate
            );

            foreach (InstallmentRecord installment in schedule.RepaymentSchedule)
            {
                _state.LoanWriteModel!.AmortizationSchedule.Add
                (
                    new LoanInstallmentWriteModel()
                    {
                        LoanInstallmentId = Guid.NewGuid(),
                        LoanId = _state.LoanWriteModel!.LoanId,
                        InstallmentNumber = installment.InstallmentNumber,
                        PaymentDueDate = installment.PaymentDueDate,
                        PaymentAmount = installment.Payment,
                        PrincipalPymtAmount = installment.Principal,
                        InterestPymtAmount = installment.Interest,
                        PrincipalRemaining = installment.RemainingBalance,
                        UserId = _state.LoanWriteModel!.UserId
                    }
                );
            }

            _state.LoanWriteModel!.NumberOfInstallments = schedule.RepaymentSchedule.Count;
            await InvokeAsync(StateHasChanged);
        }
    }
}