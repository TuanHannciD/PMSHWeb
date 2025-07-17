using BaseBusiness.bc;
using BaseBusiness.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseBusiness.BO
{
    public class ConfigSystemBO :  BaseBO
    {
        private ConfigSystemFacade facade = ConfigSystemFacade.Instance;
        protected static ConfigSystemBO instance = new ConfigSystemBO();

        protected ConfigSystemBO()
        {
            this.baseFacade = facade;
        }

        public static ConfigSystemBO Instance
        {
            get { return instance; }
        }
    }
}
