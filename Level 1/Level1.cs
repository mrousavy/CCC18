using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace CCC1 {
    internal class Program {

        static void Main(string[] args) {
            new Program();
        }


        public Program() {
            GetInput("level1-eg.txt");
            for(int i = 1; i < 5; i++)
                GetInput($"level1-{i}.txt");
        }


        private static void GetInput(string filename) {
            string[] lines = File.ReadAllLines(filename);

            int numberOfEntries = int.Parse(lines[0]);

            List<string> list = new List<string>();
            for(int i = 1; i < lines.Length; i++) {
                list.Add(lines[i]);
            }

            List<KeyValuePair<string, Tuple<int, int>>> locations = new List<KeyValuePair<string, Tuple<int, int>>>();
            string from = "", to = "";

            foreach(string line in list) {
                string[] split = line.Split(' ');

                if(split.Length == 2) {
                    from = split[0];
                    to = split[1];
                } else {
                    locations.Add(new KeyValuePair<string, Tuple<int, int>>(split[0], new Tuple<int, int>(int.Parse(split[1]), int.Parse(split[2]))));
                }
            }

            Tuple<int, int> fromLoc = locations.First(kvp => kvp.Key == from).Value;
            Tuple<int, int> toLoc = locations.First(kvp => kvp.Key == to).Value;


            double dist = Math.Sqrt(Math.Pow(Math.Max(fromLoc.Item1, toLoc.Item1) - Math.Min(fromLoc.Item1, toLoc.Item1), 2) +
                            Math.Pow(Math.Max(fromLoc.Item2, toLoc.Item2) - Math.Min(fromLoc.Item2, toLoc.Item2), 2)) / 250.0 + 200.0;

            int result = (int)Math.Round(dist);

            File.WriteAllText("output_" + filename, result.ToString());
        }
    }
}
