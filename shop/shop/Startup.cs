using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using shop.Common.Constants;
using shop.Common.Exceptions;
using shop.Common.Utilities;
using shop.DAL;
using shop.DAL.Abstract;
using shop.DAL.Repository;
using shop.DAL.UnitOfWork;
using shop.Helpers;
using shop.Helpers.SwaggerFilters;
using shop.Models.ResponseModels;
using shop.Services.Interfaces;
using shop.Services.Services;
using shop.Services.StartApp;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Reflection;

namespace shop
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
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("Connection"));
                //options.UseLazyLoadingProxies(); Lazy loading
                options.EnableSensitiveDataLogging(false);
            });

            services.AddCors();

            services.Configure<DataProtectionTokenProviderOptions>(o =>
            {
                o.Name = "Default";
                o.TokenLifespan = TimeSpan.FromHours(12);
            });

            #region Register services

            #region Basis services

            services.AddScoped<IDataContext>(provider => provider.GetService<DataContext>());
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<HashUtility>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddTransient<ICatalogService, CatalogService>();
            services.AddTransient<IBasketService, BasketService>();

            #endregion

            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfileConfiguration());
            });

            services.AddSingleton(config.CreateMapper());

            #endregion

            services
                .AddDetection()
                .AddCoreServices()
                .AddRequiredPlatformServices();

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddVersionedApiExplorer(
                 options =>
                 {
                     options.GroupNameFormat = "'v'VVV";

                     // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                     // can also be used to control the format of the API version in route templates
                     options.SubstituteApiVersionInUrl = true;
                 });

            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = true;
            });

            services.AddMvc(options =>
            {
                // Allow use optional parameters in actions
                options.AllowEmptyInputInBodyModelBinding = true;
                options.EnableEndpointRouting = false;
            })
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            })
            .ConfigureApiBehaviorOptions(o => o.SuppressModelStateInvalidFilter = true)
            .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddSwaggerGen(options =>
            {
                options.EnableAnnotations();

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header,
                    Description = "Access token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                // resolve the IApiVersionDescriptionProvider service
                // note: that we have to build a temporary service provider here because one has not been created yet
                var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                // add a swagger document for each discovered API version
                // note: you might choose to skip or document deprecated API versions differently
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
                }

                // add a custom operation filter which sets default values

                // integrate xml comments
                options.IncludeXmlComments(XmlCommentsFilePath);
                options.IgnoreObsoleteActions();

                options.OperationFilter<DefaultValues>();
                options.OperationFilter<SecurityRequirementsOperationFilter>("Bearer");
            });

            // instead of options.DescribeAllEnumsAsStrings()
            services.AddSwaggerGenNewtonsoftSupport();

            var sp = services.BuildServiceProvider();
            var serviceScopeFactory = sp.GetRequiredService<IServiceScopeFactory>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
                  {
                      options.RequireHttpsMetadata = false;
                      options.TokenValidationParameters = new TokenValidationParameters
                      {
                          ValidateIssuer = true,
                          ValidIssuer = AuthOptions.ISSUER,
                          ValidateAudience = true,
                          ValidateActor = false,
                          ValidAudience = AuthOptions.AUDIENCE,
                          ValidateLifetime = true,
                          //SignatureValidator = (string token, TokenValidationParameters validationParameters) => {

                          //    var jwt = new JwtSecurityToken(token);

                          //    var signKey = AuthOptions.GetSigningCredentials().Key as SymmetricSecurityKey;

                          //    var encodedData = jwt.EncodedHeader + "." + jwt.EncodedPayload;
                          //    var compiledSignature = Encode(encodedData, signKey.Key);

                          //    //Validate the incoming jwt signature against the header and payload of the token
                          //    if (compiledSignature != jwt.RawSignature)
                          //    {
                          //        throw new Exception("Token signature validation failed.");
                          //    }

                          //    /// TO DO: initialize user claims

                          //    return jwt;
                          //},
                          LifetimeValidator = (DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters) =>
                          {
                              var jwt = securityToken as JwtSecurityToken;

                              if (!notBefore.HasValue || !expires.HasValue || DateTime.Compare(expires.Value, DateTime.UtcNow) <= 0)
                              {
                                  return false;
                              }

                              if (jwt == null)
                                  return false;

                              var isRefresStr = jwt.Claims.FirstOrDefault(t => t.Type == "isRefresh")?.Value;

                              if (isRefresStr == null)
                                  return false;

                              var isRefresh = Convert.ToBoolean(isRefresStr);

                              if (!isRefresh)
                              {
                                  try
                                  {
                                      using (var scope = serviceScopeFactory.CreateScope())
                                      {
                                          return true;
                                      }
                                  }
                                  catch (Exception ex)
                                  {
                                      var logger = sp.GetService<ILogger<Startup>>();
                                      logger.LogError(DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ") + ": Exception occured in token validator. Exception message: " + ex.Message + ". InnerException: " + ex.InnerException?.Message);
                                      return false;
                                  }
                              }

                              return false;
                          },
                          IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                          ValidateIssuerSigningKey = true
                      };
                  });

            services.AddRouting();
            services.AddMemoryCache();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider, ILoggerFactory loggerFactory,
            IApiVersionDescriptionProvider provider)
        {
            app.UseDefaultFiles();

            env.EnvironmentName = Environments.Development;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseForwardedHeaders(new ForwardedHeadersOptions
                {
                    ForwardedHeaders = ForwardedHeaders.XForwardedFor,

                    // IIS is also tagging a X-Forwarded-For header on, so we need to increase this limit, 
                    // otherwise the X-Forwarded-For we are passing along from the browser will be ignored
                    ForwardLimit = 2
                });
            }

            if (!Directory.Exists("Logs"))
            {
                Directory.CreateDirectory("Logs");
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger(options =>
            {
                options.PreSerializeFilters.Add((swagger, httpReq) =>
                {
                    //swagger.Host = httpReq.Host.Value;

                    var ampersand = "&amp;";

                    foreach (var path in swagger.Paths)
                    {
                        if (path.Value.Operations.Any(x => x.Key == OperationType.Get && x.Value.Deprecated))
                            path.Value.Operations.First(x => x.Key == OperationType.Get).Value.Description = path.Value.Operations.First(x => x.Key == OperationType.Get).Value.Description.Replace(ampersand, "&");

                        if (path.Value.Operations.Any(x => x.Key == OperationType.Delete && x.Value?.Description != null))
                            path.Value.Operations.First(x => x.Key == OperationType.Delete).Value.Description = path.Value.Operations.First(x => x.Key == OperationType.Delete).Value.Description.Replace(ampersand, "&");
                    }

                    var paths = swagger.Paths.ToDictionary(p => p.Key, p => p.Value);
                    foreach (KeyValuePair<string, OpenApiPathItem> path in paths)
                    {
                        swagger.Paths.Remove(path.Key);
                        swagger.Paths.Add(path.Key.ToLowerInvariant(), path.Value);
                    }
                });
            });

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(options =>
            {
                options.IndexStream = () => File.OpenRead("Views/Swagger/swagger-ui.html");

                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    options.InjectStylesheet("/SwaggerDark.css");
                }

                options.EnableFilter();
            });

            app.UseReDoc(c =>
            {
                c.RoutePrefix = "docs";
                c.SpecUrl("/swagger/v1/swagger.json");
                c.ExpandResponses("200");
                c.RequiredPropsFirst();
            });

            app.UseCors(builder => builder.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod());
            app.UseStaticFiles();
            app.UseRouting();

            #region Error handler

            // Different middleware for api and ui requests  
            app.UseWhen(context => context.Request.Path.StartsWithSegments("/api"), appBuilder =>
            {
                //var localizer = serviceProvider.GetService<IStringLocalizer<ErrorsResource>>();
                var logger = loggerFactory.CreateLogger("GlobalErrorHandling");

                // Exception handler - show exception data in api response
                appBuilder.UseExceptionHandler(new ExceptionHandlerOptions
                {
                    ExceptionHandler = async context =>
                    {
                        var errorModel = new ErrorResponseModel();
                        var result = new ContentResult();

                        var exception = context.Features.Get<IExceptionHandlerPathFeature>();

                        if (exception.Error is CustomException)
                        {
                            var ex = (CustomException)exception.Error;

                            result = errorModel.Error(ex);
                        }
                        else
                        {
                            var message = exception.Error.InnerException?.Message ?? exception.Error.Message;
                            logger.LogError($"{exception.Path} - {message}");

                            errorModel.AddError("general", message);
                            result = errorModel.InternalServerError(env.IsDevelopment() ? exception.Error.StackTrace : null);
                        }

                        context.Response.StatusCode = result.StatusCode.Value;
                        context.Response.ContentType = result.ContentType;

                        await context.Response.WriteAsync(result.Content);
                    }
                });

                // Handles responses with status codes (correctly executed requests, without any exceptions)
                appBuilder.UseStatusCodePages(async context =>
                    {
                        var errorResponse = ErrorHelper.GetError(context.HttpContext.Response.StatusCode);

                        context.HttpContext.Response.ContentType = "application/json";
                        await context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse, new JsonSerializerSettings { Formatting = Formatting.Indented }));
                    });
            });

            app.UseWhen(context => !context.Request.Path.StartsWithSegments("/api"), appBuilder =>
            {
                appBuilder.UseExceptionHandler("/Error");
                appBuilder.UseStatusCodePagesWithReExecute("/Error", "?statusCode={0}");
            });

            #endregion

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }

        static string XmlCommentsFilePath
        {
            get
            {
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var fileName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml";
                return Path.Combine(basePath, fileName);
            }
        }

        static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = $"shop API {description.ApiVersion}",
                Version = description.ApiVersion.ToString(),
                Description = "The shop application with Swagger and API versioning."
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }
    }
}
