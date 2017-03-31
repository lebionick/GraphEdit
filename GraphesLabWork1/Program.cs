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
            //hardcoded intialization
            //Graph G = new Graph(new List<int>(3) { 0, 1, 2, 1, 0 }, new List<int>(3) { 1, 2, 3, 3, 2 });
			var G = new Graph(@"../../input2.txt");
            //G.Remove(3); 
            //G.Add(1, 3); 
            G.PrintToConsole();
            G.PrintToFile(@"../../output.gv");
            Console.ReadKey();     
        }
    }
}
