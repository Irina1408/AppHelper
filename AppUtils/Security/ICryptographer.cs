using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppUtils.Security
{
    public interface ICryptographer
    {
        string Encrypt(string text);
        string Decrypt(string text);
    }
}
