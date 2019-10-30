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
        public bool Overlaps(Box box)
        {
            float xDistance = 0;
            float yDistance = 0;
            if (x< box.x)
            {
                xDistance = x-box.x;
            }else if (x > box.x+box.w)
            {
                xDistance = x-box.x + box.w;
            }
            if (y< box.y)
            {
                yDistance = y-box.y;
            }else if (y > box.y+box.l)
            {
                yDistance = y-box.y + box.l;
            }
            float distance = (float)Math.Sqrt((xDistance*xDistance) + (yDistance* yDistance));
            if (distance <= radius)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool Overlaps(Vector3 p)
        {
            Vector3 center = new Vector3(x,y,0);
            Vector3 toPoint = p - center;
            return toPoint.MagnitudeSqr() <= (radius * radius);
        }
        public bool Overlaps(Circle other)
        {
            float xDistance = MathF.Abs(x - other.x);
            float yDistance = MathF.Abs(y - other.y);
            float hyp = MathF.Sqrt((xDistance*xDistance)+(yDistance *yDistance));
            float totalRad = radius + other.radius;
            return hyp <= totalRad;
        }

    }
    //Not used
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
    class Box
    {
        public float x, y, l, w;
        public Box()
        {
            x = 0;y = 0;w = 0;l = 0;
        }
        public Box(float a, float b, float c, float d)
        {
            x = a; y = b; l = c; w = d;
        }
        /// <summary>
        /// Sets the x and y varibles
        /// </summary>
        /// <param name="a">X</param>
        /// <param name="b">Y</param>
        public void SetPosition(float a, float b)
        {
            x = a; y = b;
        }
        /// <summary>
        /// Changes the x and y position by two floats
        /// </summary>
        /// <param name="a">X changes by</param>
        /// <param name="b">Y changes by</param>
        public void ChangePosition(float a, float b)
        {
            x += a; y += b;
        }
        /// <summary>
        /// Sets the Length and Width
        /// </summary>
        /// <param name="a">Length</param>
        /// <param name="b">Width</param>
        public void SetArea(float a, float b)
        {
            w = a; l = b;
        }
        /// <summary>
        /// Draws the outline of the box
        /// </summary>
        public void Draw()
        {
            DrawRectangleLines((int)x, (int)y, (int)w, (int)l, Color.BLACK);
        }
        /// <summary>
        /// Checks for the closest point given the vector
        /// </summary>
        /// <param name="p">Given vector</param>
        /// <returns></returns>
        public Vector3 ClosestPoint(Vector3 p)
        {
            return Vector3.Clamp(p, new Vector3(x, y, 1), new Vector3(x+w, y+l, 1));
        }
        /// <summary>
        /// Checks if the given box and the current box colides
        /// </summary>
        /// <param name="other">Box for collision</param>
        /// <returns>True or False if collides</returns>
        public bool Overlap(Box other)
        {
            return !(x < other.x || y < other.y || x + l > other.x+ other.l || y + w > other.y + other.w);
        }
    }
    //Not used
    public class Colour
    {
        public UInt32 colour;

        public Colour()
        {
            colour = 0x00000000;
        }
        public Colour(UInt32 r, UInt32 g, UInt32 b, UInt32 a)
        {
            r = (r >> 32);
            g = (g >> 32);
            b = (b >> 32);
            colour = (r << 24) + (g << 16) + (b << 8) + (a >> 32);
        }
        public void SetRed(UInt32 r)
        {
            colour = (r >> 32);
            colour = (colour << 24);
        }
        public void SetGreen(UInt32 g)
        {
            colour = (g >> 32);
            colour = (colour << 16);
        }
        public void SetBlue(UInt32 b)
        {
            colour = (b >> 32);
            colour = (colour << 8);
        }
        public void SetAlpha(UInt32 a)
        {
            colour = (a >> 32);
        }
        public byte GetAlpha()
        {
            UInt32 other = (colour << 24);
            return Convert.ToByte(other >> 24);
        }
        public byte GetBlue()
        {
            UInt32 other = (colour << 16);
            return Convert.ToByte(other >> 24);
        }
        public byte GetGreen()
        {
            UInt32 other = (colour << 8);
            return Convert.ToByte(other >> 24);
        }
        public byte GetRed()
        {
            return Convert.ToByte(colour >> 24);
        }
    }
}
