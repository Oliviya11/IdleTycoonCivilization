﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;

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

            if (Exponent < 3)
            {
                double value10 = Math.Pow(10, Exponent);
                Value = Math.Round(Value * value10);
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

        public static bool operator >(BigNumber a, BigNumber b)
        {
            if (a.Exponent != b.Exponent)
                return a.Exponent > b.Exponent;
            return a.Value > b.Value;
        }

        public static bool operator <(BigNumber a, BigNumber b)
        {
            if (a.Exponent != b.Exponent)
                return a.Exponent < b.Exponent;
            return a.Value < b.Value;
        }

        public static bool operator >=(BigNumber a, BigNumber b)
        {
            return a > b || a.Equals(b);
        }

        public static bool operator <=(BigNumber a, BigNumber b)
        {
            return a < b || a.Equals(b);
        }

        public override bool Equals(object obj)
        {
            if (obj is BigNumber other)
                return this.Exponent == other.Exponent && Math.Abs(this.Value - other.Value) < 1e-9;
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value, Exponent);
        }

        public int CompareTo(BigNumber other)
        {
            if (this > other) return 1;
            if (this < other) return -1;
            return 0;
        }

        public float ToFloat()
        {
            if (Value == 0) return 0f;
            return (float)(Math.Log10(Value) + Exponent);
        }

        public static BigNumber FromFloat(float logValue)
        {
            int exponent = Mathf.FloorToInt(logValue);
            double mantissa = Mathf.Pow(10, logValue - exponent);
            return new BigNumber(mantissa, exponent);
        }

        public override string ToString()
        {
            if (Exponent < 3)
            {
                return $"{(int)Value}";
            }
            else
            {
                int index = Exponent / 3;
               
                if (index >= suffixes.Count)
                    return $"{Value:F2}e{Exponent}";

                int leftover = Exponent % 3;
                double value = Value * Math.Pow(10, leftover);
                //string result = $"{value:F2}{suffixes[index]}";
                string str = value.ToString("F10");
                int dotIndex = str.IndexOf(',');
                if (dotIndex < 0) dotIndex = str.IndexOf('.');
                string result = dotIndex == -1 ? str : str.Substring(0, dotIndex + 3);
                result = result.Replace(',', '.') + suffixes[index];

                return result;
            }
        }
    }
}
