using BaseBusiness.util;
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
    }
}
