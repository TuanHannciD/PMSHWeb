using BaseBusiness.bc;
using BaseBusiness.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseBusiness.BO
{
    public class FolioDetailBO : BaseBO
    {
        private FolioDetailFacade facade = FolioDetailFacade.Instance;
        protected static FolioDetailBO instance = new FolioDetailBO();

        protected FolioDetailBO()
        {
            this.baseFacade = facade;
        }

        public static FolioDetailBO Instance
        {
            get { return instance; }
        }
    }
}
