using BaseBusiness.util;
using HouseKeeping.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseKeeping.Services.Implements
{
    public class HouseKeepingService: IHouseKeepingService
    {
        public DataTable RoomControlPanelData(DateTime fromDate, DateTime toDate, string zone)
        {
            SqlParameter[] param = new SqlParameter[]
            {
               new SqlParameter("@FromDate", fromDate),
                new SqlParameter("@ToDate", toDate),
                 new SqlParameter("@ZoneCode", zone),
          
            };

            DataTable myTable = DataTableHelper.getTableData("spRmgRoomControlPanelReport", param);
            return myTable;
        }
    }
}
