/**
 * @author SerapH
 */

using System;

public struct TimeUtility
{

    public static long localTimeInMilisecond
    {
        get
        {
            return (long)decimal.Divide(DateTime.UtcNow.Ticks - 621355968000000000, 10000);
        }
    }

    public static int localTime
    {
        get
        {
            return (int)decimal.Divide(DateTime.UtcNow.Ticks - 621355968000000000, 10000000);
        }
    }
}
