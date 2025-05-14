using BaseBusiness.bc;
using BaseBusiness.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseBusiness.BO
{
    public class CountryBO : BaseBO
    {
        private CountryFacade facade = CountryFacade.Instance;
        protected static CountryBO instance = new CountryBO();

        protected CountryBO()
        {
            this.baseFacade = facade;
        }

        public static CountryBO Instance
        {
            get { return instance; }
        }
    }
}
