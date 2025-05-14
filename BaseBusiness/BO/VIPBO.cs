using BaseBusiness.bc;
using BaseBusiness.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseBusiness.BO
{
    public class VIPBO : BaseBO
    {
        private VIPFacade facade = VIPFacade.Instance;
        protected static VIPBO instance = new VIPBO();

        protected VIPBO()
        {
            this.baseFacade = facade;
        }

        public static VIPBO Instance
        {
            get { return instance; }
        }
    }
}
