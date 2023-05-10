using Calculation;
using ValueObjects;

namespace Program;

internal class Program
{
    static void Main(string[] args)
    {
        var equations = new Equations();
        var points = new PointsFactory();
        List<string> coordinates = new List<string>();

        using (StreamReader reader = new StreamReader("input_loops_and_dups.csv"))
        {
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                coordinates.Add(line);
            }
        }

        List<Point> polylinePoints = new List<Point>();
        foreach (string coordinate in coordinates)
        {
            polylinePoints.Add(points.PointGenerator(coordinate));
        }

        Console.WriteLine($"The file contains {coordinates.Count} coordinate points along the polyline.");
        Console.WriteLine($"-------------------------------------------------------------------------");

        foreach (Point point in polylinePoints)
        {
            Console.WriteLine($"The X is {point.X} and the Y is {point.Y}.");
        }
        Console.WriteLine($"-------------------------------------------------------------------------");

        Point userPoint = new Point();
        Console.WriteLine($"Please enter an Easting value (the X):");
        double userX = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine($"Please enter an Northing value (the Y):");
        double userY = Convert.ToInt32(Console.ReadLine());

        userPoint.X = userX;
        userPoint.Y = userY;

        Console.WriteLine($"-------------------------------------------------------------------------");

        List<LinearEquation> linearEquations = new List<LinearEquation>();
        for (var i = 0; i < polylinePoints.Count - 1; i++)
        {
            LinearEquation linearEquation = equations.GetLinearEquation(polylinePoints[i], polylinePoints[i + 1]);
            linearEquations.Add(linearEquation);

            Console.WriteLine($"The linear equation of ({polylinePoints[i].X}, {polylinePoints[i].Y}) and ({polylinePoints[i + 1].X}, {polylinePoints[i + 1].Y}) is y = {linearEquation.Slope}x + ({linearEquation.Intersection}).");
        }

        Console.WriteLine($"-------------------------------------------------------------------------");

        List<IntersectionPoint> intersectionPoints = equations.GetValidIntersectionPointsFromLinearEquationsAndPoint(linearEquations, userPoint, polylinePoints, userPoint);

        Console.WriteLine($"-------------------------------------------------------------------------");

        Console.WriteLine($"List of valid intersection point(s) of the UserPoint linear equation and the current linear equation is(are):");
        foreach (IntersectionPoint intersectionPoint in intersectionPoints)
        {
            Console.WriteLine($"({intersectionPoint.Point.X}, {intersectionPoint.Point.Y})");
        }

        var smallerDistanceIntersectionPoint = intersectionPoints.OrderBy(ip => ip.Distance).First();
        var smallerDistanceIntersectionPointDistance = smallerDistanceIntersectionPoint.Distance;

        Point previousPoint = new Point();
        int previousPointIndex;
        double previousPointDistance;
        double station;

        if (smallerDistanceIntersectionPoint.Point.X == smallerDistanceIntersectionPoint.LinearEquation.X1 &&
            smallerDistanceIntersectionPoint.Point.X == smallerDistanceIntersectionPoint.LinearEquation.Y1)
        {
            previousPoint.X = smallerDistanceIntersectionPoint.Point.X;
            previousPoint.Y = smallerDistanceIntersectionPoint.Point.Y;
            station = 0;
        }
        else
        {
            previousPoint.X = smallerDistanceIntersectionPoint.LinearEquation.X1;
            previousPoint.Y = smallerDistanceIntersectionPoint.LinearEquation.Y1;
            previousPointDistance = equations.GetDistanceBetweenPoints(smallerDistanceIntersectionPoint.Point, previousPoint);
            station = previousPointDistance;
        }

        previousPointIndex = polylinePoints.FindIndex(pp => pp.X == previousPoint.X && pp.Y == previousPoint.Y);

        Console.WriteLine($"-------------------------------------------------------------------------");

        for (var i = previousPointIndex - 1; i >= 0; i--)
        {
            var pointsDistance = equations.GetDistanceBetweenPoints(polylinePoints[i], polylinePoints[i + 1]);
            station += pointsDistance;
        }

        Console.WriteLine($"OFFSET: {smallerDistanceIntersectionPointDistance}");
        Console.WriteLine($"STATION: {station}");
        Console.WriteLine($"-------------------------------------------------------------------------");
    }
}