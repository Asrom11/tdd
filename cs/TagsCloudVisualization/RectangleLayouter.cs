using System.Drawing;

namespace TagsCloudVisualization;

public class RectangleLayouter
{
    private readonly SpiralGenerator spiralGenerator;
    private readonly RectangleShifter rectangleShifter;
    private readonly List<Rectangle> rectangles;

    public RectangleLayouter(Point center)
    {
        spiralGenerator = new SpiralGenerator(center);
        rectangleShifter = new RectangleShifter(center);
        rectangles = new List<Rectangle>();
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
            throw new ArgumentException("Размеры прямоугольника должны быть положительными.");

        Rectangle newRectangle;
        do
        {
            var point = spiralGenerator.GetNextPoint();
            var location = new Point(
                point.X - rectangleSize.Width / 2,
                point.Y - rectangleSize.Height / 2);
            newRectangle = new Rectangle(location, rectangleSize);
        } while (rectangles.Any(r => r.IntersectsWith(newRectangle)));

        newRectangle = rectangleShifter.ShiftRectangleToCenter(newRectangle, rectangles);
        rectangles.Add(newRectangle);
        return newRectangle;
    }

    public List<Rectangle> GetRectangles() => rectangles;
}