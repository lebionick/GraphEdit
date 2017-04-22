using System;
using System.Collections.Generic;

namespace GraphesLabWork4
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			var L = new List<int>{ 11, 12, 56, 88, 34, 0, 3, 77, 55, 13, 21, 6 };
			var H = new Heap<int>(L, (int x, int y) => x.CompareTo(y), HeapMode.Descending);
			//Console.Write("\n{0}", H.Find(11, 0));
			H.Remove(H.Find(11, 0));
			H.Remove(H.Find(77, 0));
			H.PrintToConsole();
			//H.Remove(7);
			var I = new List<int>();
			var J = new List<int>();

			for (int i = 0; i < H.array.Length; i++)
			{
				int ch1 = 2 * i + 1;
				int ch2 = ch1 + 1;
				if (ch2 >= H.array.Length)
				{
					if (ch1 >= H.array.Length)
						break;
					I.Add(H.array[i]);
					J.Add(H.array[ch1]);
					break;
				}
				I.Add(H.array[i]);
				J.Add(H.array[ch1]);
				I.Add(H.array[i]);
				J.Add(H.array[ch2]);	
			}

			var G = new GraphesLabWork1.Graph(I, J);
			G.PrintToFile(@"../../output.gv");
		}
	}

	class Heap<T>
	{
		public T[] array;
		protected Comparison<T> comp;

		public delegate void Op0(int i0);
		public delegate void Heaper();
		public delegate void Changer(int i0, T val);
		public delegate int Finder(T a0, int i0);
		public Op0 RepairToLeaves, RepairToRoot;
		public Changer Change;
		public Finder Find;
		public Heaper Heapify;

		protected HeapMode mode;
		public HeapMode Mode
		{
			get
			{
				return mode;
			}
			set
			{
				mode = value;
				switch (mode)
				{
					case HeapMode.Descending: 
						{
							RepairToRoot = DescRepairToRoot;
							RepairToLeaves = DescRepairToLeaves;
							Heapify = DescMakeHeap;
                            Heapify();
							break;
						}
					case HeapMode.Ascending:
						{
							RepairToRoot = AscRepairExternal;
							RepairToLeaves = AscRepairInner;
							Heapify = AscHeapify;
							Heapify();
							break;
						}
				}
			}
		}

		public Heap(IList<T> list, Comparison<T> c, HeapMode m)
		{
			array = new T[list.Count];
			for (int i = 0; i < list.Count; i++)
				array[i] = list[i];
			comp = c;
			Mode = m;

			Find = (a0, i0) =>
			{
				int child1 = i0 * 2 + 1;
				int child2 = child1 + 1;
				if (child2 >= array.Length)
				{
					if (child1 >= array.Length)
						return -1;
					if (comp(array[child1], a0) == 0)
						return child1;
					return -1;
				}
				if (comp(array[child1], a0) > 0 && comp(array[child1], a0) > 0)
					return -1;
				if (comp(array[child1], a0) == 0)
					return child1;
				int f1 = Find(a0, child1);
				if (f1 != -1)
					return f1;
				
				if (comp(array[child2], a0) == 0)
					return child2;
				f1 = Find(a0, child2);
				if (f1 != -1)
					return f1;
				return -1;
			};
			Change = (i0, val) =>
			{
				if (comp(array[i0], val) > 0)
				{
					array[i0] = val;
					RepairToRoot(i0);
				}
				if (comp(array[i0], val) < 0)
				{
					array[i0] = val;
					RepairToLeaves(i0);
				}
			};

		}

		public void Add(T i0)
		{
			var newarray = new T[array.Length + 1];
			array.CopyTo(newarray, 0);
			newarray[array.Length] = i0;
			array = newarray;
			RepairToRoot(array.Length);
		}

		public void Remove(int i0)
		{
			if (i0 == -1 || i0 >= array.Length)
				return;
			var newarray = new T[array.Length - 1];
			swap(i0, array.Length - 1);
			for (int i = 0; i<array.Length - 1; i++)
			{
				newarray[i] = array[i];
			}
			array = newarray;
			RepairToLeaves(i0);
		}

		public T Pick(int i0)
		{
			T element = array[i0];
			Remove(i0);
			return element;
		}

		public void PrintToConsole()
		{
			for (int i = 0; i<array.Length; i++)
				Console.Write("{0}\t", array[i]);
		}

		void DescRepairToRoot(int i0)
		{
			int anch;
			for (int i = i0; i > 0; i=anch)
			{
				anch = (i - 1) / 2;
				if (comp(array[anch], array[i]) < 0)
					swap(anch, i);
				else
					break;
			}
		}

		void DescRepairToLeaves(int i0)
		{
			for (int i = i0;i<array.Length/2;)
			{
				int child1 = 2 * i + 1;
				int child2 = child1 + 1;

				if (child2 >= array.Length)
				{
					if (child1 >= array.Length)
						break;
					if(comp(array[i],array[child1])<0)
					swap(i, child1);
					break;
				}
				else
				{
					int maxchild = getmax(child1, child2);
  					if (comp(array[i], array[maxchild]) < 0)
					{
						swap(i, maxchild);
						i = maxchild;
					}
					else
						break;
				}
			}
		}

		int getmax(int i1, int i2)
		{
			if (comp(array[i1], array[i2]) > 0)
				return i1;
			else
				return i2;
		}

		void AscRepairExternal(int k0)
		{
			int k1;
			for (int k = k0; k < (array.Length - 1) / 2; k = k1)
			{
				k1 = 2 * k + 1;
				int k2 = k1 + 1;
				if ((k2 < array.Length) && (comp(array[k2],array[k1])<0))
					k1 = k2;
				if (comp(array[k],array[k1])<0)
				   break;
				swap(k, k1);
			}
		}

		void AscRepairInner(int k0)
		{
			int k1;
			for (int k = k0; k > 0; k = k1)
			{
				k1 = (k - 1) / 2; //anchestor of k
				if (comp(array[k1], array[k]) < 0)
					break;
				swap(k, k1);
			}
		}

		void DescMakeHeap()
		{
			for (int i = 0; i < array.Length; i++)
				DescRepairToRoot(i);
		}

		void AscHeapify()
		{
			for (int k = (array.Length - 1) / 2; k < array.Length; k++)
				AscRepairInner(k);
		}

		void swap(int i, int j)
		{
			var b = array[j];
			array[j] = array[i];
			array[i] = b;
		}
	}

	enum HeapMode
	{
		Ascending,
		Descending
	}
}
