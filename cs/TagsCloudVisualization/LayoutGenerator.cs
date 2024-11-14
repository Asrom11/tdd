using System.Drawing;

namespace TagsCloudVisualization;

public class LayoutGenerator
{
    private readonly Point center;
    private readonly Size imageSize;

    public LayoutGenerator(Point center, Size imageSize)
    {
        this.center = center;
        this.imageSize = imageSize;
    }

    public void GenerateLayout(string outputFileName, int rectangleCount, Func<Random, Size> sizeGenerator)
    {
        var layouter = new CircularCloudLayouter(center);
        var random = new Random();

        for (var i = 0; i < rectangleCount; i++)
        {
            var size = sizeGenerator(random);
            layouter.PutNextRectangle(size);
        }

        layouter.SaveImage(outputFileName, imageSize);
    }
}