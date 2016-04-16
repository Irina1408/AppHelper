using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEfServices.Settings
{
    public interface IEfConnectionSettings
    {
        string EfConnectionString { get; }
    }
}
