using Intsa.Services;
using BlazorInputFile;
using Microsoft.AspNetCore.Components;
using System.Linq;

namespace Intsa.Pages.Settings
{
    public partial class FrmFileUploadTest
    {
        [Inject]
        public IFileUploadService FileUploadServiceReference { get; set; }

        private IFileListEntry[] selectedFiles; 
        protected void HandleSelection(IFileListEntry[] files)
        {
            this.selectedFiles = files;
        }

        protected async void UploadClick()
        {
            var file = selectedFiles.FirstOrDefault();
            if (file != null)
            {
                await FileUploadServiceReference.UploadAsync(file);
            }
        }
    }
}
