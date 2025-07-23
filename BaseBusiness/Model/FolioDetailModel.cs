using BaseBusiness.bc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseBusiness.Model
{
    public class FolioDetailModel : BaseModel
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public int ShiftID { get; set; }
        public string CashierNo { get; set; }
        public int ReservationID { get; set; }
        public int FolioID { get; set; }
        public int OriginFolioID { get; set; }
        public string InvoiceNo { get; set; }
        public string TransactionNo { get; set; }
        public string ReceiptNo { get;set; }
        public DateTime TransactionDate { get; set; }
        public int ProfitCenterID { get; set; }
        public string ProfitCenterCode { get; set; }
        public int TransactionGroupID { get; set; }
        public int TransactionSubgroupID { get; set; }
        public string GroupCode { get; set; }
        public string SubgroupCode { get; set; }
        public int GroupType { get; set; }
        public string TransactionCode { get; set; }
        public string ArticleCode { get; set; }
    }
}
