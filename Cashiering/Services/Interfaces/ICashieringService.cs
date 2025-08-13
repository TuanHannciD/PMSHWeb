using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
namespace Cashiering.Services.Interfaces
{
    public interface ICashieringService
    {
     public  DataTable PostingJournal(
            string cashierNo,
            string transactionCodeList,
            string roomNoList,
            DateTime fromDate,
            DateTime toDate,
            string fromProfitCode,
            string toProfitCode,
            string groupID,
            string subgroupID);
 
        public DataTable SearchTransactionJournalByNotVatInfor(
            string cashierNo,
            string transactionCodeList,
            string roomNoList,
            DateTime fromDate,
            DateTime toDate,
            string fromProfitCode,
            string toProfitCode,
            string groupID,
            string subgroupID);
        public DataTable SearchTransactionJournalByVatInfor(
           string cashierNo,
           string transactionCodeList,
           string roomNoList,
           DateTime fromDate,
           DateTime toDate,
           string fromProfitCode,
           string toProfitCode,
           string groupID,
           string subgroupID);
        public DataTable CashierAudit(string userName, DateTime fromDate, DateTime toDate, string shiftID);
      
        public DataTable ShiftDetail(int shiftID, int type);
        public DataTable ExchangeRate();

    }
}

