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
    public partial class LoanInstallmentDataEntryDialog
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
                    CreateEmptyInstallment();
                    await _modalRef!.Show();
                    await InvokeAsync(StateHasChanged);
                }
            }
        }

        private async Task AddInstallmentsToLoanAgreement()
        {
            if (_installments.Count == 0)
            {
                await MessageService!.Error($"The amortization schedule requires at least one installment!", "Missing amortization schedule.");
                return;
            }

            DateTime dupeDueDate = CheckForDuplicateDueDate();
            if (dupeDueDate != DateTime.MinValue)
            {
                await MessageService!.Error($"The amortization schedule has duplicate payment due dates ({dupeDueDate})!", "Duplicate Payment Due Dates.");
                return;
            }

            SortAmortizationSchedule();
            CalcRemainingBalances();

            decimal balance = _installments[_installments.Count - 1].PrincipalRemaining;

            if (balance > 0)
            {
                await MessageService!.Error($"Balance remaining: ${balance}! The last installment should show a zero balance!", "Incomplete amortization schedule.");
                return;
            }

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

                isValid = (parsedDate >= LoanAgreement!.LoanDate && parsedDate <= LoanAgreement!.MaturityDate) &&
                          IsGreatestPymtDueDate(parsedDate);

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

            await _validations!.ClearAll();
        }

        private async Task AddToSchedule()
        {
            if (_installments is not null && _installments.Any())
            {
                if (_currentInstallment!.PrincipalPymtAmount > _installments[_installments.Count - 1].PrincipalRemaining)
                {
                    decimal overPayment = _currentInstallment!.PrincipalPymtAmount - _installments[_installments.Count - 1].PrincipalRemaining;
                    await MessageService!.Error($"Applying this payment would over pay the principal by ${overPayment}", "Over payment!");
                    return;
                }
            }

            _currentInstallment!.PaymentAmount =
                _currentInstallment!.PrincipalPymtAmount +
                _currentInstallment!.InterestPymtAmount;

            if (!await _validations!.ValidateAll())
                return;

            _installments!.Add(_currentInstallment!);
            CalcRemainingBalances();

            CreateEmptyInstallment();
            await InvokeAsync(StateHasChanged);
        }

        private bool IsGreatestPymtDueDate(DateTime dueDate)
        {
            List<LoanInstallmentWriteModel> SortedList = _installments.OrderBy(i => i.PaymentDueDate).ToList();

            if (SortedList is null || !SortedList.Any())
                return true;

            return dueDate > SortedList[SortedList.Count - 1].PaymentDueDate;
        }

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