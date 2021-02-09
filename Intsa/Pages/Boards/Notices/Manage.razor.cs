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
        [Parameter]
        public int ParentId { get; set; } = 0; 

        [Inject]
        public INoticeRepository NoticeRepositoryAsyncReference { get; set; }
        [Inject]
        public NavigationManager NavigationManagerReference { get; set; }

        public EditorForm EditorFormReference { get; set; }
        public DeleteDialog DeleteDialogReference { get; set; }

        protected List<BoardNotices> models;

        /// <summary>
        /// 공지사항으로 올리기 폼을 띄울건지 여부 
        /// </summary>
        public bool IsInlineDialogShow { get; set; } = false; 

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
            if (string.IsNullOrEmpty(this.searchQuery))
            {
                await DisplayData();
            }
            else
            {
                await SearchData(); 
            }

        }

        private async Task DisplayData()
        {
            if (ParentId == 0)
            {
                //await Task.Delay(3000); 
                var resultSet = await NoticeRepositoryAsyncReference.GetAllAsync(pager.PageIndex, pager.PageSize);
                pager.RecordCount = resultSet.TotalRecords;
                models = resultSet.Records.ToList();
                StateHasChanged();
            }
            else
            {
                var resultSet = await NoticeRepositoryAsyncReference.GetAllByParentIdAsync(pager.PageIndex, pager.PageSize, ParentId);
                pager.RecordCount = resultSet.TotalRecords;
                models = resultSet.Records.ToList();
                StateHasChanged();
            }
        }

        private async Task SearchData()
        {
            if (ParentId == 0)
            {
                var resultSet = await NoticeRepositoryAsyncReference.SearchAllAsync(pager.PageIndex, pager.PageSize, this.searchQuery);
                pager.RecordCount = resultSet.TotalRecords;
                models = resultSet.Records.ToList();
                StateHasChanged();
            }
            else
            {
                var resultSet = await NoticeRepositoryAsyncReference.SearchAllByParentIdAsync(pager.PageIndex, pager.PageSize, this.searchQuery, ParentId);
                pager.RecordCount = resultSet.TotalRecords;
                models = resultSet.Records.ToList();
                StateHasChanged();
            }
        }

        protected void NameClick(int id)
        {
            NavigationManagerReference.NavigateTo($"/Boards/Notices/Details/{id}");
        }

        protected async void PageIndexChanged(int pageIndex)
        {
            pager.PageIndex = pageIndex;
            pager.PageNumber = pageIndex + 1;

            if (string.IsNullOrEmpty(this.searchQuery))
            {
                await DisplayData();
            }
            else
            {
                await SearchData();
            }

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

        protected void DeleteBy(BoardNotices model)
        {
            this.model = model;
            DeleteDialogReference.Show(); 
        }
        protected void ToggleBy(BoardNotices model)
        {
            this.model = model;
            IsInlineDialogShow = true; 
        }

        protected async void CreateOrEdit()
        {
            EditorFormReference.Hide();
            await DisplayData();
            
        }

        protected async void DeleteClick()
        {
            await NoticeRepositoryAsyncReference.DeleteAsync(this.model.Id);
            DeleteDialogReference.Hide();
            this.model = new BoardNotices(); 
            await DisplayData();
        }

        protected void ToggleClose()
        {
            IsInlineDialogShow = false;
            this.model = new BoardNotices(); 
        }

        protected async void ToggleClick()
        {
            this.model.IsPinned = (this.model?.IsPinned == true) ? false : true; 

            await NoticeRepositoryAsyncReference.EditAsync(this.model);
            IsInlineDialogShow = false; 
            this.model = new BoardNotices();
            await DisplayData();
        }

        private string searchQuery; 

        protected async void Search(string query)
        {
            this.searchQuery = query;

            await SearchData();

            StateHasChanged(); 
        }
    }
}
