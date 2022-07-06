using Microsoft.AspNetCore.Components;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;

namespace PipefittersAccounting.UI.HumanResources.Components
{
    public partial class EmployeeTable
    {
        [Parameter] public List<EmployeeListItem>? Employees { get; set; }
        [Parameter] public Func<int, Task>? PageNumberChangedCallback { get; set; }
        [Parameter] public MetaData? MetaData { get; set; }

        public async Task HandleCurrentPageChanged(int currentPage)
        {
            await PageNumberChangedCallback!.Invoke(currentPage);
        }
    }
}