using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTest;

[TestFixture]
public class RectangleShifterTests
{
    private RectangleShifter shifter;
    private Point center;

    [SetUp]
    public void SetUp()
    {
        center = new Point(0, 0);
        shifter = new RectangleShifter(center);
    }

    [Test]
    public void ShiftRectangleToCenter_ShouldMoveRectangleCloserToCenter()
    {
        var rectangle = new Rectangle(10, 10, 5, 5);
        var rectangles = new List<Rectangle>();

        var shiftedRectangle = shifter.ShiftRectangleToCenter(rectangle, rectangles);

        GetDistanceToCenter(shiftedRectangle).Should().BeLessThan(GetDistanceToCenter(rectangle));
    }

    [Test]
    public void ShiftRectangleToCenter_ShouldNotIntersectWithExistingRectangles()
    {
        var existingRectangle = new Rectangle(0, 0, 10, 10);
        var rectangle = new Rectangle(5, 5, 10, 10);
        var rectangles = new List<Rectangle> { existingRectangle };

        var shiftedRectangle = shifter.ShiftRectangleToCenter(rectangle, rectangles);

        shiftedRectangle.IntersectsWith(existingRectangle).Should().BeFalse();
    }

    private double GetDistanceToCenter(Rectangle rect)
    {
        var rectangleCenter = new Point(
            rect.Left + rect.Width / 2,
            rect.Top + rect.Height / 2);

        var dx = rectangleCenter.X - center.X;
        var dy = rectangleCenter.Y - center.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }
}