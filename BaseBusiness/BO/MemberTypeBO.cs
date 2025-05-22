using BaseBusiness.bc;
using BaseBusiness.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseBusiness.BO
{
    public class MemberTypeBO : BaseBO
    {
        private MemberTypeFacade facade = MemberTypeFacade.Instance;
        protected static MemberTypeBO instance = new MemberTypeBO();

        protected MemberTypeBO()
        {
            this.baseFacade = facade;
        }

        public static MemberTypeBO Instance
        {
            get { return instance; }
        }
    }
}
