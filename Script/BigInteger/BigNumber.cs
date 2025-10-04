using System;
using System.Numerics;

namespace BigMath
{
    /// <summary>
    /// ���]�� BigInteger�A���ѲŸ��B��]+ - * / ^�^�P����B��]== != < <= > >=�^
    /// �`�N�G�����O�N ^ �����u�����v�A�ȭ��k���� int ������
    /// </summary>
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
        public static BigNumber operator +(BigNumber a, BigNumber b) => new BigNumber(a.Value + b.Value);
        public static BigNumber operator -(BigNumber a, BigNumber b) => new BigNumber(a.Value - b.Value);
        public static BigNumber operator *(BigNumber a, BigNumber b) => new BigNumber(a.Value * b.Value);
        public static BigNumber operator /(BigNumber a, BigNumber b) => new BigNumber(a.Value / b.Value);

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

    /// <summary>
    /// �̧A�ݨD�O�d Utils�G���� a �� b = result ���r��榡�u��
    /// </summary>
    public static class BigIntegerUtils
    {
        public enum Operator
        {
            Add, Subtract, Multiply, Divide, Pow, Compare
        }

        public static string ToSymbol(Operator op) => op switch
        {
            Operator.Add => "+",
            Operator.Subtract => "-",
            Operator.Multiply => "*",
            Operator.Divide => "/",
            Operator.Pow => "^",
            Operator.Compare => "cmp",
            _ => "?"
        };

        /// <summary>
        /// �^�Ǧr��G"a op b = result"�A�Y�� Compare �h��X a ? b�]<, =, >�^
        /// </summary>
        public static string FormatEval(BigNumber a, Operator op, BigNumber b)
        {
            switch (op)
            {
                case Operator.Add: return $"{a} + {b} = {a + b}";
                case Operator.Subtract: return $"{a} - {b} = {a - b}";
                case Operator.Multiply: return $"{a} * {b} = {a * b}";
                case Operator.Divide: return $"{a} / {b} = {a / b}";
                case Operator.Pow:
                    // �o�̥� ^ �� int �����A�Y�ݭn BigInteger ���ƽЧ�� BigNumber.Pow(a, exponent)
                    if (b.Value > int.MaxValue || b.Value < int.MinValue)
                        throw new ArgumentOutOfRangeException(nameof(b), "������ƥ����b int �d��");
                    return $"{a} ^ {b} = {(a ^ (int)b.Value)}";
                case Operator.Compare:
                    var cmp = a.CompareTo(b);
                    var sign = cmp < 0 ? "<" : (cmp > 0 ? ">" : "=");
                    return $"{a} {sign} {b}";
                default:
                    throw new NotSupportedException($"���䴩���B��: {op}");
            }
        }
    }
}
