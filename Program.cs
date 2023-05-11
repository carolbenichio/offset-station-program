using Calculation;
using Services;
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

        var equationsService = new EquationsService(equations, pointsFactory);

        List<Point> polylinePoints = equationsService.GetPointsListFromCSVFile();

        Point userPoint = equationsService.GetUserPoint();

        List<LinearEquation> linearEquations = equationsService.GetLinearEquations(polylinePoints);

        List<IntersectionPoint> intersectionPoints = equationsService.GetValidIntersectionPointsFromLinearEquationsAndPoint(linearEquations, polylinePoints, userPoint);

        var smallerDistanceIntersectionPoint = equationsService.GetSmallerDistanceIntersectionPoint(intersectionPoints);

        var offset = smallerDistanceIntersectionPoint.Distance;

        Point previousPoint = equationsService.GetPreviousPoint(smallerDistanceIntersectionPoint);
        double previousPointDistance = equationsService.GetPreviousPointDistance(previousPoint, smallerDistanceIntersectionPoint);
        int previousPointIndex = polylinePoints.FindIndex(pp => pp.X == previousPoint.X && pp.Y == previousPoint.Y);

        double station = equationsService.GetStation(previousPointDistance, previousPointIndex, polylinePoints);

        Console.WriteLine($"OFFSET: {offset}.");
        Console.WriteLine($"STATION: {station}.");
        Console.WriteLine($"-------------------------------------------------------------------------");
        Console.WriteLine($"Press enter to close.");
        Console.ReadLine();
    }
}