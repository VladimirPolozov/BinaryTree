using System;
using System.Security.Policy;

namespace BinaryTree
{
    public class BinaryTree<T> where T : IComparable
    {
        public enum Side
        {
            Left,
            Right
        }

        public class Node
        {
            public T Value { get; set; }
            public Node Left { get; set; }
            public Node Right { get; set; }
            public Node ParentNode { get; set; }
            public Node LeftNode { get; set; }
            public Node RightNode { get; set; }
            public Side? NodeSide
            {
                get
                {
                    if (ParentNode == null)
                    {
                        return null;
                    } else if (ParentNode.LeftNode == this)
                    {
                        return Side.Left;
                    } else
                    {
                        return Side.Right;
                    }
                }
            }

            public Node(T value)
            {
                Value = value;
            }

            public override string ToString()
            {
                return Value.ToString();
            }
        }

        public Node RootNode { get; set; }

        public Node Add(T value)
        {
            Node newNode = new Node(value);
            if (RootNode == null)
            {
                RootNode = newNode;
            } else
            {
                AddRecursively(newNode, RootNode);
            }
            return newNode;
        }

        private Node AddRecursively(Node newNode, Node currentNode)
        {
            int comparisonResult = newNode.Value.CompareTo(currentNode.Value);
            if (comparisonResult < 0)
            {
                if (currentNode.Left == null)
                {
                    currentNode.Left = newNode;
                    newNode.ParentNode = currentNode;
                } else
                {
                    return AddRecursively(newNode, currentNode.Left);
                }
            } else if (comparisonResult > 0)
            {
                if (currentNode.Right == null)
                {
                    currentNode.Right = newNode;
                    newNode.ParentNode = currentNode;
                } else
                {
                    return AddRecursively(newNode, currentNode.Right);
                }
            } else
            {
                throw new InvalidOperationException("Узел с таким значением уже существует");
            }
            return newNode;
        }

        public void PrintTree()
        {
            PrintTree(RootNode);
        }

        private void PrintTree(Node startNode, string indent = "", Side? side = null)
        {
            if (startNode != null)
            {
                string nodeSide;
                switch (side)
                {
                    case null:
                        nodeSide = "+";
                        break;
                    case Side.Left:
                        nodeSide = "L";
                        break;
                    default:
                        nodeSide = "R";
                        break;
                }

                Console.WriteLine($"{indent} [{nodeSide}] - {startNode.Value}");
                indent += "   ";
                // Рекурсивный вызов для левой и правой ветвей
                PrintTree(startNode.Left, indent, Side.Left);
                PrintTree(startNode.Right, indent, Side.Right);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var binaryTree = new BinaryTree<int>();

            binaryTree.Add(8);
            binaryTree.Add(3);
            binaryTree.Add(10);
            binaryTree.Add(1);
            binaryTree.Add(6);
            binaryTree.Add(4);
            binaryTree.Add(7);
            binaryTree.Add(14);
            binaryTree.Add(16);

            binaryTree.PrintTree();

            Console.ReadLine();
        }
    }
}
