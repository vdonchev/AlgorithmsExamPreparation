namespace Parenthesis
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public static class Parenthesis
    {
        private static readonly StringBuilder Result = new StringBuilder();
        private static int n;

        public static void Main()
        {
            n = int.Parse(Console.ReadLine());
            PutParenthesis(new LinkedList<char>(), n, n);
            Console.WriteLine(Result.ToString().Trim());
        }

        private static void PutParenthesis(LinkedList<char> seq, int open, int close, int opened = 0)
        {
            if (open == 0 & close == 0)
            {
                Result.AppendLine(string.Join(string.Empty, seq));
                return;
            }

            if (open > 0)
            {
                seq.AddLast('(');
                PutParenthesis(seq, open - 1, close, opened + 1);
                seq.RemoveLast();
            }

            if (close > 0 && opened > 0)
            {
                seq.AddLast(')');
                PutParenthesis(seq, open, close - 1, opened - 1);
                seq.RemoveLast();
            }
        }
    }
}
