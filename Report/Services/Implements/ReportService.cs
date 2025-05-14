using Microsoft.Data.SqlClient;
using Report.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseBusiness.util;
namespace Report.Services.Implements
{
    public class ReportService : IReportService
    {
        public DataTable GetBookingSourceData(DateTime fromDate, DateTime toDate)
        {
            SqlParameter[] param = new SqlParameter[]
            {
              new SqlParameter("@From", fromDate),
                new SqlParameter("@To", toDate),
            };

            DataTable myTable = DataTableHelper.getTableData("spRptBookingSource", param);
            return myTable;
        }
    }
}
