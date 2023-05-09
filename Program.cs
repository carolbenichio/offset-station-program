
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
            // Creates list from .csv file
            // Ex: ["150, 300", "300, 130", "80, 100", "-50, 40", ...]
			List<string> coordinates = new List<string>();
			while(!reader.EndOfStream)
			{
				var line = reader.ReadLine();
            	coordinates.Add(line);
			}

            // Creates a Point List from the coordinates
            // Ex: [Point1, Point2, Point3, Point4]
            // Where Point1 has X and Y properties like 
            // Point1.X = 150 (double)
            // Point1.Y = 300 (double)
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
                double slope = equations.GetEquationSlope(polylinePoints[i], polylinePoints[i + 1]);
			    double intersection = equations.GetEquationIntersection(slope, polylinePoints[i]);

                LinearEquation linearEquation = new LinearEquation();
                linearEquation.Slope = slope;
                linearEquation.Intersection = intersection;
                // linearEquation.Interval1.X = polylinePoints[i].X; 
                // linearEquation.Interval1.Y = polylinePoints[i].Y;
                // linearEquation.Interval2.X = polylinePoints[i + 1].X;
                // linearEquation.Interval2.Y = polylinePoints[i + 1].Y;

                linearEquations.Add(linearEquation);
                Console.WriteLine($"The linear equation of ({polylinePoints[i].X}, {polylinePoints[i].Y}) and ({polylinePoints[i+1].X}, {polylinePoints[i+1].Y}) is y = {slope}x + ({intersection}).");
            }

            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine($"To find the smallest distance, our Offset, between the choosing point ({userX}, {userY}), here are all the distances from every linear equation of the polyline:");

            // Calculates all the distances using the slope and intersection from the linear equation and the X and Y from the entered point and check if is valid (inside the interval).
            List<double> distances = new List<double>();
            foreach(LinearEquation linearEquation in linearEquations)
            {
                double distance = equations.GetDistanceFromPointToLinearEquation(linearEquation, userPoint);
                distances.Add(distance);
                Console.WriteLine($"The distance between the UserPoint ({userX}, {userY}) from the linear equation y = {linearEquation.Slope}x + ({linearEquation.Intersection}) is {distance}");
            }
            double offset = distances.AsQueryable().Min();
            Console.WriteLine($"Therefor, the Offset is the smallest distance: {offset}");
            Console.WriteLine("-----------------------------------------------------");

            // TO DO: insert a loop to check if the intersection point its between the seted interval (!!!!!!!!!!!)

            // if(intersectionPoint.X > polylinePoints[index].X && intersectionPoint.X < polylinePoints[index + 1].X ||
            //     (intersectionPoint.X > polylinePoints[index].Y && intersectionPoint.X < polylinePoints[index + 1].Y))
            // {
            //     distances.Remove(distances[index]);
            //     // foreach(double distance in distances)
            //     // {
            //     //     Console.WriteLine($"WORKING: {distance}");
            //     // }
            //     var newDistance = distances.AsQueryable().Min();
            //     int newIndex = distances.FindIndex(d => d == newDistance); 
            //     index = newIndex;
            // }

            // Ex: (117.6470588235294, 129.41176470588235) is the intersection point with the smallest distance but is not inside the points interval for that linear equation (200,150) and (300, 175).
            // Along with the linear expression, we need to check if the X is between 200 and 300 and/or the Y is between 150 and 175.
            // If not, take the next smaller distance and check it again. 

            // Gets the index from the smallest distance
            int index = distances.FindIndex(d => d == offset);
            Console.WriteLine($"{index}");

            // Gets the linear equation with the smallest distance
            LinearEquation intersectionLinearEquation = linearEquations[index];
            Console.WriteLine(intersectionLinearEquation.Slope);
            Console.WriteLine(intersectionLinearEquation.Intersection);

            // Gets the perpendicular linear equation that passes through the point and the previous linear equation
            LinearEquation perpendicularLinearEquation = equations.GetPerpendicularLinearEquation(userPoint, intersectionLinearEquation.Slope);
            Console.WriteLine(perpendicularLinearEquation.Slope);
            Console.WriteLine(perpendicularLinearEquation.Intersection);

            // Calculates the intersection point between perpedicular linear equation and origial linear equation.
            Point intersectionPoint = new Point();
            intersectionPoint = equations.GetIntersectionPointFromLinearEquations(intersectionLinearEquation, perpendicularLinearEquation);
            Console.WriteLine($"{intersectionPoint.X}, {intersectionPoint.Y}");

            // Calculates the distante between the Intersection Point and the previous point
            double previousPointDistance = equations.GetDistanceBetweenPoints(intersectionPoint, polylinePoints[index - 1]);
            Console.WriteLine(previousPointDistance);

            // Calculates the station from the index of the smallest distance to the start of the polyline
            double station = previousPointDistance;
            for(var i = index - 1; i > 0; i--)
            {
                var pointsDistance = equations.GetDistanceBetweenPoints(polylinePoints[i], polylinePoints[i+1]);
                station += pointsDistance;
                Console.WriteLine(station);
            }
		}
	}
}