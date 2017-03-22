using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphesLabWork1
{
    class Program
    {
        static void Main(string[] args)
        {
            //Graph G = new Graph(new List<int>(3) { 0, 1, 2, 1, 0 }, new List<int>(3) { 1, 2, 3, 3, 2 });
            Graph G = new Graph(@"C:\Users\kalmar\Desktop\g.txt");
            //G.Remove(1); G.Remove(0);
            G.Remove(3); 
            G.Add(1, 3); 
            G.PrintToConsole();
            G.PrintToFile(@"C:\Users\kalmar\Desktop\g1.gv");
            Console.ReadKey();
        }
    }
    class Graph
    {
        List<int> _I, _J, _H, _L;
        int startdel = -1;

        public Graph(List<int> I, List<int> J)
        {
            build(I, J);
        }

        public Graph(string path)
        {
            string[] lines = System.IO.File.ReadAllLines(path);
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
            int n = Math.Max(I.Max(), J.Max())+1;
            _I = new List<int>(I);
            _J = new List<int>(J);

            _L = new List<int>();
            for (int i = 0; i < m; i++)
                _L.Add(-1);
            _H = new List<int>();
            for (int i = 0; i < n; i++)
                _H.Add(-1);

            for (int i = 0; i < m; i++)
            {
                if (_H[_I[i]] == -1)
                {
                    _H[_I[i]] = i;
                }
                else if (_L[_H[_I[i]]] == -1)
                    _L[_H[_I[i]]] = i;
                else
                {
                    int final = _L[_H[_I[i]]];
                    for (int j = final; j != -1; j = _L[j])
                        final = j;

                    _L[final] = i;
                }
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
        public void PrintToFile(string path)
        {
            string begin = "graph{";
            string end = "}";
            using (System.IO.StreamWriter fw = new System.IO.StreamWriter(path))
            {
                fw.WriteLine(begin);
                List<int> skip = getDeleted();
                //foreach (int i in skip)
                //    Console.WriteLine(i);
                for (int i = 0; i < _I.Count; i++)
                {
                    if (skip!=null && skip.Contains(i))
                        continue;
                    fw.WriteLine("{0}--{1}[label=\"{2}\"];", _I[i], _J[i], i);
                }
                fw.Write(end);
            }
        }
        List<int> getDeleted()
        {
            if (startdel == -1)
                return null;
            List<int> deleted = new List<int>();
            for (int i = startdel; i != -1; i = _L[i])
            {
                deleted.Add(i);
            }
            return deleted;
        }

        public void Add(int from, int to)
        {
            int newNum;
            if(startdel!=-1)
            {
                newNum=startdel;
                _I[newNum]=from; _J[newNum]=to;
                startdel=_L[startdel];
            }
            else
            {
                newNum = _I.Count;
                _I.Add(from); _J.Add(to);
                _L.Add(-1);
            }
            if (_H[from] == -1)
                _H[from] = newNum;
            int final = _H[from];
            for (int j = final; j != -1; j = _L[j])
                final = j;

            _L[final] = newNum;
        }
        public void Remove(int num)
        {
            for (int i = 0; i < _H.Count; i++)
            {
                if (_H[i] == num)
                {
                    _H[i] = _L[_H[i]];
                    _L[num] = -1;
                    break;
                }
            }
            if (startdel == -1)
                startdel = num;
            else
            {
                int final = startdel;
                for (int i = final; i!=-1 ; i = _L[i])
                {
                    final=i;
                }
                _L[final] = num;
            }
        }
    }
}
