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
        SceneObject turretObject = new SceneObject();
        SceneObject tankObject = new SceneObject();
        SceneObject bulletSpawner = new SceneObject();
        SceneObject ammoObject = new SceneObject();
        SpriteObject tankSprite = new SpriteObject();
        SpriteObject turretSprite = new SpriteObject();
        SpriteObject ammoSprite = new SpriteObject();
        Firing bulletExplosion = new Firing();
        Texture2D[] barrels = new Texture2D[5];
        AABB tank = new AABB();
        static Circle[] bulletZone = new Circle[5000];
        static Circle c1 = new Circle();
        static Time time = new Time();
        static Texture2D bullet = new Texture2D();
        Bullet[] bullets = new Bullet[5000];
        int totalBullets = 0;
        Stopwatch stopwatch = new Stopwatch();
        long currentTime = 0;
        long lastTime = 0;
        float timer = 0;
        int speed = 0;
        int barrel = 0;
        int fps = 1;
        int frames;
        int capacity = 20;
        float deltaTime = 0.005f;
        double shotTime = 0f;
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

            turretSprite.Load("resources/barrel1.png");
            barrel = 1;
            for(int i = 0; i < barrels.Length;i++)
                barrels[i] = LoadTextureFromImage(LoadImage($"resources/barrel{i+1}.png"));

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
            
            c1.SetColor(Color.BLACK);
            c1.SetX(5);
            c1.SetY(5);
            c1.SetR(20);
        }
        public void Shutdown()
        { }
        public void Update()
        {
            tank.SetMin(new Vector3(tankSprite.GlobalTransform.m3, tankSprite.GlobalTransform.m6, 0));
            tank.SetMax(new Vector3(tankObject.GlobalTransform.m3+tankSprite.Width, tankObject.GlobalTransform.m6+tankSprite.Height, 0));
            bulletSpawner.SetPosition(turretSprite.Height+10, -4);
            bulletExplosion.SetPosition(turretSprite.Height-40, 14);
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
                tankObject.Rotate(-deltaTime*speed);
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
                turretObject.Rotate(-deltaTime);
            }
            if (IsKeyDown(KeyboardKey.KEY_E))
            {
                turretObject.Rotate(deltaTime);
            }

            if (IsKeyDown(KeyboardKey.KEY_SPACE)&&GetTime()-shotTime > .5f&&capacity > 0)
            {
                if (totalBullets < 0)
                {
                    totalBullets = 0;
                }
                bullets[totalBullets].transform = bulletSpawner.GlobalTransform;
                bullets[totalBullets].xSpeed = 300 * bulletSpawner.GlobalTransform.m1;
                bullets[totalBullets].ySpeed = 300 * bulletSpawner.GlobalTransform.m4;
                bulletZone[totalBullets] = new Circle();
                bulletZone[totalBullets].SetColor(Color.BLACK);
                bulletZone[totalBullets].SetR(bullet.height / 2f);
                bulletZone[totalBullets].SetX(bullets[totalBullets].transform.m3);
                bulletZone[totalBullets].SetY(bullets[totalBullets].transform.m6);
                totalBullets++;
                capacity--;
                shotTime = GetTime();
                bulletExplosion.time = GetTime();
            }
            if (IsKeyPressed(KeyboardKey.KEY_TAB))
            {
                barrel++;
                if (barrel > 5)
                {
                    barrel = 1;
                }
                turretSprite.texture = barrels[barrel];
            }
            if (IsKeyPressed(KeyboardKey.KEY_R)&&capacity == 0)
            {
                capacity = 20;
            }

            if (IsKeyDown(KeyboardKey.KEY_UP))
            {
                c1.ChangeY(-deltaTime * 100);
            }
            if (IsKeyDown(KeyboardKey.KEY_DOWN))
            {
                c1.ChangeY(deltaTime * 100);
            }
            if (IsKeyDown(KeyboardKey.KEY_LEFT))
            {
                c1.ChangeX(-deltaTime * 100);
            }
            if (IsKeyDown(KeyboardKey.KEY_RIGHT))
            {
                c1.ChangeX(deltaTime * 100);
            }
            for (int i = 0; i < totalBullets; i++) {
                if (CheckCollisionCircles(new Vector2(c1.x, c1.y), c1.radius, new Vector2(bulletZone[i].x,bulletZone[i].y), bulletZone[i].radius))
                {
                    c1.SetColor(Color.BLUE);
                }
                else //if(!CheckCollisionCircles(new Vector2(c1.x, c1.y), c1.radius, new Vector2(bulletZone[i].x, bulletZone[i].y), bulletZone[i].radius))
                {
                    c1.SetColor(Color.BLACK);
                }
            }
            tankObject.Update(deltaTime);

            lastTime = currentTime;
        }
        public void Draw()
        {
            BeginDrawing();
            ClearBackground(Color.WHITE);
            DrawText(fps.ToString(), 10, 10, 12, Color.RED);
            DrawText(capacity.ToString(), 10, GetScreenHeight()-20, 12, Color.RED);

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

                bulletZone[i].Draw();

                bullets[i].transform.m3 += bullets[i].xSpeed * deltaTime;
                bullets[i].transform.m6 += bullets[i].ySpeed * deltaTime;

                bulletZone[i].SetX(bullets[i].transform.m3 + bullet.width / 2f);
                bulletZone[i].SetY(bullets[i].transform.m6 + bullet.height / 2f);

                if (bullets[i].transform.m3 > GetScreenWidth() + bullet.width)//640, 480
                {
                    for (int x = i; x < 19; x++)
                    {
                        bullets[x] = bullets[x + 1];
                        bulletZone[x] = bulletZone[x + 1];
                    }
                    totalBullets--;
                }
                else if (bullets[i].transform.m3 < -bullet.width)
                {
                    for (int x = i; x < 19; x++)
                    {
                        bullets[x] = bullets[x + 1];
                        bulletZone[x] = bulletZone[x + 1];
                    }
                    totalBullets--;
                }
                else if (bullets[i].transform.m6 > GetScreenHeight() + bullet.height)
                {
                    for (int x = i; x < 19; x++)
                    {
                        bullets[x] = bullets[x + 1];
                        bulletZone[x] = bulletZone[x + 1];
                    }
                    totalBullets--;
                }
                else if (bullets[i].transform.m6 < -bullet.height)
                {
                    for (int x = i; x < 19; x++)
                    {
                        bullets[x] = bullets[x + 1];
                        bulletZone[x] = bulletZone[x + 1];
                    }
                    totalBullets--;
                }
            }
            DrawRectangleLines((int)tank.min.x, (int)tank.min.y, (int)(tank.max.x - tank.min.x), (int)(tank.max.y-tank.min.y),Color.BLACK);

            DrawRectanglePro(new Rectangle(tankObject.GlobalTransform.m3, tankObject.GlobalTransform.m6, tankSprite.Width, tankSprite.Height),new Vector2(tankSprite.Width/2f, tankSprite.Height/2f), (float)Math.Atan2(tankSprite.GlobalTransform.m4, tankSprite.GlobalTransform.m1)* (float)(180.0f / Math.PI)+90,Color.RED);

            //bulletExplosion.isFiring();
            c1.Draw();
            tankObject.Draw();
            EndDrawing();
        }
    }
}
