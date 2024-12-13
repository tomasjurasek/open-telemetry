using Eshop.ServiceDefaults;
using System.Diagnostics.Metrics;

namespace Eshop.Basket;

public static class BasketMetrics
{
    private static readonly Counter<int> basketCreated = MeterProvider.Meter.CreateCounter<int>("basket_created");
    public static void BasketCreated() => basketCreated.Add(1);

}
