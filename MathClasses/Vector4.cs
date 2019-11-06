using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathClasses
{
    public class Vector4
    {
        public float x, y, z, w;

        public Vector4()
        {
            x = 0;y = 0;z = 0; w = 0;
        }
        public Vector4(float a, float b, float c, float d)
        {
            x = a; y = b; z = c; w = d;
        }
        public Vector4(Vector4 v)
        {
            x = v.x;y = v.y;z = v.z;w = v.w;
        }
        public static Vector4 operator*(float f, Vector4 v)
        {
            return new Vector4(f*v.x,f*v.y,f*v.z,f*v.w);
        }
        public static Vector4 operator*(Vector4 v, float f)
        {
            return new Vector4(f * v.x, f * v.y, f * v.z, f * v.w);
        }
        public static Vector4 operator +(Vector4 v1, Vector4 v2)
        {
            return new Vector4(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z, v1.w + v2.w);
        }
        public static Vector4 operator -(Vector4 v1, Vector4 v2)
        {
            return new Vector4(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z, v1.w - v2.w);
        }
        public static Vector4 operator /(Vector4 v1, Vector4 v2)
        {
            return new Vector4(v1.x / v2.x, v1.y / v1.y, v1.z / v2.z, v1.w / v2.w);
        }
        public static Vector4 operator *(Vector4 v1, Vector4 v2)
        {
            return new Vector4(v1.x * v2.x, v1.y * v1.y, v1.z * v2.z, v1.w * v2.w);
        }
        public float Dot(Vector4 v)
        {
            return (float)x * v.x + y * v.y + z * v.z + w * v.w;
        }
        public Vector4 Cross(Vector4 v)
        {
            return new Vector4(y * v.z - z * v.y, z * v.x - x * v.z, x * v.y - y * v.x, 0);

        }
        public float Magnitude()
        {
            return (float)Math.Sqrt(x * x + y * y + z * z+w*w);
        }
        public void Normalize()
        {
            float m = Magnitude();
            x /= m;
            y /= m;
            z /= m;
            w /= m;
        }

    }
}
