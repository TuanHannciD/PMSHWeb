using BaseBusiness.bc;
using BaseBusiness.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseBusiness.BO
{
    public class MarketTypeBO : BaseBO
    {
        private MarketTypeFacade facade = MarketTypeFacade.Instance;
        protected static MarketTypeBO instance = new MarketTypeBO();

        protected MarketTypeBO()
        {
            this.baseFacade = facade;
        }

        public static MarketTypeBO Instance
        {
            get { return instance; }
        }
    }
}
