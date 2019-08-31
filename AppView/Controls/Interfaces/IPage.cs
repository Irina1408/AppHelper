using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppView.CommonInterfaces;

namespace AppView.Controls.Interfaces
{
    public interface IPage : IHeader, IControl
    {
        string SpecificHeader { set; }
        EventHandler OnRefreshPage { get; set; }
        void Refresh();
    }
}
