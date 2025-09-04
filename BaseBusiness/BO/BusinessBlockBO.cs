using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseBusiness.bc;
using BaseBusiness.Facade;
using BaseBusiness.Model;

namespace BaseBusiness.BO
{
    public class BusinessBlockBO : BaseBO
    {
        private BusinessBlockFacade facade = BusinessBlockFacade.Instance;
        protected static BusinessBlockBO instance = new BusinessBlockBO();

        protected BusinessBlockBO()
        {
            this.baseFacade = facade;
        }

        public static BusinessBlockBO Instance
        {
            get { return instance; }
        }
        public static int  CheckRoomInBusinessBlock(int roomID, DateTime fromDate,DateTime toDate)
        {
            string query = $"select count(*) from BusinessBlock where RoomID = {roomID} and FromDateOOO >=  cast('{fromDate}' as date) and ToDateOOO <= cast('{toDate}' as date)";
            return instance.GetFirst<int>(query);
        }
    }
}
