using System;
using System.Collections.Generic;
using System.Text;

namespace NewRaylibGame
{
    public class Vector3
    {
        public float x, y, z;

        public Vector3()
        {

        }
        public Vector3(float a, float b, float c)
        {
            x = a; y = b; z = c; 
        }
        public Vector3(Vector3 v)
        {
            x = v.x;y = v.y;z = v.z;
        }
        public static Vector3 operator *(Vector3 v, float numb)
        {
            return new Vector3(v.x*numb,v.y*numb,v.z*numb);
        }
        
    }
    class Matrix3
    {
        public float m1, m2, m3, m4, m5, m6, m7, m8, m9;
        public Matrix3()
        {

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
            Vector3 v = new Vector3(this*new Vector3(x,y,0));

            m1 += v.x;m4 += v.y;m7 += v.z;
        }
        public void SetTranslation(float x, float y)
        {
            Vector3 v = new Vector3(this * new Vector3(x, y, 0));

            m1 = v.x; m4 = v.y; m7 = v.z;
        }
        public void SetScaled(float x, float y, float z)
        {
            m1 = x; m2 = 0; m3 = 0;
            m4 = 0; m5 = y; m6 = 0;
            m7 = 0; m8 = 0; m9 = z;
        }        public void SetScaled(Vector3 v)
        {
            m1 = v.x; m2 = 0; m3 = 0;
            m4 = 0; m5 = v.y; m6 = 0;
            m7 = 0; m8 = 0; m9 = v.z;
        }        public void Set(Matrix3 m)
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
        }        public void Scale(float x, float y, float z)
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
            Set(new Matrix3((float)Math.Cos(radians), (float)Math.Sin(radians), 0,
                (float)-Math.Sin(radians), (float)Math.Cos(radians), 0,
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
