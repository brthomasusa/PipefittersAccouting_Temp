using Microsoft.AspNetCore.Components;
using Blazorise;
using Blazorise.FluentValidation;
using FluentValidation;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.UI.Interfaces;
using PipefittersAccounting.UI.Services.Sqlite;
using PipefittersAccounting.UI.Sqlite;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Finance.Components
{
    public partial class LoanInstallmentDataEntryDialog : ComponentBase
    {
        private Validations? _validations;
        private bool _isLoading;
        private Modal? _modalRef;
        private LoanInstallmentWriteModel? _currentInstallment;
        private List<LoanInstallmentWriteModel> _installments = new();
        [Inject] private LoanInstallmentModelService? SqliteDbService { get; set; }

        [Parameter] public bool ShowDialog { get; set; }
        [Parameter] public LoanAgreementWriteModel? LoanAgreement { get; set; }
        [Parameter] public EventCallback<string> OnDialogClosedHandler { get; set; }
        [Inject] public IMessageService? MessageService { get; set; }

        protected async override Task OnParametersSetAsync()
        {
            if (ShowDialog)
            {
                if (_modalRef is not null && LoanAgreement is not null)
                {
                    await CreateEmptyInstallment();
                    await _modalRef!.Show();
                    await InvokeAsync(StateHasChanged);
                }
            }
        }

        private async Task AddInstallmentsToLoanAgreement()
        {
            // Enforce the requirement that a loan agreement must have an amortization schedule
            if (_installments.Count == 0)
            {
                await MessageService!.Error($"The amortization schedule requires at least one installment!", "Missing amortization schedule.");
                return;
            }

            // Make sure there are no duplicate payment due dates
            DateTime dupeDueDate = CheckForDuplicateDueDate();
            if (dupeDueDate != DateTime.MinValue)
            {
                await MessageService!.Error($"The amortization schedule has duplicate payment due dates ({dupeDueDate})!", "Duplicate Payment Due Dates.");
                return;
            }

            // Sort schedule by payment due date
            SortAmortizationSchedule();

            // Refresh calculatiion of principal remaining, it should
            // equal zero or the schedule is not considered complete
            CalcRemainingBalances();

            decimal balance = _installments[_installments.Count - 1].PrincipalRemaining;

            if (balance > 0)
            {
                await MessageService!.Error($"Balance remaining: ${balance}! The last installment should show a zero balance!", "Incomplete amortization schedule.");
                return;
            }

            // If here, then the schedule is valid. Add it the 
            // loan agreement and close the loan installment dialog
            _isLoading = true;

            LoanAgreement!.NumberOfInstallments = _installments.Count;
            LoanAgreement!.AmortizationSchedule = _installments;

            await _modalRef!.Hide();
            await OnDialogClosedHandler.InvokeAsync("saved");

            _isLoading = false;
        }

        private void OnSelectedRowChanged(LoanInstallmentWriteModel installment)
        {
            _currentInstallment = installment;
        }

        private async Task CloseDialog()
        {
            await _modalRef!.Hide();
            await OnDialogClosedHandler.InvokeAsync("canceled");
        }

        private void ValidatePymtDueDate(ValidatorEventArgs e)
        {
            bool isValid;

            try
            {
                var dateString = Convert.ToString(e.Value);
                var parsedDate = DateTime.Parse(dateString!);

                isValid = (parsedDate >= LoanAgreement!.LoanDate && parsedDate <= LoanAgreement!.MaturityDate);

                e.Status = parsedDate == default ? ValidationStatus.Error :
                    isValid ? ValidationStatus.Success : ValidationStatus.Error;

                e.ErrorText = "Due date must be between loan date and maturity date AND greater than any other due date in the schedule!";
            }
            catch (FormatException)
            {
                isValid = false;
            }
        }

        private async Task CreateEmptyInstallment()
        {
            _currentInstallment = new()
            {
                LoanInstallmentId = Guid.NewGuid(),
                LoanId = LoanAgreement!.LoanId,
                InstallmentNumber = _installments.Count == 0 ? 1 : _installments.Count + 1,
                UserId = LoanAgreement!.UserId
            };

            if (_validations is not null)
                await _validations!.ClearAll();
        }

        private async Task AddToSchedule()
        {
            CalcRemainingBalances();

            // Calculate payment amount
            _currentInstallment!.PaymentAmount =
                _currentInstallment!.PrincipalPymtAmount +
                _currentInstallment!.InterestPymtAmount;

            // Will adding this installment payment over pay the loan principal??
            if (_installments is not null && _installments.Any())
            {
                if (_currentInstallment!.PrincipalPymtAmount > _installments[_installments.Count - 1].PrincipalRemaining)
                {
                    decimal overPayment = _currentInstallment!.PrincipalPymtAmount - _installments[_installments.Count - 1].PrincipalRemaining;
                    await MessageService!.Error($"Applying this payment would over pay the principal by ${overPayment}", "Over payment!");
                    return;
                }
            }

            // Invoke validations before attempting to add the installment to the schedule
            if (!await _validations!.ValidateAll())
                return;

            _installments!.Add(_currentInstallment!);

            // Calculate new remaining balance
            CalcRemainingBalances();

            // Clear fields of the data entry form
            await CreateEmptyInstallment();

            // Update the UI
            await InvokeAsync(StateHasChanged);
        }

        // private bool IsGreatestPymtDueDate(DateTime dueDate)
        // {
        //     List<LoanInstallmentWriteModel> SortedList = _installments.OrderBy(i => i.PaymentDueDate).ToList();

        //     if (SortedList is null || !SortedList.Any())
        //         return true;

        //     return dueDate > SortedList[SortedList.Count - 1].PaymentDueDate;
        // }

        private void CalcRemainingBalances()
        {
            decimal balance = LoanAgreement!.LoanAmount;

            if (_installments is not null && _installments.Any())
            {
                foreach (LoanInstallmentWriteModel installment in _installments)
                {
                    balance = balance - (installment.PrincipalPymtAmount);
                    installment.PrincipalRemaining = balance;
                }
            }
        }

        private void Refresh()
        {
            _currentInstallment!.PaymentAmount = _currentInstallment.PrincipalPymtAmount + _currentInstallment.InterestPymtAmount;
            SortAmortizationSchedule();
            CalcRemainingBalances();
        }

        private DateTime CheckForDuplicateDueDate()
        {
            DateTime retValue = new();

            var dueDates = _installments.Select(i => i.PaymentDueDate).ToList();

            IEnumerable<DateTime> duplicates = from dates in dueDates
                                               group dates by dates into g
                                               where g.Count() > 1
                                               select g.Key;

            if (duplicates is not null && duplicates.Any())
            {
                retValue = duplicates.ToList()[0];
            }

            return retValue;
        }

        private void SortAmortizationSchedule()
        {
            List<LoanInstallmentWriteModel> sortedList = _installments.OrderBy(i => i.PaymentDueDate).ToList();

            if (sortedList is not null || sortedList!.Any())
            {
                for (int counter = 0; counter < sortedList!.Count; counter++)
                {
                    sortedList[counter].InstallmentNumber = counter + 1;
                }
            }

            _installments = sortedList!;
        }

        private async Task RemoveInstallmentFromSchedule()
        {
            _installments.Remove(_currentInstallment!);
            SortAmortizationSchedule();
            CalcRemainingBalances();
            await InvokeAsync(StateHasChanged);
        }
    }
}