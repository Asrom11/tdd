using System.Drawing;

namespace TagsCloudVisualization;

public class RectangleShifter
{
    private readonly Point center;

    public RectangleShifter(Point center)
    {
        this.center = center;
    }

public Rectangle ShiftRectangleToCenter(Rectangle rectangle, List<Rectangle> rectangles)
{
    if (rectangles.Any(r => r.IntersectsWith(rectangle)))
    {
        rectangle = FindNearestNonIntersectingPosition(rectangle, rectangles);
        if (rectangle == Rectangle.Empty)
            throw new InvalidOperationException("Невозможно разместить прямоугольник без пересечений.");
    }

    var shiftedRectangle = rectangle;
    var directionToCenter = GetDirectionToCenter(shiftedRectangle);

    while (directionToCenter != Point.Empty)
    {
        var nextLocation = new Point(
            shiftedRectangle.Location.X + Math.Sign(directionToCenter.X),
            shiftedRectangle.Location.Y + Math.Sign(directionToCenter.Y));

        var nextRectangle = new Rectangle(nextLocation, shiftedRectangle.Size);

        if (rectangles.Any(r => r.IntersectsWith(nextRectangle)))
            break;

        shiftedRectangle = nextRectangle;
        directionToCenter = GetDirectionToCenter(shiftedRectangle);
    }

    return shiftedRectangle;
}

    private Rectangle FindNearestNonIntersectingPosition(Rectangle rectangle, List<Rectangle> rectangles)
    {
        var queue = new Queue<Point>();
        var visited = new HashSet<Point>();
        queue.Enqueue(rectangle.Location);
        visited.Add(rectangle.Location);

        var directions = new[]
        {
            new Point(-1, 0),
            new Point(1, 0),
            new Point(0, -1),
            new Point(0, 1)
        };

        while (queue.Count > 0)
        {
            var currentLocation = queue.Dequeue();
            var currentRectangle = new Rectangle(currentLocation, rectangle.Size);

            if (!rectangles.Any(r => r.IntersectsWith(currentRectangle)))
                return currentRectangle;

            foreach (var dir in directions)
            {
                var nextLocation = new Point(
                    currentLocation.X + dir.X,
                    currentLocation.Y + dir.Y);

                if (!visited.Contains(nextLocation))
                {
                    queue.Enqueue(nextLocation);
                    visited.Add(nextLocation);
                }
            }
        }

        return Rectangle.Empty;
    }

    private Point GetDirectionToCenter(Rectangle rectangle)
    {
        var rectangleCenter = new Point(
            rectangle.Left + rectangle.Width / 2,
            rectangle.Top + rectangle.Height / 2);

        var directionX = center.X - rectangleCenter.X;
        var directionY = center.Y - rectangleCenter.Y;

        if (directionX == 0 && directionY == 0)
            return Point.Empty;

        return new Point(directionX, directionY);
    }
}