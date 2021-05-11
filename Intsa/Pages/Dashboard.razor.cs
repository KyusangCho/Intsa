using Intsa.Models.Boards;

using Microsoft.AspNetCore.Components;
using Intsa.Pages.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Intsa.Pages
{
    public partial class Dashboard
    {
        [Inject]
        public INoticeRepository NoticeRepositoryAsyncReference { get; set; }
        [Inject]
        public NavigationManager NavigationManagerReference { get; set; }

        protected List<BoardNotices> models;


        public ApprovalForm EditorFormReference { get; set; }
        //public DeleteDialog DeleteDialogReference { get; set; }

        protected BoardNotices model = new BoardNotices();

        public string FormTitle { get; set; } = "Approval";

        protected void ShowEditorForm()
        {
            FormTitle = "Approval / A.T.IMPRESORA, S. A. / Trim";
            this.model = new BoardNotices();
            EditorFormReference.Show();
        }

        protected void EditBy(BoardNotices model)
        {
            FormTitle = "Edit Approval";
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

            //if ((AuthorizationService.AuthorizeAsync(context.User, "TwoFactorEnabled")).Result.Succeeded)
            //{
            //}
            //else
            //{
            //    NavigationManagerReference.NavigateTo("/Boards/Notices"); 
            //}

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

        public class ChartData
        {
            public DateTime XValue { get; set; }
            public double YValue { get; set; }
        }

        public List<ChartData> Data = new List<ChartData>
{
        new ChartData { XValue = new DateTime(2000, 2, 11), YValue = 14 },
        new ChartData { XValue = new DateTime(2001, 9, 4), YValue = 20 },
        new ChartData { XValue = new DateTime(2002, 2, 11), YValue = 25 },
        new ChartData { XValue = new DateTime(2003, 9, 16), YValue = 21 },
        new ChartData { XValue = new DateTime(2004, 2, 7), YValue = 13},
        new ChartData { XValue = new DateTime(2005, 9, 7), YValue = 18 },
        new ChartData { XValue = new DateTime(2006, 2, 11), YValue = 24 },
        new ChartData { XValue = new DateTime(2007, 9, 14), YValue = 23 },
        new ChartData { XValue = new DateTime(2008, 2, 6), YValue = 19 },
        new ChartData { XValue = new DateTime(2009, 9, 6), YValue = 31 },
        new ChartData { XValue = new DateTime(2010, 2, 11), YValue = 39},
        new ChartData { XValue = new DateTime(2011, 9, 11), YValue = 50 },
        new ChartData { XValue = new DateTime(2012, 2, 11), YValue = 24 },
    };
    }
}
