using Microsoft.AspNetCore.Components;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.UI.Interfaces;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Finance.Pages.Financiers
{
    public partial class Financiers
    {
        private GetFinanciers? _getFinanciersParameters;
        private List<FinancierListItems>? _financierList;
        private MetaData? _metaData;

        [Inject] public IFinanciersHttpService? FinanciersService { get; set; }

        protected async override Task OnInitializedAsync()
        {
            await GetFinanciers(1, 5);
        }

        private async Task GetFinanciers(int pageNumber, int pageSize)
        {
            _getFinanciersParameters = new() { Page = pageNumber, PageSize = pageSize };

            OperationResult<PagingResponse<FinancierListItems>> result =
                await FinanciersService!.GetFinancierListItems(_getFinanciersParameters);

            if (result.Success)
            {
                _financierList = result.Result.Items;
                _metaData = result.Result.MetaData;
                StateHasChanged();
            }
            else
            {
                logger!.LogError(result.NonSuccessMessage);
            }
        }
    }
}