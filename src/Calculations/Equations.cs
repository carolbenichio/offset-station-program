using Calculation.Interfaces;
using ValueObjects;

namespace Calculation;

public class Equations : IEquations
{
    public LinearEquation GetLinearEquation(Point P1, Point P2)
    {
		double slope = (P1.Y - P2.Y)/(P1.X - P2.X);
		double intersection = P1.Y - (slope * P1.X);

        LinearEquation linearEquation = new LinearEquation();
        linearEquation.Slope = slope;
        linearEquation.Intersection = intersection;
        linearEquation.X1 = P1.X;
        linearEquation.Y1 = P1.Y;
        linearEquation.X2 = P2.X;
        linearEquation.Y2 = P2.Y;

        return linearEquation;
    }

    public double GetDistanceFromPointToLinearEquation(LinearEquation linearEquation, Point point)
    {
        double absolute = Math.Abs((linearEquation.Slope * point.X) - (point.Y) + (linearEquation.Intersection));
        double divisor = Math.Sqrt(Math.Pow(linearEquation.Slope, 2) + 1);
        return absolute/divisor;
    }

    public LinearEquation GetPerpendicularLinearEquation(Point point, double slope)
    {
        LinearEquation linearEquation = new LinearEquation();
        linearEquation.Slope = -1/slope;
        linearEquation.Intersection = point.Y - (linearEquation.Slope * point.X);
        return linearEquation;
    }

    public Point GetIntersectionPointFromLinearEquations(LinearEquation linearEquation1, LinearEquation linearEquation2)
    {
        Point intersectionPoint = new Point();
        double dividend = (linearEquation2.Intersection - linearEquation1.Intersection);
        double divisor = (linearEquation1.Slope - linearEquation2.Slope);
        
        intersectionPoint.X = dividend/divisor;
        intersectionPoint.Y = (linearEquation1.Slope * intersectionPoint.X) + linearEquation1.Intersection;

        return intersectionPoint;
    }

    public double GetDistanceBetweenPoints(Point P1, Point P2)
    {
        double rightEquation = Math.Pow((P2.X - P1.X), 2) + Math.Pow(P2.Y - P1.Y, 2);
        return Math.Sqrt(rightEquation);
    }

    public IntersectionPoint GetDistanceFromPointToPolylinePoint(List<LinearEquation> linearEquations, List<Point> polylinePoints, Point userPoint, IntersectionPoint intersectionPoint)
    {
        Console.WriteLine("There are no perpendicular linear equations from the point to the polyline");
        Console.WriteLine("so the distance is going to be measured from the given point to the closest point of the polyline.");
        
        List<double> distancesPointToPolylinePoints = new List<double>();
        for(int i = 0; i < polylinePoints.Count; i++)
        {
            distancesPointToPolylinePoints.Add(GetDistanceBetweenPoints(userPoint, polylinePoints[i]));
        } 

        int smallerDistanceIndex = distancesPointToPolylinePoints.IndexOf(distancesPointToPolylinePoints.Min());
        intersectionPoint.Distance = distancesPointToPolylinePoints[smallerDistanceIndex];
        intersectionPoint.Point = polylinePoints[smallerDistanceIndex];
        if(smallerDistanceIndex == 0)
        {
            intersectionPoint.LinearEquation = linearEquations[smallerDistanceIndex + 1];
        } else 
        {
            intersectionPoint.LinearEquation = linearEquations[smallerDistanceIndex - 1];
        }

        return intersectionPoint;
    }
}