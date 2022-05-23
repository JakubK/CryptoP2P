using CryptoP2P.Backend.Data;
using CryptoP2P.Backend.Hubs;
using CryptoP2P.Backend.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<ICryptoVault, CryptoVault>();
builder.Services.AddSingleton<IConnectionManager, ConnectionManager>();

builder.Services.AddDbContext<AppDbContext>(options => {
    options.UseSqlite();
});

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

builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseStaticFiles(new StaticFileOptions
{
    ServeUnknownFileTypes = true,
    FileProvider = new PhysicalFileProvider(
        Path.Combine(app.Environment.WebRootPath, "static")
    ),
    OnPrepareResponse = ctx => {
        ctx.Context.Response.Headers.Add("Content-Disposition", "attachment");
    },
    RequestPath = "/static"
});

using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dataContext.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();
app.UseCors("CorsPolicy");
app.UseEndpoints(endpoints => {
    endpoints.MapControllers();
    endpoints.MapHub<ChatHub>("/chat");
});
app.Run();