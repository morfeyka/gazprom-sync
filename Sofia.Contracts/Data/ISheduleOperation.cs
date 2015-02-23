using System;

namespace Sofia.Contracts.Data
{
    public interface ISheduleOperation
    {
        string Name { get; set; }
        TimeSpan StartOffset { get; set; }
        long Interval { get; set; }
        int CountLanches { get; set; }
        int? CountOccurs { get; set; }
    }
}