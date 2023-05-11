using ValueObjects;

namespace Calculation;

public class PointsFactory
{
    public Point PointGenerator(string point)
    {
        Point P = new Point();
        P.X = double.Parse(point.Split(',')[0]); 
        P.Y = double.Parse(point.Split(',')[1]);
        
        return P;
    }
}