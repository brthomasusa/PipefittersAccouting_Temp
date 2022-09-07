using Microsoft.AspNetCore.Components;
using Fluxor;
using PipefittersAccounting.SharedModel.ReadModels;

namespace PipefittersAccounting.UI.Components.Common
{
    public partial class BasicGridPager
    {
        private const string PREVIOUS = "previous";
        private const string NEXT = "next";
        private string? currentPage = "1";
        private int totalPages;
        private int totalCount;
        private int pageSize;

        [Parameter] public MetaData? MetaData { get; set; }

        protected override void OnParametersSet()
        {
            if (MetaData is not null)
            {
                currentPage = MetaData.CurrentPage.ToString();
                totalPages = MetaData.TotalPages;
                totalCount = MetaData.TotalCount;
                pageSize = MetaData.PageSize;
            }
        }

        private bool IsActive(string page)
            => currentPage == page;

        private bool IsPageNavigationDisabled(string navigation)
        {
            if (navigation.Equals(PREVIOUS))
            {
                return currentPage!.Equals("1");
            }
            else if (navigation.Equals(NEXT))
            {
                return currentPage!.Equals(totalPages.ToString());
            }
            return false;
        }


    }
}