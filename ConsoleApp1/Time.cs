using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace NewRaylibGame
{
    class Time
    {
        Stopwatch stopwatch = new Stopwatch();
        private long currentTime = 0;
        private long lastTime = 0;
        private float deltaTime = 0.005f;
        public Time()
        {
            stopwatch.Start();
        }
        public void Reset()
        {
            stopwatch.Reset();
        }
        public float Seconds
        {
            get { return stopwatch.ElapsedMilliseconds / 1000f; }
        }
        public float GetDeltaTime()
        {
            lastTime = currentTime;
            currentTime = stopwatch.ElapsedMilliseconds;
            deltaTime = (currentTime - lastTime) / 1000f;
            return deltaTime;
        }
    }
}
