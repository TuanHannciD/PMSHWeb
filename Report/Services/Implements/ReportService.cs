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
        public  DataTable GroupReservation(DateTime fromDate, DateTime toDate, string NoOfRoom)
        {
            SqlParameter[] param = new SqlParameter[]
            {
              new SqlParameter("@FromDate", fromDate),
                new SqlParameter("@ToDate", toDate),
                    new SqlParameter("@NoOfRoom", NoOfRoom),
            };

            DataTable myTable = DataTableHelper.getTableData("spRptGroupReservationReport", param);
            return myTable;
        }
        public  DataTable GuestStayOver(DateTime fromDate, DateTime toDate)
        {
            SqlParameter[] param = new SqlParameter[]
            {
              new SqlParameter("@FromDate", fromDate),
                new SqlParameter("@ToDate", toDate)
            };

            DataTable myTable = DataTableHelper.getTableData("spGuestStayOver_New", param);
            return myTable;
        }
        public  DataTable GuestStay(string noofName, string stayno, string stayand)
        {
            SqlParameter[] param = new SqlParameter[]
            {
        new SqlParameter("@Name", noofName ?? string.Empty),
        new SqlParameter("@Return", stayno ?? string.Empty),
        new SqlParameter("@ReturnTo", stayand ?? string.Empty)
            };

            DataTable myTable = DataTableHelper.getTableData("spRptReturnGuest", param);
            return myTable;
        }
        public  DataTable ReportNationalityStatistics(DateTime fromDate, DateTime toDate, string status, string sortOder)
        {
            SqlParameter[] param = new SqlParameter[]
            {
               new SqlParameter("@FromMonth", fromDate),
                new SqlParameter("@ToMonth", toDate),
        new SqlParameter("@PrevFromMonth", new DateTime(2024, 4, 1, 0, 0, 0)),
        new SqlParameter("@PrevToMonth", new DateTime(2024, 4, 1, 0, 0, 0)),
                 new SqlParameter("@Status", status),
                new SqlParameter("@SortOder", sortOder),
            };

            DataTable myTable = DataTableHelper.getTableData("spRptNationalCompareStatistics", param);
            return myTable;
        }
        public  DataTable ReservationSummaryReport(DateTime fromDate, DateTime toDate)
        {
            SqlParameter[] param = new SqlParameter[]
            {
              new SqlParameter("@FromDate", fromDate),
                new SqlParameter("@ToDate", toDate),
            };

            DataTable myTable = DataTableHelper.getTableData("spRptSummaryReservation", param);
            return myTable;
        }
        public  DataTable TraceReportView(DateTime fromDate, DateTime toDate, int roomClass, int department, int status, int byAlphabetical, int byRoom, int byVip, int pseudoRoom, int reserved, int checkedIn, int dueout, int individual, int blockcode, int vipOnly)
        {
            SqlParameter[] param = new SqlParameter[]
            {
              new SqlParameter("@FromDate", fromDate),
                new SqlParameter("@ToDate", toDate),
                  new SqlParameter("@RoomClassID", roomClass),
                new SqlParameter("@DepartmentID", department),
                  new SqlParameter("@Status", status),
                new SqlParameter("@ByAlphabetical", byAlphabetical),
                  new SqlParameter("@ByRoom", byRoom),
                    new SqlParameter("@ByVIP", byVip),
                      new SqlParameter("@IsPseudo", pseudoRoom),
                        new SqlParameter("@IsReserved", reserved),
                          new SqlParameter("@CheckedIn", checkedIn),
                            new SqlParameter("@DueOut", dueout),
                new SqlParameter("@Individual", individual),
                new SqlParameter("@BlockCode", blockcode),
                new SqlParameter("@VIPOnly", vipOnly),
            };

            DataTable myTable = DataTableHelper.getTableData("spRptGuestsInHouseTrace", param);
            return myTable;
        }
        public  DataTable RatecodebyDate(DateTime fromDate, DateTime toDate, string ratecode)
        {
            SqlParameter[] param = new SqlParameter[]
            {
               new SqlParameter("@FromDate", fromDate),
                new SqlParameter("@ToDate", toDate),
                new SqlParameter("@strRateCode", ratecode),
                       new SqlParameter("@strCurrency", "USD"),
                           new SqlParameter("@strRooTypeCode", ""),
            };

            DataTable myTable = DataTableHelper.getTableData("spRptRevenue_RateCodeByDateReport", param);
            return myTable;
        }
        public  DataTable GuestMarketReport(DateTime fromDate, DateTime toDate, string currency, string zonecode)
        {
            DateTime firstDate = new DateTime(DateTime.Now.Year, 1, 1);
            SqlParameter[] param = new SqlParameter[]
            {
              new SqlParameter("@FromDate", fromDate),
                new SqlParameter("@ToDate", toDate),
                    new SqlParameter("@FirstDate", firstDate),
                        new SqlParameter("@Currency", currency),
                            new SqlParameter("@RoomTypeID", zonecode),
            };

            DataTable myTable = DataTableHelper.getTableData("spRptGroupReservationReport", param);
            return myTable;
        }
    }
}
