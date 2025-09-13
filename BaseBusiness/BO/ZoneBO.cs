using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseBusiness.bc;
using BaseBusiness.Facade;
using Microsoft.Data.SqlClient;

namespace BaseBusiness.BO
{
    using BaseBusiness.Model;
    using Dapper;
    public class ZoneBO : BaseBO
    {
        private ZoneFacade facade = ZoneFacade.Instance;
        protected static ZoneBO instance = new ZoneBO();

        protected ZoneBO()
        {
            this.baseFacade = facade;
        }

        public static ZoneBO Instance
        {
            get { return instance; }
        
        }
        public ZoneModel GetById(int id, SqlConnection conn, SqlTransaction tx)
        {
            const string sql = "SELECT ID, Code, Name, Description, CreatedBy, CreatedDate,  UpdatedBy, UpdatedDate FROM Zone WHERE ID = @id";
            return conn.QuerySingleOrDefault<ZoneModel>(sql, new { id }, tx);
        }
    }
}
