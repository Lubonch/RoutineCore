using MassTransit;
using RoutineCore.Dispatcher.Consumers;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddMassTransit(x =>
{
    // Register our typed consumers (modern "ActionCenter plugins")
    x.AddConsumer<PunchRegisteredConsumer>();
    x.AddConsumer<AbsenceApprovedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        var rabbitHost = builder.Configuration["RabbitMQ:Host"] ?? "localhost";
        cfg.Host(rabbitHost, "/", h =>
        {
            h.Username(builder.Configuration["RabbitMQ:Username"] ?? "guest");
            h.Password(builder.Configuration["RabbitMQ:Password"] ?? "guest");
        });

        cfg.ReceiveEndpoint("pulsedispatcher-punches", e =>
        {
            e.ConfigureConsumer<PunchRegisteredConsumer>(context);
        });

        cfg.ReceiveEndpoint("pulsedispatcher-absences", e =>
        {
            e.ConfigureConsumer<AbsenceApprovedConsumer>(context);
        });
    });
});

var host = builder.Build();
host.Run();
