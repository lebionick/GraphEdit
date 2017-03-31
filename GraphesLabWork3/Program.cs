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
			//BucketGraph WG = new BucketGraph(@"../../input.txt");
			var WG = new WGraph(@"../../input.txt");
			//WG.PrintToFile(@"../../output.gv");
			var BG = new BucketGraph(WG);
        }
    }
    class BucketGraph
    {
        WGraph G;
        List<List<int>> Q;

		public BucketGraph(WGraph wg)
        {
			G = wg;
            Q = new List<List<int>>();
			int n = graphRange(G);

            for (int i = 0; i < n + 1 ; i++)
                Q.Add(new List<int>());
        }

        public WGraph GetSkeleton(int first)
        {
            List<int> I = new List<int>();
            List<int> J = new List<int>();
            List<int> W = new List<int>();
            List<int> proven = new List<int>();
            Q[0].Add(first);
            //для каждого черпака
            for (int i = 0; i < Q.Count(); i++)
            {
                //для каждой вершины черпака
                for (int j = 0; j < Q[i].Count; j++)
                {
                    List<int> vert = G.ViewVertex(j);
                    //для каждой смежной вершины
                    for (int k = 0; k < vert.Count; k++)
                    {
                        //let ostov = graph
                        //добавить в соотв черпак соотв вершину
                        //if proven vertexes include this vert
                        //or this vert is included in buckets <= соотв черпак
                        //then ostov del this arc
                        //else
                        Q[mapVert(k)].Add(vert[k]);
                        //ostov add this arc
                    }
                }
                //смотрим только один черпак и удаляем просмотренный, добавляем в конец пустой
                //когда все пусто - останавливаемся
            }
            return new WGraph(new Graph(I, J), W);
        }
        int mapVert(int vert)
        {
            return vert;
        }

        int graphRange(WGraph g)
        {
            var different = new List<int>();

			foreach (int weight in g._W)
            {
                if (!different.Contains(weight))
                {
                    different.Add(weight);
                }
            }

            return different.Count;
        }
    }
}
