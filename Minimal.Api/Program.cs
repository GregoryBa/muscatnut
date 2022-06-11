using Minimal.Api;

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

// Another way, comes from Example.cs file
app.MapGet("from-class", () => Example.SomeMethod);

// int age is Required, 404 if not provided and 400 if not int provided
app.MapGet("get-params/{age}", (int age) =>
{
    return $"Age provided was {age}";
});

// RESTRICTIONS:
// Explicitly making it an int, if not int returns 404 not found
app.MapGet("get-params/{age:int}", (int age) =>
{
    return $"Age provided was {age}";
});

// Restriction Regex: Only allow numbers and letters
app.MapGet("cars/{carId:regex(^[a-z0-9]+$)}", (string carId) =>
{
    return $"Car id provided was: {carId}";
});

// Restriction Length: 
app.MapGet("book/{isbn:length(13)}", (string isbn) =>
{
    return $"ISBM was: {isbn}";
});

// PARAMETER BINDING:
// If string is not nullable then it becomes a mandatory query paramenter
// When string? is nullable it becomes optional query parameter
app.MapGet("people/search", (string? searchTerm, PeopleService peopleService) =>
{
    if (searchTerm is null)
    {
        Results.NotFound();
    }
    
    
});

var port = Environment.GetEnvironmentVariable("PORT");
app.Run($"https://localhost:{port}");