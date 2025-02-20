﻿using System;
using Raylib;
using static Raylib.Raylib;

namespace NewRaylibGame
{
    class SpriteObject : SceneObject
    {
        public Texture2D texture = new Texture2D();
        public float Width
        {
            get { return texture.width; }
        }
        public float Height
        {
            get { return texture.height; }
        }
        public SpriteObject()
        {
        }
        public void Load(string filename)
        {
            if (filename != null)
            {
                Image img = LoadImage(filename);
                texture = LoadTextureFromImage(img);
            }
            else
            {
                texture = new Texture2D();
            }
        }
        public override void OnDraw()
        {
            float rotation = (float)Math.Atan2(globalTransform.m4, globalTransform.m1);
            DrawTextureEx(texture,
                new Vector2(globalTransform.m3, globalTransform.m6),
                rotation * (float)(180.0f / Math.PI),
                1, Color.WHITE);
        }
    }
}