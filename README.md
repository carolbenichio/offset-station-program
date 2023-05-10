# Offset-Station Program 🗺️

This application is able to calculate the Offset and Station between a Point and a Polyline. 
- <i>"Offset” is defined to be the smallest perpendicular distance to the polyline from the given Point</i>
- <i>"Station” is defined to be the distance from the start of the polyline, along the polyline, down to the
location on the polyline where the smallest perpendicular line from the Point intersects the polyline.</i>

![Offset, station, polyline and point example](https://github.com/carolbenichio/offset-station-program/assets/78769105/00c7a717-d8ae-446a-89db-9ecc8098044f)

Here are the steps of the program using a example:

1. The coordinates of the Polyline are read directly from an ASCII comma separated file containing Easting and Northing values and contain only data, no headers:

`-150,-200`
`-100,-45`
`20,40`
`100,45`
`125,100`
`200,150`
`300,175`

2. The program will show the coordinates from the file and then require you to insert the new Easting (x) and Northing (y) value of the Point (x, y) to start calculating the Offset and Station:

```
The file contains 7 coordinate points along the polyline.
-------------------------------------------------------------------------
The X is -150 and the Y is -200.
The X is -100 and the Y is -45.
The X is 20 and the Y is 40.
The X is 100 and the Y is 45.
The X is 125 and the Y is 100.
The X is 200 and the Y is 150.
The X is 300 and the Y is 175.
-------------------------------------------------------------------------
Please enter an Easting value (the X):
```

3. After the coordinates are entered, the program calculates and prints the linear equation of all pairs of points and saves the X1,Y1 and X2,Y2 ranges using:
### 🧮 GetLinearEquation
- Slope = (P1.Y - P2.Y)/(P1.X - P2.X);
- Intersection = P1.Y - (slope * P1.X);
- X1, Y1 = P1.X1, P1.Y1
- X2, Y2 = P2.X2, P2.Y2

In this example, we are using Point(100,200) entered by user and the equations are:

``` 
Please enter an Easting value (the X):
100
Please enter an Northing value (the Y):
200
-------------------------------------------------------------------------
The linear equation of (-150, -200) and (-100, -45) is y = 3.1x + (265).
The linear equation of (-100, -45) and (20, 40) is y = 0.7083333333333334x + (25.833333333333343).
The linear equation of (20, 40) and (100, 45) is y = 0.0625x + (38.75).
The linear equation of (100, 45) and (125, 100) is y = 2.2x + (-175.00000000000003).
The linear equation of (125, 100) and (200, 150) is y = 0.6666666666666666x + (16.66666666666667).
The linear equation of (200, 150) and (300, 175) is y = 0.25x + (100).
``` 

4. Linear equations calculated, the program starts to search for valid Intersection Points from Linear Equations and the point entered by user using the following methods:

- 🧮 GetValidIntersectionPointsFromLinearEquationsAndPoint
    - A for loop through all linear equations:
    - 🧮 **GetPerpendicularLinearEquation**;
    - 🧮 **GetPerpendicularLinearEquation**;
    - 🧮 **GetIntersectionPointFromLinearEquations**;
    - Checks if the intersection point is valid (within range X1, Y1 and X2, Y2);
    - 🧮 **GetDistanceFromPointToLinearEquation**; 
      - If true, the point is valid and added to the list of valid intersection points;
      - If false, the poing is ignored.
    - If the list of points it's empty then we inform that to the user and the distance will be measure from the given point to the closest point of the polyline coordinates.
      - 🧮 **GetDistanceFromPointToPolylinePoint**.
    
5. Then, the program order the IntersectionPoint list by distance (or Offset) and shows it to the user:

```
This intersection point is valid: (153.84615384615384, 119.23076923076923) and it's distance is 97.0725343394151.
-------------------------------------------------------------------------
List of valid intersection point(s) of the UserPoint linear equation and the current linear equation is(are):
(153.84615384615384, 119.23076923076923)
``` 

6. After that, the program starts calculating the distance from the previous point of the polyline:
- If the intersection point it's not one of the polyline points, we calculate the distance between that point to the previous poyline point (🧮 **GetDistanceBetweenPoints**).
- If the intersection point has the same X and Y coodinate as some point of the polyline coordinates, then the previous point is already the polyline point, therefor, the Station value for now is 0.

7. Finally, the program calculates the next distances until it gets to the start of the polyline:

```
OFFSET: 97.0725343394151
STATION: 485.1594762890074
``` 

<hr>

<i>All numbers remained with their original decimal places in order not to interfere with the accuracy of the final result.</i>

Any feedback is more then welcome. Thanks! <br>
<i>Caroline Benichio Teixeira, 2023.</i>

