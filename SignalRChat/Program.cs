using SignalRChat.Hubs;
using SignalRChat.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<ICryptoVault, CryptoVault>();
builder.Services.AddSingleton<IConnectionManager, ConnectionManager>();
builder.Services.AddSingleton<ICryptoManager, CryptoManager>();
builder.Services.AddSignalR(options => {
    options.EnableDetailedErrors = true; 
});

builder.Services.AddCors(options => options.AddPolicy("CorsPolicy",
    builder =>
    {
        builder.AllowAnyHeader()
            .AllowAnyMethod()
            .SetIsOriginAllowed((host) => true)
            .AllowCredentials();
    }));

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseCors("CorsPolicy");
app.UseEndpoints(endpoints => {
    endpoints.MapHub<ChatHub>("/chat");
});
app.Run();
