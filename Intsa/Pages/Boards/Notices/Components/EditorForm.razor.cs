using Microsoft.AspNetCore.Components;
using Intsa.Models.Boards;
using System;
using Intsa.Services;
using BlazorInputFile;
using System.Linq;
using Cafe.Shared; 
using System.IO;
using Intsa.Managers;

namespace Intsa.Pages.Boards.Notices.Components
{
    public partial class EditorForm
    {

        private bool IsShow { get; set; } = false; // 팝업창 표시여부 
        
        private string parentId = "0";

        protected int[] parentIds = { 1, 2, 3 };

        public void Show()
        {
            IsShow = true; 
        }

        public void Hide()
        {
            IsShow = false; 
        }

        /// <summary>
        /// 폼의 제목 영역
        /// </summary>
        [Parameter]
        public RenderFragment EditorFormTitle { get; set; }

        /// <summary>
        /// 넘어온 모델 개체 
        /// </summary>
        [Parameter]
        public BoardNotices Model { get; set; }

        /// <summary>
        /// 부모 컴포넌트에게 생성 완료 되었음을 부모 컴포넌트에게 알림 
        /// </summary>
        [Parameter]
        public Action CreateCallback { get; set; }

        /// <summary>
        /// 부모 컴포넌트에게 수정 완료 되었음을 부모 컴포넌트에게 알림 
        /// </summary>
        [Parameter]
        public EventCallback<bool> EditCallback { get; set; }
        
        /// <summary>
        /// 리포지토리 클래스 참조 
        /// </summary>
        [Inject]
        public INoticeRepository NoticeRepositoryAsyncReference { get; set; }
        
        protected override void OnParametersSet()
        {
            parentId = Model.ParentId.ToString();
            if (parentId == "0")
            {
                parentId = ""; 
            }
        }

        protected async void CreateOrEditClick()
        {
            #region 파일 업로드 관련 추가 

            var file = selectedFiles.FirstOrDefault();
            var fileName = "";
            int fileSize = 0; 

            if (file != null)
            {
                //file.Name = $"{DateTime.Now.ToString("yyyyMMddHHmmss")}{file.Name}"; 
                fileName = file.Name;
                fileSize = Convert.ToInt32(file.Size);
                //await FileUploadServiceReference.UploadAsync(file);

                var ms = new MemoryStream();
                await file.Data.CopyToAsync(ms);    // 파일 데이터를 메모리스트림으로 변환 

                // upload 하기전 memorystream을 byte 배열로 다시변환 
                await FileStorageManager.UploadAsync(ms.ToArray(), file.Name, "", true); 

                Model.FileName = fileName; 
                Model.FileSize = fileSize; 

            }

            #endregion

            if (!int.TryParse(parentId, out int newParentId))
            {
                newParentId = 0; 
            }
            Model.ParentId = newParentId; 
            

            if (Model.Id == 0)
            {
                // Create
                await NoticeRepositoryAsyncReference.AddAsync(Model);
                CreateCallback?.Invoke();  // -> EditCallback
            }
            else
            {
                // Edit 
                await NoticeRepositoryAsyncReference.EditAsync(Model);
                await EditCallback.InvokeAsync(true); 
            }
            //IsShow = false; 
        }

        [Inject]
        public IFileUploadService FileUploadServiceReference { get; set; }

        private IFileListEntry[] selectedFiles;
        protected void HandleSelection(IFileListEntry[] files)
        {
            this.selectedFiles = files;
        }

        [Inject]
        public IFileStorageManager FileStorageManager { get; set; }

    }
}
