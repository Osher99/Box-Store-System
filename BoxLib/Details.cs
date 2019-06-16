using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxLib
{
     public class Details 
    {
        public Details(double x, double y, int quantity, DateTime lastBought)
        {
            X = x;
            Y = y;
            Quantity = quantity;
            LastBought = lastBought;
        }

        public double X { get; private set; }
        public double Y { get; private set; }
        public int Quantity { get; internal set; }

        public DateTime LastBought { get; internal set; }

        public override string ToString()
        {
            return $"X: {X}\nY: {Y}\nQuantity: {Quantity}\nLast Bought Time: {LastBought}";
        }

    }
}
