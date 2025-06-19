using BaseBusiness.bc;
using BaseBusiness.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseBusiness.BO
{
    public class AlertsSetupBO : BaseBO
    {
        private AlertsSetupFacade facade = AlertsSetupFacade.Instance;
        protected static AlertsSetupBO instance = new AlertsSetupBO();

        protected AlertsSetupBO()
        {
            this.baseFacade = facade;
        }

        public static AlertsSetupBO Instance
        {
            get { return instance; }
        }
    }
}
