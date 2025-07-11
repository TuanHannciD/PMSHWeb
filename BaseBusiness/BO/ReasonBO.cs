using BaseBusiness.bc;
using BaseBusiness.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseBusiness.BO
{
    public class ReasonBO : BaseBO
    {
        private ReasonFacade facade = ReasonFacade.Instance;
        protected static ReasonBO instance = new ReasonBO();

        protected ReasonBO()
        {
            this.baseFacade = facade;
        }

        public static ReasonBO Instance
        {
            get { return instance; }
        }
    }
}
