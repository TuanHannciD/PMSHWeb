using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseKeeping.Services.Interfaces
{
    public interface IHouseKeepingService
    {
        DataTable RoomControlPanelData(DateTime fromDate, DateTime toDate, string zone);
        DataTable RoomFacilityForecastData(DateTime fromDate, DateTime toDate, string zone);
        DataTable RoomStatusData(int cleannon_room, int clean, int dirty, int pickup, int oocheck, int oscheck, int vacant, int occupied, int arrivals, int arrived, int stayover, int dayuse, int dueout, int departed, int notReserved, int departuredarr, string roomType, string zone, string roomFrom, string roomTo);
        DataTable CheckLogStatus(string RoomNo, DateTime fromDate, DateTime toDate, string username);
        DataTable RoomPlanData(DateTime fromDate, DateTime toDate, int orderbyroom, string owner);
    }
}
