using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTest;

[TestFixture]
public class RectangleLayouterTests
{
    private RectangleLayouter layouter;

    [SetUp]
    public void SetUp()
    {
        layouter = new RectangleLayouter(new Point(0, 0));
    }

    [Test]
    public void PutNextRectangle_FirstRectangle_ShouldBeAtCenter()
    {
        var size = new Size(10, 10);
        var rectangle = layouter.PutNextRectangle(size);

        var expectedLocation = new Point(-5, -5);
        rectangle.Location.Should().Be(expectedLocation);
    }

    [Test]
    public void PutNextRectangle_RectanglesShouldNotOverlap()
    {
        var sizes = new[]
        {
            new Size(50, 10),
            new Size(10, 20),
            new Size(20, 20),
            new Size(30, 10),
            new Size(10, 40)
        };

        foreach (var size in sizes)
            layouter.PutNextRectangle(size);

        var rectangles = layouter.GetRectangles();

        for (var i = 0; i < rectangles.Count; i++)
        {
            for (var j = i + 1; j < rectangles.Count; j++)
            {
                rectangles[i].IntersectsWith(rectangles[j])
                    .Should().BeFalse($"Прямоугольники {i} и {j} пересекаются.");
            }
        }
    }
}