#pragma warning disable CS8601
#pragma warning disable CS8602

using Microsoft.AspNetCore.Components;
using PipefittersAccounting.SharedModel.ReadModels;

namespace PipefittersAccounting.UI.Components.Common
{
    public partial class DynamicPager
    {
        private const string PREVIOUS = "previous";
        private const string NEXT = "next";
        private string? currentPage = "1";
        private int pageItems;
        private int totalCount;

        [Parameter] public MetaData? MetaData { get; set; }
        [Parameter] public Func<int, Task>? PageChangedCallback { get; set; }

        protected override void OnInitialized()
        {
            currentPage = MetaData.CurrentPage.ToString();
            pageItems = MetaData.TotalPages;
            totalCount = MetaData.TotalCount;
        }

        private bool IsActive(string page)
            => currentPage == page;

        private bool IsPageNavigationDisabled(string navigation)
        {
            if (navigation.Equals(PREVIOUS))
            {
                return currentPage.Equals("1");
            }
            else if (navigation.Equals(NEXT))
            {
                return currentPage.Equals(pageItems.ToString());
            }
            return false;
        }

        private async Task Previous()
        {
            if (currentPage != null)
            {
                int currentPageAsInt = int.Parse(currentPage);

                if (currentPageAsInt > 1)
                {
                    currentPage = (currentPageAsInt - 1).ToString();
                    await PageChangedCallback!.Invoke(currentPageAsInt - 1);
                }
            }
            else
            {
                logger.LogWarning("DynamicPager.currentPage property is null!");
            }

        }

        private async Task Next()
        {
            if (currentPage != null)
            {
                var currentPageAsInt = int.Parse(currentPage);

                if (currentPageAsInt < pageItems)
                {
                    currentPage = (currentPageAsInt + 1).ToString();
                    await PageChangedCallback!.Invoke(currentPageAsInt + 1);
                }
            }
            else
            {
                logger.LogWarning("DynamicPager.currentPage property is null!");
            }
        }

        private async Task SetActive(string? page)
        {
            if (page is not null)
            {
                currentPage = page;

                if (PageChangedCallback is not null)
                    await PageChangedCallback!.Invoke(int.Parse(currentPage));
            }
            else
            {
                logger!.LogWarning($"DynamicPager.SetActive(string page) called with null parameter!");
            }
        }

    }
}