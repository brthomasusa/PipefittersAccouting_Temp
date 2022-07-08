#pragma warning disable CS8601
#pragma warning disable CS8602

using Microsoft.AspNetCore.Components;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Components.Common
{
    public partial class DynamicPager
    {
        private const string PREVIOUS = "previous";
        private const string NEXT = "next";
        private string? currentPage = "1";
        private int totalPages;
        private int totalCount;
        private int pageSize = 15;

        [CascadingParameter(Name = "MetaData")]
        public MetaData? MetaData { get; set; }

        [CascadingParameter(Name = "PagerChangedEventHandler")]
        public Func<int, int, Task>? PageChangedCallback { get; set; }

        protected override void OnParametersSet()
        {
            if (MetaData is not null)
            {
                currentPage = MetaData.CurrentPage.ToString();
                totalPages = MetaData.TotalPages;
                totalCount = MetaData.TotalCount;
                pageSize = MetaData.PageSize;
            }
            else
            {
                logger.LogWarning($"DynamicPager.MetaData is null: {MetaData!.ToJson()}");
            }
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
                return currentPage.Equals(totalPages.ToString());
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
                    await PageChangedCallback!.Invoke(currentPageAsInt - 1, pageSize);
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

                if (currentPageAsInt < totalPages)
                {
                    currentPage = (currentPageAsInt + 1).ToString();
                    await PageChangedCallback!.Invoke(currentPageAsInt + 1, pageSize);
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
                    await PageChangedCallback!.Invoke(int.Parse(currentPage), pageSize);
            }
            else
            {
                logger!.LogWarning($"DynamicPager.SetActive(string page) called with null parameter!");
            }
        }

        private async Task OnSelectedValueChanged(int value)
        {
            pageSize = value;
            if (currentPage is not null)
            {
                await PageChangedCallback!.Invoke(int.Parse(currentPage), pageSize);
            }
            else
            {
                logger!.LogWarning($"DynamicPager.OnSelectedValueChanged(); property currentPage is null!");
            }
        }

    }
}