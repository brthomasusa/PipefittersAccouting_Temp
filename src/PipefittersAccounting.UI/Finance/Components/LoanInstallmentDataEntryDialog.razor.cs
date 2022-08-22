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

        private async Task OnSave()
        {
            if (_installments.Count == 0)
            {
                await MessageService!.Error($"The amortization schedule requires at least one installment!", "Error");
                return;
            }

            _isLoading = true;

            OperationResult<bool> deleteResult = await SqliteDbService!.DeleteAll();

            if (deleteResult.Success)
            {
                OperationResult<bool> result = await SqliteDbService!.AddAsync(_installments);

                if (result.Success)
                {
                    await MessageService!.Info($"{_installments.Count} records inserted", "It Works!");
                }
                else
                {
                    await MessageService!.Error($"Error: {result.NonSuccessMessage}", "Error");
                }
            }
            else
            {
                await MessageService!.Error($"System Error: {deleteResult.NonSuccessMessage}", "Error");
            }

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
            var dateString = Convert.ToString(e.Value);

            try
            {
                var parsedDate = DateTime.Parse(dateString!);

                isValid = (parsedDate >= LoanAgreement!.LoanDate && parsedDate <= LoanAgreement!.MaturityDate);
            }
            catch (FormatException)
            {
                isValid = false;
            }


            e.Status = string.IsNullOrEmpty(dateString) ? ValidationStatus.None :
                isValid ? ValidationStatus.Success : ValidationStatus.Error;
        }

        private void CreateEmptyInstallment()
        {
            _currentInstallment = new()
            {
                LoanInstallmentId = Guid.NewGuid(),
                LoanId = LoanAgreement!.LoanId,
                UserId = LoanAgreement!.UserId
            };
        }

        private async Task AddToSchedule()
        {
            if (!await _validations!.ValidateAll())
                return;

            _installments.Add(_currentInstallment!);
            CreateEmptyInstallment();
            await InvokeAsync(StateHasChanged);
        }
    }
}