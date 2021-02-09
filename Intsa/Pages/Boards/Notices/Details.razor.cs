using Intsa.Models.Boards;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Intsa.Pages.Boards.Notices
{
    public partial class Details
    {
        [Parameter]
        public int Id { get; set; }

        [Inject]
        public INoticeRepository NoticeRepositoryAsyncReference { get; set; }

        protected BoardNotices model = new BoardNotices();

        protected string content = ""; 

        protected override async Task OnInitializedAsync()
        {
            model = await NoticeRepositoryAsyncReference.GetByIdAsync(Id);
            content = Dul.HtmlUtility.EncodeWithTabAndSpace(model.Content); 

        }
    }
}
