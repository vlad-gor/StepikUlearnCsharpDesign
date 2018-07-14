using System;
using System.Collections;
using System.Collections.Generic;


namespace Generics.BinaryTrees
{
    public class BinaryTree<T> : IEnumerable<T>
        where T:IComparable<T>
    {
        private BinaryTree<T> left;
        public BinaryTree<T> Left
        {
            get {
                    if (left == null)
                    {
                        left = new BinaryTree<T>();
                    }
                    return left;
                }
            set { left = value; }
        }
        private BinaryTree<T> right;
        public BinaryTree<T> Right
        { 
            get
            {
                if (right == null)
                {
                    right = new BinaryTree<T>();
                }
                return right;
            }
            set { right = value; }
        }
        private BinaryTree<T> parent;
        public BinaryTree<T> Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        public T Value { get; set; }
        private bool hasValue = false;
        public bool HasValue { get { return hasValue; } set { hasValue = value; } }

		public BinaryTree()
		{
			this.hasValue = false; 
		}

        public void Add(T value)
        {
            if (HasValue == false)
            {
                this.Value = value;
                HasValue = true;
                Parent = null;
                return;
            }
            else
            {
                if (value.CompareTo(this.Value) == 1)
                {
                    Insert(value, Right, this);
                }
                else //if ((value.CompareTo(this.Value)1))
                {
                    Insert(value, Left, this);
                }
            }
        }

        private void Insert(T value, BinaryTree<T> currentNode, BinaryTree<T> parent)
        {
            if (currentNode.HasValue == false)
            {
                currentNode.Value = value;
                currentNode.Parent = parent;
                currentNode.HasValue = true;
                return;
            }

            int comparisonValue = value.CompareTo(currentNode.Value);
            if (comparisonValue == 1)
            {
                Insert(value, currentNode.Right, currentNode);
            }
            else if (comparisonValue == -1)
            {
                Insert(value, currentNode.Left, currentNode);
            }
            
        }
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public IEnumerator<T> GetEnumerator()
		{ 
			return new BinaryTreeEnumerator<T>(this);
		}
	}

    public static class BinaryTree
    {
        public static BinaryTree<int> Create(params int[] items)
        {
            BinaryTree<int> binaryTree = new BinaryTree<int>();
            for (int i = 0; i < items.Length; i++)
            {
                binaryTree.Add(items[i]);
            }
            return binaryTree;
        }
    }

	class BinaryTreeEnumerator<T> : IEnumerator<T>
   where T : IComparable<T>
	{
		private BinaryTree<T> OriginalTree { get; set; }
		private BinaryTree<T> CurrentNode { get; set; }
		object IEnumerator.Current => Current;
		public T Current => CurrentNode.Value;

		public BinaryTreeEnumerator(BinaryTree<T> node)
		{
			this.OriginalTree = node ?? throw new Exception();
			//this.CurrentNode = new BinaryTree<T>();
			this.CurrentNode = null;

		}

		public bool MoveNext()
		{
			// For the first entry, find the lowest valued node in the tree
			//if (!CurrentNode.HasValue)

			if (!OriginalTree.HasValue)
			{
				return false;
			}

			if (CurrentNode == null)
				CurrentNode = FindMostLeft(OriginalTree);
			else
			{
				// Can we go right-left?
				if (CurrentNode.Right.HasValue)
					CurrentNode = FindMostLeft(CurrentNode.Right);
				else
				{
					// Note the value we have found
					T CurrentValue = CurrentNode.Value;

					// Go up the tree until we find a value larger than the largest we have
					// already found (or if we reach the root of the tree)
					while (CurrentNode.HasValue)
					{
						CurrentNode = CurrentNode.Parent;
						if (CurrentNode != null)
						{
							int Compare = Current.CompareTo(CurrentValue);
							if (Compare < 0) continue;
						}
						break;
					}
				}
			}
			return (CurrentNode != null);
		}

		public void Reset()
		{
			CurrentNode = new BinaryTree<T>();
		}

		public void Dispose()
		{

		}

		BinaryTree<T> FindMostLeft(BinaryTree<T> start)
		{
			BinaryTree<T> node = start;
			while (true)
			{
				if (node.Left.HasValue)
				{
					node = node.Left;
					continue;
				}
				break;
			}
			return node;
		}
	}
}