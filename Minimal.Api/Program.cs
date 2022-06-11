var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("example", () => "Hello from GET");
app.MapPost("example", () => "Hello from POST");

app.MapGet("ok-object", () => Results.Ok(new
{
    Name = "Gregory Baranowski"
}));

app.MapGet("slow-request", async () =>
{
    await Task.Delay(1000);
    return Results.Ok(new
    {
        Name = "Gregory Baranowski"
    });
});

app.MapGet("get", () => "This is a GET");
app.MapPost("post", () => "This is a POST");
app.MapPut("put", () => "This ia a PUT");
app.MapDelete("delete", () => "This is a DELETE");

// Minimal API does not support head or option out of the box. This ia a way to implement those:
app.MapMethods("options-or-head", new[] { "HEAD", "OPTIONS" }, () =>
    "Hello from Head or options");

// A way to pass in the method.
var handler = () => "This is coming from a var";
app.MapGet("handler", handler);

// Another way

var port = Environment.GetEnvironmentVariable("PORT");
app.Run($"https://localhost:{port}");