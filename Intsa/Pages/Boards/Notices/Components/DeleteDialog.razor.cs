using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intsa.Pages.Boards.Notices.Components
{
    public partial class DeleteDialog
    {
        private bool IsShow { get; set; } = false; // 팝업창 표시여부 

        public void Show()
        {
            IsShow = true;
        }

        public void Hide()
        {
            IsShow = false;
        }

        /// <summary>
        /// 부모에서 OnClick 속성에 지정한 이벤트 처리기 실행 
        /// </summary>
        [Parameter]
        public EventCallback<MouseEventArgs> OnClick { get; set; }

    }
}
