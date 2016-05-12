namespace _03.Towns
{
    using System;
    using System.Linq;

    public static class Towns
    {
        public static void Main()
        {
            var count = int.Parse(Console.ReadLine());
            var distances = new int[count];
            for (int i = 0; i < count; i++)
            {
                distances[i] = int.Parse(Console.ReadLine().Split(' ')[0]);
            }

            var bestPath = new int[count];
            bestPath[0] = 1;

            // find best increase
            for (int i = 1; i < count; i++)
            {
                var currentDistance = distances[i];
                var currentBest = 1;

                for (int j = i - 1; j >= 0; j--)
                {
                    var prevDistance = distances[j];
                    var prevBest = bestPath[j];

                    if (currentDistance > prevDistance &&
                        prevBest + 1 > currentBest)
                    {
                        currentBest = prevBest + 1;
                    }
                }

                bestPath[i] = currentBest;
            }

            Array.Reverse(distances);

            var bestPathB = new int[count];
            bestPathB[0] = 1;

            // find best increase
            for (int i = 1; i < count; i++)
            {
                var currentDistance = distances[i];
                var currentBest = 1;

                for (int j = i - 1; j >= 0; j--)
                {
                    var prevDistance = distances[j];
                    var prevBest = bestPathB[j];

                    if (currentDistance > prevDistance &&
                        prevBest + 1 > currentBest)
                    {
                        currentBest = prevBest + 1;
                    }
                }

                bestPathB[i] = currentBest;
            }

            Array.Reverse(bestPathB);

            for (int i = 0; i < count; i++)
            {
                bestPath[i] += bestPathB[i] - 1;
            }

            Console.WriteLine(bestPath.Max());
        }
    }
}
