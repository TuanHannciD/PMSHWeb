using BaseBusiness.bc;
using BaseBusiness.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseBusiness.BO
{
    public class SourceBO : BaseBO
    {
        private SourceFacade facade = SourceFacade.Instance;
        protected static SourceBO instance = new SourceBO();

        protected SourceBO()
        {
            this.baseFacade = facade;
        }

        public static SourceBO Instance
        {
            get { return instance; }
        }
    }
}
