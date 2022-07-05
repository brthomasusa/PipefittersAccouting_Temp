using Microsoft.AspNetCore.Components;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;

namespace PipefittersAccounting.UI.HumanResources.Components
{
    public partial class EmployeeTable
    {
        [Parameter] public List<EmployeeListItem>? Employees { get; set; }
    }
}