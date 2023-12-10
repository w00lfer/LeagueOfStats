using NodaTime;

namespace LeagueOfStats.Domain.Common;

public class Clock : IClock
{
    static Clock() => Clock.CurrentClock = (IClock) SystemClock.Instance;

    protected Clock()
    {
    }
    
    protected static IClock CurrentClock { get; set; }
    
    public static Instant GetCurrentInstant() => Clock.CurrentClock.GetCurrentInstant();
    
    Instant IClock.GetCurrentInstant() => Clock.CurrentClock.GetCurrentInstant();
}