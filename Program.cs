using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using System.Threading.Tasks;

namespace BinaryTree
{
    public class BinaryTree<T> : IEnumerable<T>  where T : IComparable
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

        public IEnumerator<T> GetEnumerator()
        {
            return new BinaryTreeEnumerator(RootNode);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class BinaryTreeEnumerator : IEnumerator<T>
        {
            private List<Node> nodes = new List<Node>();
            private int index;

            public BinaryTreeEnumerator(Node root)
            {
                PushLeftNodes(root);
            }

            private void PushLeftNodes(Node node)
            {
                while (node != null)
                {
                    nodes.Add(node);
                    node = node.Left;
                }
            }

            public T Current => nodes[index - 1].Value;

            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                if (index == 0)
                    return false;

                Node node = nodes[--index];
                PushLeftNodes(node.Right);
                return true;
            }

            public bool MovePrevious()
            {
                if (index == nodes.Count)
                    return false;

                Node node = nodes[++index];
                PushLeftNodes(node.Left);
                return true;
            }

            public static BinaryTreeEnumerator operator ++(BinaryTreeEnumerator enumerator)
            {
                enumerator.MoveNext();
                return enumerator;
            }

            public static BinaryTreeEnumerator operator --(BinaryTreeEnumerator enumerator)
            {
                enumerator.MovePrevious();
                return enumerator;
            }

            public void Dispose() {}

            public void Reset() {}
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

            Console.WriteLine("Прямой обход дерева:");
            foreach (var item in binaryTree)
            {
                Console.Write(item + " ");
            }
            Console.WriteLine();

            Console.ReadLine();
        }
    }
}
