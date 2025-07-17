using BaseBusiness.bc;
using BaseBusiness.Facade;
using BaseBusiness.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseBusiness.BO
{
    public class ReservationBO : BaseBO
    {
        private ReservationFacade facade = ReservationFacade.Instance;
        protected static ReservationBO instance = new ReservationBO();

        protected ReservationBO()
        {
            this.baseFacade = facade;
        }

        public static ReservationBO Instance
        {
            get { return instance; }
        }
        public static int GetTopConfirmationNo()
        {
            string query = "select top 1 ConfirmationNo from Reservation where ConfirmationNo <> '' order by id desc";
            return instance.GetFirst<int>(query);
        }
        public static int GetTopID()
        {
            string query = "select top 1 ID from Reservation order by id desc";
            return instance.GetFirst<int>(query);
        }
        public static List<ReservationModel> GetList(DateTime toDate, DateTime fromDate)
        {
            // Định dạng ngày thành YYYY-MM-DD
            string toDateStr = toDate.ToString("yyyy-MM-dd");
            string fromDateStr = fromDate.ToString("yyyy-MM-dd");

            string query = $"SELECT * FROM Reservation WHERE CAST(ArrivalDate AS DATE) >= CAST('{toDateStr}' AS DATE) AND CAST(DepartureDate AS DATE) <= CAST('{fromDateStr}' AS DATE) ORDER BY id DESC";
            return instance.GetList<ReservationModel>(query);
        }
        public static List<ProfileModel> GetProfileIndividual()
        {
            string query = $"select * from Profile where Type = 0 order by id desc ";
            return instance.GetList<ProfileModel>(query);
        }
    }
}
