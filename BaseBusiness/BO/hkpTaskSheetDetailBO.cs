using BaseBusiness.bc;
using BaseBusiness.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseBusiness.BO
{
    public class hkpTaskSheetDetailBO : BaseBO
    {
        private hkpTaskSheetDetailFacade facade = hkpTaskSheetDetailFacade.Instance;
        protected static hkpTaskSheetDetailBO instance = new hkpTaskSheetDetailBO();

        protected hkpTaskSheetDetailBO()
        {
            this.baseFacade = facade;
        }

        public static hkpTaskSheetDetailBO Instance
        {
            get { return instance; }
        }
    }
}
