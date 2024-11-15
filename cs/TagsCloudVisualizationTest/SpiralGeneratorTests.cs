﻿using NUnit.Framework;
using FluentAssertions;
using System.Drawing;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTest;

[TestFixture]
public class SpiralGeneratorTests
{
    private Point center;
    private SpiralGenerator generator;

    [SetUp]
    public void Setup()
    {
        center = new Point(0, 0);
        generator = new SpiralGenerator(center);
    }

    [Test]
    public void Constructor_WithNegativeSpiralStep_ThrowsArgumentException()
    {
        Action act = () => new SpiralGenerator(center, -1);
        
        act.Should().Throw<ArgumentException>()
           .WithMessage("Шаг спирали должен быть больше 0");
    }

    [Test]
    public void GetNextPoint_FirstPoint_ShouldBeAtCenter()
    {
        var firstPoint = generator.GetNextPoint();
        
        firstPoint.Should().Be(center);
    }

    [Test]
    public void GetNextPoint_ShouldGenerateUniquePoints()
    {
        var points = new HashSet<Point>();
        const int numberOfPoints = 100;

        for (var i = 0; i < numberOfPoints; i++)
        {
            points.Add(generator.GetNextPoint());
        }

        points.Count.Should().Be(numberOfPoints, 
            "все точки должны быть уникальными");
    }

    [Test]
    public void GetNextPoint_ShouldGeneratePointsWithIncreasingDistanceFromCenter()
    {
        const int numberOfPoints = 50;
        var initialPoint = generator.GetNextPoint();
        var initialDistance = GetDistanceFromCenter(initialPoint);
        const int halfwayPoint = numberOfPoints / 2;
        
        for (var i = 0; i < halfwayPoint; i++)
        {
            generator.GetNextPoint();
        }
        
        var middlePoint = generator.GetNextPoint();
        var middleDistance = GetDistanceFromCenter(middlePoint);

     
        for (var i = halfwayPoint; i < numberOfPoints; i++)
        {
            generator.GetNextPoint();
        }
        
        var finalPoint = generator.GetNextPoint();
        var finalDistance = GetDistanceFromCenter(finalPoint);

        finalDistance.Should().BeGreaterThan(middleDistance)
            .And.BeGreaterThan(initialDistance);
    }

    [Test]
    public void GetNextPoint_ShouldNotGenerateConsecutiveDuplicatePoints()
    {
        const int numberOfPoints = 100;
        Point? previousPoint = null;

        for (var i = 0; i < numberOfPoints; i++)
        {
            var currentPoint = generator.GetNextPoint();
            if (previousPoint != null)
            {
                currentPoint.Should().NotBe(previousPoint, 
                    "последовательные точки не должны повторяться");
            }
            previousPoint = currentPoint;
        }
    }

    private double GetDistanceFromCenter(Point point)
    {
        var dx = point.X - center.X;
        var dy = point.Y - center.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }
}