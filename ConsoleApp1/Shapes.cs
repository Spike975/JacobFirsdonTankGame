using System;
using System.Collections.Generic;
using System.Text;
using Raylib;
using static Raylib.Raylib;

namespace NewRaylibGame
{
    class Circle
    {
        public float x, y, radius;
        Color color;
        public Circle()
        {

        }
        public Circle(float a, float b, float c,Color d)
        {
            x = a; y = b; radius = c;color = d;
        }
        public Circle(Circle c)
        {
            x = c.x; y = c.y; radius = c.radius; color = c.color;
        }
        public void Draw()
        {
            DrawCircleLines((int)x, (int)y, radius, color);
        }
        public void SetColor(Color set)
        {
            color = set;
        }
        public void SetX(float a)
        {
            x = a;
        }
        public void SetR(float a)
        {
            radius = a;
        }
        public void SetY(float a)
        {
            y = a;
        }
        public void ChangeX(float a)
        {
            x += a;
        }
        public void ChangeY(float a)
        {
            y += a;
        }
        public void ChangeR(float a)
        {
            radius += a;
        }
        public float GetX()
        {
            return x;

        }
        public float GetY()
        {
            return y;
        }
        public float GetR()
        {
            return radius;
        }
    }
    class Sphere
    {
        Vector3 center;
        float radius;
        public Sphere()
        {
        }
        public Sphere(Vector3 p, float r)
        {
            this.center = p;
            this.radius = r;
        }
        public void Fit(Vector3[] points)
        {
            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            for(int i = 0; i < points.Length; i++)
            {
                min = Vector3.Min(min, points[i]);
            }
            center = (min + max) * .5f;
        }
    }
    class Square
    {
        float x, y, l, w;
    }
}
