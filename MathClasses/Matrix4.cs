using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathClasses
{
    public class Matrix4
    {
        public float m1, m2, m3, m4, m5, m6, m7, m8, m9, m10, m11, m12, m13, m14, m15, m16;

        public Matrix4()
        {
            m1 = 1; m2 = 0; m3 = 0; m4 = 0;
            m5 = 0; m6 = 1; m7 = 0; m8 = 0;
            m9 = 0; m10 = 0; m11 = 1; m12 = 0;
            m13 = 0; m14 = 0; m15 = 0; m16 = 1;
        }
        public Matrix4(float one, float two, float three, float four, float five, float six, float seven, float eight, float nine, float ten, float eleven, float twelve, float thirteen, float fourteen, float fifteen, float sixteen)
        {
            m1 = one; m2 = two; m3 = three; m4 = four;
            m5 = five; m6 = six; m7 = seven; m8 = eight;
            m9 = nine; m10 = ten; m11 = eleven; m12 = twelve;
            m13 = thirteen; m14 = fourteen; m15 = fifteen; m16 = sixteen;
        }
        public static Matrix4 operator *(Matrix4 one, Matrix4 two)
        {
            return new Matrix4(
                (one.m1*two.m1)+(one.m5*two.m2)+(one.m9*two.m3)+(one.m13*two.m4),
                (one.m2*two.m1)+(one.m6*two.m2)+(one.m10*two.m3)+(one.m14*two.m4),
                (one.m3*two.m1)+(one.m7*two.m2)+(one.m11*two.m3)+(one.m15*two.m4),
                (one.m4*two.m1)+(one.m8*two.m2)+(one.m12*two.m3)+(one.m16*two.m4),

                (one.m1 * two.m5) + (one.m5 * two.m6) + (one.m9 * two.m7) + (one.m13 * two.m8),
                (one.m2 * two.m5) + (one.m6 * two.m6) + (one.m10 * two.m7) + (one.m14 * two.m8),
                (one.m3 * two.m5) + (one.m7 * two.m6) + (one.m11 * two.m7) + (one.m15 * two.m8),
                (one.m4 * two.m5) + (one.m8 * two.m6) + (one.m12 * two.m7) + (one.m16 * two.m8),

                (one.m1 * two.m9) + (one.m5 * two.m10) + (one.m9 * two.m11) + (one.m13 * two.m12),
                (one.m2 * two.m9) + (one.m6 * two.m10) + (one.m10 * two.m11) + (one.m14 * two.m12),
                (one.m3 * two.m9) + (one.m7 * two.m10) + (one.m11 * two.m11) + (one.m15 * two.m12),
                (one.m4 * two.m9) + (one.m8 * two.m10) + (one.m12 * two.m11) + (one.m16 * two.m12),

                (one.m1 * two.m13) + (one.m5 * two.m14) + (one.m9 * two.m15) + (one.m13 * two.m16),
                (one.m2 * two.m13) + (one.m6 * two.m14) + (one.m10 * two.m15) + (one.m14 * two.m16),
                (one.m3 * two.m13) + (one.m7 * two.m14) + (one.m11 * two.m15) + (one.m15 * two.m16),
                (one.m4 * two.m13) + (one.m8 * two.m14) + (one.m12 * two.m15) + (one.m16 * two.m16)
                );
        }
        public static Vector4 operator *(Vector4 vect, Matrix4 mat)
        {
            return new Vector4(
                (mat.m1 * vect.x) + (mat.m2 * vect.y) + (mat.m3 * vect.z) + (mat.m4 * vect.w),
                (mat.m5 * vect.x) + (mat.m6 * vect.y) + (mat.m7 * vect.z) + (mat.m8 * vect.w),
                (mat.m9 * vect.x) + (mat.m10 * vect.y) + (mat.m11 * vect.z) + (mat.m12 * vect.w),
                (mat.m13 * vect.x) + (mat.m14 * vect.y) + (mat.m15 * vect.z) + (mat.m16 * vect.w));
        }
        public static Vector4 operator *(Matrix4 mat, Vector4 vect)
        {
            return new Vector4(
                (mat.m1 * vect.x) + (mat.m5 * vect.y) + (mat.m9 * vect.z) + (mat.m13 * vect.w),
                (mat.m2 * vect.x) + (mat.m6 * vect.y) + (mat.m10 * vect.z) + (mat.m14 * vect.w),
                (mat.m3 * vect.x) + (mat.m7 * vect.y) + (mat.m11 * vect.z) + (mat.m15 * vect.w),
                (mat.m4 * vect.x) + (mat.m8 * vect.y) + (mat.m12 * vect.z) + (mat.m16 * vect.w));
        }
        public void Set(Matrix4 mat)
        {
            m1 = mat.m1;
            m2 = mat.m2;
            m3 = mat.m3;
            m4 = mat.m4;
            m5 = mat.m5;
            m6 = mat.m6;
            m7 = mat.m7;
            m8 = mat.m8;
            m9 = mat.m9;
            m10 = mat.m10;
            m11 = mat.m11;
            m12 = mat.m12;
            m13 = mat.m13;
            m14 = mat.m14;
            m15 = mat.m15;
            m16 = mat.m16;
        }
        public void SetRotateX(float radians)
        {
            Set(new Matrix4(
                1, 0, 0, 0,
                0, (float)Math.Cos(radians), (float)Math.Sin(radians), 0,
                0, (float)-Math.Sin(radians), (float)Math.Cos(radians), 0,
                0, 0, 0, 1
                ));
        }
        public void SetRotateY(float radians)
        {
            Set(new Matrix4(
                (float)Math.Cos(radians), 0, (float)-Math.Sin(radians), 0,
                0, 1, 0, 0,
                (float)Math.Sin(radians), 0, (float)Math.Cos(radians), 0,
                0, 0, 0, 1
                ));
        }
        public void SetRotateZ(float radians)
        {
            Set(new Matrix4(
                (float)Math.Cos(radians), (float)Math.Sin(radians), 0, 0,
                (float)-Math.Sin(radians), (float)Math.Cos(radians), 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1
                ));
        }


    }
}
