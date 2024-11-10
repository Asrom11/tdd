using System.Drawing;


GenerateLayout1();
GenerateLayout2(); 
GenerateLayout3();
return;


void GenerateLayout1()
{
    var layouter = new CircularCloudLayouter(new Point(0, 0));
    var random = new Random();
    for (var i = 0; i < 100; i++)
    {
        var size = new Size(random.Next(20, 100), random.Next(10, 50));
        layouter.PutNextRectangle(size);
    }
    layouter.SaveImage("layout1.png", new Size(800, 600));
}

void GenerateLayout2()
{
    var layouter = new CircularCloudLayouter(new Point(0, 0));
    for (var i = 0; i < 150; i++)
    {
        var size = new Size(50, 20);
        layouter.PutNextRectangle(size);
    }
    layouter.SaveImage("layout2.png", new Size(800, 600));
}

void GenerateLayout3()
{
    var layouter = new CircularCloudLayouter(new Point(0, 0));
    var random = new Random();
    for (var i = 0; i < 200; i++) 
    {
        var size = new Size(random.Next(10, 30), random.Next(10, 30));
        layouter.PutNextRectangle(size);
    }
    layouter.SaveImage("layout3.png", new Size(800, 600));
}
