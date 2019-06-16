using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxLib
{
    public class Organizer
    {
        private BoxManager boxManager;

        public Organizer(Params _params)
        {
            boxManager = new BoxManager(_params);
        }

        public int GetCapacity()
        {
            try
            {
                return boxManager.GetCapacity();
            }
            catch (Exception)
            {
                return -1;
            }
        }
        public int GetMinimumCapacity()
        {
            try
            {
                return boxManager.GetMinimumCapacity();
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public string AddBox(double x, double y, int Quantity, DateTime lastBought)
        {
            try
            {
                return boxManager.AddBox(x, y, Quantity, lastBought);
            }
            catch (Exception e)
            {
                string str = e.Message;
                return String.Format(str);
            }
        }

        public int? CheckQuantity(double x, double y)
        {
            try
            {
                return boxManager.CheckQuantity(x, y);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void LoadItems(string jsonName)
        {
            try
            {
                boxManager.LoadItems(jsonName);
            }

            catch (Exception){}
        }

        public string SellBox(double x, double y, int quantity)
        {
            try
            {
                return boxManager.SellBox(x, y, quantity);
            }
            catch (Exception)
            {
                return String.Format("Sale process failed! ");
            }
        }

        public IEnumerable<Details> MakeList()
        {
            try
            {
                return boxManager.MakeList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void SaveItems(string jsonName)
        {
            try
            {
                boxManager.SaveItems(jsonName);
            }

            catch (Exception) { }
        }

        public void CheckBoxesDates()
        {
            try
            {
                boxManager.CheckBoxesDates();
            }
            catch (Exception) { }
        }
    }
}
