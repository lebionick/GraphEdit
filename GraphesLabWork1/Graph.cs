using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphesLabWork1
{
	public class Graph
    {
        public List<int> _I
        {
            get;
            protected set;
        }
        public List<int> _J
        {
            get;
            protected set;
        }
        public List<int> _H
        {
            get;
            protected set;
        }
        public List<int> _L
        {
            get;
            protected set;
        }

        protected int startdel = -1;

        protected Graph()
        {
        }

        public Graph(List<int> I, List<int> J)
        {
            build(I, J);
        }

        public Graph(Graph g)
        {
            _I = g._I;
            _J = g._J;
            _H = g._H;
            _L = g._L;
        }

        public Graph(string path)
        {
            string[] lines = System.IO.File.ReadAllLines(path);
            if (lines.Length < 2)
                throw new Exception("input file is too short");
            List<int> I = new List<int>();
            List<int> J = new List<int>();
            for (int i = 1; i < lines.Length; i++)
            {
                string[] sublines = lines[i].Split(' ');
                I.Add(Int32.Parse(sublines[0]));
                J.Add(Int32.Parse(sublines[1]));
            }
            build(I, J);
        }

        void build(List<int> I, List<int> J)
        {
            if (I.Count() != J.Count)
                throw new Exception("intial arrays do not match");

            int m = I.Count;
            int n = Math.Max(I.Max(), J.Max()) + 1;

            _I = new List<int>(I);
            _J = new List<int>(J);
            _L = new List<int>();
            _H = new List<int>();

            for (int i = 0; i < m; i++)
                _L.Add(-1);

            for (int i = 0; i < n; i++)
                _H.Add(-1);

            for (int k = 0; k < m; k++)
            {
                int v = _I[k];
                _L[k] = _H[v];
                _H[v] = k;
            }
        }

        public void PrintToConsole()
        {
            for (int i = 0; i < _I.Count; i++)
            {
                if (i >= _H.Count)
                    Console.WriteLine("I: {0}\tJ: {1}\t\tL: {2}", _I[i], _J[i], _L[i]);
                else
                    Console.WriteLine("I: {0}\tJ: {1}\tH: {2}\tL: {3}", _I[i], _J[i], _H[i], _L[i]);
            }
        }
        public virtual void PrintToFile(string path)
        {
            string begin = "digraph{";
            string end = "}";
            using (System.IO.StreamWriter fw = new System.IO.StreamWriter(path))
            {
                fw.WriteLine(begin);
                for (int h = 0; h < _H.Count; h++)
                {
                    for (int l = _H[h]; l != -1; l = _L[l])
                    {
                        fw.WriteLine("{0} -> {1}[label=\"{2}\"];", _I[l], _J[l], l);
                    }
                }
                fw.Write(end);
            }
        }
        protected List<int> getDeleted()
        {
            List<int> deleted = new List<int>();

            if (startdel != -1)
                for (int i = startdel; i != -1; i = _L[i])
                {
                    deleted.Add(i);
                }
            return deleted;
        }

        public void Add(int from, int to)
        {
            int newNum;
            if (startdel != -1)
            {
                newNum = startdel;
                _I[newNum] = from; _J[newNum] = to;
                startdel = _L[startdel];
            }
            else
            {
                newNum = _I.Count;
                _I.Add(from); _J.Add(to);
                _L.Add(-1);
            }

            _L[newNum] = _H[from];
            _H[from] = newNum;
        }
        public void Remove(int num)
        {
            if (getDeleted().Contains(num))
                return;

            _L[num] = startdel;
            startdel = num;
        }
        public void Remove(int from, int to)
        {
            for (int i = 0; i < _I.Count; i++)
            {
                if ((_I[i] == from && _J[i] == to) || (_I[i] == to && _J[i] == from))
                {
                    Remove(i);
                    break;
                }
            }
        }
        //returns list of coincident vertexes
        public List<int> ViewVertex(int vertexNum)
        {
            List<int> result = new List<int>();
            //Такой вариант для ориентированного графа
            //for (int i = _H[vertexNum]; i !=-1; i=_L[i])
            //{
            //    if (_I[i] == vertexNum)
            //        result.Add(_J[i]);
            //    else
            //        result.Add(_I[i]);
            //}
            var del = getDeleted();
            for (int i = 0; i < _I.Count; i++)
            {
                if (_I[i] == vertexNum && !del.Contains(i))
                    result.Add(_J[i]);
                if (_J[i] == vertexNum && !del.Contains(i))
                    result.Add(_I[i]);
            }
            return result;
        }
    }
}
