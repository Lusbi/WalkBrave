using System;
using System.Numerics;
namespace BigMath
{
    [Serializable]
    /// <summary> 
    /// /// ���]�� BigInteger�A���ѲŸ��B��]+ - * / ^�^�P����B��]== != < <= > >=�^ 
    /// /// �`�N�G�����O�N ^ �����u�����v�A�ȭ��k���� int ������ 
    /// /// </summary> 
    public readonly struct BigNumber : IComparable<BigNumber>, IEquatable<BigNumber>
    {
        public BigInteger Value { get; }
        public BigNumber(BigInteger value) => Value = value;
        // �����ഫ�A��K�����ξ�Ʃ� BigInteger �إ� BigNumber
        public static implicit operator BigNumber(int v) => new BigNumber(new BigInteger(v));
        public static implicit operator BigNumber(long v) => new BigNumber(new BigInteger(v));
        public static implicit operator BigNumber(BigInteger v) => new BigNumber(v);
        // �ݭn�ɤ]�i���^ BigInteger
        public static implicit operator BigInteger(BigNumber v) => v.Value;
        // 1) ��N�B�� + - * /
        public static BigNumber operator +(BigNumber a, BigNumber b) => BigInteger.Add(a.Value, b.Value);
        public static BigNumber operator -(BigNumber a, BigNumber b) => BigInteger.Subtract(a.Value, b.Value);
        public static BigNumber operator *(BigNumber a, BigNumber b) => BigInteger.Multiply(a.Value, b.Value);
        public static BigNumber operator /(BigNumber a, BigNumber b) => BigInteger.Divide(a.Value, b.Value);
        // 2) �����G�H ^ ��ܡA���w�k���� int
        public static BigNumber operator ^(BigNumber a, int exponent)
        {
            return new BigNumber(BigInteger.Pow(a.Value, exponent));
        }
        // �]���ѩ��T����k�����]�� exponent �ݭn�ˬd�ΨӦ� BigInteger �ɡ^
        public static BigNumber Pow(BigNumber a, BigInteger exponent)
        {
            if (exponent > int.MaxValue || exponent < int.MinValue)
                throw new ArgumentOutOfRangeException(nameof(exponent), "������ƥ����b int �d��");
            return new BigNumber(BigInteger.Pow(a.Value, (int)exponent));
        }
        // 3) ����B�� == != < <= > >= �ݭn�����@
        public static bool operator ==(BigNumber a, BigNumber b) => a.Value == b.Value;
        public static bool operator !=(BigNumber a, BigNumber b) => a.Value != b.Value;
        public static bool operator <(BigNumber a, BigNumber b) => a.Value < b.Value;
        public static bool operator >(BigNumber a, BigNumber b) => a.Value > b.Value;
        public static bool operator <=(BigNumber a, BigNumber b) => a.Value <= b.Value;
        public static bool operator >=(BigNumber a, BigNumber b) => a.Value >= b.Value;
        // 4) ������@
        public int CompareTo(BigNumber other) => Value.CompareTo(other.Value);
        public bool Equals(BigNumber other) => Value.Equals(other.Value);
        public override bool Equals(object obj) => obj is BigNumber bn && Equals(bn);
        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => Value.ToString();
    }
}