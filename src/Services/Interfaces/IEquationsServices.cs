using ValueObjects;

namespace Services.Interfaces;

public interface IEquationsServices
{
    public List<Point> GetPointsListFromCSVFile();
    public Point GetUserPoint();
    public List<LinearEquation> GetLinearEquations(List<Point> polylinePoints);
    public List<IntersectionPoint> GetValidIntersectionPointsFromLinearEquationsAndPoint(List<LinearEquation> linearEquations, List<Point> polylinePoints, Point userPoint);
    public IntersectionPoint GetSmallerDistanceIntersectionPoint(List<IntersectionPoint> intersectionPoints);
    public Point GetPreviousPoint(IntersectionPoint smallerDistanceIntersectionPoint);
    public double GetPreviousPointDistance(Point previousPoint, IntersectionPoint smallerDistanceIntersectionPoint);
    public double GetStation(double previousPointDistance, int previousPointIndex, List<Point> polylinePoints);
}