using Microsoft.AspNetCore.Components;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.HumanResources.Components
{
    public partial class EmployeeTable
    {
        [Parameter] public List<EmployeeListItem>? Employees { get; set; }
        [Parameter] public EventCallback<string> OnSearchChanged { get; set; }
        [Inject] public NavigationManager? NavManager { get; set; }

        private void OnMenuItemClicked(string itemName, Guid employeeId)
        {
            NavManager!.NavigateTo
            (
            itemName switch
            {
                "ShowDetails" => $"HumanResouces/Pages/EmployeeDetails/{employeeId}",
                "Edit" => $"HumanResouces/Pages/EmployeeEdit/{employeeId}",
                "Delete" => $"HumanResouces/Pages/EmployeeDetails/{employeeId}",
                _ => throw new ArgumentOutOfRangeException(nameof(itemName), $"Unexpected menu item: {itemName}"),
            }
            );
        }

        private async Task SearchChanged(string searchTerm)
        {
            await OnSearchChanged.InvokeAsync(searchTerm);
        }
    }
}