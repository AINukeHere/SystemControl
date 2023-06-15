using System;
using UnityEngine;

[System.Serializable]
public class BigInt
{
    [SerializeField]
	private string _value;
	[SerializeField]
    private bool isNagative = false;
    /// <summary>
    /// 절대값을 나타냄 음수를 나타내는 -기호는 ToString과정에서 추가됨
    /// </summary>
    public string value
    {
        get
        {
            return _value;
        }
    }
    public BigInt()
    {
        _value = "0";
    }
    public BigInt(int value, bool isNagative = false)
    {
        this.isNagative = isNagative;
        if (value < 0)
        {
            this.isNagative = !this.isNagative;
            value = Mathf.Abs(value);
        }
        _value = CheckZeroFirst(value.ToString());
    }
    /// <summary>
    /// value로 먼저 음수체크 그 후 부호를 앞에 추가할 지 isNagative로 결정
    /// </summary>
    /// <param name="value"></param>
    /// <param name="isNagative"></param>
    public BigInt(string value, bool isNagative = false)
    {
        
        if (value[0] == '-')
        {
            this.isNagative = true;
            value = value.Remove(0, 1);
        }

        //음수를 한번 더 취해주고싶다.
        if (isNagative)
            this.isNagative = !this.isNagative;

        _value = CheckZeroFirst(value);
    }
    public void SetNagative(bool bNagative)
    {
        isNagative = bNagative;
    }

    /// <summary>
    /// 앞자리에 0이 있으면 삭제함 ex) 0123 -> 123
    /// </summary>
    public static string CheckZeroFirst(string num)
    {
        int i;
        for (i = 0; i < num.Length - 1; ++i)
            if (num[i] != '0')
                break;
        if (i > 0)
        {
            char[] fixNum = new char[num.Length - i];
            for (int j = i; j < num.Length; ++j)
                fixNum[j - i] = num[j];
            return new string(fixNum);
        }
        return num;
    }
    /// <summary>
    /// 매개변수는 항상 양수여야함
    /// </summary>
    /// <param name="num1"></param>
    /// <param name="num2"></param>
    /// <returns></returns>
    public static string SubtractStringNum(string num1, string num2)
    {
        bool isNagative = false;
        if ((BigInt)num1 < (BigInt)num2)
        {
            isNagative = true;
            string temp = num1;
            num1 = num2;
            num2 = temp;
        }
        int[] inum1 = new int[num1.Length];
        int[] inum2 = new int[num2.Length];
        for (int i = 0; i < num1.Length; ++i)
            inum1[num1.Length - i - 1] = num1[i] - '0';
        for (int i = 0; i < num2.Length; ++i)
            inum2[num2.Length - i - 1] = num2[i] - '0';

        int[] iresult = new int[Mathf.Max(num1.Length, num2.Length)];
        bool isLack = false;
        for (int i = 0; i < iresult.Length; ++i)
        {
            int n1 = 0, n2 = 0;
            if (i < inum1.Length)
                n1 = inum1[i];
            if (i < inum2.Length)
                n2 = inum2[i];
            if (isLack)
            {
                n1--;
                isLack = false;
            }
            if ((n1 - n2) < 0)
            {
                isLack = true;
                iresult[i] = (n1 - n2) + 10;
            }
            else
                iresult[i] = n1 - n2;
        }

        char[] cresult;
        cresult = new char[iresult.Length];
        for (int i = 0; i < iresult.Length; ++i)
            cresult[iresult.Length - i - 1] = (char)(iresult[i] + '0');

        string result = BigInt.CheckZeroFirst(new string(cresult));
        if (isNagative)
            return "-" + result;
        return result;
    }
    /// <summary>
    /// 매개변수는 항상 양수여야함
    /// </summary>
    /// <param name="num1"></param>
    /// <param name="num2"></param>
    /// <returns></returns>
    public static string AddStringNum(string num1, string num2)
    {
        int[] inum1 = new int[num1.Length];
        int[] inum2 = new int[num2.Length];
        for (int i = 0; i < num1.Length; ++i)
            inum1[num1.Length - i - 1] = num1[i] - '0';
        for (int i = 0; i < num2.Length; ++i)
            inum2[num2.Length - i - 1] = num2[i] - '0';

        int[] iresult = new int[Mathf.Max(num1.Length, num2.Length)];
        bool isFull = false;
        for (int i = 0; i < iresult.Length; ++i)
        {
            int n1 = 0, n2 = 0;
            if (i < inum1.Length)
                n1 = inum1[i];
            if (i < inum2.Length)
                n2 = inum2[i];
            if (isFull)
            {
                n1++;
                isFull = false;
            }
            if ((n1 + n2) >= 10)
                isFull = true;
            iresult[i] = (n1 + n2) % 10;
        }

        char[] result;
        if (isFull)
        {
            result = new char[iresult.Length + 1];
            result[0] = '1';
            for (int i = 0; i < iresult.Length; ++i)
                result[iresult.Length - i] = (char)(iresult[i] + '0');
        }
        else
        {
            result = new char[iresult.Length];
            for (int i = 0; i < iresult.Length; ++i)
                result[iresult.Length - i - 1] = (char)(iresult[i] + '0');
        }
        return new string(result);
    }
    /// <summary>
    /// a가 b보다 큰가. 매개변수는 항상 양수여야함
    /// </summary>
    /// <param name="num1"></param>
    /// <param name="num2"></param>
    /// <returns></returns>
    private static bool BiggerThanStringNum(string num1, string num2, bool EqualCheck = false)
    {
        int a_len = num1.Length;
        int b_len = num2.Length;

        if (a_len == b_len)
        {
            for (int i = 0; i < a_len; ++i)
            {
                if (num1[i] == num2[i])
                    continue;
                else if (num1[i] > num2[i])
                    return true;
                else
                    return false;
            }
            return EqualCheck;
        }
        else if (a_len > b_len)
            return true;
        else
            return false;
    }
    /// <summary>
    /// a 가 b 보다 작은가.
    /// 매개변수는 항상 양수여야함
    /// </summary>
    /// <param name="num1"></param>
    /// <param name="num2"></param>
    /// <returns></returns>
    private static bool SmallerThanStringNum(string num1, string num2, bool EqualCheck = false)
    {
        int a_len = num1.Length;
        int b_len = num2.Length;
        if (a_len == b_len)
        {
            for (int i = 0; i < a_len; ++i)
            {
                if (num1[i] == num2[i])
                    continue;
                else if (num1[i] < num2[i])
                    return true;
                else
                    return false;
            }
            return EqualCheck;
        }
        else if (a_len < b_len)
            return true;
        else
            return false;
    }
    public static BigInt operator +(BigInt a, BigInt b)
    {
        //5 + 3
        if (!a.isNagative && !b.isNagative)
            return new BigInt(AddStringNum(a.value, b.value));
        //5 + -3 = 5 - 3
        else if (!a.isNagative && b.isNagative)
            return new BigInt(SubtractStringNum(a.value, b.value));
        //-5 + 3 = -(5 - 3)
        else if (a.isNagative && !b.isNagative)
            return new BigInt(SubtractStringNum(a.value, b.value), true);
        // -5 + -3 = -(5 + 3)
        else if (a.isNagative && b.isNagative)
            return new BigInt(AddStringNum(a.value, b.value), true);

        Debug.Log("BigInt operator + error");
        return null;
    }
    public static BigInt operator -(BigInt a, BigInt b)
    {
        // 5 - 3, 3 - 5
        if (!a.isNagative && !b.isNagative)
            return new BigInt(SubtractStringNum(a.value, b.value));
        //5 - -3 = 5+3
        else if (!a.isNagative && b.isNagative)
            return new BigInt(AddStringNum(a.value, b.value));
        //-5 - 3 = -(5+3)
        else if (a.isNagative && !b.isNagative)
            return new BigInt(AddStringNum(a.value, b.value), true);
        //(-5) - (-3) = -(5 - 3)
        else if (a.isNagative && b.isNagative)
            return new BigInt(SubtractStringNum(a.value, b.value), true);
        Debug.LogError("BigInt operator - error");
        return null;
    }
    public static BigInt operator -(BigInt a)
    {
        a.isNagative = !a.isNagative;
        return a;
    }
    public static bool operator >(BigInt a, BigInt b)
    {
        //0은 양수 음수 체크가 안되므로 따로 처리
        if ((a.value == "0" && b.value == "0"))
            return false;

        //+,+
        if (!a.isNagative && !b.isNagative)
            return BiggerThanStringNum(a.value, b.value);
        //+,-
        else if (!a.isNagative && b.isNagative)
            return true;
        //-,+
        else if (a.isNagative && !b.isNagative)
            return false;
        //-,-
        else if (a.isNagative && b.isNagative)
            return !BiggerThanStringNum(a.value, b.value);
        else
        {
            Debug.LogError("WTF!");
            return false;
        }
    }
    public static bool operator <(BigInt a, BigInt b)
    {
        //0은 양수 음수 체크가 안되므로 따로 처리
        if ((a.value == "0" && b.value == "0"))
            return false;

        //+,+
        if (!a.isNagative && !b.isNagative)
            return SmallerThanStringNum(a.value, b.value);
        //+,-
        else if (!a.isNagative && b.isNagative)
            return false;
        //-,+
        else if (a.isNagative && !b.isNagative)
            return true;
        //-,-
        else if (a.isNagative && b.isNagative)
            return !SmallerThanStringNum(a.value, b.value);
        else
        {
            Debug.LogError("WTF!");
            return false;
        }
    }
    public static bool operator >=(BigInt a, BigInt b)
    {
        return !(a < b);
    }
    public static bool operator <=(BigInt a, BigInt b)
    {
        return !(a > b);
    }
    public static BigInt operator %(BigInt a, BigInt b)
    {
        BigInt result = new BigInt(a.value);
        while (result >= b)
            result -= b;
        return result;
    }
    //암시적 형변환만 허용
    public static explicit operator BigInt(int value)
    {
        return new BigInt(value);
    }
    public static explicit operator BigInt(string value)
    {
        return new BigInt(value);
    }
    public static explicit operator int (BigInt v)
    {
        return int.Parse(v.ToString());
    }

    public override string ToString()
    {
        if (isNagative)
            return "-" + value;
        else
            return value;
    }
    /// <summary>
    /// 0 ~ value 중 하나의 값을 반환합니다.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static BigInt RandomRange(BigInt value)
    {
        char[] result;
        result = new char[value.value.Length + 3];
        for (int i = 0; i < result.Length; ++i)
        {
            result[i] = (char)(UnityEngine.Random.Range('0', '9' + 1));
        }
        return new BigInt(new string(result)) % (value + (BigInt)1);
    }
    /// <summary>
    /// 값을 복제한 객체를 반환합니다.
    /// </summary>
    /// <returns></returns>
    public BigInt Clone()
    {
        return new BigInt(ToString());
    }
}