using BaseBusiness.bc;
using BaseBusiness.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseBusiness.BO
{
    public class ReservationTypeBO : BaseBO
    {
        private ReservationTypeFacade facade = ReservationTypeFacade.Instance;
        protected static ReservationTypeBO instance = new ReservationTypeBO();

        protected ReservationTypeBO()
        {
            this.baseFacade = facade;
        }

        public static ReservationTypeBO Instance
        {
            get { return instance; }
        }
    }
}
