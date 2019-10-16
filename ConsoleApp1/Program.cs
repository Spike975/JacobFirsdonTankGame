﻿using System;
using System.Diagnostics;
using Raylib;
using static Raylib.Raylib;

namespace NewRaylibGame
{

    static class Program
    {
        
        public static int Main()
        {
            Game game = new Game();
            // Initialization
            //--------------------------------------------------------------------------------------
            InitWindow(640, 480, "(bad tank pun)");
            game.Init();
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())    // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                // TODO: Update your variables here
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                game.Update();
                game.Draw();

                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            game.Shutdown();
            CloseWindow();        // Close window and OpenGL context
                                     //--------------------------------------------------------------------------------------

            return 0;
        }
    }
    struct Bullet
    {
        public Matrix3 transform;
        public float xSpeed;
        public float ySpeed;
    }
    class Game
    {
        public static SceneObject tankObject = new SceneObject();
        public static SceneObject turretObject = new SceneObject();
        public static SpriteObject tankSprite = new SpriteObject();
        public static SpriteObject turretSprite = new SpriteObject();
        public static SpriteObject bulletSpawner = new SpriteObject();
        Time time = new Time();
        Texture2D bullet = new Texture2D();
        Bullet[] bullets = new Bullet[5000];
        public int totalBullets = 0;
        Stopwatch stopwatch = new Stopwatch();
        private long currentTime = 0;
        private long lastTime = 0;
        private float timer = 0;
        private int fps = 1;
        private int frames;
        private float deltaTime = 0.005f;
        public void Init()
        {
            stopwatch.Start();
            lastTime = stopwatch.ElapsedMilliseconds;
            bullet = LoadTextureFromImage(LoadImage("resources/bulletBlue1_outline.png"));
            tankSprite.Load("resources/tankBody_blue_outline.png");
            // sprite is facing the wrong way... fix that here
            tankSprite.SetRotate(-90 * (float)(Math.PI / 180.0f));
            // sets an offset for the base, so it rotates around the centre
            tankSprite.SetPosition(-tankSprite.Width / 2.0f, tankSprite.Height / 2.0f);

            turretSprite.Load("resources/tankBlue_barrel2_outline.png");

            turretSprite.SetRotate(-90 * (float)(Math.PI / 180.0f));

            // set the turret offset from the tank base
            turretSprite.SetPosition(0, turretSprite.Width / 2.0f);


            turretObject.AddChild(turretSprite);
            turretObject.AddChild(bulletSpawner);
            tankObject.AddChild(tankSprite);
            tankObject.AddChild(turretObject);

            tankObject.SetPosition(GetScreenWidth() / 2.0f, GetScreenHeight() / 2.0f);

        }
        public void Shutdown()
        { }
        public void Update()
        {
            bulletSpawner.SetPosition(turretSprite.Height+10, -4);
            currentTime = stopwatch.ElapsedMilliseconds;
            deltaTime = (currentTime - lastTime) / 1000.0f;
            timer += deltaTime;
            if (timer >= 1)
            {
                fps = frames;
                frames = 0;
                timer -= 1;
            }
            frames++;
            if (IsKeyDown(KeyboardKey.KEY_A))
            {
                tankObject.Rotate(-deltaTime);
            }
            if (IsKeyDown(KeyboardKey.KEY_D))
            {
                tankObject.Rotate(deltaTime);
            }
            if (IsKeyDown(KeyboardKey.KEY_W))
            {
                Vector3 facing = new Vector3(
               tankObject.LocalTransform.m1,
               tankObject.LocalTransform.m4, 1) * deltaTime * 100;
                tankObject.Translate(facing.x, facing.y);
            }
            if (IsKeyDown(KeyboardKey.KEY_S))
            {
                Vector3 facing = new Vector3(
               tankObject.LocalTransform.m1,
               tankObject.LocalTransform.m4, 1) * deltaTime * -100;
                tankObject.Translate(facing.x, facing.y);
            }
            if (IsKeyDown(KeyboardKey.KEY_Q))
            {
                turretObject.Rotate(-deltaTime);
            }
            if (IsKeyDown(KeyboardKey.KEY_E))
            {
                turretObject.Rotate(deltaTime);
            }

            if (IsKeyPressed(KeyboardKey.KEY_SPACE))
            {
                if (totalBullets < 0)
                {
                    totalBullets = 0;
                }
                bullets[totalBullets].transform = bulletSpawner.GlobalTransform;
                bullets[totalBullets].xSpeed = 300 * bulletSpawner.GlobalTransform.m1;
                bullets[totalBullets].ySpeed = 300 * bulletSpawner.GlobalTransform.m4;
                totalBullets++;
            }
            tankObject.Update(deltaTime);

            lastTime = currentTime;
        }
        public void Draw()
        {
            BeginDrawing();
            ClearBackground(Color.WHITE);
            DrawText(fps.ToString(), 10, 10, 12, Color.RED);

            if (tankObject.GlobalTransform.m3 < -25)
            {
                tankObject.GlobalTransform.m3 = 660;
            }
            if (tankObject.GlobalTransform.m3 > 660)
            {
                tankObject.GlobalTransform.m3 = -25;
            }

            if (tankObject.GlobalTransform.m6 < -20)
            {
                tankObject.GlobalTransform.m6 = 510;
            }
            if (tankObject.GlobalTransform.m6 > 510)
            {
                tankObject.GlobalTransform.m6 = -20;
            }
            for (int i = 0; i < totalBullets; i++)
            {
                float rotation = (float)Math.Atan2(bullets[i].transform.m4, bullets[i].transform.m1);
                DrawTextureEx(bullet, new Vector2(bullets[i].transform.m3, bullets[i].transform.m6), rotation * (float)(180.0f / Math.PI) + 90, 1, Color.WHITE);
                bullets[i].transform.m3 += bullets[i].xSpeed * deltaTime;
                bullets[i].transform.m6 += bullets[i].ySpeed * deltaTime;

                if (bullets[i].transform.m3 > GetScreenWidth() + bullet.width)//640, 480
                {
                    for (int x = i; x < 19; x++)
                    {
                        bullets[x] = bullets[x + 1];
                    }
                    totalBullets--;
                }
                else if (bullets[i].transform.m3 < -bullet.width)
                {
                    for (int x = i; x < 19; x++)
                    {
                        bullets[x] = bullets[x + 1];
                    }
                    totalBullets--;
                }
                else if (bullets[i].transform.m6 > GetScreenHeight() + bullet.height)
                {
                    for (int x = i; x < 19; x++)
                    {
                        bullets[x] = bullets[x + 1];
                    }
                    totalBullets--;
                }
                else if (bullets[i].transform.m6 < -bullet.height)
                {
                    for (int x = i; x < 19; x++)
                    {
                        bullets[x] = bullets[x + 1];
                    }
                    totalBullets--;
                }
            }
            tankObject.Draw();
            EndDrawing();
        }
    }
}
