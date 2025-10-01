using System;
using System.Globalization;
using System.Numerics;

namespace BigMath
{
    [Serializable]
    public readonly struct BigNumber : IComparable<BigNumber>, IEquatable<BigNumber>
    {
        private const int ScaleDigits = 6;
        private static readonly BigInteger ScaleFactor = BigInteger.Pow(10, ScaleDigits);
        private static readonly decimal ScaleFactorDecimal = PowDecimal10(ScaleDigits);
        private static readonly double ScaleFactorDouble = Math.Pow(10d, ScaleDigits);

        public BigInteger Value { get; }

        private BigNumber(BigInteger value, bool alreadyScaled)
        {
            Value = alreadyScaled ? value : value * ScaleFactor;
        }

        public BigNumber(BigInteger value) : this(value, false)
        {
        }

        public BigNumber(decimal value) : this(ToScaled(value), true)
        {
        }

        private static BigInteger ToScaled(decimal value)
        {
            decimal scaled = decimal.Round(value * ScaleFactorDecimal, 0, MidpointRounding.AwayFromZero);
            return new BigInteger(scaled);
        }

        private static decimal PowDecimal10(int exponent)
        {
            decimal result = 1m;
            for (int i = 0; i < exponent; i++)
            {
                result *= 10m;
            }
            return result;
        }

        public static BigNumber FromDouble(double value) => new BigNumber(ToScaled((decimal)value), true);

        public decimal ToDecimal()
        {
            decimal scaledValue;
            try
            {
                scaledValue = (decimal)Value;
            }
            catch (OverflowException)
            {
                return (decimal)ToDouble();
            }
            return scaledValue / ScaleFactorDecimal;
        }

        public double ToDouble() => (double)Value / ScaleFactorDouble;

        public static implicit operator BigNumber(int v) => new BigNumber(new BigInteger(v));
        public static implicit operator BigNumber(long v) => new BigNumber(new BigInteger(v));
        public static implicit operator BigNumber(float v) => new BigNumber((decimal)v);
        public static implicit operator BigNumber(double v) => FromDouble(v);
        public static implicit operator BigNumber(decimal v) => new BigNumber(v);
        public static implicit operator BigNumber(BigInteger v) => new BigNumber(v);

        public static implicit operator BigInteger(BigNumber v) => BigInteger.Divide(v.Value, ScaleFactor);
        public static explicit operator double(BigNumber v) => v.ToDouble();
        public static explicit operator decimal(BigNumber v) => v.ToDecimal();

        public static BigNumber operator +(BigNumber a, BigNumber b) => new BigNumber(BigInteger.Add(a.Value, b.Value), true);
        public static BigNumber operator -(BigNumber a, BigNumber b) => new BigNumber(BigInteger.Subtract(a.Value, b.Value), true);

        public static BigNumber operator *(BigNumber a, BigNumber b)
        {
            BigInteger scaledProduct = BigInteger.Divide(BigInteger.Multiply(a.Value, b.Value), ScaleFactor);
            return new BigNumber(scaledProduct, true);
        }

        public static BigNumber operator /(BigNumber a, BigNumber b)
        {
            if (b.Value.IsZero)
            {
                throw new DivideByZeroException();
            }

            BigInteger scaledDividend = BigInteger.Multiply(a.Value, ScaleFactor);
            return new BigNumber(BigInteger.Divide(scaledDividend, b.Value), true);
        }

        public static BigNumber operator ^(BigNumber a, int exponent)
        {
            return Pow(a, new BigNumber(exponent));
        }

        public static BigNumber Pow(BigNumber a, BigInteger exponent)
        {
            return Pow(a, new BigNumber(exponent));
        }

        public static BigNumber Pow(BigNumber a, BigNumber exponent)
        {
            double result = Math.Pow(a.ToDouble(), exponent.ToDouble());
            return FromDouble(result);
        }

        public static bool operator ==(BigNumber a, BigNumber b) => a.Value == b.Value;
        public static bool operator !=(BigNumber a, BigNumber b) => a.Value != b.Value;
        public static bool operator <(BigNumber a, BigNumber b) => a.Value < b.Value;
        public static bool operator >(BigNumber a, BigNumber b) => a.Value > b.Value;
        public static bool operator <=(BigNumber a, BigNumber b) => a.Value <= b.Value;
        public static bool operator >=(BigNumber a, BigNumber b) => a.Value >= b.Value;

        public int CompareTo(BigNumber other) => Value.CompareTo(other.Value);
        public bool Equals(BigNumber other) => Value.Equals(other.Value);
        public override bool Equals(object obj) => obj is BigNumber bn && Equals(bn);
        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString()
        {
            if (Value.IsZero)
            {
                return "0";
            }

            bool isNegative = Value.Sign < 0;
            BigInteger absValue = BigInteger.Abs(Value);
            BigInteger integerPart = BigInteger.Divide(absValue, ScaleFactor);
            BigInteger fractionalPart = BigInteger.Remainder(absValue, ScaleFactor);

            string integerString = integerPart.ToString(CultureInfo.InvariantCulture);
            if (fractionalPart.IsZero)
            {
                return isNegative ? $"-{integerString}" : integerString;
            }

            string fractionalString = fractionalPart.ToString(CultureInfo.InvariantCulture).PadLeft(ScaleDigits, '0').TrimEnd('0');
            string result = fractionalString.Length > 0 ? $"{integerString}.{fractionalString}" : integerString;
            return isNegative ? $"-{result}" : result;
        }
    }
}
