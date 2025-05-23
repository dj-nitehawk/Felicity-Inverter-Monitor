﻿namespace InverterMon.Server.Persistence.PVGen;

public static class PVGenExtensions
{
    public static TimeSpan BucketDuration => TimeSpan.FromMinutes(5);
    const string BucketKey = "HH:mm";

    public static string ToTimeBucket(this DateTime dt)
    {
        var delta = dt.Ticks % BucketDuration.Ticks;

        return new DateTime(dt.Ticks - delta, dt.Kind).ToString(BucketKey);
    }

    public static void AllocateBuckets(this PVGeneration pvGen, int startHour, int endHour)
    {
        var timeOfDay = new TimeOnly(startHour, 0);

        while (timeOfDay.Hour < endHour)
        {
            pvGen.WattPeaks[timeOfDay.ToString(BucketKey)] = 0;
            timeOfDay = timeOfDay.AddMinutes(BucketDuration.TotalMinutes);
        }
    }
}