using Microsoft.Data.SqlClient;
using Report.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseBusiness.util;
using System.Globalization;
using System.Reflection.Metadata;
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
        public DataTable BlacklistReporteData(DateTime fromDate, DateTime toDate)
        {
            SqlParameter[] param = new SqlParameter[]
            {
              new SqlParameter("@FromDate", fromDate),
                new SqlParameter("@ToDate", toDate),
                  new SqlParameter("@Type", "0"),
            };

            DataTable myTable = DataTableHelper.getTableData("spRptBlacklistReport", param);
            return myTable;
        }
        public DataTable GroupReservation(DateTime fromDate, DateTime toDate, string NoOfRoom)
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
        public DataTable GuestStayOver(DateTime fromDate, DateTime toDate)
        {
            SqlParameter[] param = new SqlParameter[]
            {
              new SqlParameter("@FromDate", fromDate),
                new SqlParameter("@ToDate", toDate)
            };

            DataTable myTable = DataTableHelper.getTableData("spGuestStayOver_New", param);
            return myTable;
        }
        public DataTable GuestStay(string noofName, string stayno, string stayand)
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
        public DataTable OTAMonthlyReport(string fromDate, string Number, string type, string currencyID)
        {
            DateTime fromDateParsed;
            string toDate = fromDate;

            if (DateTime.TryParseExact(fromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out fromDateParsed))
            {
                // Cộng thêm 4 tháng
                DateTime toDateParsed = fromDateParsed.AddMonths(4);

                // Lấy ngày cuối cùng của tháng
                int lastDay = DateTime.DaysInMonth(toDateParsed.Year, toDateParsed.Month);
                DateTime lastDateOfMonth = new DateTime(toDateParsed.Year, toDateParsed.Month, lastDay);

                // Chuyển sang chuỗi theo định dạng cần thiết
                toDate = lastDateOfMonth.ToString("yyyy-MM-dd");
            }
            SqlParameter[] param = new SqlParameter[]
            {
          new SqlParameter("@FromDate", fromDate),
                new SqlParameter("@ToDate", toDate),
        new SqlParameter("@ProfileID",0),
        new SqlParameter("@CurrencyID", currencyID),
                new SqlParameter("@Type", type),
                 new SqlParameter("@Number", Number),
            };

            DataTable myTable = DataTableHelper.getTableData("spRptRevenue_MonthlyByHolderReport", param);
            return myTable;
        }
        public DataTable ReportNationalityStatistics(DateTime fromDate, DateTime toDate, string status, string sortOder)
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

        public DataTable AlertsData(DateTime fromDate, DateTime toDate, string viewBy, string altercode)
        {
            SqlParameter[] param = new SqlParameter[]
            {
               new SqlParameter("@FromDate", fromDate),
                new SqlParameter("@ToDate", toDate),
                 new SqlParameter("@Area", viewBy),
                new SqlParameter("@AlertCode", altercode),
            };

            DataTable myTable = DataTableHelper.getTableData("spRptAlerts", param);
            return myTable;
        }
        public DataTable ReservationSummaryReport(DateTime fromDate, DateTime toDate)
        {
            SqlParameter[] param = new SqlParameter[]
            {
              new SqlParameter("@FromDate", fromDate),
                new SqlParameter("@ToDate", toDate),
            };

            DataTable myTable = DataTableHelper.getTableData("spRptSummaryReservation", param);
            return myTable;
        }
        public DataTable TraceReportView(DateTime fromDate, DateTime toDate, int roomClass, int department, int status, int byAlphabetical, int byRoom, int byVip, int pseudoRoom, int reserved, int checkedIn, int dueout, int individual, int blockcode, int vipOnly)
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


        public DataTable ArrivalsandCheckInTodayData(string roomClass, string roomtype, string paymethod, string vip, string viewBy, string pseudo, string chkviponly, int disRoomSharer, string nopost)
        {
            SqlParameter[] param = new SqlParameter[]
            {
               new SqlParameter("@RoomClass", roomClass),
                new SqlParameter("@RoomType", roomtype),
                new SqlParameter("@Payment", paymethod),
                       new SqlParameter("@VIP",vip),
                           new SqlParameter("@SortOrder",viewBy),

                                new SqlParameter("@Pseudo", pseudo),

                       new SqlParameter("@ChkVIPOnly",chkviponly),
                       new SqlParameter("@BusinessDate", DateTime.Now.Date),


                               new SqlParameter("@DisRoomSharer", disRoomSharer),
                       new SqlParameter("@NoPost",nopost),
            };

            DataTable myTable = DataTableHelper.getTableData("spRptArrivalAndCheckedInToday", param);
            return myTable;
        }

        public DataTable RatecodebyDate(DateTime fromDate, DateTime toDate, string ratecode)
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
        public DataTable GuestMarketReport(DateTime fromDate, DateTime toDate, string currency, string zonecode)
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

            DataTable myTable = DataTableHelper.getTableData("spRptMarketReportFolio", param);
            return myTable;
        }
        public DataTable DailyPickupReport(DateTime fromDate, DateTime toDate)
        {
            SqlParameter[] param = new SqlParameter[]
            {
               new SqlParameter("@Year", fromDate),
                new SqlParameter("@YearTo", toDate),
            };

            DataTable myTable = DataTableHelper.getTableData("spRptDailyPickup", param);
            return myTable;
        }
        public DataTable DailyBreakfastDetail(DateTime fromDate)
        {
            SqlParameter[] param = new SqlParameter[]
            {
               new SqlParameter("@ViewDate", fromDate),
            };

            DataTable myTable = DataTableHelper.getTableData("spRptDailyBreakfastDetail", param);
            return myTable;
        }
        public DataTable FreeUpgradeReport(DateTime fromDate, DateTime toDate, string viewBy, string status)
        {
            SqlParameter[] param = new SqlParameter[]
            {
               new SqlParameter("@FromDate", fromDate),
                        new SqlParameter("@ToDate", toDate),
                        new SqlParameter("@Status", status),
                        new SqlParameter("@ViewBy", viewBy),
                                          new SqlParameter("@Type", "0"),
            };

            DataTable myTable = DataTableHelper.getTableData("spFreeUpgradeReport", param);
            return myTable;
        }
        public DataTable FixChargeReport(DateTime fromDate, DateTime toDate, string trancode, string status)
        {
            SqlParameter[] param = new SqlParameter[]
            {
               new SqlParameter("@FromDate", fromDate),
                        new SqlParameter("@ToDate", toDate),
                        new SqlParameter("@Status", status),
                        new SqlParameter("@TransactionCode", trancode)
            };

            DataTable myTable = DataTableHelper.getTableData("spRptFixServices", param);
            return myTable;
        }

        public DataTable ReveunueByData(DateTime fromDate, DateTime toDate, string reservation, string roomType, string zone, string viewBy, string sortOrder)
        {
            SqlParameter[] param = new SqlParameter[]
            {
               new SqlParameter("@FromDate", fromDate),
                        new SqlParameter("@ToDate", toDate),
                        new SqlParameter("@Searchby", reservation),
                        new SqlParameter("@RoomType", roomType),
                        new SqlParameter("@Zone", zone),
                        new SqlParameter("@ViewBy", viewBy),
                        new SqlParameter("@ViewRate", "1"),
                        new SqlParameter("@SortOrder", sortOrder)
            };

            DataTable myTable = DataTableHelper.getTableData("spRptRevenueBy", param);
            return myTable;
        }
        public DataTable DepartureExtendedReport(DateTime fromDate)
        {
            SqlParameter[] param = new SqlParameter[]
            {
               new SqlParameter("@date", fromDate),
            };

            DataTable myTable = DataTableHelper.getTableData("spRptDepartureExtended", param);
            return myTable;
        }
        public DataTable RoomOccupancyReport(DateTime fromDate, DateTime toDate, string zone)
        {
            SqlParameter[] param = new SqlParameter[]
            {
               new SqlParameter("@dtpFromDate", fromDate),
                                   new SqlParameter("@dtpToDate", toDate),
                           new SqlParameter("@Zone", zone),
            };

            DataTable myTable = DataTableHelper.getTableData("spRptRoomOccupancy", param);
            return myTable;
        }

        public DataTable ReservationCancellationsReport(DateTime fromDate, DateTime toDate, string commnet, string typeDate)
        {
            SqlParameter[] param = new SqlParameter[]
            {
               new SqlParameter("@FromDate", fromDate),
                new SqlParameter("@ToDate", toDate),
               new SqlParameter("@Reason", commnet),
                   new SqlParameter("@TypeDate", typeDate),
            };

            DataTable myTable = DataTableHelper.getTableData("spRptReservationCancellations", param);
            return myTable;
        }

        public DataTable ReservationStatisticsReport(DateTime fromDate)
        {
            SqlParameter[] param = new SqlParameter[]
            {
               new SqlParameter("@Month", fromDate),
            };

            DataTable myTable = DataTableHelper.getTableData("spRptReservationStatistic", param);
            return myTable;
        }

        public DataTable ReservationbyCompanyReport(DateTime fromDate, DateTime toDate, string roomClass, string roomType, string searchCrip, string sortOrder, string noOfRoom)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                 new SqlParameter("@FromDate", fromDate),
                new SqlParameter("@ToDate", toDate),
               new SqlParameter("@RoomClass", roomClass),
                   new SqlParameter("@RoomType", roomType),
                      new SqlParameter("@SearchCrip", searchCrip),
                   new SqlParameter("@SortOrder", sortOrder),
                    new SqlParameter("@NoOfRoom", noOfRoom),

                     new SqlParameter("@Company", ""),
                   new SqlParameter("@Agent", ""),
                      new SqlParameter("@Source", ""),
                   new SqlParameter("@Group", ""),
                    new SqlParameter("@RateCode", ""),
                      new SqlParameter("@ReservationType", ""),
            };

            DataTable myTable = DataTableHelper.getTableData("spRptReservationByCompany", param);
            return myTable;
        }

        public DataTable NoShowReportData(DateTime fromDate, DateTime toDate, int roomClass)
        {
            SqlParameter[] param = new SqlParameter[]
            {
               new SqlParameter("@dtpFromDate", fromDate),
                  new SqlParameter("@dtpToDate", toDate),
                     new SqlParameter("@RoomClassID", roomClass),
            };

            DataTable myTable = DataTableHelper.getTableData("spRptNoShow", param);
            return myTable;
        }
        public DataTable ReservationSummaryData(DateTime fromDate, DateTime toDate, string roomType, string zone, string viewBy, string market)
        {
            SqlParameter[] param = new SqlParameter[]
            {
               new SqlParameter("@dtpFromDate", fromDate),
                  new SqlParameter("@dtpToDate", toDate),
                     new SqlParameter("@Zone", zone),
                           new SqlParameter("@RoomType", roomType),
                                 new SqlParameter("@Market", market),
                                 new SqlParameter("@Currency", viewBy),
            };

            DataTable myTable = DataTableHelper.getTableData("spRptReservationSummaryNew", param);
            return myTable;
        }
        public DataTable ProductActivityData(DateTime fromDate, DateTime toDate, string type, string currency)
        {
            SqlParameter[] param = new SqlParameter[]
            {
               new SqlParameter("@FromDate", fromDate),
                  new SqlParameter("@ToDate", toDate),
                     new SqlParameter("@Type", type),
                           new SqlParameter("@CurrencyID", currency),
                                 new SqlParameter("@ProfileID", "0"),
            };

            DataTable myTable = DataTableHelper.getTableData("spRptProductActivity", param);
            return myTable;
        }
        public DataTable NationalStatisticsData(DateTime fromDate, DateTime toDate, string roomtype, string viewBy)
        {
            SqlParameter[] param = new SqlParameter[]
            {
               new SqlParameter("@dtpFromDate", fromDate),
                  new SqlParameter("@dtpToDate", toDate),
                     new SqlParameter("@Type", viewBy),
                           new SqlParameter("@RoomType", roomtype),
            };

            DataTable myTable = DataTableHelper.getTableData("spRptNational", param);
            return myTable;
        }

        public DataTable SalesinChargeReportsForm1Data(DateTime fromDate, string viewBy)
        {
            SqlParameter[] param = new SqlParameter[]
            {
               new SqlParameter("@Year", fromDate),
                     new SqlParameter("@Currency", viewBy),
                       new SqlParameter("@PersonInCharge", ""),
                         new SqlParameter("@Zone", ""),
                                  new SqlParameter("@BusDate",DateTime.Now),
            };

            DataTable myTable = DataTableHelper.getTableData("spRptSaleInChargeReport", param);
            return myTable;
        }
        public DataTable SalesinChargeActivityData(DateTime fromDate, DateTime toDate, string type, string currency)
        {
            SqlParameter[] param = new SqlParameter[]
            {
               new SqlParameter("@FromDate", fromDate),
                  new SqlParameter("@ToDate", toDate),
                     new SqlParameter("@Type", type),
                           new SqlParameter("@CurrencyID", currency),
                                 new SqlParameter("@ProfileID", "0"),
            };

            DataTable myTable = DataTableHelper.getTableData("spRptSalesInChargeActivity", param);
            return myTable;
        }

        public DataTable RevenueDetailData(DateTime fromDate, DateTime toDate)
        {
            SqlParameter[] param = new SqlParameter[]
            {
               new SqlParameter("@dtpFromDate", fromDate),
                           new SqlParameter("@dtpToDate", toDate),
            };

            DataTable myTable = DataTableHelper.getTableData("spRptRevenueFull", param);
            return myTable;
        }

        public DataTable RevenueSummaryData(DateTime fromDate, DateTime toDate)
        {
            SqlParameter[] param = new SqlParameter[]
            {
               new SqlParameter("@dtpFromDate", fromDate),
                           new SqlParameter("@dtpToDate", toDate),
            };

            DataTable myTable = DataTableHelper.getTableData("spRptRevenueSummary", param);
            return myTable;
        }

        public DataTable TAProductionReportData(DateTime fromDate, DateTime toDate, string type, string currency)
        {
            SqlParameter[] param = new SqlParameter[]
            {
               new SqlParameter("@FromDate", fromDate),
                  new SqlParameter("@ToDate", toDate),
                     new SqlParameter("@Type", type),
                           new SqlParameter("@CurrencyID", currency),
                                 new SqlParameter("@ProfileID", "0"),
            };

            DataTable myTable = DataTableHelper.getTableData("spRptRevenue_HolderProductionReport", param);
            return myTable;
        }
        public DataTable LeadtimeReportsData(DateTime fromDate, DateTime toDate, string zone, string isDaily, string day, string daysNames)
        {
            SqlParameter[] param = new SqlParameter[]
            {
               new SqlParameter("@FromDate", fromDate),
                  new SqlParameter("@ToDate", toDate),
                     new SqlParameter("@isDaily", isDaily),
                           new SqlParameter("@Zone", zone),
                                 new SqlParameter("@DaysNames", daysNames),
                                         new SqlParameter("@Days", day),
            };

            DataTable myTable = DataTableHelper.getTableData("spRptLeadTime", param);
            return myTable;
        }

        public DataTable RatecodeReportsData(DateTime fromDate, DateTime toDate, string zone, string rate, string viewby, string day, string daysNames)
        {
            SqlParameter[] param = new SqlParameter[]
            {
               new SqlParameter("@FromDate", fromDate),
                  new SqlParameter("@ToDate", toDate),
                     new SqlParameter("@Zone", zone),
                           new SqlParameter("@RateCode", rate),
                                 new SqlParameter("@Currency", viewby),
                                 new SqlParameter("@MonthNames", daysNames),
                                         new SqlParameter("@Months", day),
            };

            DataTable myTable = DataTableHelper.getTableData("spRptRateCodeReport", param);
            return myTable;
        }
        public DataTable AnnualRoomOccupancyData(DateTime fromDate, DateTime toDate, string zone, string day, string daysNames)
        {
            SqlParameter[] param = new SqlParameter[]
            {
               new SqlParameter("@FromDate", fromDate),
                  new SqlParameter("@ToDate", toDate),
                     new SqlParameter("@Zone", zone),
                                 new SqlParameter("@MonthNames", daysNames),
                                         new SqlParameter("@Months", day),
            };

            DataTable myTable = DataTableHelper.getTableData("spRptAnnualRoomOccupancy", param);
            return myTable;
        }

        public DataTable RoomMovesData(DateTime fromDate, DateTime toDate)
        {

            SqlParameter[] param = new SqlParameter[]
             {
             new SqlParameter("@FromDate", fromDate),
             new SqlParameter("@ToDate", toDate),
             };

            DataTable myTable = DataTableHelper.getTableData("spRptRoomMoves", param);
            return myTable;
        }
        public DataTable DepositTransferredAtCheckIn(DateTime fromDate)
        {
            SqlParameter[] param = new SqlParameter[]
            {
               new SqlParameter("@dtpFromDate", fromDate),
            };

            DataTable myTable = DataTableHelper.getTableData("spRptDepositTransferredAtCheckIn", param);
            return myTable;
        }
        public DataTable RoomDiscrepancy(int Sleep, int Skip, int Person)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Sleep", Sleep),
                new SqlParameter("@Skip", Skip),
                new SqlParameter("@Person", Person)
            };

            return DataTableHelper.getTableData("spRptRoomDescrepancy", parameters);
        }

        public DataTable DepositLedger(DateTime fromDate)
        {
            SqlParameter[] param = new SqlParameter[]
            {
        new SqlParameter("@BusinessDate", SqlDbType.Date) { Value = fromDate.Date }
            };

            DataTable myTable = DataTableHelper.getTableData("spRptDepositLedger", param);
            return myTable;
        }
        public DataTable RevenueReports(DateTime fromDate, int type)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@dtpFromDate", fromDate),
                new SqlParameter("@Type", type)
            };

            return DataTableHelper.getTableData("spRptRevenue", param);
        }
        public DataTable PostingJournalInvoicing(DateTime fromDate, DateTime toDate)
        {

            SqlParameter[] param = new SqlParameter[]
             {
                 new SqlParameter("@FromDate", fromDate),
                 new SqlParameter("@ToDate", toDate),
             };

            DataTable myTable = DataTableHelper.getTableData("spRptPostingJournal", param);
            return myTable;
        }
        public DataTable CancellationJournal(DateTime fromDate, DateTime toDate, string transactionCodeList)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@FromDate", fromDate),
                new SqlParameter("@ToDate", toDate),
                new SqlParameter("@TransactionCodeList", transactionCodeList),
            };

            return DataTableHelper.getTableData("spRptCancellationJournal", param);
        }

        public DataTable TrialBalance(DateTime dtpDate, string currency)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@dtpDate", dtpDate),
                new SqlParameter("@Currency", currency),

            };

            return DataTableHelper.getTableData("spRptTrialBalance", param);

        }
        public DataTable ReservationRateCheck(DateTime date, string status, int ind, int pseudo, int variance,
                                      int fixRate, int package, int dcReason, string sort,
                                      int showFixCharge, int showAlerts)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@date", date),
                new SqlParameter("@status", status),
                new SqlParameter("@ind", ind),
                new SqlParameter("@pseudo", pseudo),
                new SqlParameter("@variance", variance),
                new SqlParameter("@fixRate", fixRate),
                new SqlParameter("@package", package), 
                new SqlParameter("@dcReason", dcReason),
                new SqlParameter("@sort", sort),
                new SqlParameter("@showFixCharge", showFixCharge),
                new SqlParameter("@showAlerts", showAlerts),
             };

            return DataTableHelper.getTableData("spRptReservationRateCodeCheck", param);
        }
        public DataTable GuestLedger(DateTime date, string statusList)
        {        
            string trimmed = statusList.Trim('\''); 
            string formattedStatus = $"{trimmed.Replace("'", "'")}";

            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@Date", date),
                new SqlParameter("@Status", formattedStatus),
            };
            return DataTableHelper.getTableData("spRptGuestLedger", param);
        }
        public DataTable OccupancybyPerson(DateTime fromDate, DateTime toDate, int roomTypeID)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@FromDate", fromDate),
                new SqlParameter("@ToDate", toDate),
                new SqlParameter("@RoomTypeID", roomTypeID)
            };

            return DataTableHelper.getTableData("spRptOccupancyByPerson", param);
        }








    }

}
