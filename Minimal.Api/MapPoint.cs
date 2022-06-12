using System.Reflection;

namespace Minimal.Api;

public class MapPoint
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    // Solution 1 for parsing the request body
    /*
    public static bool TryParse(string? value, out MapPoint? point)
    {
        try
        {
            var splitValue = value?.Split(',').Select(double.Parse).ToArray();
            point = new MapPoint()
            {
                Latitude = splitValue![0],
                Longitude = splitValue[1]
            };
            return true;
        }
        catch (Exception e)
        {
            point = null;
            return false;
        }
    }
    */
    
    // This automatically gets bind and HttpContext and ParameterInfo gets injecected
    // Solution 2 for parsing request body: gives more flexitibility

    public static async ValueTask<MapPoint?> BindAsync(HttpContext context, ParameterInfo parameterInfo)
    {
        // Read the request body till the end and parse it as a string
        var rawRequestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
        
        try
        {
            var splitValue = rawRequestBody?.Split(',').Select(double.Parse).ToArray();
            return new MapPoint()
            {
                Latitude = splitValue![0],
                Longitude = splitValue[1]
            };
        }
        catch (Exception e)
        {
            return null;
        }
    }
}