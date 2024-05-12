namespace NewsService.Core.Services;

public class BackgroundNewsServiceConfiguration
{
    public TimeSpan ScaningIntervalTime { get; init; }

    public BackgroundNewsServiceConfiguration(TimeSpan scaningIntervalTime) => ScaningIntervalTime = scaningIntervalTime;
}