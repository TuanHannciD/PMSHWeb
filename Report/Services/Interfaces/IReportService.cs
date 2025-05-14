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
    }
}
