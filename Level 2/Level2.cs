using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CCC2 {
    internal class Program {

        static void Main(string[] args) {
            new Program();
        }


        public Program() {
            GetInput("level2-eg.txt");
            for(int i = 1; i < 5; i++)
                GetInput($"level2-{i}.txt");
        }


        private static void GetInput(string filename) {
            string[] lines = File.ReadAllLines(filename);

            int numberOfEntries = int.Parse(lines[0]);

            List<string> list = new List<string>();
            for(int i = 1; i < lines.Length - 2; i++) {
                list.Add(lines[i]);
            }

            List<KeyValuePair<string, Tuple<int, int>>> locations = list.Select(line => line.Split(' ')).Select(split => new KeyValuePair<string, Tuple<int, int>>(split[0], new Tuple<int, int>(int.Parse(split[1]), int.Parse(split[2])))).ToList();

            string[] journedLine = lines[lines.Length - 2].Split(' ');
            string[] locationLine = lines[lines.Length - 1].Split(' ');

            string from = journedLine[0];
            string to = journedLine[1];
            string stop1 = locationLine[0];
            string stop2 = locationLine[1];

            Tuple<string, string> journey = new Tuple<string, string>(journedLine[0], journedLine[1]);
            Tuple<string, string> stops = new Tuple<string, string>(locationLine[0], locationLine[1]);


            Tuple<int, int> fromLoc = locations.First(kvp => kvp.Key == from).Value;
            Tuple<int, int> toLoc = locations.First(kvp => kvp.Key == to).Value;
            Tuple<int, int> hyperloopStart = locations.First(kvp => kvp.Key == stop1).Value;
            Tuple<int, int> hyperloopEnd = locations.First(kvp => kvp.Key == stop2).Value;


            List<Tuple<int, int>> locs = new List<Tuple<int, int>> { hyperloopStart, hyperloopEnd };
            double distStart1 = Math.Sqrt(
                Math.Pow(Math.Abs(locs[0].Item1 - fromLoc.Item1), 2) +
                Math.Pow(Math.Abs(locs[0].Item2 - fromLoc.Item2), 2)
            );
            double distStart2 = Math.Sqrt(
                Math.Pow(Math.Abs(locs[1].Item1 - fromLoc.Item1), 2) +
                Math.Pow(Math.Abs(locs[1].Item2 - fromLoc.Item2), 2)
            );
            double distEnd1 = Math.Sqrt(
                Math.Pow(Math.Abs(locs[0].Item1 - toLoc.Item1), 2) +
                Math.Pow(Math.Abs(locs[0].Item2 - toLoc.Item2), 2)
            );
            double distEnd2 = Math.Sqrt(
                Math.Pow(Math.Abs(locs[1].Item1 - toLoc.Item1), 2) +
                Math.Pow(Math.Abs(locs[1].Item2 - toLoc.Item2), 2)
            );

            double distToLoop = Math.Min(distStart1, distStart2) / 15;

            double distLoop = Math.Sqrt(
                Math.Pow(Math.Abs(hyperloopStart.Item1 - hyperloopEnd.Item1), 2) + Math.Pow(Math.Abs(hyperloopStart.Item2 - hyperloopEnd.Item2), 2)
            ) / 250 + 200;

            double distFromLoop = Math.Min(distEnd1, distEnd2) / 15;


            double distTotal = distToLoop + distLoop + distFromLoop;
            int result = (int)Math.Round(distTotal);


            File.WriteAllText("output_" + filename, result.ToString());
        }
    }
}
