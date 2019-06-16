using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxLib
{
    class XBox : IComparable<XBox>
    {

        public XBox(double x, bool isDummy = true)
        {
            X = x;

            if (!isDummy)
                YboxBinaryTree = new BST<YBox>();
        }

        public double X { get; set; }
        public BST<YBox> YboxBinaryTree { get; set; }

        public int CompareTo(XBox obj)
        {
            return X.CompareTo(obj.X);
        }
    }
}
