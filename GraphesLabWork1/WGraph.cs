using System;
using System.Collections.Generic;
using System.Linq;
using GraphesLabWork1;

namespace GraphesLabWork1
{
	public class WGraph : Graph
    {
        //weights
        public List<int> _W
        {
            get;
            private set;
        }

        public WGraph(WGraph wg)
        {
            build(wg, wg._W);
        }

        public WGraph(Graph g, List<int> w)
            : base(g)
        {
            _W = w;
        }

        void build(Graph g, List<int> w)
        {
            _I = g._I;
            _J = g._J;
            _H = g._H;
            _L = g._L;
            _W = w;
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

            build(new Graph(I, J), W);
        }

        public int GetWeight(int from, int to)
        {
            if (from == to)
                return 0;
            int weight = -1;
            for (int i = 0; i < _I.Count; i++)
            {
                if ((_I[i] == from && _J[i] == to) || (_I[i] == to && _J[i] == from))
                {
                    if (!getDeleted().Contains(i))
                        weight = _W[i];
                    break;
                }
            }
            return weight;
        }

        public override void PrintToFile(string path)
        {
            string begin = "graph{";
            string end = "}";
            using (System.IO.StreamWriter fw = new System.IO.StreamWriter(path))
            {
                fw.WriteLine(begin);
                for (int h = 0; h < _H.Count; h++)
                {
                    for (int l = _H[h]; l != -1; l = _L[l])
                    {
                        fw.WriteLine("{0} -- {1}[label=\"{2}\",weight=\"{2}\"];", _I[l], _J[l], _W[l]);
                    }
                }
                fw.Write(end);
            }
        }
        public int GetSum()
        {
            int sum = 0;
            for (int h = 0; h < _H.Count; h++)
            {
                for (int l = _H[h]; l != -1; l = _L[l])
                {
                    sum += _W[l];
                }
            }
            return sum;
        }
    }
}
