using Intsa.Models.Boards;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intsa.Pages.Boards.Notices
{
    public partial class Index
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
            PageSize = 2, 
            PagerButtonCount = 5, 
        };

        public bool VisibleProperty { get; set; } = false;
        
        // SignalR
        private HubConnection hubConnection;
        private List<string> messages = new List<string>();
        private string userInput;
        private string messageInput;

        protected override async Task OnInitializedAsync()
        {
            #region SignalR
            hubConnection = new HubConnectionBuilder()
                    .WithUrl(NavigationManagerReference.ToAbsoluteUri("/noticehub"))
                    .Build();

            hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                var encodeMsg = $"{user}: {message}";
                messages.Add(encodeMsg);
                StateHasChanged();
            });

            await hubConnection.StartAsync();
            #endregion

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
            VisibleProperty = true;
            //await Task.Delay(3000);
            var resultSet = await NoticeRepositoryAsyncReference.GetAllAsync(pager.PageIndex, pager.PageSize);
            pager.RecordCount = resultSet.TotalRecords; 
            models = resultSet.Records.ToList();
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

        #region SignalR
        Task Send() =>
            hubConnection.SendAsync("SendMessage", userInput, messageInput);

        public bool IsConnected =>
            hubConnection.State == HubConnectionState.Connected;

        public async ValueTask DisposeAsync()
        {
            await hubConnection.DisposeAsync();
        } 
        #endregion
    }
}
