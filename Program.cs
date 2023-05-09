using Calculation;
using ValueObjects;

namespace Program;

internal class Program
{
    static void Main(string[] args)
	{
		var equations = new Equations();
		var points = new PointsFactory();

		using(StreamReader reader = new StreamReader("input_loops_and_dups.csv"))
		{
            // Creates list from .csv file. Ex: ["150, 300", "300, 130", "80, 100", "-50, 40", ...]
			List<string> coordinates = new List<string>();
			while(!reader.EndOfStream)
			{
				var line = reader.ReadLine();
            	coordinates.Add(line);
			}

            // Creates a Point List from the coordinates
            // Ex: [Point1, Point2, Point3, Point4] where Point1 has X and Y properties like Point1.X = 150 and Point1.Y = 300 (double)
			List<Point> polylinePoints = new List<Point>();
			foreach(string coordinate in coordinates)
			{
				polylinePoints.Add(points.PointGenerator(coordinate));
			}

            // Prints information from the file and each point on console
            Console.WriteLine($"The file contains {coordinates.Count} coordinate points along the polyline.");
            Console.WriteLine($"-------------------------------------------------------------------------");
			foreach(Point point in polylinePoints)
			{
			    Console.WriteLine($"The X is {point.X} and the Y is {point.Y}.");
			}
			Console.WriteLine($"-------------------------------------------------------------------------");

            // Expects the X and Y coordinates from the user
            Point userPoint = new Point();
            Console.WriteLine($"Please enter an Easting value (the X):");
            double userX = Convert.ToInt32(Console.ReadLine()); 

            Console.WriteLine($"Please enter an Northing value (the Y):");
            double userY = Convert.ToInt32(Console.ReadLine());

            userPoint.X = userX;
            userPoint.Y = userY;

            Console.WriteLine($"-------------------------------------------------------------------------");

            // Calculates the linear equations of every point in the list and saves each one on linearEquations list
            // Ex: From this list [(150, 300), (300, 130), (80, 100), (-50, 40)]
            // The fist linear equation is generated from (150, 300) and (300, 130), therefor, polylinePoints[0] and polylinePoints[1]
            // The second linear equation is generated from (300, 130) and (80, 100), therefor, polylinePoints[1] and polylinePoints[2]
            // The third linear equation is generated from (80, 100) and (-50, 40), therefor, polylinePoints[2] and polylinePoints[3]
            List<LinearEquation> linearEquations = new List<LinearEquation>();
            for(var i = 0; i < polylinePoints.Count - 1; i++)
            {
                LinearEquation linearEquation = equations.GetLinearEquation(polylinePoints[i], polylinePoints[i + 1]);
                linearEquations.Add(linearEquation);

                Console.WriteLine($"The linear equation of ({polylinePoints[i].X}, {polylinePoints[i].Y}) and ({polylinePoints[i+1].X}, {polylinePoints[i+1].Y}) is y = {linearEquation.Slope}x + ({linearEquation.Intersection}).");
            }

            Console.WriteLine($"-------------------------------------------------------------------------");

            List<IntersectionPoint> intersectionPoints = equations.GetValidIntersectionPointFromLinearEquationsAndPoint(linearEquations, userPoint, polylinePoints, userPoint);

            Console.WriteLine($"List of valid intersection point(s) of the UserPoint linear equation and the current linear equation is(are):");
            foreach(IntersectionPoint intersectionPoint in intersectionPoints)
            { 
                Console.WriteLine($"({intersectionPoint.Point.X}, {intersectionPoint.Point.Y})");
            }

            // Gets the smaller distance on the list
            var smallerDistanceIntersectionPoint = intersectionPoints.OrderBy(ip => ip.Distance).First();
            var smallerDistanceIntersectionPointDistance = smallerDistanceIntersectionPoint.Distance;

            // Calculates first distance between intersection point and next coordinate
            Point previousPoint = new Point();

            foreach(Point point in polylinePoints)
            {
                if(point.X == smallerDistanceIntersectionPoint.LinearEquation.X1 && point.Y == smallerDistanceIntersectionPoint.LinearEquation.Y1)
                {
                    previousPoint.X = point.X;
                    previousPoint.Y = point.Y;
                }
            }

            previousPoint.X = smallerDistanceIntersectionPoint.LinearEquation.X1;
            previousPoint.Y = smallerDistanceIntersectionPoint.LinearEquation.Y1;
            double previousPointDistance = equations.GetDistanceBetweenPoints(smallerDistanceIntersectionPoint.Point, previousPoint);

            // Find index of previous point
            int previousPointIndex = polylinePoints.FindIndex(pp => pp.X == previousPoint.X && pp.Y == previousPoint.Y);

            Console.WriteLine($"-------------------------------------------------------------------------");

            // Calculates the station from the index of the smallest distance to the start of the polyline
            double station = previousPointDistance;
            for(var i = previousPointIndex - 1; i >= 0; i--)
            {
                // Console.WriteLine($"{station}");
                var pointsDistance = equations.GetDistanceBetweenPoints(polylinePoints[i], polylinePoints[i+1]);
                station += pointsDistance;
                // Console.WriteLine($"{station}, {pointsDistance}");
            }

            Console.WriteLine($"OFFSET: {smallerDistanceIntersectionPointDistance}");
            Console.WriteLine($"STATION: {station}");
		}
	}
}