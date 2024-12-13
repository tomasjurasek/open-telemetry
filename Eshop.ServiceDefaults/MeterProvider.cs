using System.Diagnostics.Metrics;

namespace Eshop.ServiceDefaults;

public class MeterProvider
{
    public static void Create(string serviceName)
    {
        Meter = new Meter(serviceName);
    }

    public static Meter Meter { get; private set; }
}
