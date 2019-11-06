using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathClasses
{
    public class Vector3
    {
        public float x, y, z;

        public Vector3()
        {
            x = 0;
            y = 0;
            z = 0;
        }
        public Vector3(float a, float b, float c)
        {
            x = a; y = b; z = c;
        }
        public Vector3(Vector3 v)
        {
            x = v.x; y = v.y; z = v.z;
        }
        public static Vector3 operator *(Vector3 v, float numb)
        {
            return new Vector3(v.x * numb, v.y * numb, v.z * numb);
        }
        public static Vector3 operator *(float numb, Vector3 v)
        {
            return new Vector3(v.x * numb, v.y * numb, v.z * numb);
        }
        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }
        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }
        public static Vector3 operator *(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
        }
        public static Vector3 operator /(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x / v2.x, v1.y / v2.y, v1.z / v2.z);
        }
        public float Magnitude()
        {
            return (float)Math.Sqrt(x*x+y*y+z*z);
        }
        public void Normalize()
        {
            float m = Magnitude();
            x /= m;
            y /= m;
            z /= m;
        }
        public float Dot(Vector3 v)
        {
            return (float)x*v.x+y*v.y+z*v.z;
        }
        public Vector3 Cross(Vector3 v)
        {
            return new Vector3(y * v.z - z * v.y,z * v.x - x * v.z,x * v.y - y * v.x);
        }

        public static Vector3 Min(Vector3 a, Vector3 b)
        {
            return new Vector3(Math.Min(a.x, b.x), Math.Min(a.y, b.y), Math.Min(a.z, b.z));
        }
        public static Vector3 Max(Vector3 a, Vector3 b)
        {
            return new Vector3(Math.Max(a.x, b.x), Math.Max(a.y, b.y), Math.Max(a.z, b.z));
        }
        public static Vector3 Clamp(Vector3 t, Vector3 a, Vector3 b)
        {
            return Max(b, Min(b, t));
        }
    }
}
