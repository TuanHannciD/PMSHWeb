using BaseBusiness.bc;
using BaseBusiness.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
