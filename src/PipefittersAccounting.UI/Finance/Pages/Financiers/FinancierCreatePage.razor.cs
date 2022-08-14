#pragma warning disable CS8602

using Microsoft.AspNetCore.Components;
using System.Net.Mail;
using Blazorise;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.UI.Finance.Validators;
using PipefittersAccounting.UI.Interfaces;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Finance.Pages.Financiers
{
    public partial class FinancierCreatePage
    {
        private string _returnUri = "Finance/Pages/Financiers/FinanciersListPage";
        private string? _snackBarMessage;
        private FinancierWriteModel? _financierDetailModel;

        [Inject] public IFinanciersRepository? FinanciersService { get; set; }
        [Inject] public IMessageService? MessageService { get; set; }

        protected override void OnInitialized()
        {
            _financierDetailModel = new()
            {
                Id = Guid.NewGuid(),
                StateCode = "",
                IsActive = true,
                UserId = new Guid("660bb318-649e-470d-9d2b-693bfb0b2744")
            };
        }

        private async Task<OperationResult<bool>> Create()
        {
            _financierDetailModel!.StateCode = _financierDetailModel!.StateCode.ToUpper();
            OperationResult<FinancierReadModel> createResult = await FinanciersService!.CreateFinancier(_financierDetailModel!);

            if (createResult.Success)
            {
                _snackBarMessage = $"Information for {_financierDetailModel!.FinancierName} was successfully created.";
                await InvokeAsync(StateHasChanged);
                return OperationResult<bool>.CreateSuccessResult(true);
            }
            else
            {
                await MessageService!.Error($"Error while saving financier info: {createResult.NonSuccessMessage}", "Error");
                return OperationResult<bool>.CreateFailure(createResult.NonSuccessMessage);
            }
        }

        void ValidateEmail(ValidatorEventArgs e)
        {
            bool isValid;

            var email = Convert.ToString(e.Value);

            try
            {
                MailAddress emailAddress = new MailAddress(email!);
                isValid = true;
            }
            catch (FormatException)
            {
                isValid = false;
            }


            e.Status = string.IsNullOrEmpty(email) ? ValidationStatus.None :
                isValid ? ValidationStatus.Success : ValidationStatus.Error;
        }
    }
}