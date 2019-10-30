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
            //This seemes to work better then the original code, no clue why
            if (x < other.x + other.w && y < other.y + other .l &&x > other.x && y > other.y)
                return true;
            if (x + w > other.x && x + w < other.x+other.w&& y+l>other.y&&y+l<other.y+other.l)
                return true;
            if (x + w > other.x && x + w < other.x + other.w && y> other.y && y < other.y + other.l)
                return true;
            if (x > other.x && x < other.x + other.w && y + l > other.y && y + l < other.y + other.l)
                return true;
            return false;
            //return !(x < other.x || y < other.y || x+w > other.x + other.w || y+l > other.y+other.l);
        }
    }
    //Not used
    
}
