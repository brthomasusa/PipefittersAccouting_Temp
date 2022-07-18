using Microsoft.AspNetCore.Components;

namespace PipefittersAccounting.UI.Components.Common
{
    public partial class ReadOnlyDetailsForm<TItem>
    {
        [Parameter] public string? PageTitle { get; set; }
        [Parameter] public string? FormTitle { get; set; }
        [Parameter] public RenderFragment? FormFields { get; set; }
        [Parameter] public TItem? ViewModel { get; set; }
    }
}