using System;

public static class Extension
{
    public static String ToTimeString(this float time)
    {
        var totalSeconds = (int)time;
        var days = totalSeconds / 86400;
        var hours = totalSeconds % 86400 / 3600;
        var minutes = totalSeconds % 3600 / 60;
        var seconds = totalSeconds % 60;

        if (days > 0)
            return $"{days}ÀÏ {hours:00}:{minutes:00}:{seconds:00}";

        if (hours > 0)
            return $"{hours:00}:{minutes:00}:{seconds:00}";

        return $"{minutes:00}:{seconds:00}";
    }
}
