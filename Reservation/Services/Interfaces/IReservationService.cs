using BaseBusiness.util;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Services.Interfaces
{
    public interface IReservationService
    {
        DataTable GetRateCode(DateTime arrival, DateTime departure, int adults, int roomType);

        DataTable GetRoomAvailable(DateTime fromDate, DateTime toDate,string floor,string roomTypeID,string smoking,string foStatus,string hkStatus,string isDummy,string roomNo,int roomID,int Type );
        DataTable GetAllotment(string code, string marketID, string profileID,string isDefault,string allotmentTypeID);
        DataTable GetAllotmentDetail(int allotmentID,string roomType,DateTime showHistory);


    }
}
