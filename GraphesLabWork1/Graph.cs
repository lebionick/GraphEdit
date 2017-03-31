﻿using System;
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
		//, _J, _H, _L;
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
		public virtual void PrintToFile(string path)
		{
			string begin = "digraph{";
			string end = "}";
			using (System.IO.StreamWriter fw = new System.IO.StreamWriter(path))
			{
				fw.WriteLine(begin);
				List<int> skip = getDeleted();
				for (int i = 0; i < _I.Count; i++)
				{
					if (skip != null && skip.Contains(i))
						continue;
					fw.WriteLine("{0} -> {1}[label=\"{2}\"];", _I[i], _J[i], i);
				}
				fw.Write(end);
			}
		}
		protected List<int> getDeleted()
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
				for (int i = final; i != -1; i = _L[i])
				{
					final = i;
				}
				_L[final] = num;
			}
		}
		//returns list of coinc vertexes
		public List<int> ViewVertex(int vertexNum)
		{
			List<int> result = new List<int>();
			for (int i = _H[vertexNum]; i != -1; i = _L[i])
				result.Add(i);
			return result;
		}
	}
}
