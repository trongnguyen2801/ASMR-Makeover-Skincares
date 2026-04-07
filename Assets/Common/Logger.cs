using UnityEngine;

public class Logger
{
    public static bool enableLog = true;

    public static void Info(string message)
    {
        if (!enableLog) return;
        Debug.Log(message);
    }

    public static void Error(string message)
    {
        if (!enableLog) return;
        Debug.LogError(message);
    }

    public static void ErrorFormat(string format, params object[] args)
    {
        if (!enableLog) return;
        Debug.LogErrorFormat(format, args);
    }
}