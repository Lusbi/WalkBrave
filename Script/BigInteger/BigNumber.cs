using System;
using System.Numerics;
namespace BigMath
{
    [Serializable]
    /// <summary> 
    /// /// 薄包裝 BigInteger，提供符號運算（+ - * / ^）與比較運算（== != < <= > >=） 
    /// /// 注意：此型別將 ^ 視為「冪次」，僅限右側為 int 的情境 
    /// /// </summary> 
    public readonly struct BigNumber : IComparable<BigNumber>, IEquatable<BigNumber>
    {
        public BigInteger Value { get; }
        public BigNumber(BigInteger value) => Value = value;
        // 隱式轉換，方便直接用整數或 BigInteger 建立 BigNumber
        public static implicit operator BigNumber(int v) => new BigNumber(new BigInteger(v));
        public static implicit operator BigNumber(long v) => new BigNumber(new BigInteger(v));
        public static implicit operator BigNumber(BigInteger v) => new BigNumber(v);
        // 需要時也可取回 BigInteger
        public static implicit operator BigInteger(BigNumber v) => v.Value;
        // 1) 算術運算 + - * /
        public static BigNumber operator +(BigNumber a, BigNumber b) => BigInteger.Add(a.Value, b.Value);
        public static BigNumber operator -(BigNumber a, BigNumber b) => BigInteger.Subtract(a.Value, b.Value);
        public static BigNumber operator *(BigNumber a, BigNumber b) => BigInteger.Multiply(a.Value, b.Value);
        public static BigNumber operator /(BigNumber a, BigNumber b) => BigInteger.Divide(a.Value, b.Value);
        // 2) 冪次：以 ^ 表示，限定右側為 int
        public static BigNumber operator ^(BigNumber a, int exponent)
        {
            return new BigNumber(BigInteger.Pow(a.Value, exponent));
        }
        // 也提供明確的方法版本（當 exponent 需要檢查或來自 BigInteger 時）
        public static BigNumber Pow(BigNumber a, BigInteger exponent)
        {
            if (exponent > int.MaxValue || exponent < int.MinValue)
                throw new ArgumentOutOfRangeException(nameof(exponent), "次方指數必須在 int 範圍內");
            return new BigNumber(BigInteger.Pow(a.Value, (int)exponent));
        }
        // 3) 比較運算 == != < <= > >= 需要成對實作
        public static bool operator ==(BigNumber a, BigNumber b) => a.Value == b.Value;
        public static bool operator !=(BigNumber a, BigNumber b) => a.Value != b.Value;
        public static bool operator <(BigNumber a, BigNumber b) => a.Value < b.Value;
        public static bool operator >(BigNumber a, BigNumber b) => a.Value > b.Value;
        public static bool operator <=(BigNumber a, BigNumber b) => a.Value <= b.Value;
        public static bool operator >=(BigNumber a, BigNumber b) => a.Value >= b.Value;
        // 4) 介面實作
        public int CompareTo(BigNumber other) => Value.CompareTo(other.Value);
        public bool Equals(BigNumber other) => Value.Equals(other.Value);
        public override bool Equals(object obj) => obj is BigNumber bn && Equals(bn);
        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => Value.ToString();
    }
}