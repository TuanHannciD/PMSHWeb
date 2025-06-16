using BaseBusiness.bc;
using BaseBusiness.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseBusiness.BO
{
    public class CommentBO : BaseBO
    {
        private CommentFacade facade = CommentFacade.Instance;
        protected static CommentBO instance = new CommentBO();

        protected CommentBO()
        {
            this.baseFacade = facade;
        }

        public static CommentBO Instance
        {
            get { return instance; }
        }
    }
}
