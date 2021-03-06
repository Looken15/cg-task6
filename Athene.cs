using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using static task6_new.myGeometry;
using static task6_new.Form1;

namespace task6_new
{
    public static class Athene
    {
        public static double Distance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }

        public static double Distance(Point3D p1, Point3D p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2) + Math.Pow(p1.Z - p2.Z, 2));
        }
        public static double AngleBetween(Point p1, Point p2)
        {
            return Math.Atan2(p1.X, p1.Y) - Math.Atan2(p2.X, p2.Y);
        }

        public static double[,] AtheneRotate(double angle, char axis)
        {
            if (axis == 'x')
                return new double[4, 4]
                {   { 1, 0, 0, 0 },
                    { 0, Math.Cos(angle), -Math.Sin(angle), 0},
                    {0, Math.Sin(angle), Math.Cos(angle), 0 },
                    { 0, 0, 0, 1 } };
            if (axis == 'y')
                return new double[4, 4]
                {   { Math.Cos(angle), 0, Math.Sin(angle), 0},
                    { 0, 1, 0, 0 },
                    {-Math.Sin(angle), 0, Math.Cos(angle), 0 },
                    { 0, 0, 0, 1 } };
            if (axis == 'z')
                return new double[4, 4]
                {   { Math.Cos(angle), -Math.Sin(angle), 0, 0},
                    { Math.Sin(angle), Math.Cos(angle), 0, 0 },
                    { 0, 0, 1, 0 },
                    { 0, 0, 0, 1 } };
            return new double[4, 4];
        }

        public static double[,] AtheneMove(int dx, int dy, int dz)
        {
            return new double[4, 4]
                {{ 1, 0, 0, 0 },
                { 0, 1, 0, 0 },
                { 0, 0, 1, 0 },
                { dx, dy, dz, 1} };
        }
        public static double[,] AtheneScale(double s1, double s2, double s3)
        {
            return new double[4, 4]
                {{ s1, 0, 0, 0 },
                { 0, s2, 0, 0 },
                { 0, 0, s3, 0 },
                { 0, 0, 0, 1} };
        }
        public static double[,] LineRotate(double angle)
        {
            Point3D p1 = rotMesh.points[0];
            Point3D p2 = rotMesh.points[1];

            double l = (p2.X - p1.X) / Distance(p1, p2);
            double m = (p2.Y - p1.Y) / Distance(p1, p2);
            double n = (p2.Z - p1.Z) / Distance(p1, p2);

            return new double[4, 4]
                {{ l*l + Math.Cos(angle)*(1 - l*l), l*(1-Math.Cos(angle))*m + n*Math.Sin(angle), l*(1 - Math.Cos(angle))*n - m*Math.Sin(angle), 0 },
                { l*(1 - Math.Cos(angle))*m - n*Math.Sin(angle), m*m + Math.Cos(angle)*(1 - m*m), m*(1 - Math.Cos(angle))*n + l*Math.Sin(angle), 0 },
                { l*(1 - Math.Cos(angle))*n + m*Math.Sin(angle), m*(1 - Math.Cos(angle))*n - l*Math.Sin(angle), n*n + Math.Cos(angle)*(1 - n*n), 0 },
                { 0, 0, 0, 1}
                };
        }
        public static int RowsCount(double[,] matrix)
        {
            return matrix.GetUpperBound(0) + 1;
        }

        public static int ColumnsCount(double[,] matrix)
        {
            return matrix.GetUpperBound(1) + 1;
        }
        public static double[,] MatrixMult(double[,] m1, double[,] m2)
        {
            double[,] m = new double[RowsCount(m1), ColumnsCount(m2)];

            for (int k = 0; k < RowsCount(m1); k++)
            {
                for (int i = 0; i < RowsCount(m2); i++)
                {
                    double t = 0;
                    for (int j = 0; j < ColumnsCount(m1); j++)
                    {
                        t += m1[k, j] * m2[j, i];
                    }
                    m[k, i] = t;
                }
            }
            return m;
        }

        public static void AtheneTransform(ref Mesh mes, double[,] m2)
        {
            foreach (Point3D p in mes.points)
            {
                double[,] m1 = new double[1, 4] { { p.X, p.Y, p.Z, 1 } };
                double[,] m = MatrixMult(m1, m2);
                Point3D newp = new Point3D((float)m[0, 0], (float)m[0, 1], (float)m[0, 2], 0);
                p.X = newp.X;
                p.Y = newp.Y;
                p.Z = newp.Z;
            }
        }
    }
}
