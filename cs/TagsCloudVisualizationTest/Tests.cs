using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualizationTest;

[TestFixture]
public class CircularCloudLayouterTests
{
    private CircularCloudLayouter layouter;
    private Point center;

    [SetUp]
    public void SetUp()
    {
        center = new Point(0, 0);
        layouter = new CircularCloudLayouter(center);
    }

    [Test]
    public void PutNextRectangle_FirstRectangle_ShouldBePlacedAtCenter()
    {
        var size = new Size(10, 10);
        var rectangle = layouter.PutNextRectangle(size);

        var expectedLocation = new Point(center.X - size.Width / 2, center.Y - size.Height / 2);
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
        {
            layouter.PutNextRectangle(size);
        }

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

    [Test]
    public void PutNextRectangle_RectanglesShouldFormACircleShape()
    {
        var sizes = Enumerable.Repeat(new Size(10, 10), 100).ToList();

        foreach (var size in sizes)
        {
            layouter.PutNextRectangle(size);
        }

        var rectangles = layouter.GetRectangles();
        var distances = rectangles
            .Select(r => GetDistanceToCenter(GetRectangleCenter(r), center))
            .ToList();

        var averageDistance = distances.Average();
        var maxDistance = distances.Max();

        maxDistance.Should().BeLessOrEqualTo(averageDistance * 2, "Прямоугольники расположены не компактно.");
    }

    [Test]
    public void GenerateLayout_ShouldCreateCircularCloud()
    {
        var random = new Random();
        for (var i = 0; i < 100; i++)
        {
            var size = new Size(random.Next(10, 50), random.Next(10, 50));
            layouter.PutNextRectangle(size);
        }

        var rectangles = layouter.GetRectangles();
        rectangles.Count.Should().Be(100);
        
        var distances = rectangles
            .Select(r => GetDistanceToCenter(GetRectangleCenter(r), center))
            .ToList();

        var averageDistance = distances.Average();
        var maxDistance = distances.Max();

        maxDistance.Should().BeLessOrEqualTo(averageDistance * 2, "Облако не является компактным.");
    }

    private Point GetRectangleCenter(Rectangle rectangle)
    {
        return new Point(
            rectangle.Left + rectangle.Width / 2,
            rectangle.Top + rectangle.Height / 2);
    }

    private double GetDistanceToCenter(Point point, Point center)
    {
        var dx = point.X - center.X;
        var dy = point.Y - center.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }

    [TearDown]
    public void TearDown()
    {
        if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Failed)
        {
            return;
        }

        var testName = TestContext.CurrentContext.Test.Name;
        var directory = Path.Combine(TestContext.CurrentContext.WorkDirectory, "FailedTests");

        Directory.CreateDirectory(directory);

        var filePath = Path.Combine(directory, $"{testName}.png");

        layouter.SaveImage(filePath, new Size(800, 600));
        Console.WriteLine($"Tag cloud visualization saved to file {filePath}");
    }
}