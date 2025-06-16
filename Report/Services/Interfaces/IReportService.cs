using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Services.Interfaces
{
    public interface IReportService
    {
        DataTable GetBookingSourceData(DateTime fromDate, DateTime toDate);
        DataTable GroupReservation(DateTime fromDate, DateTime toDate, string NoOfRoom);
        DataTable GuestStayOver(DateTime fromDate, DateTime toDate);

        DataTable GuestStay(string noofName, string stayno, string stayand);
        DataTable ReportNationalityStatistics(DateTime fromDate, DateTime toDate, string status, string sortOder);
        DataTable ReservationSummaryReport(DateTime fromDate, DateTime toDate);
        DataTable TraceReportView(DateTime fromDate, DateTime toDate, int roomClass, int department, int status, int byAlphabetical, int byRoom, int byVip, int pseudoRoom, int reserved, int checkedIn, int dueout, int individual, int blockcode, int vipOnly);
        DataTable RatecodebyDate(DateTime fromDate, DateTime toDate, string ratecode);
         DataTable GuestMarketReport(DateTime fromDate, DateTime toDate, string currency, string zonecode);
        DataTable OTAMonthlyReport(string fromDate, string Number, string type, string currencyID);
        DataTable DailyPickupReport(DateTime fromDate, DateTime toDate);
        DataTable DailyBreakfastDetail(DateTime fromDate);
        DataTable FreeUpgradeReport(DateTime fromDate, DateTime toDate, string viewBy, string status);
        DataTable FixChargeReport(DateTime fromDate, DateTime toDate, string trancode, string status);
        DataTable ReveunueByData(DateTime fromDate, DateTime toDate, string reservation, string roomType, string zone, string viewBy, string sortOrder);
        DataTable DepartureExtendedReport(DateTime fromDate);
        DataTable RoomOccupancyReport(DateTime fromDate, DateTime toDate, string zone);
        DataTable ReservationCancellationsReport(DateTime fromDate, DateTime toDate, string commnet, string typeDate);
        DataTable ReservationStatisticsReport(DateTime fromDate);
        DataTable  ReservationbyCompanyReport(DateTime fromDate, DateTime toDate, string roomClass, string roomType, string searchCrip, string sortOrder, string noOfRoom);
        DataTable NoShowReportData(DateTime fromDate, DateTime toDate, int roomClass);
        DataTable ReservationSummaryData(DateTime fromDate, DateTime toDate, string roomType, string zone, string viewBy, string market);
        DataTable ProductActivityData(DateTime fromDate, DateTime toDate, string type, string currency);
        DataTable NationalStatisticsData(DateTime fromDate, DateTime toDate, string roomtype, string viewBy);
        DataTable SalesinChargeReportsForm1Data(DateTime fromDate, string viewBy);
        DataTable SalesinChargeActivityData(DateTime fromDate, DateTime toDate, string type, string currency);
        DataTable RevenueDetailData(DateTime fromDate, DateTime toDate);
        DataTable RevenueSummaryData(DateTime fromDate, DateTime toDate);
        DataTable TAProductionReportData(DateTime fromDate, DateTime toDate, string type, string currency);
        DataTable LeadtimeReportsData(DateTime fromDate, DateTime toDate, string zone, string isDaily, string day, string daysNames);
        DataTable RatecodeReportsData(DateTime fromDate, DateTime toDate, string zone, string rate, string viewby, string day, string daysNames);
        DataTable AnnualRoomOccupancyData(DateTime fromDate, DateTime toDate, string zone, string day, string daysNames);
    }
}
