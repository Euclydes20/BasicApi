using Api;
using Api.Infra.Database;
using Api.Security;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Rewrite;
using MyCSharp.HttpUserAgentParser.DependencyInjection;
using System.Net.Sockets;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading.RateLimiting;
using YamlDotNet.Core.Tokens;
using Microsoft.AspNetCore.DataProtection;
using static LinqToDB.Sql;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMvc().AddJsonOptions(options =>
{
    // options.JsonSerializerOptions.IgnoreNullValues = true;
    options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    options.JsonSerializerOptions.PropertyNamingPolicy = null; //O padrão é "CamelCase"
});

builder.Services.AddCors();
builder.Services.AddControllers(options =>
{
    // > .NET6 Só ignora valor null com esta definição
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
});
builder.Services.AddDirectoryBrowser();

builder.Services.AddBrowserDetection();
builder.Services.AddHttpUserAgentParser();

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    /*options.AddPolicy("fixed", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            httpContext.Connection.RemoteIpAddress?.ToString(),
            options => new FixedWindowRateLimiterOptions()
            {
                PermitLimit = 10,
                Window = TimeSpan.FromMinutes(1),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0,
            }));*/

    /*options.AddPolicy("fixed2", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            httpContext.Connection.RemoteIpAddress?.ToString(),
            options => new FixedWindowRateLimiterOptions()
            {
                PermitLimit = 2,
                Window = TimeSpan.FromSeconds(4),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0,
                AutoReplenishment = true,
            }));*/

    /*options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            httpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty,
            partition => new FixedWindowRateLimiterOptions()
            {
                PermitLimit = 10,
                Window = TimeSpan.FromMinutes(1),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0,
                AutoReplenishment = true,
            }));*/

    options.AddFixedWindowLimiter("fixed", options =>
    {
        options.PermitLimit = 10;
        options.Window = TimeSpan.FromMinutes(1);
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        options.QueueLimit = 0;
        options.AutoReplenishment = true;
    });

    options.AddFixedWindowLimiter("fixed2", options =>
    {
        options.PermitLimit = 2;
        options.Window = TimeSpan.FromSeconds(4);
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        options.QueueLimit = 0;
        options.AutoReplenishment = true;
    });

    options.OnRejected = async (context, cancellationToken) =>
    {
        if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
        {
            await context.HttpContext.Response.WriteAsync(
                $"Too many requests. Please try again after {retryAfter.TotalMinutes} minute(s). " +
                $"Read more about our rate limits at https://example.org/docs/ratelimiting.", cancellationToken);
        }
        else
        {
            await context.HttpContext.Response.WriteAsync(
                "Too many requests. Please try again later. " +
                "Read more about our rate limits at https://example.org/docs/ratelimiting.", cancellationToken);
        }

        /*context.HttpContext.Response.Headers.Add("Retry-After", "60");
        await context.HttpContext.Response.WriteAsync("Rate limit exceeded. Try again in 60 seconds.", cancellationToken);*/
    };
});

Startup.SetAuthenticationService(builder.Services);
Startup.SetApiDocumentation(builder.Services);
DatabaseConnection.LoadDatabaseConfig(DbType.Postgres);
ServiceRegister.Register(builder.Services);

var app = builder.Build();

try
{
    DatabaseConnection.CreateDatabase();
    using (var scope = app.Services.CreateScope())
    {
        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        runner.MigrateUp();
    }    
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.Error.WriteLine(ex.Message);
    Console.ResetColor();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
    app.UseExceptionHandler("/Error");

app.UseHttpsRedirection();
// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
app.UseHsts();

Startup.SetDocumentationCustomization(app);

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseRateLimiter();
app.UseAuthorization();

app.UseCors(x =>
{
    x.SetIsOriginAllowed(orgin => true);
    x.AllowAnyHeader();
    x.AllowAnyMethod();
    x.AllowCredentials();
});

var rewriteOptions = new RewriteOptions();
rewriteOptions.AddRedirect("^$", "index.html");
app.UseRewriter(rewriteOptions);

//app.UseSession();
app.UseAuthentication();
app.UseMiddleware<AuthorizationMiddleware>();
app.UseWebSockets();
app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllers().RequireRateLimiting("fixed");
});


app.Map("/ws", async (context) =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        using var ws = await context.WebSockets.AcceptWebSocketAsync();
        while (true)
        {
            if (ws.State != WebSocketState.Open)
                break;

            // Enviar mensagem
            string message = "The current time is: " + DateTime.Now.ToString("HH:mm:ss");
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            ArraySegment<byte> arraySegment = new(bytes, 0, bytes.Length);
            await ws.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);

            await Task.Delay(1000);
            //Thread.Sleep(10000);
        }
        return;
    }
    context.Response.StatusCode = StatusCodes.Status400BadRequest;
});

List<WebSocket> connections = [];
static async Task ReceiveMessage(WebSocket socket, Action<WebSocketReceiveResult, byte[]> handleMessage)
{
    byte[] buffer = new byte[1024 * 4];
    while (socket.State is WebSocketState.Open)
    {
        var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        handleMessage(result, buffer);
    }
}
async Task Broadcast(string message)
{
    if (!string.IsNullOrWhiteSpace(message))
    {
        if (message.EndsWith("number 7"))
            throw new Exception("Number 7 is not allowed");
        message += ". ";
    }
    byte[] bytes = Encoding.UTF8.GetBytes(message + "The current time is: " + DateTime.Now.ToString("HH:mm:ss"));
    foreach (var socket in connections)
    {
        if (socket.State is WebSocketState.Open)
        {
            var arraySegment = new ArraySegment<byte>(bytes, 0, bytes.Length);
            await socket.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}
app.Map("/ws2", async (context) =>
{
    /*string? token = context.Request.Query["token"];
    if (string.IsNullOrWhiteSpace(token))
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return;
    }

    token = token.Insert(0, "Bearer ");
    context.Request.Headers.TryAdd("Authorization", token);

    AuthenticateResult authenticateResult = await context.AuthenticateAsync();
    if (!authenticateResult.Succeeded)
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return;
    }*/

    if (context.WebSockets.IsWebSocketRequest)
    {
        string? curName = context.Request.Query["name"];
        if (string.IsNullOrWhiteSpace(curName))
            curName = "Anonymous";

        using var ws = await context.WebSockets.AcceptWebSocketAsync();
        connections.Add(ws);

        await Broadcast($"{curName} joined the room");
        await Broadcast($"{connections.Count} users connected");
        await ReceiveMessage(ws, async (result, buffer) =>
        {
            try
            {
                if (result.MessageType is WebSocketMessageType.Text)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    await Broadcast($"{curName}: {message}");
                    return;
                }
                if (result.MessageType is WebSocketMessageType.Close || ws.State is WebSocketState.Aborted)
                {
                    connections.Remove(ws);
                    await Broadcast($"{curName} left the room");
                    await Broadcast($"{connections.Count} users connected");
                    await ws.CloseAsync(result.CloseStatus ?? WebSocketCloseStatus.NormalClosure, result.CloseStatusDescription, CancellationToken.None);
                    return;
                }
            }
            catch (Exception ex)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(ex.Message);
                var arraySegment = new ArraySegment<byte>(bytes, 0, bytes.Length);
                await ws.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        });
        return;
    }
    context.Response.StatusCode = StatusCodes.Status400BadRequest;
}).RequireAuthorization();

app.Run();
