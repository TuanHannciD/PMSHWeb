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
    }
}
