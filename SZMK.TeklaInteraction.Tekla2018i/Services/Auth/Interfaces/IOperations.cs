using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SZMK.TeklaInteraction.Shared.Models;

namespace SZMK.TeklaInteraction.Tekla2018i.Services.Auth.Interfaces
{
    interface IOperations
    {
        List<User> GetUsers();
        User GetUser(string Login);
        bool Authrozation(string Login, string Password);
        bool UpdatePassword(User user, string NewPassword);
    }
}
