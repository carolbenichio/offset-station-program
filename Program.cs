using Calculation;
using Services;
using ValueObjects;

namespace Program;

internal class Program
{
    static void Main(string[] args)
    {
        string menu = "1";
        do {
            Console.WriteLine($"-------------------------------------------------------------------------");
            Console.WriteLine($"OFFSET AND STATION CALCULATION PROGRAM");
            Console.WriteLine($"-------------------------------------------------------------------------");

            Equations equations = new Equations();
            PointsFactory pointsFactory = new PointsFactory();

            EquationsService equationsService = new EquationsService(equations, pointsFactory);

            List<Point> polylinePoints = equationsService.GetPointsListFromCSVFile();

            Point userPoint = equationsService.GetUserPoint();

            List<LinearEquation> linearEquations = equationsService.GetLinearEquations(polylinePoints);

            List<IntersectionPoint> intersectionPoints = equationsService.GetValidIntersectionPointsFromLinearEquationsAndPoint(linearEquations, polylinePoints, userPoint);

            IntersectionPoint smallerDistanceIntersectionPoint = equationsService.GetSmallerDistanceIntersectionPoint(intersectionPoints);

            double offset = smallerDistanceIntersectionPoint.Distance;

            Point previousPoint = equationsService.GetPreviousPoint(smallerDistanceIntersectionPoint);
            double previousPointDistance = equationsService.GetPreviousPointDistance(previousPoint, smallerDistanceIntersectionPoint);
            int previousPointIndex = polylinePoints.FindIndex(pp => pp.X == previousPoint.X && pp.Y == previousPoint.Y);

            double station = equationsService.GetStation(previousPointDistance, previousPointIndex, polylinePoints);

            Console.WriteLine($"OFFSET: {offset}.");
            Console.WriteLine($"STATION: {station}.");
            Console.WriteLine($"-------------------------------------------------------------------------");
            Console.WriteLine($"Press any key to close or 1 to enter another point.");
            menu = Console.ReadLine();
        } while(menu == "1");
    }
}