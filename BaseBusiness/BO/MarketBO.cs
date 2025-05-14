using BaseBusiness.bc;
using BaseBusiness.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseBusiness.BO
{
    public class MarketBO : BaseBO
    {
        private MarketFacade facade = MarketFacade.Instance;
        protected static MarketBO instance = new MarketBO();

        protected MarketBO()
        {
            this.baseFacade = facade;
        }

        public static MarketBO Instance
        {
            get { return instance; }
        }
    }
}
