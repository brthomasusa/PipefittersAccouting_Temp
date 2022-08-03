using Microsoft.AspNetCore.Components;
using Blazorise;
using PipefittersAccounting.SharedModel;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;
using PipefittersAccounting.UI.HumanResources.Components;
using PipefittersAccounting.UI.Interfaces;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.HumanResources.Pages
{
    public partial class TimeCards
    {
        private List<EmployeeManager>? _managers;
        private EmployeeManager? selectedManager;

        [Inject] public IEmployeeHttpService? EmployeeService { get; set; }
        [Inject] public IMessageService? MessageService { get; set; }

        protected async override Task OnInitializedAsync()
        {
            await GetManagers();
            await base.OnInitializedAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task GetManagers()
        {
            OperationResult<List<EmployeeManager>> result = await EmployeeService!.GetEmployeeManagers();

            if (result.Success)
            {
                _managers = result.Result;
            }
            else
            {
                await MessageService!.Error($"Error while retrieving list of managers: {result.NonSuccessMessage}", "Error");
            }
        }

        private async Task GetWorkerForManager(Guid managerId)
        {
            await InvokeAsync(StateHasChanged);
        }
    }
}
