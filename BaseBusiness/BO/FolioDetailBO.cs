using BaseBusiness.bc;
using BaseBusiness.Facade;
using BaseBusiness.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseBusiness.BO
{
    public class FolioDetailBO : BaseBO
    {
        private FolioDetailFacade facade = FolioDetailFacade.Instance;
        protected static FolioDetailBO instance = new FolioDetailBO();

        protected FolioDetailBO()
        {
            this.baseFacade = facade;
        }

        public static FolioDetailBO Instance
        {
            get { return instance; }
        }
        public static int GetTopInvoiceNo()
        {
            string query = "select max(cast(InvoiceNo as int)) as InvoiceNo from FolioDetail";
            return instance.GetFirst<int>(query);
        }
        public static int GetTopTransactioNo()
        {
            string query = "SELECT TOP 1 TransactionNo + (SELECT COUNT(*) \r\n                             FROM FolioDetail \r\n                             WHERE TransactionNo = (SELECT TOP 1 TransactionNo \r\n                                                   FROM FolioDetail \r\n                                                   ORDER BY id DESC)) AS NextTransactionNo\r\nFROM FolioDetail\r\nORDER BY id DESC;";
            return instance.GetFirst<int>(query);
        }
        public static FolioDetailModel GetFolioDetailMaster(string transactionNo)
        {
            string query = $"select top 1 * from FolioDetail where TransactionNo = '{transactionNo}' and (RowState in (2,1) and IsSplit = 1)";
            return instance.GetFirst<FolioDetailModel>(query);
        }
        public static decimal CalculateBalance(int reservationID)
        {
            string query = $"select sum(AmountMaster) as Amount from FolioDetail where ReservationID = {reservationID} and RowState = 1 AND Status = 0";
            return instance.GetFirst<decimal>(query);
        }
    }
}
