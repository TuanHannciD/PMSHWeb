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

        /// <summary>
        /// DatVP: Lây danh sách room available từ store procedure
        /// </summary>
        /// <returns>Data table chứa danh sách room available</returns>
        DataTable GetRoomAvailable(DateTime fromDate, DateTime toDate,string floor,string roomTypeID,string smoking,string foStatus,string hkStatus,string isDummy,string roomNo,int roomID,int Type );
        DataTable GetAllotment(string code, string marketID, string profileID,string isDefault,string allotmentTypeID);
        DataTable GetAllotmentDetail(int allotmentID,string roomType,DateTime showHistory);
        DataTable GetReservationPreference(string code, int group);
        DataTable ReservationRateQueryDetail(DateTime fromDate, DateTime toDate,int roomType,int adults,int noOfNight,int packageID,int promotionID,
            string tableName,string onRows,string onRowsAlias,string onCols,string sumcol,int func,string currency,int display,int dayUse,int c1,int c2,
            int c3,int noOfRoom);

        /// <summary>
        /// DatVP: lấy rate query detail
        /// </summary>
        /// <param name="fromDate">arival date</param>
        /// <param name="toDate">departure date</param>
        /// <param name="rateCodeID">id rate code</param>
        /// <param name="roomType">id room type</param>
        /// <param name="currency">Currency</param>
        /// <param name="packageID">id package</param>
        /// <param name="day">day</param>
        /// <returns>Giá trị net</returns>
        DataTable ReservationGetRateQueryDetail(DateTime fromDate, DateTime toDate,int rateCodeID, int roomType, string currency,int packageID,int day);

        /// <summary>
        /// DatVP: Tính net cho rate code
        /// </summary>
        /// <param name="Price">Giá trị tiền rate code</param>
        /// <param name="TransactionCode">Giá trị tiền rate code</param>
        /// <returns>Giá trị net</returns>
        (decimal price, decimal priceAfter, decimal priceDiscount, decimal priceAfterDiscount) CalculateNet(decimal Price,string TransactionCode,decimal DiscountAmount, decimal DiscountPercent);
        (decimal Price, decimal priceAfter, decimal priceDiscount, decimal priceAfterDiscount) CalculateNetReverse(decimal priceAfterDiscount, string TransactionCode, decimal DiscountAmount, decimal DiscountPercent);

        /// <summary>
        /// DatVP: Tính net cho fixed charge
        /// </summary>
        /// <param name="transactionCode"> code transaction</param>
        /// <param name="price">giá trị tiền</param>
        /// <returns>Giá trị net</returns>
        decimal CalculateNetFixedCharge(string transactionCode,decimal price);


        /// <summary>
        /// DatVP: search reservation
        /// </summary>
        /// <returns>datâtble list reservation</returns>
        DataTable SearchReservation(int searchType, string name, string firstName, string reservationHolder, string confirmationNo,
            string crsNo, string roomNo, string roomType, string package, string zone, DateTime arrivalFrom, DateTime arrivalTo, string roomSharer, string owner);


        /// <summary>
        /// DatVP: search waitt list
        /// </summary>
        /// <returns>datatable list wait lít</returns>
        DataTable SearchWaitlist(string name, string priority, string market, string roomType,  string reason, string rateCode, string phone, DateTime date);

        /// <summary>
        /// DatVP: se
        /// </summary>
        /// <returns>datatable list wait lít</returns>
        DataTable SearchOverBooking(string sqlCommand);

        DataTable ActivityLogOverbooking(string sqlCommand);

        string GetConfigETA();
        string GetConfigETD();
    }
}
