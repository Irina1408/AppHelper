using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AppUtils.Settings
{
    public static class AppSettings
    {
        [Description("Decimal separator")] 
        public static string DecimalSeparator = ".";

        [Description("Date format")]
        public static string DateFormat = "yyyy-MM-dd";
    }
}
