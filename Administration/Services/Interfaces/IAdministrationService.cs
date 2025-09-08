using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraRichEdit.Model;

namespace Administration.Services.Interfaces
{
    public interface IAdministrationService
    {
        public DataTable MemberList(string code, string name, int inactive);
        public DataTable MemberCategory(string code, string name, int inactive);

        public DataTable City(string code, string name, int inactive);
        public DataTable Country(string code, string name, int inactive);
        public DataTable Language(string code, string name, int inactive);
        public DataTable Nationality(string code, string name, int inactive);
        public DataTable Title(string code, string name, int inactive);
    }
}
