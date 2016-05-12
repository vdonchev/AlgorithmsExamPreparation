namespace _04.FastАndFurious
{
    using System;
    using System.Collections.Generic;

    public static class FastАndFurious
    {
        private static double[,] bestRoads;

        public static void Main()
        {
            // Read roads
            Console.ReadLine(); // wtf?

            var roadsList = new List<string[]>();

            var roadIndex = 0;
            var roadIndexes = new Dictionary<string, int>();

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

            // read records
            var drivers = new Dictionary<string, List<Position>>();
            input = Console.ReadLine();
            while (input != "End")
            {
                var tokens = input.Split(' ');

                var location = tokens[0];
                var plateNUmber = tokens[1];
                var time = TimeSpan.Parse(tokens[2]);

                if (!drivers.ContainsKey(plateNUmber))
                {
                    drivers[plateNUmber] = new List<Position>();
                }

                drivers[plateNUmber].Add(new Position(time, location));

                input = Console.ReadLine();
            }

            bestRoads = new double[totalRoads, totalRoads];

            var speeders = new SortedSet<string>();
            foreach (var driver in drivers)
            {
                driver.Value.Sort();
                for (int i = 0; i < driver.Value.Count - 1; i++)
                {
                    var startTime = driver.Value[i].Time;
                    var endTime = driver.Value[i + 1].Time;

                    var roadTime = (endTime - startTime).TotalHours;

                    var startCity = driver.Value[i].City;
                    var endCity = driver.Value[i + 1].City;

                    var startCityIndex = roadIndexes[startCity];
                    var endCityIndex = roadIndexes[endCity];

                    FindFastestRoad(roadsMap, startCityIndex, endCityIndex);

                    var distanceTime = bestRoads[startCityIndex, endCityIndex];
                    if (distanceTime > roadTime)
                    {
                        speeders.Add(driver.Key);
                    }
                }
            }

            foreach (var speeder in speeders)
            {
                Console.WriteLine(speeder);
            }
        }

        private static void FindFastestRoad(double[,] roadsMap, int startTown, int endTown)
        {
            var n = roadsMap.GetLength(0);

            var distance = new double[n];
            for (int i = 0; i < n; i++)
            {
                distance[i] = double.MaxValue;
            }

            distance[startTown] = 0;

            var used = new bool[n];
            var previous = new int?[n];

            while (true)
            {
                double minDistance = double.MaxValue;
                var minNode = 0;
                for (int i = 0; i < n; i++)
                {
                    if (!used[i] && minDistance > distance[i])
                    {
                        minDistance = distance[i];
                        minNode = i;
                    }
                }

                if (minDistance == double.MaxValue)
                {
                    break;
                }

                used[minNode] = true;

                for (int i = 0; i < n; i++)
                {
                    if (roadsMap[minNode, i] > 0)
                    {
                        var shortestToMinNode = distance[minNode];
                        var distanceToNextNode = roadsMap[minNode, i];

                        var totalDistance = shortestToMinNode + distanceToNextNode;

                        if (totalDistance < distance[i])
                        {
                            distance[i] = totalDistance;
                            previous[i] = minNode;
                        }
                    }
                }
            }

            if (distance[endTown] != double.MaxValue)
            {
                bestRoads[startTown, endTown] = distance[endTown];
                bestRoads[endTown, startTown] = distance[endTown];
            }
        }

        private static void DevPrinter(double[,] roads)
        {
            for (int row = 0; row < roads.GetLength(0); row++)
            {
                for (int col = 0; col < roads.GetLength(1); col++)
                {
                    Console.Write($"{roads[row, col],-8:f3}");
                }

                Console.WriteLine();
            }
        }

        internal class Position : IComparable<Position>
        {
            public Position(TimeSpan time, string city)
            {
                this.Time = time;
                this.City = city;
            }

            public TimeSpan Time { get; set; }

            public string City { get; set; }

            public int CompareTo(Position other)
            {
                return this.Time.CompareTo(other.Time);
            }
        }
    }
}