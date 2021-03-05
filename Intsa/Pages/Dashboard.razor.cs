using Intsa.Models.Boards;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intsa.Pages
{
    public partial class Dashboard
    {
        [Inject]
        public INoticeRepository NoticeRepositoryAsyncReference { get; set; }
        [Inject]
        public NavigationManager NavigationManagerReference { get; set; }

        protected List<BoardNotices> models;

        protected BeanyPager.BeanyPagerBase pager = new BeanyPager.BeanyPagerBase()
        {
            PageNumber = 1,
            PageIndex = 0,
            PageSize = 10,
            PagerButtonCount = 5,
        };

        public bool VisibleProperty { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            await DisplayData();
        }

        private async Task DisplayData()
        {
            VisibleProperty = true;
            var resultSet = await NoticeRepositoryAsyncReference.GetAllAsync(pager.PageIndex, pager.PageSize);
            pager.RecordCount = resultSet.TotalRecords;
            
            if (resultSet.Records != null)
            {
                models = resultSet.Records.ToList();
            }

            VisibleProperty = false;
        }

        protected void NameClick(int id)
        {
            NavigationManagerReference.NavigateTo($"/Boards/Notices/Details/{id}");
        }

        protected async void PageIndexChanged(int pageIndex)
        {
            pager.PageIndex = pageIndex;
            pager.PageNumber = pageIndex + 1;

            await DisplayData();

            StateHasChanged();
        }
    }
}
