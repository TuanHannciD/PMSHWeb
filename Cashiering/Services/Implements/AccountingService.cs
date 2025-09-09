using BaseBusiness.BO;
using BaseBusiness.util;
using Cashiering.Services.Interfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashiering.Services.Implements
{
    public class AccountingService : IAccountingService
    {
        public DataTable AccountSearch(string accountName, string accountNo, int accountType, string balance)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@AccountName", accountName ?? ""),

                     new SqlParameter("@AccountNo", accountNo ?? ""),
                    new SqlParameter("@AccountTypeID", accountType),
                     new SqlParameter("@Balance", balance ?? ""),



                };

                DataTable myTable = DataTableHelper.getTableData("spARAccountReceivableSearch", param);
                return myTable;
            }
            catch (SqlException ex)
            {

                throw new Exception($"ERROR: {ex.Message}", ex);
            }
        }
    }
}
