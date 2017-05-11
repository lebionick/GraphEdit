using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphesLabWork1;
namespace GraphesLabWork3
{
    class Program
    {
        static void Main(string[] args)
        {
			var WG = new WGraph(@"../../input2.txt");
			WG.PrintToFile(@"../../output2.gv");
			var BG = new BucketDijkstra(WG);
			BG.GetTree(0).PrintToFile(@"../../skeleton2.gv");
        }
    }

class BucketDijkstra
{
	//исходный граф
	public WGraph G;
	//черпаки
	List<List<int>> Q;
	//хранит предка вершины
	List<int> Ancest;
	//максимальное значение веса
	int maxw;
	//число вершин
	int n;
	//список просмотренных
	List<int> Seen;


	public BucketDijkstra(WGraph wg)
	{
		G = new WGraph(wg);
		Ancest = new List<int>();
		Q = new List<List<int>>();
		//предполагается что все ребра имеют целое значение
		maxw = G._W.Max();
		n = G._H.Count;

		for (int i = 0; i < maxw + 1; i++)
			Q.Add(new List<int>());

		for (int i = 0; i < n; i++)
			Ancest.Add(-1);

		Seen = new List<int>();
	}

	int? findBucket(int vert)
	{
		for (int i = 0; i < Q.Count; i++)
			if (Q[i].Contains(vert))
				return i;
		return null;
	}

	void changeBucket(int vert, int from, int to)
	{
		Q[from].Remove(vert);
		Q[to].Add(vert);
	}
	//returns minimal tree
	public WGraph GetTree(int root)
	{
		Q[0].Add(root);
		while (!QisEmpty())
		{
			//для каждой вершины 0го черпака
			for (int j = 0; j < Q[0].Count; j++)
			{
				int v = Q[0][j];
				//посмотреть смежные вершины
				List<int> vert = G.ViewVertex(v);
				//для каждой смежной вершины
				for (int k = 0; k < vert.Count; k++)
				{
					int curv = vert[k];
					//если просмотрена - не трогаем
					if (Seen.Contains(curv))
						continue;

					int? bucketNum = findBucket(curv);
					int curDist = G.GetWeight(v, curv);

					if (bucketNum == null)
					{
						Q[curDist].Add(curv);
						Ancest[curv] = v;
					}
					else if (curDist < bucketNum)
					{
						Ancest[curv] = v;
						changeBucket(curv, (int)bucketNum, curDist);
					}
				}
				Seen.Add(Q[0][j]);
			}
			//смещаем очереди
			Q.RemoveAt(0);
			Q.Add(new List<int>());
		}
		//делаем граф из списка предков
		List<int> I = new List<int>();
		List<int> J = new List<int>();
		List<int> W = new List<int>();
		for (int k = 0; k < Ancest.Count; k++)
		{
			if (k == root)
				continue;
			if (Ancest[k] == -1)
				continue;
			J.Add(k);
			I.Add(Ancest[k]);
			W.Add(G.GetWeight(k, Ancest[k]));
		}
		return new WGraph(new Graph(I, J), W);
	}

	bool QisEmpty()
	{
		foreach (var q in Q)
			if (q.Count > 0)
				return false;
		return true;
	}

}
}
