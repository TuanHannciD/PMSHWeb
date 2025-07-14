using BaseBusiness.BO;
using BaseBusiness.util;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.Data.SqlClient;
using Reservation.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Services.Implements
{
    public class ReservationService : IReservationService
    {
    

        public DataTable GetAllotment(string code, string marketID, string profileID, string isDefault,string allotmentTypeID)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@Code", code),
                    new SqlParameter("@MarketID", marketID),
                    new SqlParameter("@AllotmentTypeID", allotmentTypeID),
                    new SqlParameter("@ProfileID", profileID),
                    new SqlParameter("@IsDefault", isDefault),

                };

                DataTable myTable = DataTableHelper.getTableData("spAllotmentSearch", param);
                return myTable;
            }
            catch (SqlException ex)
            {

                throw new Exception($"ERROR: {ex.Message}", ex);
            }
            catch (Exception ex)
            {

                throw new Exception($"ERROR: {ex.Message}", ex);
            }
        }

        public DataTable GetAllotmentDetail(int allotmentID, string roomType, DateTime showHistory)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@AllotmentID", allotmentID),
                    new SqlParameter("@RoomType", roomType),
                    new SqlParameter("@ShowHistory", showHistory)
,

                };

                DataTable myTable = DataTableHelper.getTableData("spAllotmentDetailSearch_Temp", param);
                return myTable;
            }
            catch (SqlException ex)
            {

                throw new Exception($"ERROR: {ex.Message}", ex);
            }
            catch (Exception ex)
            {

                throw new Exception($"ERROR: {ex.Message}", ex);
            }
        }

        public DataTable GetRateCode(DateTime arrival, DateTime departure, int adults, int roomType)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@ArrivalDate", arrival),
                    new SqlParameter("@DepartureDate", departure),
                    new SqlParameter("@Adults", adults),
                    new SqlParameter("@RoomType", roomType),
                };

                DataTable myTable = DataTableHelper.getTableData("Web_GetRateCode", param);
                return myTable;
            }
            catch (SqlException ex)
            {

                throw new Exception($"Lỗi cơ sở dữ liệu khi lấy RateCode: {ex.Message}", ex);
            }
            catch (Exception ex)
            {

                throw new Exception($"Lỗi không xác định khi lấy RateCode: {ex.Message}", ex);
            }
        }

        public DataTable GetReservationPreference(string code, int group)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@Code", code),
                    new SqlParameter("@Group", group),


                };

                DataTable myTable = DataTableHelper.getTableData("spReservationPreference", param);
                return myTable;
            }
            catch (SqlException ex)
            {

                throw new Exception($"ERROR: {ex.Message}", ex);
            }
            catch (Exception ex)
            {

                throw new Exception($"ERROR: {ex.Message}", ex);
            }
        }

        public DataTable GetRoomAvailable(DateTime fromDate, DateTime toDate, string floor, string roomTypeID, string smoking, string foStatus, string hkStatus, string isDummy, string roomNo, int roomID, int Type)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@FromDate", fromDate),
                    new SqlParameter("@ToDate", toDate),
                    new SqlParameter("@Floor", floor),
                    new SqlParameter("@RoomTypeID", roomTypeID),
                    new SqlParameter("@Smoking", smoking),
                    new SqlParameter("@FOStatus", foStatus),
                    new SqlParameter("@HKStatusID", hkStatus),
                    new SqlParameter("@IsDummy", isDummy),
                    new SqlParameter("@RoomNo", roomNo),
                    new SqlParameter("@RoomID", roomID),
                    new SqlParameter("@Type", Type),
                };

                DataTable myTable = DataTableHelper.getTableData("spAvailableRoomsSearch", param);
                return myTable;
            }
            catch (SqlException ex)
            {

                throw new Exception($"Error: {ex.Message}", ex);
            }
            catch (Exception ex)
            {

                throw new Exception($"Error: {ex.Message}", ex);
            }
        }

        public DataTable ReservationRateQueryDetail(DateTime fromDate, DateTime toDate, int roomType, int adults, int noOfNight, 
            int packageID, int promotionID, string tableName, string onRows, string onRowsAlias, string onCols, string sumcol,
            int func, string currency, int display, int dayUse, int c1, int c2, int c3, int noOfRoom)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@fromDate", fromDate),
                    new SqlParameter("@toDate", toDate),
                    new SqlParameter("@roomType", roomType),
                    new SqlParameter("@adults", adults),
                    new SqlParameter("@NoOfNight", noOfNight),
                    new SqlParameter("@PackageID", packageID),
                    new SqlParameter("@PromotionID", promotionID),
                    new SqlParameter("@table", tableName),
                    new SqlParameter("@onrows", onRows),
                    new SqlParameter("@onrowsalias", onRowsAlias),
                    new SqlParameter("@oncols", onCols),
                    new SqlParameter("@sumcol", sumcol),
                    new SqlParameter("@func", func),
                    new SqlParameter("@currency", currency),
                    new SqlParameter("@display", display),
                    new SqlParameter("@dayuse", dayUse),
                    new SqlParameter("@c1", c1),
                    new SqlParameter("@c2", c2),
                    new SqlParameter("@c3", c3),
                    new SqlParameter("@NoOfRoom", noOfRoom),
                };

                DataTable myTable = DataTableHelper.getTableData("spReservationRateQueryDetail", param);
                return myTable;
            }
            catch (SqlException ex)
            {

                throw new Exception($"ERROR: {ex.Message}", ex);
            }
            catch (Exception ex)
            {

                throw new Exception($"ERROR: {ex.Message}", ex);
            }
        }
    }
}
