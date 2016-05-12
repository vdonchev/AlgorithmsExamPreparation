namespace _04.FastАndFurious
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class FastАndFurious
    {
        public static void Main()
        {
            var roadIndexes = new Dictionary<string, int>();
            var roadsMap = GetRoads(roadIndexes);
            FindAllFastestRoads(roadsMap);

            var drivers = new Dictionary<string, List<Record>>();
            var speeders = new SortedSet<string>();
            ProcessRecords(drivers, roadIndexes, roadsMap, speeders);

            speeders.ToList().ForEach(Console.WriteLine);
        }

        private static void ProcessRecords(Dictionary<string, List<Record>> drivers, Dictionary<string, int> roadIndexes, double[,] roadsMap, SortedSet<string> speeders)
        {
            var input = Console.ReadLine();
            while (input != "End")
            {
                var tokens = input.Split(' ');
                var city = tokens[0];
                var plate = tokens[1];
                var time = TimeSpan.Parse(tokens[2]);

                if (!drivers.ContainsKey(plate))
                {
                    drivers[plate] = new List<Record>();
                }

                foreach (var driver in drivers[plate])
                {
                    var endTime = driver.Time;
                    var endCity = driver.City;

                    var roadTime = Math.Abs((endTime - time).TotalHours);

                    var startCityIndex = roadIndexes[city];
                    var endCityIndex = roadIndexes[endCity];

                    var distanceTime = roadsMap[startCityIndex, endCityIndex];

                    if (!double.IsPositiveInfinity(distanceTime) &&
                        distanceTime > roadTime)
                    {
                        speeders.Add(plate);
                        break;
                    }
                }

                drivers[plate].Add(new Record(time, city));
                input = Console.ReadLine();
            }
        }

        private static double[,] GetRoads(Dictionary<string, int> roadIndexes)
        {
            var roadsList = new List<string[]>();
            var roadIndex = 0;

            Console.ReadLine();
            var input = Console.ReadLine();
            while (input != "Records:")
            {
                var roadInfo = input.Split(' ');
                roadsList.Add(roadInfo);

                var startTown = roadInfo[0];
                var endTown = roadInfo[1];

                if (!roadIndexes.ContainsKey(startTown))
                {
                    roadIndexes[startTown] = roadIndex++;
                }

                if (!roadIndexes.ContainsKey(endTown))
                {
                    roadIndexes[endTown] = roadIndex++;
                }

                input = Console.ReadLine();
            }

            var totalRoads = roadIndexes.Count;
            var roadsMap = new double[totalRoads, totalRoads];
            for (var i = 0; i < roadsMap.GetLength(0); i++)
            {
                for (var j = 0; j < roadsMap.GetLength(1); j++)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    roadsMap[i, j] = double.PositiveInfinity;
                }
            }

            foreach (var road in roadsList)
            {
                var startTown = road[0];
                var endTown = road[1];

                var roadTime = double.Parse(road[2]) / double.Parse(road[3]);

                var startTownIndex = roadIndexes[startTown];
                var endTownIndex = roadIndexes[endTown];

                roadsMap[startTownIndex, endTownIndex] = roadTime;
                roadsMap[endTownIndex, startTownIndex] = roadTime;
            }

            return roadsMap;
        }

        private static void FindAllFastestRoads(double[,] roadsMap)
        {
            var v = roadsMap.GetLength(0);
            for (var k = 0; k < v; k++)
            {
                for (var i = 0; i < v; i++)
                {
                    for (var j = 0; j < v; j++)
                    {
                        if (roadsMap[i, k] + roadsMap[k, j] < roadsMap[i, j])
                        {
                            roadsMap[i, j] = roadsMap[i, k] + roadsMap[k, j];
                        }
                    }
                }
            }
        }

        private class Record
        {
            public Record(TimeSpan time, string city)
            {
                this.Time = time;
                this.City = city;
            }

            public TimeSpan Time { get; private set; }

            public string City { get; private set; }
        }
    }
}