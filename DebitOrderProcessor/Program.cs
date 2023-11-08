using DebitOrderProcessor;
using DebitOrderProcessor.Services;
using Microsoft.Extensions.Configuration;
using Serilog;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
    })
   .ConfigureServices((hostContext, services) =>
   {
       IConfiguration configuration = hostContext.Configuration;

       services.AddSingleton<XmlService>();
       services.AddSingleton(configuration);
       services.AddSingleton<DebitOrderProcessService>();
       services.AddLogging(loggingBuilder =>
       {
           loggingBuilder.ClearProviders();
           loggingBuilder.AddSerilog(new LoggerConfiguration()
               .WriteTo.File("log.txt")
               .CreateLogger());
       });
       services.AddHostedService<Worker>();
   })


    .Build();

host.Run();


