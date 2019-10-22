using System;
using Raylib;
using static Raylib.Raylib;

namespace NewRaylibGame
{
    class Firing : SpriteObject
    {
        public double time = 0;
        private Texture2D one = new Texture2D();
        private Texture2D two = new Texture2D();
        private Texture2D three = new Texture2D();
        private Texture2D four = new Texture2D();
        public Firing()
        {
            one = LoadTextureFromImage(LoadImage("resources/shotThin.png"));
            two = LoadTextureFromImage(LoadImage("resources/shotRed.png"));
            three = LoadTextureFromImage(LoadImage("resources/shotOrange.png"));
            four = LoadTextureFromImage(LoadImage("resources/shotLarge.png"));
        }
        public void isFiring()
        {
            Wait(0);
            texture = one;
            Wait(.0125f);
            texture = two;
            Wait(.0125f);
            texture = three;
            Wait(.0125f);
            texture = four;
            Wait(.0125f);
            texture = new Texture2D();
        }
        public void Wait(float gTime)
        {
            double times = GetTime();
            while(GetTime()-times > gTime)
            {
            }
        }
    }
}