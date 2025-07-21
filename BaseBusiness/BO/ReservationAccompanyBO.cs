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
    public class ReservationAccompanyBO : BaseBO
    {
        private ReservationAccompanyFacade facade = ReservationAccompanyFacade.Instance;
        protected static ReservationAccompanyBO instance = new ReservationAccompanyBO();

        protected ReservationAccompanyBO()
        {
            this.baseFacade = facade;
        }

        public static ReservationAccompanyBO Instance
        {
            get { return instance; }
        }
        public static List<ReservationAccompanyModel> GetReservationAccompanyByReservationID(int reservationID)
        {


            string query = $"SELECT * FROM ReservationAccompany WHERE ReservationID = {reservationID} ORDER BY id DESC";
            return instance.GetList<ReservationAccompanyModel>(query);
        }
    }
}
