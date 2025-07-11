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
    }
}
