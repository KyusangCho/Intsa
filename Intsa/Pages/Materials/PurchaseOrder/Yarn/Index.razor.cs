using Intsa.Models.Boards;
using Intsa.Pages.Materials.PurchaseRequisition.Yarn.Components;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intsa.Pages.Materials.PurchaseOrder.Yarn
{
    public partial class Index
    {
        [Inject]
        public INoticeRepository NoticeRepositoryAsyncReference { get; set; }
        [Inject]
        public NavigationManager NavigationManagerReference { get; set; }

        protected List<BoardNotices> models;

        public EditorForm EditorFormReference { get; set; }
        public UploadExcel UploadExcelReference { get; set; }
        //public DeleteDialog DeleteDialogReference { get; set; }

        protected BoardNotices model = new BoardNotices();

        public string EditorFormTitle { get; set; } = "Request Yarn";
        public string UploadFormTitle { get; set; } = "";

        protected void ShowEditorForm()
        {
            EditorFormTitle = "Request Yarn";
            this.model = new BoardNotices();
            EditorFormReference.Show();
        }

        protected void UploadExcel()
        {
            UploadFormTitle = "Upload Excel";
            this.model = new BoardNotices();
            UploadExcelReference.Show();
        }

        protected void EditBy(BoardNotices model)
        {
            EditorFormTitle = "Edit Request Yarn";
            this.model = model;
            EditorFormReference.Show();
        }

        protected void DeleteBy(BoardNotices model)
        {
            this.model = model;
            //DeleteDialogReference.Show();
        }

        protected async void CreateOrEdit()
        {
            EditorFormReference.Hide();
            await DisplayData();
        }

        protected async void UploadExcelData()
        {
            UploadExcelReference.Hide();
            await DisplayData();
        }

        protected BeanyPager.BeanyPagerBase pager = new BeanyPager.BeanyPagerBase()
        {
            PageNumber = 1,
            PageIndex = 0,
            PageSize = 2,
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

            if (string.IsNullOrEmpty(this.searchQuery))
            {
                await DisplayData();
            }
            else
            {
                await SearchData();
            }

            StateHasChanged();
        }

        private string searchQuery;

        protected async void Search(string query)
        {
            this.searchQuery = query;

            await SearchData();

            StateHasChanged();
        }

        private async Task SearchData()
        {
            //await Task.Delay(3000); 
            var resultSet = await NoticeRepositoryAsyncReference.SearchAllAsync(pager.PageIndex, pager.PageSize, this.searchQuery);
            pager.RecordCount = resultSet.TotalRecords;
            models = resultSet.Records.ToList();
        }
    }
}
