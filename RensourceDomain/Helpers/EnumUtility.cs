using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RensourceDomain.Helpers
{
    public static class EnumUtility
    {
        public enum UserRoles : int
        {
            SuperAdmin = 1,
            Admin = 2,
        }
    }
}
