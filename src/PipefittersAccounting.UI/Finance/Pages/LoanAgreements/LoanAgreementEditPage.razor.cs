using Microsoft.AspNetCore.Components;
using Blazorise;
using PipefittersAccounting.SharedModel;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.UI.Finance.Components;
using PipefittersAccounting.UI.Interfaces;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Finance.Pages.LoanAgreements
{
    public partial class LoanAgreementEditPage
    {
        private string _selectedTab = "loanInfo";
        private string? _selectedLoanNumber;
        private const string _returnUri = "Finance/Pages/LoanAgreements/LoanAgreementsPage";
        private const string _formTitle = "Edit Loan Agreement Info";
        private string? _snackBarMessage;
        private LoanAgreementDataEntryState _state = new();

        [Parameter] public Guid LoanId { get; set; }
        [Inject] public ILoanAgreementRepository? LoanAgreementService { get; set; }
        [Inject] public IMessageService? MessageService { get; set; }

        protected async override Task OnInitializedAsync()
        {
            await GetLoanAgreement();
        }

        private async Task GetLoanAgreement()
        {
            GetLoanAgreement loanAgreementParameters = new() { LoanId = this.LoanId };

            OperationResult<LoanAgreementDetail> result =
                await LoanAgreementService!.GetLoanAgreementDetails(loanAgreementParameters);

            if (result.Success)
            {
                await GetFinanciers();
                _selectedLoanNumber = result.Result.LoanNumber;
                _state.LoanWriteModel = result.Result.Map();

                List<LoanInstallmentDetail> schedule = result.Result.LoanInstallmentDetailsList!;

                foreach (LoanInstallmentDetail item in schedule!)
                {
                    _state.LoanWriteModel.AmortizationSchedule.Add(item.Map());
                }

                await InvokeAsync(StateHasChanged);
            }
            else
            {
                await MessageService!.Error($"Error while retrieving loan agreement details: {result.NonSuccessMessage}", "Error");
            }
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
            OperationResult<bool> updateResult = await LoanAgreementService!.EditLoanAgreement(_state.LoanWriteModel!);

            if (updateResult.Success)
            {
                _snackBarMessage = $"Information for loan number {_selectedLoanNumber} was successfully updated.";
                await InvokeAsync(StateHasChanged);
                return OperationResult<bool>.CreateSuccessResult(true);
            }
            else
            {
                await MessageService!.Error($"Error while retrieving employee: {updateResult.NonSuccessMessage}", "Error");
                return OperationResult<bool>.CreateFailure(updateResult.NonSuccessMessage);
            }
        }

        private Task OnSelectedTabChanged(string name)
        {
            _selectedTab = name;

            return Task.CompletedTask;
        }

        private void OnActionItemClicked(string action, Guid installmentId)
        {

        }
    }
}