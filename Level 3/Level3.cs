using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CCC3 {
    internal class Program {

        static void Main(string[] args) {
            new Program();
        }


        public Program() {
            GetInput("level3-eg.txt");
            for(int i = 1; i < 5; i++)
                GetInput($"level3-{i}.txt");
        }


        private static void GetInput(string filename) {
            string[] lines = File.ReadAllLines(filename);

            int numberOfEntries = int.Parse(lines[0]);

            List<string> list = new List<string>();
            for(int i = 0; i < numberOfEntries; i++) {
                list.Add(lines[i + 1]);
            }

            int numberOfJourneys = int.Parse(lines[numberOfEntries + 1]);
            int journeyIndex = numberOfEntries + 2;

            List<KeyValuePair<string, Tuple<int, int>>> locations = list.Select(line => line.Split(' ')).Select(split => new KeyValuePair<string, Tuple<int, int>>(split[0], new Tuple<int, int>(int.Parse(split[1]), int.Parse(split[2])))).ToList();
            List<Tuple<Tuple<string, string>, long>> journeys = new List<Tuple<Tuple<string, string>, long>>();

            for(int i = 0; i < numberOfJourneys; i++) {
                string line = lines[i + journeyIndex];
                string[] split = line.Split(' ');

                journeys.Add(new Tuple<Tuple<string, string>, long>(new Tuple<string, string>(split[0], split[1]), long.Parse(split[2])));
            }

            int last = numberOfJourneys + journeyIndex;
            string[] splitHyper = lines[last].Split(' ');
            Tuple<string, string> hyperloop = new Tuple<string, string>(splitHyper[0], splitHyper[1]);

            List<int> results = new List<int>();

            foreach(Tuple<Tuple<string, string>, long> journey in journeys) {
                string from = journey.Item1.Item1;
                string to = journey.Item1.Item2;
                string hyperFrom = hyperloop.Item1;
                string hyperTo = hyperloop.Item2;


                Tuple<int, int> fromLoc = locations.First(kvp => kvp.Key == from).Value;
                Tuple<int, int> toLoc = locations.First(kvp => kvp.Key == to).Value;
                Tuple<int, int> hyperloopStart = locations.First(kvp => kvp.Key == hyperFrom).Value;
                Tuple<int, int> hyperloopEnd = locations.First(kvp => kvp.Key == hyperTo).Value;


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
                if(result < journey.Item2)
                    results.Add(result);
            }

            File.WriteAllText("output_" + filename, results.Count.ToString());
        }
    }
}
