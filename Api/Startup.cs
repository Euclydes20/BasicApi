using Api.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Security.Claims;

namespace Api
{
    public class Startup
    {
        internal static void SetAuthenticationService(IServiceCollection services)
        {
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;

                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        System.Text.Encoding.ASCII.GetBytes(TokenService.SECRETKEY)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // Retira o tempo extra (geralmente 5 minutos) de validade do token
                    ClockSkew = TimeSpan.Zero
                };
                x.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;

                        if (string.IsNullOrEmpty(context.Token) &&
                        (path.StartsWithSegments("/SessionService")))
                        {
                            if (!string.IsNullOrEmpty(accessToken))
                            {
                                context.Request.Headers["Authorization"] = "Bearer " + accessToken;
                                context.Token = accessToken;
                            }
                        }

                        return Task.CompletedTask;
                    }
                };
            });

            services.AddAuthorization(o =>
            {
                var defaultAuthorizationPolicyBuilder = 
                    new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme, "Bearer")
                    .RequireAuthenticatedUser().Build();
                
                o.DefaultPolicy = defaultAuthorizationPolicyBuilder;
            });

            services.Configure<IISOptions>(options =>
            {
                options.ForwardClientCertificate = false;
            });
        }

        internal static void SetApiDocumentation(IServiceCollection services)
        {
            var versaoAPI = Assembly.GetEntryAssembly().GetName().Version;

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "LEARN - Euclydes - API Core",
                    Description = $"API - Versao {versaoAPI}",
                    Version = "v1 "
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Token de autenticação, após informar usuário e senha copie o token gerado, no botão autorizar, digite: Bearer [seu Token]",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                           // Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
                c.EnableAnnotations();

                //c.OrderActionsBy((apiDesc) => $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.HttpMethod}");
                //c.SchemaFilter<SwaggerRemovePropertyFilter>();

                // Set the comments path for the Swagger JSON and UI.
                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //c.IncludeXmlComments(xmlPath);
            });
        }

        internal static void SetDocumentationCustomization(IApplicationBuilder app)
        {

            var title = Assembly.GetEntryAssembly().GetName().Name;
            var versaoAPI = Assembly.GetEntryAssembly().GetName().Version;

            app.UseSwagger(c =>
            {
                c.RouteTemplate = "documentation/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/documentation/v1/swagger.json", $"{title} Versão {versaoAPI}");
                c.RoutePrefix = "documentation";

                c.InjectJavascript("/assets/js/doc.js");
                c.InjectStylesheet("/assets/css/custom.css");
                c.DocumentTitle = "BASIC API Core";

                //c.ConfigObject.AdditionalItems.Add("syntaxHighlight", false);
            });
        }
    }
}
