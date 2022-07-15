using Microsoft.AspNetCore.Components;

namespace PipefittersAccounting.UI.Components.Common
{
    public partial class BasicGrid<TItem>
    {
        [Parameter]
        public RenderFragment? TableHeader { get; set; }

        [Parameter]
        public RenderFragment<TItem>? RowTemplate { get; set; }

        [Parameter]
        public RenderFragment? TableFooter { get; set; }

        [Parameter]
        public IReadOnlyList<TItem>? Items { get; set; }
    }
}