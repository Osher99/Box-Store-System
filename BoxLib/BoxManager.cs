using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Threading;

namespace BoxLib
{
    public class BoxManager
    {
        private int maxCapacity, minCapacity;
        private BST<XBox> bst;
        private ListLinked<Details> ListLinkedInfo;
        DispatcherTimer timer;
        TimeSpan expireDate;
        Params _params;

        public BoxManager(Params _params)
        {
            bst = new BST<XBox>();
            ListLinkedInfo = new ListLinked<Details>();
            this._params = _params;
            maxCapacity = _params.MaxCapacity;
            minCapacity = _params.MinCapacity;
            expireDate = _params.ExpirationDate;
            CheckBoxesDates();
            timer = new DispatcherTimer();
            timer.Interval = _params.TimerInterval;
            timer.Tick += new EventHandler(TimerOnElapsed);
            timer.Start();
        }

        public string AddBox(double x, double y, int quantity, DateTime lastBought) 
        {
            XBox NodeX = null;
            YBox NodeY = null;
            string messageOfMaxCap = String.Empty;
            if (quantity > maxCapacity)
            {
                messageOfMaxCap = $"The quantity entered is bigger then our capacity!\n Total boxes entered: {maxCapacity}.\nPlease take back {NodeY.ListNodeRef.value.Quantity - quantity} boxes";
                quantity = maxCapacity;
            }

            if (bst.IsValueInTree(new XBox(x), out NodeX)) // check if X Exist
            {
                if (NodeX.YboxBinaryTree.IsValueInTree(new YBox(y), out NodeY)) //  Update count of Y (X Is Exist)
                {
                    if (NodeY.ListNodeRef.value.Quantity + quantity >= maxCapacity)
                    {
                        int calculation = NodeY.ListNodeRef.value.Quantity - quantity;
                        NodeY.ListNodeRef.value.Quantity = maxCapacity;
                        return String.Format($"The capacity of the item chosen is now at maximum {maxCapacity}! \nplease take back {calculation} Boxes");
                    }
                    else
                    {
                        NodeY.ListNodeRef.value.Quantity += quantity;
                        return String.Format($"Quantity has been added successfully! \nThe capacity of the item chosen is now at {NodeY.ListNodeRef.value.Quantity} Boxes");
                    }
                }
                else // No Y Scenrio (X Is Exist)
                {
                    ListLinkedInfo.AddLast(new Details(x, y, quantity, lastBought));
                    var newY = new YBox(y, ListLinkedInfo.Last);
                    NodeX.YboxBinaryTree.Add(newY);
                    return String.Format($"Item has been added successfully! " + messageOfMaxCap);
                }
            }

            else // No X Scenrio
            {
                var newX = new XBox(x, false);
                bst.Add(newX);
                ListLinkedInfo.AddLast(new Details(x, y, quantity, lastBought));
                var newY = new YBox(y, ListLinkedInfo.Last);
                newX.YboxBinaryTree.Add(newY);
                return String.Format($"Item has been added successfully!" + messageOfMaxCap);
            }
        }

        public int? CheckQuantity(double x, double y)
        {
            XBox NodeX = null;
            YBox NodeY = null;

            if (bst.IsValueInTree(new XBox(x), out NodeX)) // check if X Exist
            {
                if (NodeX.YboxBinaryTree.IsValueInTree(new YBox(y), out NodeY)) // Box exist with the same match
                    return NodeY.ListNodeRef.value.Quantity;
                else if (NodeX.YboxBinaryTree.GetClosest(new YBox(y), out NodeY)) // Box with slightly differnet Y 
                    return NodeY.ListNodeRef.value.Quantity;
            }
            return null;
        }

        public string SellBox(double x, double y, int quantity = 1)
        {
            XBox NodeX = null;
            YBox NodeY = null;

            if (bst.IsValueInTree(new XBox(x), out NodeX)) // check if X Exist
            {
                if (NodeX.YboxBinaryTree.IsValueInTree(new YBox(y), out NodeY)) // Box exist with the same match
                {
                    string detailsOfSale;
                    if (NodeY.ListNodeRef.value.Quantity - quantity <= 0)
                        detailsOfSale = $"You took the maximum number of boxes there is to take {NodeY.ListNodeRef.value.Quantity},\n Quantity now at 0";
                    else
                        detailsOfSale = $"You took {quantity} of boxes Quantity now at {NodeY.ListNodeRef.value.Quantity - quantity}";
                    RemoveItem(NodeX, NodeY, quantity);
                    return $"Exact Box with Y size box has been sold!\n X Size: {NodeX.X} Y Size: {NodeY.Y},\n" + detailsOfSale;
                }
                else if (NodeX.YboxBinaryTree.GetClosest(new YBox(y), out NodeY)) // Box exist with the Y slightly different
                {
                    string detailsOfSale;
                    if (NodeY.ListNodeRef.value.Quantity - quantity <= 0)
                        detailsOfSale = $"You took the maximum number of boxes there is to take {NodeY.ListNodeRef.value.Quantity},\n Quantity now at 0";
                    else
                        detailsOfSale = $"You took {quantity} of boxes Quantity now at {NodeY.ListNodeRef.value.Quantity - quantity}";
                    RemoveItem(NodeX, NodeY, quantity);
                    return $"Slightly different with Y size box has been sold!\n X Size: {NodeX.X} Y Size: {NodeY.Y},\n"+ detailsOfSale;
                }
                return "X has been found but system could not find an Y that matches request!";

            }
            return "No box found! X size isn't at our werehouse!";
        }

        private void RemoveItem(XBox NodeX, YBox NodeY, int quantity = 1)
        {
            NodeY.ListNodeRef.value.Quantity -= quantity;
            NodeY.ListNodeRef.value.LastBought = DateTime.Now;
            ListLinkedInfo.UpdatePosition(NodeY.ListNodeRef);

            if (NodeY.ListNodeRef.value.Quantity <= 0)
            {
                NodeX.YboxBinaryTree.DeleteItem(NodeY);
                ListLinkedInfo.RemoveLast(out Details detailsToRemove);
            }

            if (NodeX.YboxBinaryTree.GetDepth() == 0)
                bst.DeleteItem(NodeX);
        }

        public void CheckBoxesDates()
        {
            XBox NodeX = null;
            YBox NodeY = null;

            foreach (var item in ListLinkedInfo)
            {
                if (item.LastBought < (DateTime.Now - expireDate))
                {
                    bst.IsValueInTree(new XBox(item.X), out NodeX);
                    NodeX.YboxBinaryTree.IsValueInTree(new YBox(item.Y), out NodeY);
                    ListLinkedInfo.RemoveFirst(out Details detailsToRemove);
                    NodeY.ListNodeRef = null;
                    NodeX.YboxBinaryTree.DeleteItem(NodeY);
                    if (NodeX.YboxBinaryTree.GetDepth() == 0)
                        bst.DeleteItem(NodeX);
                }
                else
                    break;
            }
        }

        public IEnumerable<Details> MakeList()
        {
            return ListLinkedInfo;
        }

        public void AddCollection(List<Details> other)
        {
            if (other == null)
                return;
            if (!other.Any())
                return;
            foreach (var item in other)
            {
                if (item != null && item.Quantity > 0)
                {
                    AddBox(item.X, item.Y, item.Quantity, item.LastBought);
                }
            }
        }
        
        public void SaveItems(string boxesJsonFilename)
        {
            var Boxes = ListLinkedInfo;
            var jsonedBoxes = JsonConvert.SerializeObject(Boxes);
            File.WriteAllText(boxesJsonFilename, jsonedBoxes, Encoding.UTF8);
        }

        public void LoadItems(string boxesJsonFilename)
        {
            if (File.Exists(boxesJsonFilename))
            {
                var jsonedBoxes = File.ReadAllText(boxesJsonFilename, Encoding.UTF8);
                var boxes = JsonConvert.DeserializeObject<List<Details>>(jsonedBoxes);
                AddCollection(boxes);
            }      
        }

        public int GetCapacity()
        {
            return maxCapacity;
        }

        public int GetMinimumCapacity()
        {
            return minCapacity;
        }

        private void TimerOnElapsed(object sender, EventArgs e)
        {
            CheckBoxesDates();
        }
    }
}

