using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pentamic.SSBI.Services
{
    public interface IUserResolver
    {
        string GetUserId();
        string GetUserName();
    }
}
