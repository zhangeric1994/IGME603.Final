/**
 * @author SerapH
 */

using UnityEngine;

public struct LogUtility
{
    public static string MakeLogString(string type, object content)
    {
        return "[" + System.DateTime.Now.ToShortTimeString() + "][" + type + "] " + content.ToString();
    }

    public static string MakeLogStringFormat(string type, string format, params object[] args)
    {
        return "[" + System.DateTime.Now.ToShortTimeString() + "][" + type + "] " + string.Format(format, args);
    }

    public static void PrintLog(string type, object content)
    {
#if UNITY_EDITOR
        Debug.Log(MakeLogString(type, content));
#endif
    }

    public static void PrintLogFormat(string type, string format, params object[] args)
    {
#if UNITY_EDITOR
        Debug.Log(MakeLogStringFormat(type, format, args));
#endif
    }
}
