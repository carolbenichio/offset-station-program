using Calculation;
using Service;
using ValueObjects;

namespace Program;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine($"-------------------------------------------------------------------------");
        Console.WriteLine($"OFFSET AND STATION CALCULATION PROGRAM");
        Console.WriteLine($"-------------------------------------------------------------------------");

        var equations = new Equations();
        var pointsFactory = new PointsFactory();

        var services = new Services(equations, pointsFactory);

        List<Point> polylinePoints = services.GetPointsListFromCSVFile();

        Point userPoint = services.GetUserPoint();

        List<LinearEquation> linearEquations = services.GetLinearEquations(polylinePoints);

        List<IntersectionPoint> intersectionPoints = services.GetValidIntersectionPointsFromLinearEquationsAndPoint(linearEquations, userPoint, polylinePoints, userPoint);

        var smallerDistanceIntersectionPoint = services.GetSmallerDistanceIntersectionPoint(intersectionPoints);

        var offset = smallerDistanceIntersectionPoint.Distance;

        Point previousPoint = services.GetPreviousPoint(smallerDistanceIntersectionPoint);
        double previousPointDistance = services.GetPreviousPointDistance(previousPoint, smallerDistanceIntersectionPoint);
        int previousPointIndex = polylinePoints.FindIndex(pp => pp.X == previousPoint.X && pp.Y == previousPoint.Y);

        double station = services.GetStation(previousPointDistance, previousPointIndex, polylinePoints);

        Console.WriteLine($"OFFSET: {offset}.");
        Console.WriteLine($"STATION: {station}.");
        Console.WriteLine($"-------------------------------------------------------------------------");
        Console.WriteLine($"Press enter to close.");
        Console.ReadLine();
    }
}