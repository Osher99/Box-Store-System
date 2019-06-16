using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxLib
{
    class YBox: IComparable<YBox>
    {
        public YBox(double y, ListLinked<Details>.Node list = null)
        {
            Y = y;
            ListNodeRef = list;
        }

        public double Y { get; set; }
        public ListLinked<Details>.Node ListNodeRef { get; set; }
        
        public int CompareTo(YBox obj)
        {
            return obj.Y.CompareTo(Y);
        }
    }
}
