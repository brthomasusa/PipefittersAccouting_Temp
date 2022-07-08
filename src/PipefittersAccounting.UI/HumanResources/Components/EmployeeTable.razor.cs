using Microsoft.AspNetCore.Components;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.HumanResources.Components
{
    public partial class EmployeeTable
    {
        [Parameter] public List<EmployeeListItem>? Employees { get; set; }
        [Inject] public NavigationManager? NavManager { get; set; }

        private void OnMenuItemClicked(string itemName)
        {
            NavManager!.NavigateTo
            (
            itemName switch
            {
                "ShowDetails" => "HumanResouces/Pages/EmployeeDetails",
                "EditEmployee" => "HumanResouces/Pages/EmployeeEdit",
                "DeleteEmployee" => "HumanResouces/Pages/EmployeeDetails",
                _ => throw new ArgumentOutOfRangeException(nameof(itemName), $"Unexpected menu item: {itemName}"),
            }
            );
        }
    }
}