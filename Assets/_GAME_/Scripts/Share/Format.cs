using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Format
{
    public static string FormatCount(float count)             // format view
    {
        if (count >= 999999999)
        {
            return (count / 1000000000f).ToString("F1") + "B";
        }
        else if (count >= 999999f)
        {
            return (count / 1000000f).ToString("F1") + "M";
        }
        else if (count >= 9999)
        {
            return (count / 1000f).ToString("F1") + "K";
        }
        else
        {
            return count.ToString();
        }
    }
}

