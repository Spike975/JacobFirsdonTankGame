using System;
using System.Collections.Generic;
using System.Text;

namespace NewRaylibGame
{
    //Math used For game. Highly likely that it wont work for testing. For that use MathForTest Namespace or go to that class
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
        /// <summary>
        /// Multiplies a vector by a float
        /// </summary>
        /// <param name="v">vector</param>
        /// <param name="numb">float</param>
        /// <returns></returns>
        public static Vector3 operator *(Vector3 v, float numb)
        {
            return new Vector3(v.x * numb, v.y * numb, v.z * numb);
        }
        /// <summary>
        /// Multiplies a vector by a float
        /// </summary>
        /// <param name="v">vector</param>
        /// <param name="numb">float</param>
        /// <returns></returns>
        public static Vector3 operator *(float numb, Vector3 v)
        {
            return new Vector3(v.x * numb, v.y * numb, v.z * numb);
        }
        /// <summary>
        /// Adds two vectors together
        /// </summary>
        /// <param name="v1">Vector 1</param>
        /// <param name="v2">Vector 2</param>
        /// <returns></returns>
        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }
        /// <summary>
        /// Subtracts two vectors
        /// </summary>
        /// <param name="v1">Vector 1</param>
        /// <param name="v2">Vector 2</param>
        /// <returns></returns>
        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }
        /// <summary>
        /// Multiplies two vectors together
        /// </summary>
        /// <param name="v1">Vector 1</param>
        /// <param name="v2">Vector 2</param>
        /// <returns></returns>
        public static Vector3 operator *(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
        }
        /// <summary>
        /// Divides two vectors
        /// </summary>
        /// <param name="v1">Vector 1</param>
        /// <param name="v2">Vector 2</param>
        /// <returns></returns>
        public static Vector3 operator /(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x / v2.x, v1.y / v2.y, v1.z / v2.z);
        }
        /// <summary>
        /// Returns the Magnitude of the vectors
        /// </summary>
        public float Magnitude()
        {
            return (float)Math.Sqrt(x * x + y * y + z * z);
        }
        /// <summary>
        /// Returns the square of the vectors
        /// </summary>
        public float MagnitudeSqr()
        {
            return Magnitude() * Magnitude();
        }
        /// <summary>
        /// Normalizes the vector
        /// </summary>
        public void Normalize()
        {
            float m = Magnitude();
            x /= m;
            y /= m;
            z /= m;
        }
        /// <summary>
        /// Finds the Dot of the given vector and the current vector
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public float Dot(Vector3 v)
        {
            return (float)x * v.x + y * v.y + z * v.z;
        }
        /// <summary>
        /// Finds the cross product of the given and curent vector
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public Vector3 Cross(Vector3 v)
        {
            return new Vector3(y * v.z - z * v.y, z * v.x - x * v.z, x * v.y - y * v.x);
        }
        /// <summary>
        /// Makes a new vector based off the given and current vectors to find the smallest numbers
        /// </summary>
        /// <param name="a">Vector 1</param>
        /// <param name="b">Vector 2</param>
        /// <returns></returns>
        public static Vector3 Min(Vector3 a, Vector3 b)
        {
            return new Vector3(Math.Min(a.x, b.x), Math.Min(a.y, b.y), Math.Min(a.z, b.z));
        }
        /// <summary>
        /// Makes a new vector based off the given and current vectors to find the largest numbers
        /// </summary>
        /// <param name="a">Vector 1</param>
        /// <param name="b">Vector 2</param>
        /// <returns></returns>
        public static Vector3 Max(Vector3 a, Vector3 b)
        {
            return new Vector3(Math.Max(a.x, b.x), Math.Max(a.y, b.y), Math.Max(a.z, b.z));
        }
        /// <summary>
        /// Clamps the three given vectors
        /// </summary>
        /// <param name="t"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector3 Clamp(Vector3 t, Vector3 a, Vector3 b)
        {
            return Max(b, Min(b, t));
        }
    }
    class Matrix3
    {
        public float m1, m2, m3, m4, m5, m6, m7, m8, m9;
        public Matrix3()
        {
            m1 = 1; m2 = 0; m3 = 0; 
            m4 = 0; m5 = 1; m6 = 0;
            m7 = 0;  m8 = 0; m9 = 1; 
        }
        public Matrix3(float one, float two, float three, float four, float five, float six, float seven, float eight, float nine)
        {
            m1 = one; m2 = two; m3 = three;
            m4 = four; m5 = five; m6 = six;
            m7 = seven; m8 = eight; m9 = nine;
        }
        public Matrix3(Matrix3 m)
        {
            m1 = m.m1; m2 = m.m2; m3 = m.m3;
            m4 = m.m4; m5 = m.m5; m6 = m.m6;
            m7 = m.m7; m8 = m.m8; m9 = m.m9;
        }
        public static Matrix3 operator *(Matrix3 m1, Matrix3 m2)
        {
            return new Matrix3(
            (m1.m1 * m2.m1) + (m1.m2 * m2.m4) + (m1.m3 * m2.m7),
            (m1.m1 * m2.m2) + (m1.m2 * m2.m5) + (m1.m3 * m2.m8),
            (m1.m1 * m2.m3) + (m1.m2 * m2.m6) + (m1.m3 * m2.m9),
            (m1.m4 * m2.m1) + (m1.m5 * m2.m4) + (m1.m6 * m2.m7),
            (m1.m4 * m2.m2) + (m1.m5 * m2.m5) + (m1.m6 * m2.m8),
            (m1.m4 * m2.m3) + (m1.m5 * m2.m6) + (m1.m6 * m2.m9),
            (m1.m7 * m2.m1) + (m1.m8 * m2.m4) + (m1.m9 * m2.m7),
            (m1.m7 * m2.m2) + (m1.m8 * m2.m5) + (m1.m9 * m2.m8),
            (m1.m7 * m2.m3) + (m1.m8 * m2.m6) + (m1.m9 * m2.m9));
        }
        public static Matrix3 operator +(Matrix3 m1, Matrix3 m2)
        {
            Matrix3 m3 = new Matrix3();
            m3.m1 = m1.m1 + m2.m1;
            m3.m2 = m1.m2 + m2.m2;
            m3.m3 = m1.m3 + m2.m3;
            m3.m4 = m1.m4 + m2.m4;
            m3.m5 = m1.m5 + m2.m5;
            m3.m6 = m1.m6 + m2.m6;
            m3.m7 = m1.m7 + m2.m7;
            m3.m8 = m1.m8 + m2.m8;
            m3.m9 = m1.m9 + m2.m9;
            return m3;
        }
        public static Matrix3 operator -(Matrix3 m1, Matrix3 m2)
        {
            Matrix3 m3 = new Matrix3();
            m3.m1 = m1.m1 - m2.m1;
            m3.m2 = m1.m2 - m2.m2;
            m3.m3 = m1.m3 - m2.m3;
            m3.m4 = m1.m4 - m2.m4;
            m3.m5 = m1.m5 - m2.m5;
            m3.m6 = m1.m6 - m2.m6;
            m3.m7 = m1.m7 - m2.m7;
            m3.m8 = m1.m8 - m2.m8;
            m3.m9 = m1.m9 - m2.m9;
            return m3;
        }
        public static Matrix3 Transpose(Matrix3 m)
        {
            return new Matrix3(m.m1, m.m4, m.m7, m.m2, m.m5, m.m8, m.m3, m.m6, m.m9);
        }
        public static Vector3 operator *(Matrix3 m, Vector3 v)
        {
            Vector3 vN = new Vector3();
            vN.x = (m.m1 * v.x) + (m.m2 * v.y) + (m.m3 * v.z);
            vN.y = (m.m4 * v.x) + (m.m5 * v.y) + (m.m6 * v.z);
            vN.z = (m.m7 * v.x) + (m.m8 * v.y) + (m.m9 * v.z);
            return vN;
        }
        public void Translate(float x,float y)
        {
            m3 += x; m6 += y;
        }
        public void SetTranslation(float x, float y)
        {
            m3 = x; m6 = y;
        }
        public void SetScaled(float x, float y, float z)
        {
            m1 = x; m2 = 0; m3 = 0;
            m4 = 0; m5 = y; m6 = 0;
            m7 = 0; m8 = 0; m9 = z;
        }
        public void SetScaled(Vector3 v)
        {
            m1 = v.x; m2 = 0; m3 = 0;
            m4 = 0; m5 = v.y; m6 = 0;
            m7 = 0; m8 = 0; m9 = v.z;
        }
        public void Set(Matrix3 m)
        {
            m1 = m.m1;
            m2 = m.m2;
            m3 = m.m3;
            m4 = m.m4;
            m5 = m.m5;
            m6 = m.m6;
            m7 = m.m7;
            m8 = m.m8;
            m9 = m.m9;
        }
        public void Scale(float x, float y, float z)
        {
            Matrix3 m = new Matrix3();
            m.SetScaled(x, y, z);
            Set(this * m);
        }
        public void SetRotateX(double radians)
        {
            Set(new Matrix3(1, 0, 0,
                0, (float)Math.Cos(radians), (float)Math.Sin(radians),
                0, (float)-Math.Sin(radians), (float)Math.Cos(radians)));
        }
        public void SetRotateY(double radians)
        {
            Set(new Matrix3((float)Math.Cos(radians), 0, (float)-Math.Sin(radians),
                0, 1,0,
                (float)Math.Sin(radians), 0, (float)Math.Cos(radians)));
        }
        public void SetRotateZ(double radians)
        {
            Set(new Matrix3((float)Math.Cos(radians), (float)-Math.Sin(radians), 0,
                (float)Math.Sin(radians), (float)Math.Cos(radians), 0,
                0, 0, 1));
        }
        public void RotateX(double radians)
        {
            Matrix3 m = new Matrix3();
            m.SetRotateX(radians);
            Set(this * m);
        }
        public void RotateY(double radians)
        {
            Matrix3 m = new Matrix3();
            m.SetRotateY(radians);
            Set(this * m);
        }
        public void RotateZ(double radians)
        {
            Matrix3 m = new Matrix3();
            m.SetRotateZ(radians);
            Set(this * m);
        }
    }
}