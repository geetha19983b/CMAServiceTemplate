using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using CMAService.Business;
using CMAService.Repository;
//#if (AddSwagger)
using Microsoft.OpenApi.Models;
//#endif
//#if (AddSerilog)
using Serilog;
//#endif
//#if (AddPolly)
using Polly;
using Polly.Extensions.Http;
using System.Diagnostics;
//#endif
//#if (AddPromethus)
using Prometheus;
//#endif
//#if (AddHealthCheck)
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
//#endif
//#if(AddMongo)
using Microsoft.Extensions.Options;
//#endif
//#if(AddSql)
using Microsoft.EntityFrameworkCore;
//#endif
//#if(AddKafka)
using Confluent.Kafka;
//#endif
//#if(AddJager)
using OpenTracing;
using OpenTracing.Util;
//#endif
//#if(AddCouch)
using CouchDB.Driver;
//#endif
namespace CMAService.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowFromAll",
                    builder => builder
                    .WithMethods("GET", "POST", "PUT")
                    .AllowAnyOrigin()
                    .AllowAnyHeader());
            });
            services.AddControllers();

            //#if (AddHealthCheck)
            // Register health checks to be enabled in api
            services.AddHealthChecks()
                    .AddCheck("self", () => HealthCheckResult.Healthy());
            //#endif

            //#if (AddSwagger)
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CMAService API", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            //#endif

            services.AddScoped<IBusinessAccess, BusinessAccess>();
            //#if (!AddSql && !AddMongo)
             services.AddScoped<IDataAccess, DataAccess>();
            //#endif
           // services.AddScoped<IDataAccess, CouchDataAccess>();
            //#if(AddSql)
            //services.AddScoped<IDataAccess, SqlDataAccess>();

            services.AddDbContext<AuthorContext>(options =>
            {
                options.UseSqlServer(
                   Configuration.GetConnectionString("SqlDatabase"));
            });

            //#endif

            //#if(AddMongo)
            // requires using Microsoft.Extensions.Options
            services.Configure<MongoDbSettings>(
               Configuration.GetSection(nameof(MongoDbSettings)));

            services.AddSingleton<IMongoDbSettings>(sp =>
                sp.GetRequiredService<IOptions<MongoDbSettings>>().Value);

            //services.AddScoped<IDataAccess, MongoDataAccess>();


            //#endif
            //#if (AddPolly)
            IAsyncPolicy<HttpResponseMessage> httpWaitAndRetryPolicy = GetWaitAndRetryPolicy();
            IAsyncPolicy<HttpResponseMessage> circuitBreakerPolicy = GetCircuitBreakerPolicy();
            IAsyncPolicy<HttpResponseMessage> fallbackPolicy = GetFallbackPolicy();
            IAsyncPolicy<HttpResponseMessage> httpRetryPolicy = GetRetryPolicy();
            IAsyncPolicy<HttpResponseMessage> allPoliciesWrapped = Policy.WrapAsync(fallbackPolicy, httpWaitAndRetryPolicy, circuitBreakerPolicy);

            services.AddHttpClient("WeatherForecastController", client =>
            {
                client.BaseAddress = new Uri("https://localhost:44358/weatherforecast/GetErrorPollyTest?id=5");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            }).AddPolicyHandler(httpRetryPolicy);

            //#endif
            //#if(AddKafka)
            var producerConfig = new ProducerConfig();
            var consumerConfig = new ConsumerConfig();
            Configuration.Bind("producer", producerConfig);
            Configuration.Bind("consumer", consumerConfig);

            services.AddSingleton<ProducerConfig>(producerConfig);
            services.AddSingleton<ConsumerConfig>(consumerConfig);
            //#endif

            //#if(AddJager)
            services.AddSingleton<ITracer>(serviceProvider =>
            {
                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                Environment.SetEnvironmentVariable(Jaeger.Configuration.JaegerServiceName, Configuration["JagerConnection:servicename"]);
                Environment.SetEnvironmentVariable(Jaeger.Configuration.JaegerSamplerType, Configuration["JagerConnection:sampletype"]);
                Environment.SetEnvironmentVariable(Jaeger.Configuration.JaegerReporterLogSpans, Configuration["JagerConnection:reportlogspan"]);
                Environment.SetEnvironmentVariable(Jaeger.Configuration.JaegerSamplerParam, Configuration["JagerConnection:samplerparam"]);
                Environment.SetEnvironmentVariable(Jaeger.Configuration.JaegerEndpoint, Configuration["JagerConnection:endpoint"]);
                // This will log to a default localhost installation of Jaeger.
                var tracer = Jaeger.Configuration.FromEnv(loggerFactory).GetTracer();

                // Allows code that can't use DI to also access the tracer.
                GlobalTracer.Register(tracer);

                return tracer;
            });

            services.AddOpenTracing();
            //#endif
            //#if(AddRedis)
            services.AddDistributedRedisCache(option =>
            {
                option.Configuration = Configuration["RedisConnection:hostname"];
                option.InstanceName = Configuration["RedisConnection:instancename"];
            });
            //#endif

            //#if(AddCouch)
            var constring = Configuration["Couchbase:Server"];

            services.AddSingleton<CouchClient>(new CouchClient(constring));
            //#endif

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            Microsoft.Extensions.Hosting.IHostApplicationLifetime applicationLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("AllowFromAll");
            //#if (AddSwagger)
            app.UseSwagger();

            app.UseReDoc(c =>
            {
                c.RoutePrefix = "swagger";
            });
            //#endif

            //#if (AddSerilog)
            app.UseSerilogRequestLogging();
            //#endif

            app.UseHttpsRedirection();

            //#if (AddPromethus)
            app.UseMetricServer();
            //#endif
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //#if (AddHealthCheck)
                // for doing deep health check
                endpoints.MapHealthChecks("/health", new HealthCheckOptions()
                {
                    ResponseWriter = WriteResponse
                });
                // for doing only basic health check
                endpoints.MapHealthChecks("/liveness", new HealthCheckOptions()
                {
                    Predicate = r => r.Name.Contains("self")
                });
                //#endif
            });

          
        }
        //#if (AddHealthCheck)
        private static Task WriteResponse(HttpContext context, HealthReport result)
        {
            context.Response.ContentType = "application/json";

            var json = new JObject(
            new JProperty("status", result.Status.ToString()),
            new JProperty("results", new JObject(result.Entries.Select(pair =>
            new JProperty(pair.Key, new JObject(
            new JProperty("status", pair.Value.Status.ToString()),
            new JProperty("description", pair.Value.Description),
            new JProperty("data", new JObject(pair.Value.Data.Select(
            p => new JProperty(p.Key, p.Value))))))))));

            return context.Response.WriteAsync(
            json.ToString(Formatting.Indented));
        }
        //#endif
        //#if (AddPolly)
        static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(4, TimeSpan.FromSeconds(5),
                     onBreak: (ex, breakDelay) =>
                     {
                         Debug.WriteLine("SVC RESILIENCY LOG- .Breaker logging: Breaking the circuit for " + breakDelay.TotalMilliseconds + "ms!");
                         Debug.WriteLine("SVC RESILIENCY LOG-..due to: " + ex.Exception);
                     },
                     onReset: () =>
                     {
                         Debug.WriteLine("SVC RESILIENCY LOG-.Breaker logging: Call ok! Closed the circuit again!");
                     },
                     onHalfOpen: () =>
                     {
                         Debug.WriteLine("SVC RESILIENCY LOG-.Breaker logging: Half-open: Next call is a trial!");
                     });
        }
        //#endif
        //#if (AddPolly)
        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.BadRequest)
                .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
        static IAsyncPolicy<HttpResponseMessage> GetWaitAndRetryPolicy()
        {
            return Policy.HandleResult<HttpResponseMessage>
                            (r => !r.IsSuccessStatusCode)
                             .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(retryAttempt),

                            (result, span, retryCount, ctx) => Debug.WriteLine($"SVC RESILIENCY LOG-Retrying({retryCount})...")
                            );


        }
        //#endif
        //#if (AddPolly)
        static IAsyncPolicy<HttpResponseMessage> GetFallbackPolicy()
        {
            return Policy.HandleResult<HttpResponseMessage>
                    (r => !r.IsSuccessStatusCode)
                       .FallbackAsync(
                            (responseToFailedRequest, context, cancellationToken) =>
                            {
                                Debug.WriteLine("SVC RESILIENCY LOG-Fallback action is executing");
                                HttpResponseMessage httpResponseMessage = new HttpResponseMessage(responseToFailedRequest.Result.StatusCode)
                                {
                                    Content = new StringContent($"The fallback executed, the original error was {responseToFailedRequest.Result.ReasonPhrase}")
                                };
                                return Task.FromResult(httpResponseMessage);
                            },
                            (response, context) =>
                            {
                                Debug.WriteLine("SVC RESILIENCY LOG-About to call the fallback action. This is a good place to do some logging");
                                return Task.CompletedTask;
                            });
        }
        //#endif
    }
}
