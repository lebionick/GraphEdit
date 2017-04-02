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
			var BG = new BucketGraph(WG);
			BG.GetSkeleton(0).PrintToFile(@"../../skeleton2.gv");
        }
    }
    class BucketGraph
    {
        WGraph G;
		//черпаки
        List<List<int>> Q;
		//хранит путь до вершины
		List<int> Ancest;
		//максимальное значение веса
		int maxw;
		//число вершин
		int n;
		//список просмотренных
		List<int> Seen;

		public BucketGraph(WGraph wg)
        {
			G = wg;
			Ancest = new List<int>();
            Q = new List<List<int>>();
			//начало черпаков - с 1 или с мин?
			maxw = graphRange(wg).Max();
			n = G._H.Count;

            for (int i = 0; i < maxw + 1 ; i++)
                Q.Add(new List<int>());

			for (int i = 0; i < n; i++)
				Ancest.Add(-1);
			
			Seen = new List<int>();
        }

        public WGraph GetSkeleton(int root)
        {
			var Skel = new WGraph(G);

			Q[0].Add(root);
			Ancest[root] = root;
			while(!QisEmpty()){
                //для каждой вершины черпака
                for (int j = 0; j < Q[0].Count; j++)
                {
					int v = Q[0][j];
                    List<int> vert = G.ViewVertex(v);
					Console.WriteLine("вершина: {0}", v);
					foreach (var f in vert)
						Console.Write(f);
					Console.WriteLine();
					//для каждой смежной вершины
					for (int k = 0; k < vert.Count; k++)
					{
						//если просмотрена - не трогаем
						if (Seen.Contains(vert[k]))
							continue;
						//если вершина смотрится в первый раз
						if (Ancest[vert[k]] == -1)
						{
							bool has = false;
							for (int y = 0; y < Q.Count; y++)
								for (int u = 0; u < Q[y].Count; u++)
									if (Q[y][u] == vert[k])
									{
										has = true;
										break;
									}
							if(!has)
							{
								Q[G.getWeight(v, vert[k])].Add(vert[k]);
								Ancest[vert[k]] = v;
							}
						}
						else {
							//иначе - ищем эту вершину
							int curd = G.getWeight(v, vert[k]);
							for (int i = 0; i < Q.Count; i++)
							{
								for (int l = 0; l < Q[i].Count; l++)
								{
									if (Q[i][l] == vert[k])
									{
										//если улучшаем расстояние, то выкидываем прошлое ребро
										if (i>curd)
										{
											Console.WriteLine("добавляем ребро {0} в ряд {1}", vert[k], G.getWeight(v, vert[k]));
											Q[i].RemoveAt(l);
											Q[G.getWeight(v, vert[k])].Add(vert[k]);
											Skel.Remove(Ancest[vert[k]], vert[k]);
											Ancest[vert[k]] = v;
										}
										//если ухудшаем, то выкидываем текущее
										else {
											Skel.Remove(v, vert[k]);
										}
									}
								}
							}
						}
					}
					Seen.Add(Q[0][j]);
                }
				//Seen.AddRange(Q[0]);

				Q.RemoveAt(0);
				Q.Add(new List<int>());
				Console.WriteLine("iter");
            }
			return Skel;
        }
		bool QisEmpty()
		{
			foreach (var q in Q)
				if (q.Count > 0)
					return false;
			return true;
		}

        List<int> graphRange(WGraph g)
        {
            var different = new List<int>();

			foreach (int weight in g._W)
            {
                if (!different.Contains(weight))
                {
                    different.Add(weight);
                }
            }

			return different;
        }
    }
}
