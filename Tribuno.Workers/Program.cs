using Tribuno.Kafka.DependencyInjection;
using Tribuno.Worker;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddKafka(builder.Configuration);

builder.Services.AddHostedService<Worker>();

var host = builder.Build();

host.Run();