using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Minimal.Api;

var builder = WebApplication.CreateBuilder(args);
 
// service registration starts here
builder.Services.AddSingleton<PeopleService>();
builder.Services.AddSingleton<GuidGenerator>();

// Service registrations stops here
var app = builder.Build();

// MIDDLEWARE order matters
app.UseAuthorization(); //  adding middleware has to be registered before the endpoints

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
// If string is not nullable then it becomes a mandatory query parameter
// When string? is nullable it becomes optional query parameter
app.MapGet("people/search", (string? searchTerm, PeopleService peopleService) =>
{
    if (searchTerm is null)
    {
        Results.NotFound();
    }

    var results = peopleService.Search(searchTerm);
    return Results.Ok(results);
});

// You don't need to specify where does it come from but you can:
app.MapGet("mix/{routeParam}", 
    (string routeParam, int queryParam, GuidGenerator guidGenerator) =>
    {
        return $"{routeParam} {queryParam} {guidGenerator.NewGuid}";
    });

app.MapGet("mix2/{routeParam}", (
        [FromRoute] string routeParam,
        [FromQuery(Name = "query")] int queryParam,
        [FromServices] GuidGenerator guidGenerator,
        [FromHeader(Name = "Accept-Encoding")] string encoding) =>
    {
        return $"{routeParam} {queryParam} {guidGenerator.NewGuid} {encoding}";
    });

app.MapPost("people", (Person person) =>
{
    return Results.Ok(person);
});

// Special parameters:
// context = the same as HttpContext. You don't need to specify it
app.MapGet("httpcontext-1", async context =>
{
    await context.Response.WriteAsync("Hello from HttpContext 1");
});

app.MapGet("httpcontext-2", async (HttpContext context) =>
{
    await context.Response.WriteAsync("Hello from HttpContext 1");
});

//HttpContext tells you about HttpRequest and HttpResponse
// Using those two separately gives you more narrow definition
app.MapGet("http", async (HttpRequest request, HttpResponse response) =>
{
    var queries = request.QueryString.Value;
    await response.WriteAsync($"Hello from HttpResponse. Queries where {queries}");
});

// You can get claims from the user
app.MapGet("claims", (ClaimsPrincipal user) =>
{
    var claims = user.Claims;
    var identities = user.Identities;
});

// You can pass in cancellation token and use it for database calls or anything you need in your application
app.MapGet("cancel", (CancellationToken token) =>
{
    return Results.Ok();
});

// Parameter binding:
// When we want to get data from query =?5.01,51.30
// Normally we would split them inside a controller like this:
app.MapGet("map-point", (string latAndLong) =>
{
    // mapping logic
});

// Minimal API approach: In this case TryParse form MapPoint is being called
app.MapGet("map-point", (MapPoint point) =>
{
    return Results.Ok(point);
});

// Couple of ways doing the same thing
app.MapGet("simple-string", () => "Hello world");
app.MapGet("json-obj", () => new { message = "Hello world" });
app.MapGet("ok-obj", () => Results.Ok(new {message = "Hello world"}));
app.MapGet("ok-obj", () => Results.Json(new {message = "Hello world"}));
app.MapGet("text-string", () => Results.Text("Hello world"));

app.MapGet("stream-result", () =>
{
    var memoryStream = new MemoryStream();
    var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8);
    streamWriter.Write("Hello world");
    streamWriter.Flush();
    memoryStream.Seek(0, SeekOrigin.Begin);
    return Results.Stream(memoryStream);
});

var port = Environment.GetEnvironmentVariable("PORT");
app.Run($"https://localhost:{port}");