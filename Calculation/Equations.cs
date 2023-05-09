using ValueObjects;

namespace Calculation;

public class Equations : IEquations
{
  	public List<double> LinearEquation { get; set; }

	public Equations()
	{
		// LinearEquation = new List<double>();
	}

	public List<LinearEquation> GetLinearEquations(List<Point> polylinePoints)
	{
		List<LinearEquation> linearEquations = new List<LinearEquation>();
		
		return linearEquations;
	}

	public double GetEquationSlope(Point P1, Point P2) // working
	{    
		double slope = ((P1.Y - P2.Y)/(P1.X - P2.X));
		return slope;
	}

	public double GetEquationIntersection(double slope, Point point) // working
	{
		return point.Y - (slope * point.X);
	}

    public double GetDistanceFromPointToLinearEquation(LinearEquation linearEquation, Point point)
    {
        var absolute = Math.Abs((linearEquation.Slope * point.X) - (point.Y) + (linearEquation.Intersection));
        var divisor = Math.Sqrt(Math.Pow(linearEquation.Slope, 2) + 1);
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
        var dividend = (linearEquation2.Intersection - linearEquation1.Intersection);
        var divisor = (linearEquation1.Slope - linearEquation2.Slope);
        
        intersectionPoint.X = dividend/divisor;
        intersectionPoint.Y = (linearEquation1.Slope * intersectionPoint.X) + linearEquation1.Intersection;

        return intersectionPoint;
    }

    public double GetDistanceBetweenPoints(Point P1, Point P2)
    {
        return Math.Sqrt(Math.Pow((P2.X - P1.X), 2) + Math.Pow(P2.Y - P1.Y, 2));
    }
}