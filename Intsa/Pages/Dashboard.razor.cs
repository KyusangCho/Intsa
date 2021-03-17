using Intsa.Models.Boards;

using Microsoft.AspNetCore.Components;
using Intsa.Pages.Components;
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
    }
}
