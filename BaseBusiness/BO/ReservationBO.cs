using BaseBusiness.bc;
using BaseBusiness.Facade;
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
    }
}
