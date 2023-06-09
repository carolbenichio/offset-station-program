using Calculation;
using Services.Interfaces;
using ValueObjects;

namespace Services;

public class EquationsService : IEquationsServices
{
    private readonly Equations _equations;
    private readonly PointsFactory _pointsFactory;

    public EquationsService
    (
        Equations equations,
        PointsFactory pointsFactory
    )
    {
        _equations = equations;
        _pointsFactory = pointsFactory;
    }

    public List<Point> GetPointsListFromCSVFile()
    {
        List<string> coordinates = new List<string>();
        List<Point> polylinePoints = new List<Point>();

        using (StreamReader reader = new StreamReader("input_loops_and_dups.csv"))
        {
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                coordinates.Add(line);
            }
        }

        foreach (string coordinate in coordinates)
        {
            polylinePoints.Add(_pointsFactory.PointGenerator(coordinate));
        }

        Console.WriteLine($"The file contains {coordinates.Count} coordinate points along the polyline.");
        Console.WriteLine($"-------------------------------------------------------------------------");
        
        foreach (Point point in polylinePoints)
        {
            Console.WriteLine($"The X is {point.X} and the Y is {point.Y}.");
        }
        Console.WriteLine($"-------------------------------------------------------------------------");
        
        return polylinePoints;
    }
    
    public Point GetUserPoint()
    {
        double x;
        string input;
        do
        {
            Console.WriteLine("Enter a valid Easting value X: ");
            input = Console.ReadLine();
        } while (!double.TryParse(input, out x));

        double y;
        string input2;
        do
        {
            Console.WriteLine("Enter a valid Northing value Y: ");
            input2 = Console.ReadLine();
        } while (!double.TryParse(input2, out y));

        Point userPoint = new Point();
        userPoint.X = x;
        userPoint.Y = y;
        Console.WriteLine($"User point: ({x}, {y}).");
        Console.WriteLine($"-------------------------------------------------------------------------");
        
        return userPoint; 
    }

    public List<LinearEquation> GetLinearEquations(List<Point> polylinePoints)
    {
        List<LinearEquation> linearEquations = new List<LinearEquation>();
        for (int i = 0; i < polylinePoints.Count - 1; i++)
        {
            LinearEquation linearEquation = _equations.GetLinearEquation(polylinePoints[i], polylinePoints[i + 1]);
            linearEquations.Add(linearEquation);

            Console.WriteLine($"The linear equation of ({polylinePoints[i].X}, {polylinePoints[i].Y}) and ({polylinePoints[i + 1].X}, {polylinePoints[i + 1].Y}) is y = {linearEquation.Slope}x + ({linearEquation.Intersection}).");
        }
        Console.WriteLine($"-------------------------------------------------------------------------");

        return linearEquations;
    }

    public List<IntersectionPoint> GetValidIntersectionPointsFromLinearEquationsAndPoint(List<LinearEquation> linearEquations, List<Point> polylinePoints, Point userPoint)
    {
        List<IntersectionPoint> intersectionPoints = new List<IntersectionPoint>();
        for(int i = 0; i < linearEquations.Count; i++)
        {
            IntersectionPoint intersectionPoint = new IntersectionPoint();
            LinearEquation perpendicularLinearEquation = _equations.GetPerpendicularLinearEquation(userPoint, linearEquations[i].Slope);
            intersectionPoint.Point = _equations.GetIntersectionPointFromLinearEquations(perpendicularLinearEquation, linearEquations[i]);
            
            if(intersectionPoint.Point.X > linearEquations[i].X1 && intersectionPoint.Point.X < linearEquations[i].X2 &&
                intersectionPoint.Point.Y > linearEquations[i].Y1 && intersectionPoint.Point.Y < linearEquations[i].Y2)
            {
                intersectionPoint.Distance = _equations.GetDistanceFromPointToLinearEquation(linearEquations[i], userPoint);
                Console.WriteLine($"This intersection point is valid: ({intersectionPoint.Point.X}, {intersectionPoint.Point.Y}) and it's distance is {intersectionPoint.Distance}.");
                intersectionPoint.LinearEquation = linearEquations[i];
                intersectionPoints.Add(intersectionPoint);
            }
        }

        if(intersectionPoints.Count == 0)
        {
            IntersectionPoint intersectionPoint = new IntersectionPoint();
            intersectionPoint = _equations.GetDistanceFromPointToPolylinePoint(linearEquations, polylinePoints, userPoint, intersectionPoint);
            intersectionPoints.Add(intersectionPoint);
        }

        Console.WriteLine($"-------------------------------------------------------------------------");
        Console.WriteLine($"List of valid intersection point(s) of the UserPoint linear equation and the current linear equation is(are):");

        foreach (IntersectionPoint intersectionPoint in intersectionPoints)
        {
            Console.WriteLine($"({intersectionPoint.Point.X}, {intersectionPoint.Point.Y})");
        }

        return intersectionPoints;
    }

    public IntersectionPoint GetSmallerDistanceIntersectionPoint(List<IntersectionPoint> intersectionPoints)
    {
        IntersectionPoint smallerDistanceIntersectionPoint = intersectionPoints.OrderBy(ip => ip.Distance).First();
        double smallerDistanceIntersectionPointDistance = smallerDistanceIntersectionPoint.Distance;
        
        return smallerDistanceIntersectionPoint;
    }

    public Point GetPreviousPoint(IntersectionPoint smallerDistanceIntersectionPoint)
    {
        Point previousPoint = new Point();

        if (smallerDistanceIntersectionPoint.Point.X == smallerDistanceIntersectionPoint.LinearEquation.X1 &&
            smallerDistanceIntersectionPoint.Point.X == smallerDistanceIntersectionPoint.LinearEquation.Y1)
        {
            previousPoint.X = smallerDistanceIntersectionPoint.Point.X;
            previousPoint.Y = smallerDistanceIntersectionPoint.Point.Y;
        } else
        {
            previousPoint.X = smallerDistanceIntersectionPoint.LinearEquation.X1;
            previousPoint.Y = smallerDistanceIntersectionPoint.LinearEquation.Y1;
        }

        return previousPoint;
    }

    public double GetPreviousPointDistance(Point previousPoint, IntersectionPoint smallerDistanceIntersectionPoint)
    {
        double previousPointDistance = _equations.GetDistanceBetweenPoints(previousPoint, smallerDistanceIntersectionPoint.Point);
        return previousPointDistance;
    }

    public double GetStation(double previousPointDistance, int previousPointIndex, List<Point> polylinePoints)
    {
        double station = previousPointDistance;

        for (int i = previousPointIndex - 1; i >= 0; i--)
        {
            double pointsDistance = _equations.GetDistanceBetweenPoints(polylinePoints[i], polylinePoints[i + 1]);
            station += pointsDistance;
        }
        Console.WriteLine($"-------------------------------------------------------------------------");

        return station;
    }
}