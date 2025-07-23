using BaseBusiness.bc;
using BaseBusiness.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseBusiness.BO
{
    public class FolioBO : BaseBO
    {
        private FolioFacade facade = FolioFacade.Instance;
        protected static FolioBO instance = new FolioBO();

        protected FolioBO()
        {
            this.baseFacade = facade;
        }

        public static FolioBO Instance
        {
            get { return instance; }
        }
        public static int GetFolioIDByReservationID(int reservationID)
        {
            string query = $"select top 1 ID from Folio where ReservationID = {reservationID}";
            return instance.GetFirst<int>(query);
        }
    }
}
