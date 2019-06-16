using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxLib
{
    class BST<T> where T : IComparable<T>
    {
        Node root;

        public void Add(T item)
        {
            if (root == null)
            {
                root = new Node(item);
                return;
            }
            Node tmp = root;
            Node parent = null;

            while (tmp != null)
            {
                parent = tmp;
                if (item.CompareTo(tmp.value) < 0) //go left
                {
                    tmp = tmp.left;
                }
                else
                {
                    tmp = tmp.right;
                }
            }

            Node newNode = new Node(item);
            if (item.CompareTo(parent.value) < 0)
                parent.left = newNode;
            else
                parent.right = newNode;
        }

        public void PrintAll() => PrintAll(root);

        private void PrintAll(Node node)
        {
            if (node == null) return;
            PrintAll(node.left);
            Console.WriteLine(node.value);
            PrintAll(node.right);
        }

        public void InOrder(Action<T> action) => InOrder(root, action);
        private void InOrder(Node node, Action<T> action)
        {
            if (node == null) return;
            InOrder(node.left, action);
            action(node.value);
            InOrder(node.right, action);

        }

        public bool IsValueInTree(T value, out T value2) => IsValueInTree(root, value, out value2);

        private bool IsValueInTree(Node node, T value, out T value2)
        {
            Node temp = node;
            value2 = default(T);
            while (temp != null)
            {
                if (value.CompareTo(temp.value) > 0)
                    temp = temp.right;
                else if (value.CompareTo(temp.value) < 0)
                    temp = temp.left;
                else
                {
                    value2 = temp.value;
                    return true;
                }
            }
            return false;
        }
        public bool GetClosest(T value, out T value2) => GetClosest(root, value, out value2);

        private bool GetClosest(Node node, T value, out T value2)
        {
            Node temp = node;
            value2 = default(T);

            while (temp != null)
            {
                if (value.CompareTo(temp.value) > 0)
                {
                    if (temp.right == null && value.CompareTo(temp.value) > 0)
                        return false;

                    if (value.CompareTo(temp.value) < 0)
                    {
                        value2 = temp.value;
                        return true;
                    }

                    temp = temp.right;

                }
                if (value.CompareTo(temp.value) < 0)
                {
                    if (temp.left == null && value.CompareTo(temp.value) < 0)
                    {
                        value2 = temp.value;
                        return true;
                    }
                    temp = temp.left;

                }
                if (value.CompareTo(temp.value) == 0)
                {
                    value2 = temp.value;
                    return true;
                }
            }
            return false;
        }

        public bool DeleteItem(T value) => DeleteItem(root, value);

        private bool DeleteItem(Node node, T value)
        {
            Node temp = node;
            Node parent = null;

            while (temp != null)
            {
                if (value.CompareTo(temp.value) > 0)
                {
                    parent = temp;
                    temp = temp.right;
                }
                else if (value.CompareTo(temp.value) < 0)
                {
                    parent = temp;
                    temp = temp.left;
                }
                else//if (value.CompareTo(temp.value) == 0)
                {
                    if (temp.left == null && temp.right == null)
                        NoChildrenCheck(temp, parent, value);

                    else if (temp.left != null && temp.right == null)
                        LeftChildCheck(temp, parent, value);

                    else if (temp.right != null && temp.left == null)
                        RightChildCheck(temp, parent, value);

                    else// (temp.right != null && temp.left != null)
                        TwoChildrenCheck(temp, parent, value);

                    return true;
                }
            }
            return false;
        }

        private void NoChildrenCheck(Node temp, Node parent, T value) //No children to the leaf (with root check)
        {
            if (parent != null)
            {
                if (parent.left != null)
                    if (parent.left.value.CompareTo(value) == 0)
                    {
                        parent.left = null;
                        return;
                    }
                if (parent.right != null)
                    if (parent.right.value.CompareTo(value) == 0)
                    {
                        parent.right = null;
                        return;
                    }
            }
            root = null;
            return;
        }


        private void LeftChildCheck(Node temp, Node parent, T value) //No children to the leaf (with root check)
        {
            if (parent != null)
            {
                if (parent.left == temp)
                    parent.left = temp.left;
                else if (parent.right == temp)
                    parent.right = temp.left;
                temp = null;
                return;
            }
            root = root.left;
            return;
        }
        private void RightChildCheck(Node temp, Node parent, T value) //No children to the leaf (with root check)
        {
            if (parent != null)
            {
                if (parent.right == temp)
                    parent.right = temp.right;
                else if (parent.left == temp)
                    parent.left = temp.right;
                temp = null;
                return;
            }
            root = root.right;
            return;
        }

        private void TwoChildrenCheck(Node temp, Node parent, T value)
        {
            if (parent != null)
            {
                if (value.CompareTo(root.value) < 0) //left subtree
                {

                    while (temp.right != null)
                    {
                        temp = parent;
                        temp = temp.right;
                    }
                    if (temp.right == null)
                    {
                        parent.value = temp.value;
                        parent.right = temp.right;
                        temp = null;
                        return;
                    }
                }

                if (value.CompareTo(root.value) >= 0) //right subtree
                {
                    while (temp.left != null)
                    {
                        parent = temp;
                        temp = temp.left;
                    }
                    if (temp.left == null)
                    {
                        parent.value = temp.value;
                        parent.left = temp.left;
                        temp = null;
                        return;
                    }
                }
            }

            parent = temp;
            temp = temp.right;

            while (temp.left != null)
            {
                parent = temp;
                temp = temp.left;
                if (temp.left == null)
                {
                    parent.left = temp.right;
                    root.value = temp.value;
                    return;
                }
            }
            root.value = temp.value;
            parent.right = temp.left;
            return;
        }
        public int GetDepth() => GetDepth(root);

        private int GetDepth(Node node)
        {
            if (node == null) return 0;

            return Math.Max(GetDepth(node.left), GetDepth(node.right)) + 1;
        }

        public int GetLeaves() => GetLeaves(root);

        private int GetLeaves(Node node)
        {
            if (node == null)
                return 0;
            if (node.left == null && node.right == null)
                return 1;

            return GetLeaves(node.right) + GetLeaves(node.left);
        }
        class Node
        {
            public T value;
            public Node right;
            public Node left;

            public Node(T value)
            {
                this.value = value;
            }
        }
    }
}
