var builder = WebApplication.CreateBuilder(args);
// Services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
// Middleware
app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("get-example", () => "Hello from GET");

var port = Environment.GetEnvironmentVariable("PORT");
app.Run($"https://localhost:{port}");