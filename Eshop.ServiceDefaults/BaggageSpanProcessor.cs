using OpenTelemetry;
using System.Diagnostics;

namespace Eshop.ServiceDefaults;

public class BaggageSpanProcessor : BaseProcessor<Activity>
{
    /// <inheritdoc />
    public override void OnStart(Activity activity)
    {
        foreach (var entry in activity.Baggage)
        {
            activity.SetTag(entry.Key, entry.Value);
        }
    }
}
