using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace gpuNeuralNet2
{
    static internal class utils
    {
        static Random rng = new Random();
        public static float random(float lowerBound, float upperBound)
        {
            return ((float)rng.NextDouble()) * MathF.Abs(upperBound - lowerBound) + lowerBound;
        }
        public static int randomInt(int lowerBound, int upperBound)
        {
            return rng.Next(lowerBound, upperBound);
        }
        public static string substringUntil(string text, int start, char until)
        {
            int pos = start;
            string output = "";
            char currentChar = text[pos];
            while (currentChar != until && pos < text.Length)
            {
                currentChar = text[pos];
                output += currentChar;
                pos++;
            }
            return output.Length > 0 ? output.Substring(0, output.Length - 1) : "";
        }

        public static IEnumerable<T> Section<T>(this IEnumerable<T> source, int from, int to)
        {
            return source.Skip(Math.Max(0, from)).Reverse().Skip(Math.Max(0, source.Count() - (to + 1))).Reverse();
        }

        public static int RoundToInt(this float source, float mode = 1)
        {
            return Convert.ToInt32(mode == 0 ? MathF.Floor(source) : mode == 1 ? MathF.Round(source) : MathF.Ceiling(source));
        }

        public static float Round(this float source, float degree = 1f)
        {
            return MathF.Round(source / degree) * degree;
        }

        public static float Fit(this float source, float lowerRangeA, float upperRangeA, float lowerRangeB, float upperRangeB)
        {
            return lowerRangeB + ((source - lowerRangeA) / (upperRangeA - lowerRangeA)) * (upperRangeB - lowerRangeB);
        }

        public static IEnumerable<T> Shift2D<T>(this IEnumerable<T> source, int xSize, int ySize, int xAmount, int yAmount, T filler)
        {
            if (xSize * ySize != source.Count())
            {

                throw new ArgumentOutOfRangeException();
            }

            T[,] arr = new T[xSize, ySize];
            T[] outputArr = new T[xSize * ySize];


            for (int x = 0; x < xSize; x++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    arr[x, y] = source.ToArray()[y * xSize + x];
                }
            }
            for (int x = 0; x < xSize; x++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    int targetX = x + xAmount;
                    int targetY = y + yAmount;
                    outputArr[y * xSize + x] = (targetX >= 0 && targetX < xSize && targetY >= 0 && targetY < ySize) ? arr[targetX, targetY] : filler;
                }
            }
            return outputArr;
        }

        public static int Wrap(this int source, int bracketSize)
        {
            return source - ((source * 1f / bracketSize).RoundToInt(0) * bracketSize);
        }
        public static TKey[] ReverseLookup<TKey, TValue>(this Dictionary<TKey, TValue> me, TValue value)
        {
            try
            {
                if (me.ContainsValue(value))
                    return me.Where(a => a.Value.Equals(value)).Select(b => b.Key).ToArray();
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }
        public static T ThrowIfNull<T>(
       this T? argument,
       string? message = default,
       [CallerArgumentExpression("argument")] string? paramName = default
   ) where T : notnull
        {
            if (argument is null)
            {
                throw new ArgumentNullException(paramName, message);
            }
            else
            {
                return argument;
            }
        }
        public static T ThrowIfNull<T>(
        this T? argument,
        string? message = default,
        [CallerArgumentExpression("argument")] string? paramName = default
    ) where T : unmanaged
        {
            if (argument is null)
            {
                throw new ArgumentNullException(paramName, message);
            }
            else
            {
                return (T)argument;
            }
        }
    }
}
