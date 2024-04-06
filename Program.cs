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
            private Stack<Node> stack = new Stack<Node>();
            private Node current;

            public BinaryTreeEnumerator(Node root)
            {
                current = root;
                PushLeftNodes();
            }

            private void PushLeftNodes()
            {
                while (current != null)
                {
                    stack.Push(current);
                    current = current.Left;
                }
            }

            public T Current
            {
                get
                {
                    if (stack.Count == 0)
                        return default; // Возвращаем значение по умолчанию, если стек пуст
                    return stack.Peek().Value;
                }
            }
            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                if (stack.Count == 0)
                    return false;

                current = stack.Pop();
                Node node = current.Right;
                while (node != null)
                {
                    stack.Push(node);
                    node = node.Left;
                }
                return true;
            }

            public void Reset()
            {
                throw new NotImplementedException();
            }

            public void Dispose() { }
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

            Console.WriteLine("Двоичное дерево:");
            while (true)
            {
                Console.WriteLine("\nВыберите действие:");
                Console.WriteLine("1. Добавить новый узел");
                Console.WriteLine("2. Вывести дерево прямым обходом");
                Console.WriteLine("3. Вывести дерево обратным обходом");
                Console.WriteLine("4. Вывести дерево центральным обходом");
                Console.WriteLine("0. Выйти");

                Console.Write("Введите номер действия: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Введите значение нового узла: ");
                        if (int.TryParse(Console.ReadLine(), out int value))
                        {
                            binaryTree.Add(value);
                            Console.WriteLine("Новый узел успешно добавлен.");
                        } else
                        {
                            Console.WriteLine("Ошибка при вводе значения узла.");
                        }
                        break;
                    case "2":
                        Console.WriteLine("Прямой обход дерева:");
                        foreach (var item in binaryTree)
                        {
                            Console.Write(item + " ");
                        }
                        Console.WriteLine();
                        break;
                    case "3":
                        Console.WriteLine("Обратный обход дерева:");
                        BinaryTreeReverseTraversal(binaryTree.RootNode);
                        Console.WriteLine();
                        break;
                    case "4":
                        Console.WriteLine("Центральный обход дерева:");
                        BinaryTreeCentralTraversal(binaryTree.RootNode);
                        Console.WriteLine();
                        break;
                    case "0":
                        Console.WriteLine("Выход из программы.");
                        return;
                    default:
                        Console.WriteLine("Неверный выбор.");
                        break;
                }
            }
        }

        static void BinaryTreeReverseTraversal<T>(BinaryTree<T>.Node node) where T : IComparable
        {
            if (node != null)
            {
                BinaryTreeReverseTraversal(node.Right);
                Console.Write(node.Value + " ");
                BinaryTreeReverseTraversal(node.Left);
            }
        }

        static void BinaryTreeCentralTraversal<T>(BinaryTree<T>.Node node) where T : IComparable
        {
            if (node != null)
            {
                BinaryTreeCentralTraversal(node.Left);
                Console.Write(node.Value + " ");
                BinaryTreeCentralTraversal(node.Right);
            }
        }
    }
}
