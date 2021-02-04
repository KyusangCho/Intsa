using Intsa.Models.Boards;
using Microsoft.AspNetCore.Components;

namespace Intsa.Pages.Boards.Notices
{
    public partial class Create
    {
        [Inject]
        public INoticeRepositoryAsync NoticeRepositoryAsyncReference { get; set; }
        [Inject]
        public NavigationManager NavigationManagerReference { get; set; }

        protected BoardNotices model = new BoardNotices();

        public string ParentId { get; set; }

        protected int[] parentIds = { 1, 2, 3 }; 

        protected async void FormSubmit()
        {
            int.TryParse(ParentId, out int parentId);
            model.ParentId = parentId; 
            await NoticeRepositoryAsyncReference.AddAsync(model);
            NavigationManagerReference.NavigateTo("/Boards/Notices"); 
        }
    }
}
