using System;
using System.Collections.Generic;
using System.Drawing;

namespace Library.Draw
{
    /// <summary>
    /// 多邊形
    /// </summary>
    public class RegularPolygon
    {
        /// <summary>
        /// 多邊形
        /// </summary>
        /// <param name="sides"></param>
        /// <param name="radius"></param>
        /// <param name="startingAngle"></param>
        /// <param name="center"></param>
        /// <param name="canvasSize"></param>
        /// <returns></returns>
        public Bitmap CreateRegularPolygon(int sides, int radius, int startingAngle, Point center, Size canvasSize)
        {
            //Get the location for each vertex of the polygon
            Point[] verticies = CalculateVertices(sides, radius, startingAngle, center);

            //Render the polygon
            Bitmap polygon = new Bitmap(canvasSize.Width, canvasSize.Height);
            using (Graphics g = Graphics.FromImage(polygon))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.DrawPolygon(Pens.Black, verticies);
            }

            return polygon;
        }

        private Point[] CalculateVertices(int sides, int radius, int startingAngle, Point center)
        {
            if (sides < 3)
                throw new ArgumentException("Polygon must have 3 sides or more.");

            List<Point> points = new List<Point>();
            float step = 360.0f / sides;

            float angle = startingAngle; //starting angle
            for (double i = startingAngle; i < startingAngle + 360.0; i += step) //go in a circle
            {
                points.Add(DegreesToXY(angle, radius, center));
                angle += step;
            }

            return points.ToArray();
        }

        private Point DegreesToXY(float degrees, float radius, Point origin)
        {
            Point xy = new Point();
            double radians = degrees * Math.PI / 180.0;

            xy.X = (int)(Math.Cos(radians) * radius + origin.X);
            xy.Y = (int)(Math.Sin(-radians) * radius + origin.Y);

            return xy;
        }
    }
}