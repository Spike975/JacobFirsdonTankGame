using System;
using System.Collections.Generic;
using System.Text;

namespace NewRaylibGame
{
    //Not used
    class OBB
    {

    }
    //Not used
    class AABB
    {
        public Vector3 min = new Vector3();
        public Vector3 max = new Vector3();

        public AABB()
        {

        }
        public AABB(Vector3 min, Vector3 max)
        {
            this.max = max; this.min = min;
        }
        public void SetMin(Vector3 v)
        {
            min = v;
        }
        public void SetMax(Vector3 v)
        {
            max = v;
        }
        public Vector3 Center()
        {
            return (min + max) * .5f;
        }
        public Vector3 Extents()
        {
            return new Vector3(Math.Abs(max.x - min.x) * 0.5f,
                Math.Abs(max.y - min.y) * 0.5f,
                Math.Abs(max.z - min.z) * 0.5f);
        }
        public List<Vector3> Corners()
        {
            // ignoring z axis for 2D
            List<Vector3> corners = new List<Vector3>(4);
            corners.Add(min);
            corners.Add(new Vector3(min.x, max.y, min.z));
            corners.Add(max);
            corners.Add(new Vector3(max.x, min.y, min.z));
            return corners;
        }
        public void Fit(List<Vector3> points)
        {
            min = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
            max = new Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);
            foreach (Vector3 p in points)
            {
                min = Vector3.Min(min, p);
                max = Vector3.Max(max, p);
            }
        }
        public void Fit(Vector3[] points)
        {
            min = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
            max = new Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);
            foreach (Vector3 p in points)
            {
                min = Vector3.Min(min, p);
                max = Vector3.Max(max, p);
            }
        }
        public bool Overlaps(Vector3 p)
        {
            return !(p.x < min.x || p.y < min.y || p.x > max.x || p.y > max.y);
        }
        public bool Overlaps(AABB other)
        {
            return !(max.x < other.min.x || max.y < other.min.y || min.x > other.max.x || min.y > other.max.y);
        }
        public Vector3 ClosestPoint(Vector3 p)
        {
            return Vector3.Clamp(p, min, max);
        }
    }
}
