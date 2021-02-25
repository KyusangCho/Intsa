using Intsa.Models.Boards;
using Intsa.Shared.Utils;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.RichTextEditor;
using Syncfusion.Blazor.Inputs; 
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;

namespace Intsa.Pages.Boards.Notices
{
    public partial class Create
    {
        [Inject]
        public INoticeRepository NoticeRepositoryAsyncReference { get; set; }
        [Inject]
        public NavigationManager NavigationManagerReference { get; set; }

        protected BoardNotices model = new BoardNotices();

        public string ParentId { get; set; }

        protected int[] parentIds = { 1, 2, 3 };

        protected string userName; 

        protected override async Task OnInitializedAsync()
        {
            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            userName = authenticationState.User.Identity.Name;
            model.Name = userName; 
        }

        protected async void FormSubmit()
        {
            int.TryParse(ParentId, out int parentId);
            model.ParentId = parentId;
            await NoticeRepositoryAsyncReference.AddAsync(model);
            
            //Socketlabs.SendMessage(model.Title, model.Content, "");     // 전체메일 발송 
            
            NavigationManagerReference.NavigateTo("/Boards/Notices"); 
        }

        private void OnChange(UploadChangeEventArgs args)
        {
            //foreach (var file in args.Files)
            //{
                var path = args.Files[0].FileInfo.FileSource;
                System.Console.WriteLine(path);

                AmazonS3.Main(args.Files[0].FileInfo.Name, path); 

                //FileStream filestream = new FileStream(path, FileMode.Create, FileAccess.Write);
                //file.Stream.WriteTo(filestream);
                //filestream.Close();
                //file.Stream.Close();
            //}
        }

        private void onRemove(RemovingEventArgs args)
        {
            foreach (var removeFile in args.FilesData)
            {
                if (File.Exists(Path.Combine(@"rootPath", removeFile.Name)))
                {
                    File.Delete(Path.Combine(@"rootPath", removeFile.Name)); 
                }
            }
        }

        private List<ToolbarItemModel> Tools = new List<ToolbarItemModel>()
        {
            new ToolbarItemModel() { Command = ToolbarCommand.Bold },
            new ToolbarItemModel() { Command = ToolbarCommand.Italic },
            new ToolbarItemModel() { Command = ToolbarCommand.Underline },
            new ToolbarItemModel() { Command = ToolbarCommand.StrikeThrough },
            new ToolbarItemModel() { Command = ToolbarCommand.FontName },
            new ToolbarItemModel() { Command = ToolbarCommand.FontSize },
            new ToolbarItemModel() { Command = ToolbarCommand.FontColor },
            new ToolbarItemModel() { Command = ToolbarCommand.BackgroundColor },
            new ToolbarItemModel() { Command = ToolbarCommand.LowerCase },
            new ToolbarItemModel() { Command = ToolbarCommand.UpperCase },
            new ToolbarItemModel() { Command = ToolbarCommand.SuperScript },
            new ToolbarItemModel() { Command = ToolbarCommand.SubScript },
            new ToolbarItemModel() { Command = ToolbarCommand.Separator },
            new ToolbarItemModel() { Command = ToolbarCommand.Formats },
            new ToolbarItemModel() { Command = ToolbarCommand.Alignments },
            new ToolbarItemModel() { Command = ToolbarCommand.OrderedList },
            new ToolbarItemModel() { Command = ToolbarCommand.UnorderedList },
            new ToolbarItemModel() { Command = ToolbarCommand.Outdent },
            new ToolbarItemModel() { Command = ToolbarCommand.Indent },
            new ToolbarItemModel() { Command = ToolbarCommand.Separator },
            new ToolbarItemModel() { Command = ToolbarCommand.CreateLink },
            new ToolbarItemModel() { Command = ToolbarCommand.Image },
            new ToolbarItemModel() { Command = ToolbarCommand.CreateTable },
            new ToolbarItemModel() { Command = ToolbarCommand.Separator },
            new ToolbarItemModel() { Command = ToolbarCommand.ClearFormat },
            new ToolbarItemModel() { Command = ToolbarCommand.Print },
            new ToolbarItemModel() { Command = ToolbarCommand.SourceCode },
            new ToolbarItemModel() { Command = ToolbarCommand.Separator },
            new ToolbarItemModel() { Command = ToolbarCommand.Undo },
            new ToolbarItemModel() { Command = ToolbarCommand.Redo }
        };
        
        
    }
}
