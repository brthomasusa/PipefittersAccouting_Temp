using Microsoft.AspNetCore.Components;
using Blazorise;
using Blazorise.Snackbar;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.UI.Interfaces;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Finance.Pages.LoanAgreements
{
    public partial class LoanAgreementsPage : ComponentBase
    {
        private bool _showDeleteDialog;
        private Guid _selectedLoanId;
        private string _placeHolderTextForSearch = "Search by loan number";
        private string? _snackBarMessage;
        private Snackbar? _snackbar;
        private GetLoanAgreements? _getLoanAgreementsParameters;
        private GetLoanAgreementByLoanNumber? _getLoanAgreementByLoanNumber;
        private List<LoanAgreementListItem>? _loanAgreementList;
        private LoanAgreementDetail? _selectedLoanAgreement;
        private MetaData? _metaData;
        private Func<int, int, Task>? _pagerChangedEventHandler;

        [Inject] public ILoanAgreementRepository? LoanAgreementService { get; set; }
        [Inject] public IMessageService? MessageService { get; set; }
        [Inject] public NavigationManager? NavManager { get; set; }

        protected async override Task OnInitializedAsync()
        {
            _pagerChangedEventHandler = GetAllLoanAgreements;
            await _pagerChangedEventHandler.Invoke(1, 10);
        }

        private async Task GetAllLoanAgreements(int pageNumber, int pageSize)
        {
            _showDeleteDialog = false;
            _getLoanAgreementsParameters = new() { Page = pageNumber, PageSize = pageSize };

            OperationResult<PagingResponse<LoanAgreementListItem>> result =
                await LoanAgreementService!.GetLoanAgreementListItems(_getLoanAgreementsParameters);

            if (result.Success)
            {
                _loanAgreementList = result.Result.Items;
                _metaData = result.Result.MetaData;
                StateHasChanged();
            }
            else
            {
                await MessageService!.Error($"Error while retrieving employees: {result.NonSuccessMessage}", "Error");
            }
        }

        private async Task GetLoanAgreement(Guid loanId)
        {
            GetLoanAgreement queryParam = new() { LoanId = loanId };

            OperationResult<LoanAgreementDetail> result =
                await LoanAgreementService!.GetLoanAgreementDetails(queryParam);

            if (result.Success)
            {
                _showDeleteDialog = true;
                _selectedLoanAgreement = result.Result;
                await InvokeAsync(StateHasChanged);
            }
            else
            {
                await MessageService!.Error($"Error while retrieving employees: {result.NonSuccessMessage}", "Error");
            }
        }

        private async Task GetAllLoanAgreements(string loanNumber, int pageNumber, int pageSize)
        {
            _showDeleteDialog = false;
            _getLoanAgreementByLoanNumber = new() { LoanNumber = loanNumber, Page = pageNumber, PageSize = pageSize };

            OperationResult<PagingResponse<LoanAgreementListItem>> result =
                await LoanAgreementService!.GetLoanAgreementListItems(_getLoanAgreementByLoanNumber);

            if (result.Success)
            {
                _loanAgreementList = result.Result.Items;
                _metaData = result.Result.MetaData;
                StateHasChanged();
            }
            else
            {
                await MessageService!.Error($"Error while retrieving loan agreements: {result.NonSuccessMessage}", "Error");
            }
        }

        private void OnActionItemClicked(string action, Guid loanId)
        {
            NavManager!.NavigateTo
            (
                action switch
                {
                    "Edit" => $"Finance/Pages/LoanAgreements/LoanAgreementEditPage/{loanId}",
                    _ => throw new ArgumentOutOfRangeException(nameof(action), $"Unexpected menu item: {action}"),
                }
            );
        }

        private async Task OnDeleteDialogClosed(string action)
        {
            if (action == "deleted")
            {
                _snackBarMessage = $"Loan agreement information for loan number {_selectedLoanAgreement!.LoanNumber} was successfully deleted.";
                await GetAllLoanAgreements(1, 10);
                await _snackbar!.Show();
            }

            _showDeleteDialog = false;
        }

        private async Task SearchChanged(string searchTerm) => await GetAllLoanAgreements(searchTerm, 1, 10);

        private void ShowDetailDialog(Guid loanId) => _selectedLoanId = loanId;

        private string ConvertToShortDateString(DateTime date) => date.ToShortDateString();
    }
}