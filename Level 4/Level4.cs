using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace CCC4 {
    internal class Program {

        static void Main(string[] args) {
            new Program();
        }

        public Program() {
            Test("level4-eg.txt");

            Thread[] threads = new Thread[4];

            for(int i = 1; i < 4; i++) {
                threads[i - 1] = new Thread(() => { Test($"level4-{i}.txt"); });
                threads[i - 1].Start();
                Console.WriteLine("Started #" + i);
            }

            threads[0].Join();
            threads[1].Join();
            threads[2].Join();
            threads[3].Join();

            Console.WriteLine("DONE!");
            Console.ReadKey();
        }


        private static void Test(string filename) {
            #region Get Input
            string[] lines = File.ReadAllLines(filename);

            int numberOfEntries = int.Parse(lines[0]);

            List<string> list = new List<string>();
            for(int i = 0; i < numberOfEntries; i++) {
                list.Add(lines[i + 1]);
            }

            int numberOfJourneys = int.Parse(lines[numberOfEntries + 1]);
            int journeyIndex = numberOfEntries + 2;

            List<KeyValuePair<string, Tuple<int, int>>> locations = list.Select(line => line.Split(' ')).Select(split => new KeyValuePair<string, Tuple<int, int>>(split[0], new Tuple<int, int>(int.Parse(split[1]), int.Parse(split[2])))).ToList();
            List<Tuple<string, string, long>> journeys = new List<Tuple<string, string, long>>();

            for(int i = 0; i < numberOfJourneys; i++) {
                string line = lines[i + journeyIndex];
                string[] split = line.Split(' ');

                journeys.Add(new Tuple<string, string, long>(split[0], split[1], long.Parse(split[2])));
            }

            int last = numberOfJourneys + journeyIndex;
            int n = int.Parse(lines[last]);
            #endregion

            List<Tuple<string, string>> results = new List<Tuple<string, string>>();

            //Loop through Hyperloop Start Point
            foreach(KeyValuePair<string, Tuple<int, int>> hyperloopFrom in locations) {
                #region Values
                string friendlyNameFrom = hyperloopFrom.Key;
                Tuple<int, int> coordsFrom = hyperloopFrom.Value;
                #endregion

                //Loop through Hyperloop End Point
                foreach(KeyValuePair<string, Tuple<int, int>> hyperloopTo in locations) {
                    #region Values
                    string friendlyNameTo = hyperloopTo.Key;
                    Tuple<int, int> coordsTo = hyperloopTo.Value;
                    #endregion

                    double hyperloopDuration = Math.Sqrt(
                        Math.Pow(Math.Abs(coordsFrom.Item1 - coordsTo.Item1), 2) +
                        Math.Pow(Math.Abs(coordsFrom.Item2 - coordsTo.Item2), 2)
                    ) / 250 + 200;

                    int fasterFor = 0;

                    foreach(Tuple<string, string, long> journey in journeys) {
                        Tuple<int, int> journeyStartCoords = locations.First(l => l.Key == journey.Item1).Value;
                        Tuple<int, int> journeyEndCoords = locations.First(l => l.Key == journey.Item2).Value;

                        double toLoopDuration = Math.Sqrt(
                            Math.Pow(Math.Abs(coordsFrom.Item1 - journeyStartCoords.Item1), 2) +
                            Math.Pow(Math.Abs(coordsFrom.Item2 - journeyStartCoords.Item2), 2)
                        ) / 15;
                        double fromLoopDuration = Math.Sqrt(
                            Math.Pow(Math.Abs(coordsTo.Item1 - journeyEndCoords.Item1), 2) +
                            Math.Pow(Math.Abs(coordsTo.Item2 - journeyEndCoords.Item2), 2)
                        ) / 15;

                        double totalDuration = hyperloopDuration + toLoopDuration + fromLoopDuration;

                        if(totalDuration < journey.Item3)
                            fasterFor++;
                    }

                    if(fasterFor >= n)
                        results.Add(new Tuple<string, string>(friendlyNameFrom, friendlyNameTo));
                }
            }
            if(results.Count > 0)
                Console.WriteLine(filename + ": " + results[0]);
            else
                Console.WriteLine(filename + ": 0 found");
        }
    }
}
