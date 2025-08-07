using BaseBusiness.bc;
using BaseBusiness.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseBusiness.BO
{
    public class hkpTaskSheetBO : BaseBO
    {
        private hkpTaskSheetFacade facade = hkpTaskSheetFacade.Instance;
        protected static hkpTaskSheetBO instance = new hkpTaskSheetBO();

        protected hkpTaskSheetBO()
        {
            this.baseFacade = facade;
        }

        public static hkpTaskSheetBO Instance
        {
            get { return instance; }
        }
    }
}
