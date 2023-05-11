using ValueObjects;

namespace Calculation.Interfaces;

interface IEquations
{
    public LinearEquation GetLinearEquation(Point P1, Point P2);
    public double GetDistanceFromPointToLinearEquation(LinearEquation linearEquation, Point point);
    public LinearEquation GetPerpendicularLinearEquation(Point point, double slope);
    public Point GetIntersectionPointFromLinearEquations(LinearEquation linearEquation1, LinearEquation linearEquation2);
    public double GetDistanceBetweenPoints(Point P1, Point P2);
    public IntersectionPoint GetDistanceFromPointToPolylinePoint(List<LinearEquation> linearEquations, List<Point> polylinePoints, Point userPoint, IntersectionPoint intersectionPoint);
}