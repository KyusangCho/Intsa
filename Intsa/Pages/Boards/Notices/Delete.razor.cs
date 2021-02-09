using Intsa.Models.Boards;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Intsa.Pages.Boards.Notices
{
    public partial class Delete
    {
        [Parameter]
        public int Id { get; set; }

        [Inject]
        public INoticeRepository NoticeRepositoryAsyncReference { get; set; }
        [Inject]
        public NavigationManager NavigationManagerReference { get; set; }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        protected BoardNotices model = new BoardNotices();

        protected string content = "";

        protected override async Task OnInitializedAsync()
        {
            model = await NoticeRepositoryAsyncReference.GetByIdAsync(Id);
            content = Dul.HtmlUtility.EncodeWithTabAndSpace(model.Content);

        }

        protected async void DeleteClick()
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{Id}번 글을 정말로 삭제하시겠습니까?");

            if (isDelete)
            {
                await NoticeRepositoryAsyncReference.DeleteAsync(Id);
                NavigationManagerReference.NavigateTo("/Boards/Notices"); 
            }
            else
            {
                await JSRuntime.InvokeAsync<object>("alarm", "취소되었습니다."); 
            }
        }

        protected override void OnInitialized()
        {

        }
    }
}
