using System;
using System.Diagnostics;
using Raylib;
using static Raylib.Raylib;

namespace NewRaylibGame
{

    static class Program
    {
        public static Game game = new Game();

        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            InitWindow(1000, 1000, "(bad tank pun)");
            SetTargetFPS(60);
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
        public double deathTime;
    }
    class Game
    {
        Random rand = new Random();
        SceneObject turretObject = new SceneObject();
        SceneObject tankObject = new SceneObject();
        SceneObject bulletSpawner = new SceneObject();
        SceneObject ammoObject = new SceneObject();
        SpriteObject tankSprite = new SpriteObject();
        SpriteObject turretSprite = new SpriteObject();
        SpriteObject ammoSprite = new SpriteObject();
        SpriteObject bulletExplosion = new SpriteObject();
        Texture2D[] barrels = new Texture2D[5];
        Texture2D one = new Texture2D();
        Texture2D two = new Texture2D();
        Texture2D three = new Texture2D();
        Texture2D four = new Texture2D();
        Texture2D crate = new Texture2D();
        Texture2D bulletSprite = new Texture2D();
        Time time = new Time();
        Stopwatch stopwatch = new Stopwatch();
        long currentTime = 0;
        long lastTime = 0;
        bool fire = false;
        float timer = 0;
        float tankSpace = 18;//17.3969696f;
        float deltaTime = 0.005f;
        float bulletSpeed = 0;
        double shotTime = 0f;
        double crateTime = 0;
        double phase = 0;
        int totalBullets = 0;
        int flash = 0;
        int speed = 0;
        int barrel = 0;
        int totalCrates = 0;
        int fps = 1;
        int frames;
        static int capacity = 10;
        int bull = capacity;
        AABB tankOuter = new AABB();
        AABB obb = new AABB();
        AABB[] crates = new AABB[10];
        AABB[] bulletOuter = new AABB[capacity];
        Bullet[] bullets = new Bullet[capacity];
        public void Init()
        {
            stopwatch.Start();
            lastTime = stopwatch.ElapsedMilliseconds;
            bulletSprite = LoadTextureFromImage(LoadImage("resources/bulletBlue1_outline.png"));
            tankSprite.Load("resources/tankBody_blue_outline.png");
            // sprite is facing the wrong way... fix that here
            tankSprite.SetRotate(-90 * (float)(Math.PI / 180.0f));
            // sets an offset for the base, so it rotates around the centre
            tankSprite.SetPosition(-tankSprite.Width / 2.0f, tankSprite.Height / 2.0f);
            crate = LoadTextureFromImage(LoadImage($"resources/crate.png"));
            Console.WriteLine(crate.width + ", " + crate.height);
            turretSprite.Load("resources/barrel1.png");
            one = LoadTextureFromImage(LoadImage("resources/shotThin.png"));
            two = LoadTextureFromImage(LoadImage("resources/shotRed.png"));
            three = LoadTextureFromImage(LoadImage("resources/shotOrange.png"));
            four = LoadTextureFromImage(LoadImage("resources/shotLarge.png"));
            for (int i = 0; i < barrels.Length; i++)
                barrels[i] = LoadTextureFromImage(LoadImage($"resources/barrel{i + 1}.png"));

            for (int i = 0; i < bulletOuter.Length; i++)
                bulletOuter[i] = new AABB();

            for (int i = 0; i < crates.Length; i++)
                crates[i] = new AABB();


            turretSprite.SetRotate(-90 * (float)(Math.PI / 180.0f));
            bulletExplosion.SetRotate(-90 * (float)(Math.PI / 180.0f));

            // set the turret offset from the tank base
            turretSprite.SetPosition(0, turretSprite.Width / 2.0f);

            bulletSpawner.AddChild(bulletExplosion);
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
            //MAKE FOUR DOTS AROUND TANK FOR OBB! PLEASES
            tankOuter.min = new Vector3(tankObject.GlobalTransform.m3 - tankSprite.Width / 2f - (tankSpace * Math.Abs(tankObject.GlobalTransform.m1 * tankObject.GlobalTransform.m2)),
                (tankObject.GlobalTransform.m6 - tankSprite.Height / 2f) - (tankSpace * Math.Abs(tankObject.GlobalTransform.m4 * tankObject.GlobalTransform.m5)), 0f);

            tankOuter.max = new Vector3(tankSprite.Width + 1 + (2 * tankSpace * Math.Abs(tankObject.GlobalTransform.m1 * tankObject.GlobalTransform.m2)),
                tankSprite.Height + 1 + (2 * tankSpace * Math.Abs(tankObject.GlobalTransform.m4 * tankObject.GlobalTransform.m5)), 0);

            obb.min.x = tankSprite.GlobalTransform.m3;
            obb.min.y = tankSprite.GlobalTransform.m6;
            obb.max.x = tankSprite.GlobalTransform.m6 - tankSprite.Width / 2f - (tankSpace * Math.Abs(tankObject.GlobalTransform.m1 * tankObject.GlobalTransform.m2));
            obb.max.y = tankSprite.GlobalTransform.m3 - tankSprite.Height / 2f - (tankSpace * Math.Abs(tankObject.GlobalTransform.m4 * tankObject.GlobalTransform.m5));

            for (int i = 0; i < totalBullets; i++)
            {
                bulletOuter[i].min = new Vector3(bullets[i].transform.m3 - bulletSprite.width / 2f - (6 * Math.Abs(bullets[i].transform.m1 * bullets[i].transform.m2)),
                    bullets[i].transform.m6 - bulletSprite.height / 2f - (6 * Math.Abs(bullets[i].transform.m4 * bullets[i].transform.m5)), 0);
                bulletOuter[i].max = new Vector3(bulletSprite.width + (6 * 2 * Math.Abs(bullets[i].transform.m1 * bullets[i].transform.m2)),
                    bulletSprite.height + (6 * 2 * Math.Abs(bullets[i].transform.m4 * bullets[i].transform.m5)), 0);
            }

            bulletSpawner.SetPosition(turretSprite.Height + 10, -4);
            bulletExplosion.SetPosition(turretSprite.Height - 40, 14);
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
            totalCrates = 0;

            for (int i = 0; i < totalBullets; i++)
            {
                bullets[i].deathTime += deltaTime;
            }
            foreach (AABB f in crates)
            {
                if (f.max.x != 0 && f.max.y != 0)
                {
                    totalCrates++;
                }
            }
            if ((GetTime() - crateTime > 1f || crateTime == 0) && totalCrates < 10)
            {
                crates[totalCrates].min.x = rand.Next(10, GetScreenWidth() - crate.width - 10);
                crates[totalCrates].min.y = rand.Next(10, GetScreenHeight() - crate.height - 10);
                crates[totalCrates].max.x = crate.width;
                crates[totalCrates].max.y = crate.height;
                crateTime = GetTime();
                totalCrates++;
                Console.WriteLine(totalCrates);
            }

            if (IsKeyDown(KeyboardKey.KEY_LEFT_SHIFT))
            {
                speed = 2;
            }
            else
            {
                speed = 1;
            }

            if (IsKeyDown(KeyboardKey.KEY_A))
            {
                tankObject.Rotate(-deltaTime * speed);
            }
            if (IsKeyDown(KeyboardKey.KEY_D))
            {
                tankObject.Rotate(deltaTime * speed);
            }
            if (IsKeyDown(KeyboardKey.KEY_W))
            {
                Vector3 facing = new Vector3(
               tankObject.LocalTransform.m1,
               tankObject.LocalTransform.m4, 1) * deltaTime * 100 * speed;
                tankObject.Translate(facing.x, facing.y);
            }
            if (IsKeyDown(KeyboardKey.KEY_S))
            {
                Vector3 facing = new Vector3(
               tankObject.LocalTransform.m1,
               tankObject.LocalTransform.m4, 1) * deltaTime * -100 * speed;
                tankObject.Translate(facing.x, facing.y);
            }
            if (IsKeyDown(KeyboardKey.KEY_Q))
            {
                turretObject.Rotate(-deltaTime * speed);
            }
            if (IsKeyDown(KeyboardKey.KEY_E))
            {
                turretObject.Rotate(deltaTime * speed);
            }
            if (IsKeyDown(KeyboardKey.KEY_SPACE) && GetTime() - shotTime > 1f && capacity > 0)
            {

                bullets[totalBullets].transform = bulletSpawner.GlobalTransform;
                totalBullets++;
                capacity--;
                shotTime = GetTime();
                fire = true;
                phase = GetTime();
            }
            
            if (IsKeyDown(KeyboardKey.KEY_R))
            {
                capacity += bull;
            }

            if (IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON))
            {
                bulletSpeed += 200;
            }
            if (IsMouseButtonPressed(MouseButton.MOUSE_RIGHT_BUTTON))
            {
                bulletSpeed -= 200;
            }
            if (IsKeyPressed(KeyboardKey.KEY_U))
            {
                tankObject.SetPosition(GetScreenWidth() / 2f, GetScreenHeight() / 2f);
            }
            if (IsKeyPressed(KeyboardKey.KEY_TAB))
            {
                barrel++;
                if (barrel > 4)
                {
                    barrel = 0;
                }
                turretSprite.texture = barrels[barrel];
            }
            if (IsKeyPressed(KeyboardKey.KEY_O))
            {
                float x = tankObject.GlobalTransform.m3;
                float y = tankObject.GlobalTransform.m6;
                tankObject.SetRotate(0);
                tankObject.SetPosition(x, y);
            }

            for (int i = 0; i < totalCrates; i++)
            {
                if (CheckCollisionRecs(new Rectangle(tankOuter.min.x, tankOuter.min.y, tankOuter.max.x, tankOuter.max.y), new Rectangle(crates[i].min.x, crates[i].min.y, crates[i].max.x, crates[i].max.y)))
                {
                    capacity += bull;
                    crates[i] = new AABB();
                    totalCrates--;
                }
            }
            for (int i = 0; i < totalCrates; i++)
            {
                for (int x = i + 1; x < totalCrates; x++)
                {
                    if (CheckCollisionRecs(new Rectangle(crates[x].min.x, crates[x].min.y, crates[x].max.x, crates[x].max.y), new Rectangle(crates[i].min.x, crates[i].min.y, crates[i].max.x, crates[i].max.y)))
                    {
                        crates[x].min.x = rand.Next(10, GetScreenWidth() - crate.width - 10);
                        crates[x].min.y = rand.Next(10, GetScreenHeight() - crate.height - 10);
                    }
                }
            }
            for (int i = 0; i < totalBullets; i++)
            {
                if (bullets[i].deathTime > 10)
                {
                    for (int x = i; x < bull - 1; x++)
                    {
                        bullets[x] = bullets[x + 1];
                    }
                    totalBullets--;
                }
            }
            tankObject.Update(deltaTime);
            lastTime = currentTime;
        }
        public void Draw()
        {
            BeginDrawing();
            ClearBackground(Color.WHITE);
            DrawLine((int)obb.min.x, (int)obb.min.y, (int)obb.max.x, (int)obb.max.y, Color.BLACK);
            //DrawLine((int)(obb.min.x + tankSprite.Width), (int)obb.min.y, 0, 0, Color.BLACK);
            //DrawLine((int)(obb.min.x), (int)(obb.min.y - tankSprite.Height), 0, 0, Color.BLACK);
            //DrawLine((int)(obb.min.x + tankSprite.Width), (int)(obb.min.y - tankSprite.Height), 0, 0, Color.BLACK);

            if (fire && GetTime() - phase > .02f)
            {
                if (flash == 0)
                {
                    bulletExplosion.texture = one;
                    phase = GetTime();
                }
                if (flash == 1)
                {
                    bulletExplosion.texture = two;
                    phase = GetTime();
                }
                if (flash == 2)
                {
                    bulletExplosion.texture = three;
                    phase = GetTime();
                }
                if (flash == 3)
                {
                    bulletExplosion.texture = four;
                    phase = GetTime();
                }
                if (flash == 4)
                {
                    bulletExplosion.texture = new Texture2D();
                    fire = false;
                    phase = GetTime();
                }
                flash++;
                if (flash == 5)
                {
                    flash = 0;
                }
            }

            if (tankObject.GlobalTransform.m3 < -tankSprite.Width)
            {
                tankObject.GlobalTransform.m3 = GetScreenWidth() + tankSprite.Width;
            }
            if (tankObject.GlobalTransform.m3 > GetScreenWidth() + tankSprite.Width)
            {
                tankObject.GlobalTransform.m3 = -tankSprite.Width;
            }

            if (tankObject.GlobalTransform.m6 < -tankSprite.Height)
            {
                tankObject.GlobalTransform.m6 = GetScreenHeight() + tankSprite.Height;
            }
            if (tankObject.GlobalTransform.m6 > GetScreenHeight() + tankSprite.Height)
            {
                tankObject.GlobalTransform.m6 = -tankSprite.Height;
            }

            for (int i = 0; i < crates.Length; i++)
            {
                bool c = false;
                if (crates[i].max.x == 0 && crates[i].max.y == 0)
                {
                    for (int x = i; x < crates.Length; x++)
                    {
                        if (crates[x].max.x != 0 && crates[x].max.y != 0)
                        {
                            c = true;
                            break;
                        }
                    }
                    if (c)
                    {
                        for (int x = i; x < crates.Length; x++)
                        {
                            if (x < crates.Length - 1)
                            {
                                crates[x] = crates[x + 1]; crates[x + 1] = new AABB();
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < totalBullets; i++)
            {
                float rotation = (float)Math.Atan2(bullets[i].transform.m4, bullets[i].transform.m1);
                DrawTextureEx(bulletSprite, new Vector2(bullets[i].transform.m3, bullets[i].transform.m6), rotation * (float)(180.0f / Math.PI) + 90, 1, Color.WHITE);

                //DrawRectangleLines((int)bulletOuter[i].min.x, (int)bulletOuter[i].min.y, (int)bulletOuter[i].max.x, (int)bulletOuter[i].max.y, Color.BLACK);

                //bulletZone[i].Draw();

                bullets[i].transform.m3 += bulletSpeed * bullets[i].transform.m1 * deltaTime;
                bullets[i].transform.m6 += bulletSpeed * bullets[i].transform.m4 * deltaTime;

                if (bullets[i].transform.m3 > GetScreenWidth() + bulletSprite.width)
                {
                    bullets[i].transform.m3 = -bulletSprite.width;
                }
                if (bullets[i].transform.m3 < -bulletSprite.width)
                {
                    bullets[i].transform.m3 = GetScreenWidth() + bulletSprite.width;
                }
                if (bullets[i].transform.m6 > GetScreenHeight() + bulletSprite.height)
                {
                    bullets[i].transform.m6 = -bulletSprite.height;
                }
                if (bullets[i].transform.m6 < -bulletSprite.height)
                {
                    bullets[i].transform.m6 = GetScreenHeight() + bulletSprite.height;
                }
                //if (bullets[i].transform.m3 > GetScreenWidth() + bulletSprite.width)//640, 480
                //{
                //    for (int x = i; x < bull - 1; x++)
                //    {
                //        bullets[x] = bullets[x + 1];
                //        bulletZone[x] = bulletZone[x + 1];
                //    }
                //    totalBullets--;
                //}
                //else if (bullets[i].transform.m3 < -bulletSprite.width)
                //{
                //    for (int x = i; x < bull - 1; x++)
                //    {
                //        bullets[x] = bullets[x + 1];
                //        bulletZone[x] = bulletZone[x + 1];
                //    }
                //    totalBullets--;
                //}
                //else if (bullets[i].transform.m6 > GetScreenHeight() + bulletSprite.height)
                //{
                //    for (int x = i; x < bull - 1; x++)
                //    {
                //        bullets[x] = bullets[x + 1];
                //        bulletZone[x] = bulletZone[x + 1];
                //    }
                //    totalBullets--;
                //}
                //else if (bullets[i].transform.m6 < -bulletSprite.height)
                //{
                //    for (int x = i; x < bull - 1; x++)
                //    {
                //        bullets[x] = bullets[x + 1];
                //        bulletZone[x] = bulletZone[x + 1];
                //    }
                //    totalBullets--;
                //}
            }

            for (int i = 0; i < totalCrates; i++)
            {
                DrawTexture(crate, (int)crates[i].min.x, (int)crates[i].min.y, Color.WHITE);
                DrawRectangleLines((int)crates[i].min.x, (int)crates[i].min.y, (int)crates[i].max.x, (int)crates[i].max.y, Color.BLACK);
            }
            //DrawRectangleLines((int)tank.min.x, (int)tank.min.y, (int)(tank.max.x - tank.min.x), (int)(tank.max.y-tank.min.y),Color.BLACK);
            //DrawRectangleLines((int)tankOuter.min.x, (int)tankOuter.min.y,(int)tankOuter.max.x, (int)tankOuter.max.y, Color.BLACK);

            //DrawRectanglePro(new Rectangle(tankObject.GlobalTransform.m3, tankObject.GlobalTransform.m6, tankSprite.Width, tankSprite.Height), new Vector2(tankSprite.Width / 2f, tankSprite.Height / 2f), (float)Math.Atan2(tankSprite.GlobalTransform.m4, tankSprite.GlobalTransform.m1) * (float)(180.0f / Math.PI) + 90, Color.RED);
            //bulletExplosion.isFiring();

            tankObject.Draw();

            DrawText(fps.ToString(), 10, 10, 12, Color.RED);
            DrawText(capacity.ToString(), 10, GetScreenHeight() - 20, 12, Color.RED);
            EndDrawing();
        }
    }
}
