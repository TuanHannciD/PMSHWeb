using BaseBusiness.bc;
using BaseBusiness.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseBusiness.BO
{
    public class LanguageBO : BaseBO
    {
        private LanguageFacade facade = LanguageFacade.Instance;
        protected static LanguageBO instance = new LanguageBO();

        protected LanguageBO()
        {
            this.baseFacade = facade;
        }

        public static LanguageBO Instance
        {
            get { return instance; }
        }
    }
}
