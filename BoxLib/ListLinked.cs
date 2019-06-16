using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxLib
{
    internal class ListLinked<T> : IEnumerable<T>
    {
        public Node Last { get; private set; }

        public Node First { get; private set; }

        public void AddFirst(T val)
        {
            Node tmp = new Node(val);

            if (First == null)
                First = Last = tmp;
            else
            {
                tmp.next = First;
                First.previous = tmp;
                First = tmp;
            }
        }
        public int GetLength() => GetLength(0, First);
        private int GetLength(int counter, Node temp)
        {
            if (temp == null)
                return counter;

            return GetLength(counter + 1, temp.next);

        }

        public void AddLast(T val)
        {
            Node tmp = new Node(val);

            if (First == null)    // if the link is empty make first and last = tmp;
                First = Last = tmp;
            else
            {
                Last.next = tmp;       // connecting the last's next with tmp ;     (connect tmp with list)
                tmp.previous = Last;   // connection the tmp's previous with last;
                Last = tmp;      // making the last tmp;
            }
        }


        public void AddLast(Node val)
        {
            Node tmp =val;

            if (First == null)    // if the link is empty make first and last = tmp;
                First = Last = tmp;
            else
            {
                Last.next = tmp;       // connecting the last's next with tmp ;     (connect tmp with list)
                tmp.previous = Last;   // connection the tmp's previous with last;
                Last = tmp;      // making the last tmp;
            }
        }


        public bool RemoveFirst(out T item)
        {
            item = default(T);

            if (First == null)   // if list empty
                return false;
            // else
            item = First.value;

            if (Last == First)  // if the list is 1 item
            {
                First = Last = null;    // delete the item
                return true;
            }
            // else 
            First = First.next;       // first = first.next
            First.previous = null;      //first.privous = null
            return true;
        }

        public bool RemoveFirst(out Node item)
        {
            item = null;

            if (First == null)   // if list empty
                return false;
            // else
            item = First;

            if (Last == First)  // if the list is 1 item
            {
                First = Last = null;    // delete the item
                return true;
            }
            // else 
            First = First.next;       // first = first.next
            First.previous = null;      //first.privous = null
            return true;
        }



        public bool RemoveLast(out T item)
        {
            item = default(T);

            if (Last == null) // if list empty
                return false;
            // else
            item = Last.value;

            if (Last == First)     // if the list is 1 item
            {
                First = Last = null;  //  // delete the item
                return true;
            }
            // else 
            Last = Last.previous;
            Last.next = null;
            return true;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<T> GetEnumerator()
        {
            Node tmp = First;
            while (tmp != null)
            {
                yield return tmp.value;
                tmp = tmp.next;
            }
        }
        public void ReverseListWithPrev()
        {
            if (First == null)
                return;

            Node head = First;
            Node headNext = null;

            while (head != null)
            {
                headNext = head.next;
                head.next = head.previous;
                head.previous = headNext;
                head = headNext;
            }
            head = First;
            First = Last;
            Last = head;

        }
      

        public void ReverseList()
        {
            if (First == null)
                return;

            Node current, prev, next;
            prev = null;
            current = First;
            Node temp;

            while (current != null)
            {
                next = current.next;
                current.next = prev;
                prev = current;
                current = next;
            }
            temp = First;
            First = Last;
            Last = temp;
            Last.next = null;
        }

        //public void PutInPlace(T item)
        //{
        //    if (first == null || first.value.CompareTo(item) <= 0)
        //    {
        //        AddFirst(item);
        //        return;
        //    }
        //    Node node = new Node(item);

        //    Node temp = first;
        //    Node newN = null;
        //    while (temp != null)
        //    {
        //        if (temp.value.CompareTo(node.value) < 0 && temp.next.value.CompareTo(node.value) >= 0)
        //        {
        //            newN = temp.next;
        //            temp.next = node;
        //            node.next = newN;
        //            return;
        //        }
        //        temp = temp.next;
        //    }
        //    last.next = node;
        //}

        public string PrintAll()
        {
            StringBuilder sb = new StringBuilder();
            Node temp = First;
            while (temp != null)
            {
                sb.AppendLine(temp.value.ToString());
                temp = temp.next;
            }
            return sb.ToString();
        }
        public void UpdatePosition(Node refToNode)
        {
            if (First == null || First.next == null || refToNode == Last)
                return;
            if (refToNode == First)
            {
                Node tmp;
                RemoveFirst(out tmp);
                tmp.next = null;
                AddLast(tmp);                          
                return;
            }
            refToNode.next.previous = refToNode.previous;
            refToNode.previous.next = refToNode.next;
            refToNode.next = null;
            refToNode.previous = null;
            AddLast(refToNode);
        }
        //public T GetHighestNumber(ListLinked<T> list2)
        //{
        //    T highest = default(T);
        //    Node temp, temp2;

        //    temp = first;
        //    temp2 = list2.first;


        //    while (temp != null)
        //    {
        //        while (temp2 != null)
        //        {
        //            if (temp.value.CompareTo(temp2.value) == 0)
        //                if (highest != null)
        //                    if (temp.value.CompareTo(highest) > 0)
        //                        highest = temp.value;
        //            temp2 = temp2.next;
        //        }
        //        temp2 = list2.first;
        //        temp = temp.next;
        //    }
        //    return highest;
        //}
        //public ListLinked<T> GetNumbersNotAtList(ListLinked<T> list2)
        //{
        //    ListLinked<T> newList = new ListLinked<T>();
        //    Node temp, temp2, temp3;

        //    temp = first;
        //    temp2 = list2.first;
        //    temp3 = newList.first;
        //    while (temp != null)
        //    {
        //        while (temp2 != null)
        //        {
        //            if (temp.value.CompareTo(temp2.value) != 0)

        //                temp2 = temp2.next;
        //        }
        //        temp2 = list2.first;
        //        temp = temp.next;
        //    }
        //    return newList;
        //}

        internal class Node
        {
            public T value;
            public Node next;
            public Node previous;
            public Node(T value)
            {
                this.value = value;
            }

        }
    }
}
