using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using TagsCloudVisualization;

public class CircularCloudLayouter
{
    private readonly RectangleLayouter rectangleLayouter;

    public CircularCloudLayouter(Point center)
    {
        rectangleLayouter = new RectangleLayouter(center);
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        return rectangleLayouter.PutNextRectangle(rectangleSize);
    }

    public List<Rectangle> GetRectangles()
    {
        return rectangleLayouter.GetRectangles();
    }

    public void SaveImage(string filename, Size imageSize)
    {
        using var bitmap = new Bitmap(imageSize.Width, imageSize.Height);
        using var graphics = Graphics.FromImage(bitmap);
        graphics.Clear(Color.Black);

        var random = new Random();
        foreach (var rectangle in GetRectangles())
        {
            var color = Color.FromArgb(
                random.Next(128, 255),
                random.Next(128, 255),
                random.Next(128, 255)
            );

            using var brush = new SolidBrush(Color.FromArgb(180, color));
            using var pen = new Pen(color, 2f);

            var rect = new Rectangle(
                rectangle.X + imageSize.Width / 2,
                rectangle.Y + imageSize.Height / 2,
                rectangle.Width,
                rectangle.Height);

            graphics.FillRectangle(brush, rect);
            graphics.DrawRectangle(pen, rect);
        }

        bitmap.Save(filename, ImageFormat.Png);
    }
}