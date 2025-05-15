using BaseBusiness.bc;
using BaseBusiness.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseBusiness.BO
{
    public class ZoneBO : BaseBO
    {
        private ZoneFacade facade = ZoneFacade.Instance;
        protected static ZoneBO instance = new ZoneBO();

        protected ZoneBO()
        {
            this.baseFacade = facade;
        }

        public static ZoneBO Instance
        {
            get { return instance; }
        }
    }
}
