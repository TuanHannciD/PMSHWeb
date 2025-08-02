using BaseBusiness.bc;
using BaseBusiness.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseBusiness.BO
{
    public class hkpAttendantBO : BaseBO
    {
        private hkpAttendantFacade facade = hkpAttendantFacade.Instance;
        protected static hkpAttendantBO instance = new hkpAttendantBO();

        protected hkpAttendantBO()
        {
            this.baseFacade = facade;
        }

        public static hkpAttendantBO Instance
        {
            get { return instance; }
        }
    }
}
