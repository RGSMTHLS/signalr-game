var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSignalR();
builder.Services.AddSingleton<IUserManager, UserManager>();
builder.Services.AddSingleton<SocketHub>();
builder.Services.AddSingleton<GameTimer>();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();
app.MapHub<SocketHub>("/hub");

app.Services.GetRequiredService<GameTimer>().StartTimer();

app.Run();
