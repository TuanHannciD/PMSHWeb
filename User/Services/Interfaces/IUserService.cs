using BaseBusiness.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Services.Interfaces
{
    public interface IUserService
    {

        UsersModel Login(string LoginName, string Password);
    }
}
