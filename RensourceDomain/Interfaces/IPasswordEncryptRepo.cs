using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RensourceDomain.Interfaces
{
    public interface IPasswordEncryptRepo
    {
        string EncryptPassword(string plainPassword);
        string DecryptPassword(string encryptedPassword);
    }
}
