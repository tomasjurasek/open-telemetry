using System.Diagnostics;

namespace Eshop.ServiceDefaults;

public class ActivityProvider
{
    public static void Create(string serviceName)
    {
        ActivitySource = new ActivitySource(serviceName);
    }

    public static ActivitySource ActivitySource { get; private set; }
}
