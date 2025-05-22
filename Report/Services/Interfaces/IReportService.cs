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
    }
}
