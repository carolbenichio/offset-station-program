using ValueObjects;

namespace Calculation;

public class PointsFactory
{
    public Point Point { get; set; }
    public List<Point> PolylinePoints { get; set; } 

    // public PointsFactory() { }

      public Point PointGenerator(string point) // working
      {
        Point P = new Point();
        P.X = double.Parse(point.Split(',')[0]); 
        P.Y = double.Parse(point.Split(',')[1]);
        return P;
      }
}