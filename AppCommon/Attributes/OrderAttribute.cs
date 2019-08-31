using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCommon.Attributes
{
    /// <summary>
    /// Property order
    /// </summary>
    public class OrderAttribute : Attribute
    {
        public static readonly int DefaultOrder = int.MaxValue;   // int.MaxValue

        public OrderAttribute(int order)
        {
            Order = order;
        }

        public int Order { get; private set; }
    }
}
