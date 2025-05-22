using BaseBusiness.bc;
using BaseBusiness.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseBusiness.BO
{
    public class RoomBO : BaseBO
    {
        private RoomFacade facade = RoomFacade.Instance;
        protected static RoomBO instance = new RoomBO();

        protected RoomBO()
        {
            this.baseFacade = facade;
        }

        public static RoomBO Instance
        {
            get { return instance; }
        }
    }
}
