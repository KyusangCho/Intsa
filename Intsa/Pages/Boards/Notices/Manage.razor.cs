using Intsa.Models.Boards;
using Intsa.Pages.Boards.Notices.Components;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intsa.Pages.Boards.Notices
{
    public partial class Manage
    {
        [Inject]
        public INoticeRepositoryAsync NoticeRepositoryAsyncReference { get; set; }
        [Inject]
        public NavigationManager NavigationManagerReference { get; set; }

        public EditorForm EditorFormReference { get; set; }

        protected List<BoardNotices> models;
        protected BoardNotices model = new BoardNotices(); 

        protected BeanyPager.BeanyPagerBase pager = new BeanyPager.BeanyPagerBase()
        {
            PageNumber = 1,
            PageIndex = 0,
            PageSize = 10,
            PagerButtonCount = 5,
        };

        protected override async Task OnInitializedAsync()
        {
            await DisplayData();

        }

        private async Task DisplayData()
        {
            //await Task.Delay(3000); 
            var resultSet = await NoticeRepositoryAsyncReference.GetAllAsync(pager.PageIndex, pager.PageSize);
            pager.RecordCount = resultSet.TotalRecords;
            models = resultSet.Records.ToList();
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

        public string EditorFormTitle { get; set; } = "CREATE"; 

        protected void ShowEditorForm()
        {
            EditorFormTitle = "CREATE";
            this.model = new BoardNotices(); 
            EditorFormReference.Show(); 
        }

        protected void EditBy(BoardNotices model)
        {
            EditorFormTitle = "EDIT";
            this.model = model; 
            EditorFormReference.Show(); 
        }

        protected async void CreateOrEdit()
        {
            EditorFormReference.Hide();
            await DisplayData();
            StateHasChanged(); 
        }
    }
}
