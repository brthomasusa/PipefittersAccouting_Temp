using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace PipefittersAccounting.UI.Components.Common
{
    public partial class SingleFieldGridSearch
    {
        private string? SearchTerm { get; set; }

        [Parameter] public string? PlaceHolderText { get; set; }
        [Parameter] public EventCallback<string> OnSearchTermChangedEventHandler { get; set; }

        private async Task OnSearchTermChanged(KeyboardEventArgs e)
        {
            if (e.Code == "Enter" || e.Code == "NumpadEnter")
            {
                if (SearchTerm is not null)
                {
                    await OnSearchTermChangedEventHandler.InvokeAsync(SearchTerm);
                }
            }
        }
    }
}