using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using AppView.CommonInterfaces;

namespace AppView.Controls.Interfaces
{
    public interface ITabMenu : IPage
    {
        TabControl TabControl { get; }
    }
}
