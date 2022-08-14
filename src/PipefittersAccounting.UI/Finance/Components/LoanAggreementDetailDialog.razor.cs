using Microsoft.AspNetCore.Components;
using Blazorise;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.UI.Interfaces;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Finance.Components
{
    public partial class LoanAggreementDetailDialog
    {
        private string _selectedTab = "loanInfo";
        private string? _formTitle;
        private Modal? _detailModalRef;
        private LoanAgreementDetail? _loanDetail;

        [Parameter] public Guid LoanId { get; set; }
        [Inject] public ILoanAgreementRepository? LoanAgreementService { get; set; }
        [Inject] public IMessageService? MessageService { get; set; }

        protected async override Task OnParametersSetAsync()
        {
            if (LoanId != default)
            {
                await GetLoanAgreement();
                await InvokeAsync(StateHasChanged);

                if (_detailModalRef is not null)
                {
                    await _detailModalRef!.Show();
                }
            }
        }

        private async Task GetLoanAgreement()
        {
            GetLoanAgreement loanAgreementParameters = new() { LoanId = this.LoanId };

            OperationResult<LoanAgreementDetail> result =
                await LoanAgreementService!.GetLoanAgreementDetails(loanAgreementParameters);

            if (result.Success)
            {
                _loanDetail = result.Result;
                _formTitle = $"Loan number: {_loanDetail.LoanNumber}";
            }
            else
            {
                await MessageService!.Error($"Error while retrieving loan agreement details: {result.NonSuccessMessage}", "Error");
            }
        }

        private Task OnSelectedTabChanged(string name)
        {
            _selectedTab = name;

            return Task.CompletedTask;
        }

        private async Task CloseDialog() => await _detailModalRef!.Hide();

        private string ConvertPercentage() => $"{_loanDetail!.InterestRate * 100}%";
    }
}