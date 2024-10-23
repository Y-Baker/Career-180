using System;

namespace ConsoleApp;

public class Point3D
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Z { get; set; }

    public Point3D() : this(0, 0, 0) { }
    public Point3D(int x, int y) : this(x, y, 0) { }
    public Point3D(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public override string ToString()
    {
        return $"({X}, {Y}, {Z})";
    }

    public static bool operator ==(Point3D p1, Point3D p2)
    {
        return p1.X == p2.X && p1.Y == p2.Y && p1.Z == p2.Z;
    }
    public static bool operator !=(Point3D p1, Point3D p2)
    {
        return !(p1 == p2);
    }
}
