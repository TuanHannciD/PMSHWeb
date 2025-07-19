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

        public DataTable RoomFacilityForecastData(DateTime fromDate, DateTime toDate, string zone)
        {
            SqlParameter[] param = new SqlParameter[]
            {
               new SqlParameter("@FromDate", fromDate),
                new SqlParameter("@ToDate", toDate),
                 new SqlParameter("@ZoneCode", zone),

            };

            DataTable myTable = DataTableHelper.getTableData("spRmgRoomFacilityForecastReport", param);
            return myTable;
        }
        public DataTable RoomStatusData(int cleannon_room, int clean, int dirty, int pickup, int oocheck, int oscheck, int vacant, int occupied, int arrivals, int arrived, int stayover, int dayuse, int dueout, int departed, int notReserved, int departuredarr, string roomType, string zone, string roomFrom, string roomTo)
        {
            SqlParameter[] param = new SqlParameter[]
            {
               new SqlParameter("@Clean", cleannon_room),
                new SqlParameter("@Dirty", dirty),
                 new SqlParameter("@Pickup", pickup),
                        new SqlParameter("@Inspected", clean),

                          new SqlParameter("@OOOrder", oocheck),
                new SqlParameter("@OOService", oscheck),
                 new SqlParameter("@Vacant", vacant),
                        new SqlParameter("@Occupied", occupied),

                             new SqlParameter("@Arrivals", arrivals),
                new SqlParameter("@Arrived", arrived),
                 new SqlParameter("@Stayover", stayover),
                        new SqlParameter("@DayUse", dayuse),
                          new SqlParameter("@DueOut", dueout),
                new SqlParameter("@Departed", departed),
                 new SqlParameter("@NotReserved", notReserved),
                        new SqlParameter("@Departured", departuredarr),
                          new SqlParameter("@CurrentDate", DateTime.Now),
            };

            DataTable myTable = DataTableHelper.getTableData("spHkpRoomStatus", param);
            return myTable;
        }

        public DataTable CheckLogStatus(string  RoomNo, DateTime fromDate, DateTime toDate, string username)
        {

            SqlParameter[] param = new SqlParameter[]
            {
               new SqlParameter("@RoomNo", RoomNo),
                new SqlParameter("@UserName",username),
                 new SqlParameter("@FromDate", fromDate),
                        new SqlParameter("@ToDate", toDate),

                 
            };

            DataTable myTable = DataTableHelper.getTableData("spRoomStatusHistory", param);
            return myTable;
        }
        public DataTable RoomPlanData(DateTime fromDate, DateTime toDate, int  orderbyroom, string owner)
        {

            SqlParameter[] param = new SqlParameter[]
            {
               new SqlParameter("@Owner", owner),
                new SqlParameter("@OrderByRoom",orderbyroom),
                 new SqlParameter("@MinDate", fromDate),
                        new SqlParameter("@MaxDate", toDate),


            };

            DataTable myTable = DataTableHelper.getTableData("spRmgRoomPlan", param);
            return myTable;
        }
        public DataTable SummaryTotalPhysicalRoom(string roomtype, string zone)
        {

            SqlParameter[] param = new SqlParameter[]
            {
               new SqlParameter("@RoomTypeID", roomtype),
                new SqlParameter("@ZoneID",zone)
     ,


            };

            DataTable myTable = DataTableHelper.getTableData("spRmgStatusSummaryTotalPhysicalRoom", param);
            return myTable;
        }
        public DataTable StatusSummaryOutOfOrder(DateTime datebunisess, string roomtype, string zone)
        {

            SqlParameter[] param = new SqlParameter[]
            {
               new SqlParameter("@RoomTypeID", roomtype),
                new SqlParameter("@ZoneID",zone) ,
                 new SqlParameter("@BusinessDate",datebunisess)


            };

            DataTable myTable = DataTableHelper.getTableData("spRmgStatusSummaryOutOfOrder", param);
            return myTable;
        }
        public DataTable SummaryOutOfService(DateTime datebunisess, string roomtype, string zone)
        {

            SqlParameter[] param = new SqlParameter[]
            {
               new SqlParameter("@RoomTypeID", roomtype),
                new SqlParameter("@ZoneID",zone) ,
                 new SqlParameter("@BusinessDate",datebunisess)


            };

            DataTable myTable = DataTableHelper.getTableData("spRmgStatusSummaryOutOfService", param);
            return myTable;
        }
        public DataTable ActivityStayOver(DateTime datebunisess, string roomtype, string zone)
        {

            SqlParameter[] param = new SqlParameter[]
            {
               new SqlParameter("@RoomTypeID", roomtype),
                new SqlParameter("@ZoneID",zone) ,
                 new SqlParameter("@BusinessDate",datebunisess)


            };

            DataTable myTable = DataTableHelper.getTableData("spRmgStatusActivityStayOver", param);
            return myTable;
        }
        public DataTable ActivityDepartureExpected(DateTime datebunisess, string roomtype, string zone)
        {

            SqlParameter[] param = new SqlParameter[]
            {
               new SqlParameter("@RoomTypeID", roomtype),
                new SqlParameter("@ZoneID",zone) ,
                 new SqlParameter("@BusinessDate",datebunisess)


            };

            DataTable myTable = DataTableHelper.getTableData("spRmgStatusActivityDepartureExpected", param);
            return myTable;
        }

        public DataTable ActivityDepartureActual(DateTime datebunisess, string roomtype, string zone)
        {

            SqlParameter[] param = new SqlParameter[]
            {
               new SqlParameter("@RoomTypeID", roomtype),
                new SqlParameter("@ZoneID",zone) ,
                 new SqlParameter("@BusinessDate",datebunisess)


            };

            DataTable myTable = DataTableHelper.getTableData("spRmgStatusActivityDepartureActual", param);
            return myTable;
        }

        public DataTable ActivityArrivalExpected(DateTime datebunisess, string roomtype, string zone)
        {

            SqlParameter[] param = new SqlParameter[]
            {
               new SqlParameter("@RoomTypeID", roomtype),
                new SqlParameter("@ZoneID",zone) ,
                 new SqlParameter("@BusinessDate",datebunisess)


            };

            DataTable myTable = DataTableHelper.getTableData("spRmgStatusActivityArrivalExpected", param);
            return myTable;
        }
        public DataTable ActivityArrivalActual(DateTime datebunisess, string roomtype, string zone)
        {

            SqlParameter[] param = new SqlParameter[]
            {
               new SqlParameter("@RoomTypeID", roomtype),
                new SqlParameter("@ZoneID",zone) ,
                 new SqlParameter("@BusinessDate",datebunisess)


            };

            DataTable myTable = DataTableHelper.getTableData("spRmgStatusActivityArrivalActual", param);
            return myTable;
        }
        public DataTable ActivityExtendedStay(DateTime datebunisess, string roomtype, string zone)
        {

            SqlParameter[] param = new SqlParameter[]
            {
               new SqlParameter("@RoomTypeID", roomtype),
                new SqlParameter("@ZoneID",zone) ,
                 new SqlParameter("@BusinessDate",datebunisess)


            };

            DataTable myTable = DataTableHelper.getTableData("spRmgStatusActivityExtendedStay", param);
            return myTable;
        }

        public DataTable ActivityEarlyDeparture(DateTime datebunisess, string roomtype, string zone)
        {

            SqlParameter[] param = new SqlParameter[]
            {
               new SqlParameter("@RoomTypeID", roomtype),
                new SqlParameter("@ZoneID",zone) ,
                 new SqlParameter("@BusinessDate",datebunisess)


            };

            DataTable myTable = DataTableHelper.getTableData("spRmgStatusActivityEarlyDeparture", param);
            return myTable;
        }
        public DataTable ActivityDayUseRoom(DateTime datebunisess, string roomtype, string zone)
        {

            SqlParameter[] param = new SqlParameter[]
            {
               new SqlParameter("@RoomTypeID", roomtype),
                new SqlParameter("@ZoneID",zone) ,
                 new SqlParameter("@BusinessDate",datebunisess)


            };

            DataTable myTable = DataTableHelper.getTableData("spRmgStatusActivityDayUseRoom", param);
            return myTable;
        }
        public DataTable StatusHKInspected( string roomtype, string zone)
        {

            SqlParameter[] param = new SqlParameter[]
            {
               new SqlParameter("@RoomTypeID", roomtype),
                new SqlParameter("@ZoneID",zone) ,
         


            };

            DataTable myTable = DataTableHelper.getTableData("spRmgStatusHKInspected", param);
            return myTable;
        }
        public DataTable StatusHKClean(string roomtype, string zone)
        {

            SqlParameter[] param = new SqlParameter[]
            {
               new SqlParameter("@RoomTypeID", roomtype),
                new SqlParameter("@ZoneID",zone) ,



            };

            DataTable myTable = DataTableHelper.getTableData("spRmgStatusHKClean", param);
            return myTable;
        }
        public DataTable StatusHKDirty(string roomtype, string zone)
        {

            SqlParameter[] param = new SqlParameter[]
            {
               new SqlParameter("@RoomTypeID", roomtype),
                new SqlParameter("@ZoneID",zone) ,

            };

            DataTable myTable = DataTableHelper.getTableData("spRmgStatusHKDirty", param);
            return myTable;
        }
        public DataTable StatusHKOutOfOrder(string roomtype, string zone)
        {

            SqlParameter[] param = new SqlParameter[]
            {
               new SqlParameter("@RoomTypeID", roomtype),
                new SqlParameter("@ZoneID",zone) ,

            };

            DataTable myTable = DataTableHelper.getTableData("spRmgStatusHKOutOfOrder", param);
            return myTable;
        }
        public DataTable StatusHKOutOfService(string roomtype, string zone)
        {

            SqlParameter[] param = new SqlParameter[]
            {
               new SqlParameter("@RoomTypeID", roomtype),
                new SqlParameter("@ZoneID",zone) ,

            };

            DataTable myTable = DataTableHelper.getTableData("spRmgStatusHKOutOfService", param);
            return myTable;
        }

        public DataTable StatusEndOfDayGroupAndBlock(DateTime datebunisess, string roomtype, string zone)
        {

            SqlParameter[] param = new SqlParameter[]
            {
               new SqlParameter("@RoomTypeID", roomtype),
                new SqlParameter("@ZoneID",zone) ,
                  new SqlParameter("@BusinessDate",datebunisess)
            };

            DataTable myTable = DataTableHelper.getTableData("spRmgStatusEndOfDayGroupAndBlock", param);
            return myTable;
        }

        public DataTable StatusEndOfDayIndividual(DateTime datebunisess, string roomtype, string zone)
        {

            SqlParameter[] param = new SqlParameter[]
            {
               new SqlParameter("@RoomTypeID", roomtype),
                new SqlParameter("@ZoneID",zone) ,
                  new SqlParameter("@BusinessDate",datebunisess)
            };

            DataTable myTable = DataTableHelper.getTableData("spRmgStatusEndOfDayIndividual", param);
            return myTable;
        }
        public DataTable StatusEndOfDayCHU(DateTime datebunisess, string roomtype, string zone)
        {

            SqlParameter[] param = new SqlParameter[]
            {
               new SqlParameter("@RoomTypeID", roomtype),
                new SqlParameter("@ZoneID",zone) ,
                  new SqlParameter("@BusinessDate",datebunisess)
            };

            DataTable myTable = DataTableHelper.getTableData("spRmgStatusEndOfDayCHU", param);
            return myTable;
        }
        public DataTable StatusEndOfDayMaxOccTonight(DateTime datebunisess, string roomtype, string zone)
        {

            SqlParameter[] param = new SqlParameter[]
            {
               new SqlParameter("@RoomTypeID", roomtype),
                new SqlParameter("@ZoneID",zone) ,
                  new SqlParameter("@BusinessDate",datebunisess)
            };

            DataTable myTable = DataTableHelper.getTableData("spRmgStatusEndOfDayMaxOccTonight", param);
            return myTable;
        }
        public DataTable StatusEndOfDayRoomRevenue(DateTime datebunisess, string roomtype, string zone)
        {

            SqlParameter[] param = new SqlParameter[]
            {
               new SqlParameter("@RoomTypeID", roomtype),
                new SqlParameter("@ZoneID",zone) ,
                  new SqlParameter("@BusinessDate",datebunisess)
            };

            DataTable myTable = DataTableHelper.getTableData("spRmgStatusEndOfDayRoomRevenue", param);
            return myTable;
        }
    }
}
