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
    }
}
