using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphesLabWork1;

namespace GraphesLabWork2
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			var WG = new WGraph(@"../../input.txt");
			WG.PrintToFile(@"../../output.gv");
			var KK = new Kraskal(WG);
			KK.GetTree().PrintToFile(@"../../tree.gv");
		}
	}
	class Kraskal
	{
		WGraph WG;
		int[] M;
		int[] id;
		int[] size;

		delegate int finder(int i);
		delegate void unioner(int x, int y);
		finder find;
		unioner union;

		public Kraskal(WGraph wg)
		{
			WG = new WGraph(wg);
			//naiveMode();
			logcompMode(wg._H.Count);
		}

		public WGraph GetTree()
		{
			var I = new List<int>();
			var J = new List<int>();
			var W = new List<int>();
			M = new int[WG._H.Count];
			for (int i = 0; i < M.Length; i++)
				M[i] = i;
			
			var sEdges = getTransp(WG._W);

			for (int i = 0; i < sEdges.Length; i++)
			{
				int ednum = sEdges[i];
				int from = WG._I[ednum];
				int to = WG._J[ednum];

				//Console.WriteLine("ребро номер {0}\tиз {1} в {2}",ednum,from,to);
				//Console.WriteLine("{0}\t{1}",M[from],M[to]);
				if (!makesCycle(from, to))
				{
					//Console.WriteLine("добавить из {0} в {1}", WG._I[ednum], WG._J[ednum]);
					I.Add(WG._I[ednum]);
					J.Add(WG._J[ednum]);
					W.Add(WG._W[ednum]);
				}
			}
			return new WGraph(new Graph(I, J), W);
		}

		bool makesCycle(int from, int to)
		{
			int ini = find(from);
			int eve = find(to);
			if (ini == eve)
				return true;
			else {
				union(ini, eve);
				return false;
			}
		}

		void logcompMode(int n)
		{
			id = new int[n];
			size = new int[n];
			for (int i = 0; i < n; i++)
			{
				id[i] = i;
				size[i] = 1;
			}
			find = logFind;
			union = logUnion;
		}

		void logUnion(int x, int y)
		{
			if (size[x] < size[y])
			{
				id[x] = y;
				size[y] += size[x];
			}
			else {
				id[y] = x;
				size[x] += size[y];
			}
		}

		int logFind(int i)
		{
			if (id[i] == i)
				return i;
			id[i] = logFind(id[i]);
			return id[i];
		}

		void naiveMode()
		{
			find = naiveFind;
			union = naiveUnion;
		}

		int naiveFind(int i)
		{
			return M[i];
		}

		void naiveUnion(int l, int k)
		{
			for (int i = 0; i < M.Length; i++)
			{
				if (M[i] == l)
					M[i]=k;
				Console.Write(M[i]);
			}
			Console.WriteLine();
		}

		int[] getTransp(List<int> W)
		{
			var V = new int[W.Count];
			var K = new int[W.Count];
			W.CopyTo(K);
			for (int i = 0; i < W.Count; i++)
				V[i] = i;
			Array.Sort(K,V);
			return V;
		}
	}
}
