using BaseBusiness.bc;
using BaseBusiness.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseBusiness.BO
{
    public class CurrencyBO : BaseBO
    {
        private CurrencyFacade facade = CurrencyFacade.Instance;
        protected static CurrencyBO instance = new CurrencyBO();

        protected CurrencyBO()
        {
            this.baseFacade = facade;
        }

        public static CurrencyBO Instance
        {
            get { return instance; }
        }
    }
}
