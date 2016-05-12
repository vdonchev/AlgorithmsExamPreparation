namespace _02.Guitar
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class Program
    {
        public static void Main()
        {
            var seq = Console.ReadLine()
                .Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList();
            var startSum = int.Parse(Console.ReadLine());
            seq.Insert(0, startSum);
            var maxSum = int.Parse(Console.ReadLine());
            var sums = new HashSet<int>[seq.Count];
            sums[0] = new HashSet<int>()
            {
                startSum
            };

            for (int i = 1; i < seq.Count; i++)
            {
                sums[i] = new HashSet<int>();
                foreach (var s in sums[i - 1])
                {
                    if (s + seq[i] <= maxSum)
                    {
                        sums[i].Add(s + seq[i]);
                    }

                    if (s - seq[i] >= 0)
                    {
                        sums[i].Add(s - seq[i]);
                    }
                }

                if (sums[i].Count == 0)
                {
                    Console.WriteLine(-1);
                    return;
                }
            }

            Console.WriteLine(sums[seq.Count - 1].Max());
        }
    }
}
