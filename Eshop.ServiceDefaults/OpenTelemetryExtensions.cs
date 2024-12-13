using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using MassTransit.Logging;
using MassTransit.Monitoring;
using System.Diagnostics;

namespace Eshop.ServiceDefaults;

public static class OpenTelemetryExtensions
{
    public static TBuilder AddOpenTelemetry<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.IncludeFormattedMessage = true;
            logging.IncludeScopes = true;
        });

        builder.Services.AddOpenTelemetry()
            .UseOtlpExporter()
            .WithMetrics(metrics =>
            {
                metrics
                    .AddMeter(builder.Environment.ApplicationName, InstrumentationOptions.MeterName)
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation();
            })
            .WithTracing(tracing =>
            {
                tracing.AddSource(builder.Environment.ApplicationName, DiagnosticHeaders.DefaultListenerName)
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRedisInstrumentation()
                    .AddProcessor<BaggageSpanProcessor>();
            });



        ActivityProvider.Create(builder.Environment.ApplicationName);
        MeterProvider.Create(builder.Environment.ApplicationName);

        return builder;
    }
}
