using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Assets.Scripts.Core
{
    public class BigNumber
    {
        private static readonly List<string> suffixes = new() { "", "K", "M", "B", "T", "Qa", "Qi", "Sx", "Sp", "Oc", "No", "Dc" };

        public double Value { get; private set; }
        public int Exponent { get; private set; }

        public BigNumber(string number)
        {
            ParseFromString(number);
        }

        public BigNumber(double value, int exponent)
        {
            Value = value;
            Exponent = exponent;
            Normalize();
        }

        private void ParseFromString(string number)
        {
            number = number.Trim();
            var match = Regex.Match(number, @"([\d.]+)([A-Za-z]*)");
            if (!match.Success)
                throw new ArgumentException($"Invalid number format: {number}");

            Value = double.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
            string suffix = match.Groups[2].Value;

            Exponent = suffixes.IndexOf(suffix) * 3;
            Normalize();
        }

        private void Normalize()
        {
            while (Value >= 1000)
            {
                Value /= 1000;
                Exponent += 3;
            }
            while (Value < 1 && Exponent > 0)
            {
                Value *= 1000;
                Exponent -= 3;
            }
        }

        private static BigNumber AlignAndOperate(BigNumber a, BigNumber b, Func<double, double, double> operation)
        {
            if (a.Exponent > b.Exponent)
            {
                double scaledB = b.Value / Math.Pow(10, a.Exponent - b.Exponent);
                return new BigNumber(operation(a.Value, scaledB), a.Exponent);
            }
            else if (b.Exponent > a.Exponent)
            {
                double scaledA = a.Value / Math.Pow(10, b.Exponent - a.Exponent);
                return new BigNumber(operation(scaledA, b.Value), b.Exponent);
            }
            return new BigNumber(operation(a.Value, b.Value), a.Exponent);
        }

        public static BigNumber operator +(BigNumber a, BigNumber b)
        {
            return AlignAndOperate(a, b, (x, y) => x + y);
        }

        public static BigNumber operator -(BigNumber a, BigNumber b)
        {
            return AlignAndOperate(a, b, (x, y) => x - y);
        }

        public static BigNumber operator *(BigNumber a, BigNumber b)
        {
            return new BigNumber(a.Value * b.Value, a.Exponent + b.Exponent);
        }

        public static BigNumber operator /(BigNumber a, BigNumber b)
        {
            return new BigNumber(a.Value / b.Value, a.Exponent - b.Exponent);
        }

        public override string ToString()
        {
            int index = Exponent / 3;
            if (index >= suffixes.Count)
                return $"{Value:F2}e{Exponent}";

            return $"{Value:F2}{suffixes[index]}";
        }
    }
}
