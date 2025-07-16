using BaseBusiness.bc;
using BaseBusiness.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseBusiness.BO
{
    public class RoomStatusHistoryBO : BaseBO
    {
        private RoomStatusHistoryFacade facade = RoomStatusHistoryFacade.Instance;
        protected static RoomStatusHistoryBO instance = new RoomStatusHistoryBO();

        protected RoomStatusHistoryBO()
        {
            this.baseFacade = facade;
        }

        public static RoomStatusHistoryBO Instance
        {
            get { return instance; }
        }
    }
}
