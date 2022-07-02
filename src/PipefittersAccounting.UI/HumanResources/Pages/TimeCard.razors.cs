using Microsoft.AspNetCore.Components;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;

namespace PipefittersAccounting.UI.HumanResources.Pages
{
    public partial class TimeCards
    {
        private IEnumerable<PayrollRegister>? inMemoryData;

        protected override void OnInitialized()
        {
            inMemoryData = FakeData.GetPayrollRegister_02282022();
            base.OnInitialized();
        }
    }
}