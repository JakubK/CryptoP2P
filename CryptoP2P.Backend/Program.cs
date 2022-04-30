using CryptoP2P.Backend.Hubs;
using CryptoP2P.Backend.Services;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<ICryptoVault, CryptoVault>();
builder.Services.AddSingleton<IConnectionManager, ConnectionManager>();
builder.Services.AddSingleton<ICryptoManager, CryptoManager>();
builder.Services.AddSignalR(options => {
    options.EnableDetailedErrors = true; 
});
builder.Services.AddControllers();

builder.Services.AddCors(options => options.AddPolicy("CorsPolicy",
    builder =>
    {
        builder.AllowAnyHeader()
            .AllowAnyMethod()
            .SetIsOriginAllowed((host) => true)
            .AllowCredentials();
    }));

var app = builder.Build();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(app.Environment.WebRootPath, "static")
    ),
    OnPrepareResponse = ctx => {
        ctx.Context.Response.Headers.Add("Content-Disposition", "attachment");
    },
    RequestPath = "/static"
});
app.UseRouting();
app.UseCors("CorsPolicy");
app.UseEndpoints(endpoints => {
    endpoints.MapControllers();
    endpoints.MapHub<ChatHub>("/chat");
});
app.Run();