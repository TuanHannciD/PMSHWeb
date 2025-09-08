using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseBusiness.bc;
using BaseBusiness.Facade;

namespace BaseBusiness.BO
{
    public class MemberCategoryBO : BaseBO
    {
        private MemberCategoryFacade facade = MemberCategoryFacade.Instance;
        protected static MemberCategoryBO instance = new MemberCategoryBO();

        protected MemberCategoryBO()
        {
            this.baseFacade = facade;
        }

        public static MemberCategoryBO Instance
        {
            get { return instance; }
        }
    }
}
