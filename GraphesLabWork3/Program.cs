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
            BucketGraph WG = new BucketGraph(@"..\..\input.txt");
        }
    }
    class BucketGraph
    {
        WGraph G;
        List<List<int>> Q;

        public BucketGraph(string path)
        {
            G = new WGraph(path);
            Q = new List<List<int>>();
            for (int i = 0; i < graphRange(G) + 1 ; i++)
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
                    List<int> vert = G.G.ViewVertex(j);
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
            List<int> different = new List<int>();
            foreach (int weight in g.W)
            {
                if (!different.Contains(weight))
                {
                    different.Add(weight);
                }
            }
            return different.Count;
        }
    }
    public class WGraph
    {
        public Graph G
        {
            get;
            private set;
        }
        public List<int> W
        {
            get;
            private set;
        }
        public WGraph(Graph g, List<int> w)
        {
            G = g;
            W = w;
        }
        public WGraph(string path)
        {
            string[] lines = System.IO.File.ReadAllLines(path);
            if (lines.Length < 2)
                throw new Exception("input file is too short");
            List<int> I = new List<int>();
            List<int> J = new List<int>();
            List<int> W = new List<int>();
            for (int i = 1; i < lines.Length; i++)
            {
                string[] sublines = lines[i].Split(' ');
                I.Add(Int32.Parse(sublines[0]));
                J.Add(Int32.Parse(sublines[1]));
                W.Add(Int32.Parse(sublines[2]));
            }
            G = new Graph(I, J);
        }

    }
}
