using Api;
using Api.Infra.Database;
using Microsoft.AspNetCore.Rewrite;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMvc()
    .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddDirectoryBrowser();

Startup.SetAuthenticationService(builder.Services);
Startup.SetApiDocumentation(builder.Services);

var app = builder.Build();

try
{
    DatabaseConnection.LoadDatabaseConfig(DbType.Postgres);
    DatabaseConnection.CreateDatabase();
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

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
