using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StringUtils
{

    public static string Format<T1>(string format, T1 arg1)
    {
        return string.Format(
            format,
            arg1.ToString()
        );
    }

    public static string Format<T1, T2>(string format, T1 arg1, T2 arg2)
    {
        return string.Format(
            format,
            arg1.ToString(),
            arg2.ToString()
        );
    }

    public static string Format<T1, T2, T3>(string format, T1 arg1, T2 arg2, T3 arg3)
    {
        return string.Format(
            format,
            arg1.ToString(),
            arg2.ToString(),
            arg3.ToString()
        );
    }

    public static string Format<T1, T2, T3, T4>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        return string.Format(
            format,
            arg1.ToString(),
            arg2.ToString(),
            arg3.ToString(),
            arg4.ToString()
        );
    }
}

