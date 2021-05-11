using Intsa.Models.Boards;
using Intsa.Pages.Components;
using Microsoft.AspNetCore.Components;
using System;
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

        public class ChartData
        {
            public DateTime XValue { get; set; }
            public int YValue { get; set; }
            public int YValue1 { get; set; }
            public int YValue2 { get; set; }
        }

        public List<ChartData> Data = new List<ChartData>
        {
            new ChartData { XValue = new DateTime(2021, 4, 1), YValue = 14, YValue1 = 39, YValue2 = 60 },
            new ChartData { XValue = new DateTime(2021, 4, 2), YValue = 20, YValue1 = 30, YValue2 = 55 },
            new ChartData { XValue = new DateTime(2021, 4, 3), YValue = 125, YValue1 = 28, YValue2 = 148 },
            new ChartData { XValue = new DateTime(2021, 4, 4), YValue = 121, YValue1 = 35, YValue2 = 357 },
            new ChartData { XValue = new DateTime(2021, 4, 5), YValue = 313, YValue1 = 39, YValue2 = 62 },
            new ChartData { XValue = new DateTime(2021, 4, 8), YValue = 318, YValue1 = 41, YValue2 = 64 },
            new ChartData { XValue = new DateTime(2021, 4, 9), YValue = 124, YValue1 = 245, YValue2 = 157 },
            new ChartData { XValue = new DateTime(2021, 4, 10), YValue = 123, YValue1 = 248, YValue2 = 353 },
            new ChartData { XValue = new DateTime(2021, 4, 11), YValue = 19, YValue1 = 154, YValue2 = 63 },
            new ChartData { XValue = new DateTime(2021, 4, 12), YValue = 31, YValue1 = 155, YValue2 = 50 },
            new ChartData { XValue = new DateTime(2021, 4, 15), YValue = 39, YValue1 = 57, YValue2 = 66 },
            new ChartData { XValue = new DateTime(2021, 4, 16), YValue = 50, YValue1 = 60, YValue2 = 165 },
            new ChartData { XValue = new DateTime(2021, 4, 17), YValue = 24, YValue1 = 60, YValue2 = 279 },
            new ChartData { XValue = new DateTime(2021, 4, 18), YValue = 24, YValue1 = 60, YValue2 = 279 },
            new ChartData { XValue = new DateTime(2021, 4, 19), YValue = 24, YValue1 = 60, YValue2 = 279 },
            new ChartData { XValue = new DateTime(2021, 4, 20), YValue = 24, YValue1 = 60, YValue2 = 279 },
            new ChartData { XValue = new DateTime(2021, 4, 21), YValue = 24, YValue1 = 60, YValue2 = 279 },
            new ChartData { XValue = new DateTime(2021, 4, 22), YValue = 24, YValue1 = 60, YValue2 = 279 },
            new ChartData { XValue = new DateTime(2021, 4, 23), YValue = 24, YValue1 = 60, YValue2 = 279 },
            new ChartData { XValue = new DateTime(2021, 4, 24), YValue = 24, YValue1 = 60, YValue2 = 279 },
            new ChartData { XValue = new DateTime(2021, 4, 25), YValue = 24, YValue1 = 60, YValue2 = 279 },
        };

        public class ChartDataBuyerSales
        {
            public string Buyer { get; set; }
            public double PlanQty { get; set; }
            public double OrderQty { get; set; }
            public double ShipQty { get; set; }
        }
        public List<ChartDataBuyerSales> OrderSummary = new List<ChartDataBuyerSales>
{
         new ChartDataBuyerSales{ Buyer= "BELUGA INC", PlanQty=50, OrderQty=70, ShipQty=45 },
         new ChartDataBuyerSales{ Buyer="EXPRESS", PlanQty=40, OrderQty= 60, ShipQty=55 },
         new ChartDataBuyerSales{ Buyer= "JCREW", PlanQty=70, OrderQty= 60, ShipQty=50 },
         new ChartDataBuyerSales{ Buyer= "MATERNITY OPCO", PlanQty=60, OrderQty= 56, ShipQty=40 },
         new ChartDataBuyerSales{ Buyer= "KOHL'S", PlanQty=50, OrderQty= 45, ShipQty=35 },
         new ChartDataBuyerSales{ Buyer= "TORRID", PlanQty=40, OrderQty=30, ShipQty=22 },
         new ChartDataBuyerSales{ Buyer= "MAURICES", PlanQty=40, OrderQty=35, ShipQty=37 },
         new ChartDataBuyerSales{ Buyer= "LUCKY OPCO", PlanQty=30, OrderQty=25, ShipQty=27 }
    };
        public String[] palettes = new String[] { "#E94649", "#F6B53F", "#6FAAB0" };


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
