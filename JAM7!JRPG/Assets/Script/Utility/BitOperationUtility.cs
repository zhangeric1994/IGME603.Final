/**
 * @author SerapH
 */

using System;
using System.Collections.Generic;

public struct BitOperationUtility
{
    public static int ReadBit(int number, int index)
    {
        if (index < 0 || index > 31)
            throw new ArgumentException(string.Format("[BitOperationUtility] Invalid bit to read ({0})", index));

        return number & (1 << index);
    }

    public static void WriteBit(ref int number, int index, int value)
    {
        if (value != 0 && value != 1)
            throw new ArgumentException(string.Format("[BitOperationUtility] Invalid value to write ({0})", value));

        if (ReadBit(number, index) != value)
        {
            if (value == 0)
                number -= 1 << index;
            else
                number += 1 << index;
        }
    }

    public static void WriteBit(ref int number, int index, bool flag)
    {
        WriteBit(ref number, index, flag ? 1 : 0);
    }

    public static int WriteBit(int number, int index, int value)
    {
        WriteBit(ref number, index, value);
        return number;
    }

    public static int WriteBit(int number, int index, bool flag)
    {
        return WriteBit(number, index, flag ? 1 : 0);
    }

    public static void WriteBits(ref int A, int B, int lowerIndex, int upperIndex)
    {
        int mask = WriteZeros(lowerIndex, upperIndex);

        A = (A & mask) | (B & ~mask);
    }

    public static int WriteBits(int A, int B, int lowerIndex, int upperIndex)
    {
        WriteBits(ref A, B, lowerIndex, upperIndex);
        return A;
    }

    public static int WriteOnes(int lowerIndex, int upperIndex)
    {
        return ~WriteZeros(lowerIndex, upperIndex);
    }

    public static int WriteZeros(int lowerIndex, int upperIndex)
    {
        if (lowerIndex > upperIndex)
            throw new ArgumentException(string.Format("[BitOperationUtility] Invalid interval to write ({0} ~ {1})", lowerIndex, upperIndex));

        if (lowerIndex == upperIndex)
            return WriteBit(-1, lowerIndex, 0);

        return WriteOnesFromLeft(31 - upperIndex) + WriteOnesFromRight(lowerIndex);
    }

    public static List<int> GetIndicesOfOne(int number, int lowerIndex = 0, int upperIndex = 31)
    {
        List<int> list = new List<int>();

        GetIndicesOfOne(number & WriteOnes(lowerIndex, upperIndex), 0, 31, ref list);

        return list;
    }

    private static void GetIndicesOfOne(int number, int lowerIndex, int upperIndex, ref List<int> list)
    {
        if (number == 0)
            return;

        if (lowerIndex == upperIndex)
        {
            list.Add(lowerIndex);

            return;
        }

        int mid = (upperIndex + lowerIndex) / 2;

        GetIndicesOfOne(number & WriteOnes(lowerIndex, mid), lowerIndex, mid, ref list);
        GetIndicesOfOne(number & WriteOnes(mid + 1, upperIndex), mid + 1, upperIndex, ref list);
    }

    private static int WriteOnesFromRight(int n)
    {
        if (n == 0)
            return 0;

        return ~WriteOnesFromLeft(32 - n);
    }

    private static int WriteOnesFromLeft(int n)
    {
        if (n == 0)
            return 0;

        return int.MinValue >> n - 1;
    }
}
