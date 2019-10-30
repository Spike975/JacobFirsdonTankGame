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
            InitWindow(1000, 1000, "(bad tank pun)");
            SetTargetFPS(60);
            game.Init();

            // Main game loop
            while (!WindowShouldClose())
            {
                game.Update();
                game.Draw();
            }
            game.Shutdown();
            CloseWindow();
            return 0;
        }
    }
    struct Bullet
    {
        public Matrix3 transform;
        public double deathTime;
    }
    class Game
    {
        Random rand = new Random();
        SceneObject bulletObject = new SceneObject();
        SceneObject turretObject = new SceneObject();
        SceneObject tankObject = new SceneObject();
        SceneObject bulletSpawner = new SceneObject();
        SceneObject ammoObject = new SceneObject();
        SpriteObject tankSprite = new SpriteObject();
        SpriteObject buletSprite = new SpriteObject();
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
        Texture2D targetSprite = new Texture2D();
        Time time = new Time();
        Stopwatch stopwatch = new Stopwatch();
        long currentTime = 0;
        long lastTime = 0;
        bool fire = false;
        float sprintTime = 0;
        float startSprint = 0f;
        float penaltySprint = 5f;
        float timer = 0;
        float tankSpace = 18;//17.396f;
        float deltaTime = 0.005f;
        float bulletSpeed = 400;
        double Score = 0;
        double shotTime = 0f;
        double targetTime = 0;
        double crateTime = 0;
        double phase = 0;
        int currentTargets = 0;
        int totalBullets = 0;
        int flash = 0;
        int speed = 0;
        int barrel = 0;
        int totalCrates = 0;
        int fps = 1;
        int frames;
        static int capacity = 10;
        int bull = 10;
        Circle[] bulletTest = new Circle[10000];
        Circle[] targets = new Circle[1500];
        Box outerTank = new Box();
        Box[] crates = new Box[10];
        Bullet[] bullets = new Bullet[10000];
        public void Init()
        {
            stopwatch.Start();
            lastTime = stopwatch.ElapsedMilliseconds;
            bulletSprite = LoadTextureFromImage(LoadImage("resources/bulletBlue1_outline.png"));
            targetSprite = LoadTextureFromImage(LoadImage("resources/target.png"));
            tankSprite.Load("resources/tankBody_blue_outline.png");
            // sprite is facing the wrong way... fix that here
            tankSprite.SetRotate(-90 * (float)(Math.PI / 180.0f));
            // sets an offset for the base, so it rotates around the centre
            tankSprite.SetPosition(-tankSprite.Width / 2.0f, tankSprite.Height / 2.0f);
            crate = LoadTextureFromImage(LoadImage($"resources/crate.png"));
            Console.WriteLine(crate.width + ", " + crate.height);
            turretSprite.Load("resources/barrel1.png");
            bulletObject.SetPosition(20,20);
            buletSprite.SetPosition(-buletSprite.Width/2f,0);
            one = LoadTextureFromImage(LoadImage("resources/shotThin.png"));
            two = LoadTextureFromImage(LoadImage("resources/shotRed.png"));
            three = LoadTextureFromImage(LoadImage("resources/shotOrange.png"));
            four = LoadTextureFromImage(LoadImage("resources/shotLarge.png"));
            for (int i = 0; i < barrels.Length; i++)
                barrels[i] = LoadTextureFromImage(LoadImage($"resources/barrel{i + 1}.png"));

            for (int i = 0; i < crates.Length; i++)
                crates[i] = new Box();

            for (int i = 0; i < bulletTest.Length; i++)
                bulletTest[i] = new Circle();

            for (int i = 0; i < targets.Length; i++)
                targets[i] = new Circle();
            

            buletSprite.Load("resources/bulletBlue1_outline.png");
            turretSprite.SetRotate(-90 * (float)(Math.PI / 180.0f));
            buletSprite.Rotate(90 * (float)(Math.PI / 180.0f));
            bulletExplosion.SetRotate(-90 * (float)(Math.PI / 180.0f));

            // set the turret offset from the tank base
            turretSprite.SetPosition(0, turretSprite.Width / 2.0f);

            bulletObject.AddChild(buletSprite);
            bulletSpawner.AddChild(bulletExplosion);
            turretObject.AddChild(turretSprite);
            turretObject.AddChild(bulletSpawner);
            tankObject.AddChild(tankSprite);
            tankObject.AddChild(turretObject);
            //tankObject.AddChild(tankCorner1);
            //tankObject.AddChild(tankCorner2);
            //tankObject.AddChild(tankCorner3);
            //tankObject.AddChild(tankCorner4);

            tankObject.SetPosition(GetScreenWidth() / 2.0f, GetScreenHeight() / 2.0f);
        }
        public void Shutdown()
        { }
        //Updates every object in game
        public void Update()
        {
            outerTank.x = tankObject.GlobalTransform.m3 - tankSprite.Width / 2f - (tankSpace * Math.Abs(tankObject.GlobalTransform.m1 * tankObject.GlobalTransform.m2));
            outerTank.y = tankObject.GlobalTransform.m6 - tankSprite.Height / 2f - (tankSpace * Math.Abs(tankObject.GlobalTransform.m4 * tankObject.GlobalTransform.m5));
            outerTank.w = tankSprite.Width + 1 + (2 * tankSpace * Math.Abs(tankObject.GlobalTransform.m1 * tankObject.GlobalTransform.m2));
            outerTank.l = tankSprite.Height + 1 + (2 * tankSpace * Math.Abs(tankObject.GlobalTransform.m4 * tankObject.GlobalTransform.m5));

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
            currentTargets = 0;

            for (int i = 0; i < totalBullets; i++)
            {
                bullets[i].deathTime += deltaTime;
            }
            //gets current amount of crates spawned
            foreach (Box c in crates)
            {
                if (c.x != 0 && c.y != 0)
                {
                    totalCrates++;
                }
            }
            //gets current amount of targets spawned
            foreach(Circle c in targets){
                if (c.x != 0 && c.y != 0)
                {
                    currentTargets++;
                }
            }
            //Spawns Crates
            if ((GetTime() - crateTime > 15f || crateTime == 0) && totalCrates < 10)
            {
                crates[totalCrates].x = rand.Next(10, GetScreenWidth() - crate.width - 10);
                crates[totalCrates].y = rand.Next(10, GetScreenHeight() - crate.height - 10);
                crates[totalCrates].w = crate.width;
                crates[totalCrates].l = crate.height;
                crateTime = GetTime();
                totalCrates++;
            }
            //Spawns targets
            if ((GetTime() - targetTime > 3f || targetTime == 0) && currentTargets < 1500)
            {
                targets[currentTargets].x = rand.Next(targetSprite.width+10, GetScreenWidth() - targetSprite.width-10);
                targets[currentTargets].y = rand.Next(targetSprite.height+10, GetScreenHeight() - targetSprite.height-10);
                targets[currentTargets].radius = targetSprite.width / 2f;
                targetTime = GetTime();
                Console.WriteLine(currentTargets);
            }

            //Sets speed
            if (IsKeyDown(KeyboardKey.KEY_LEFT_SHIFT) && sprintTime >= startSprint)
            {
                if (sprintTime == startSprint)
                {
                    startSprint = (float)GetTime();
                    sprintTime = startSprint;
                }
                sprintTime += deltaTime;
                speed = 2;
                if (sprintTime - startSprint >= penaltySprint)
                {
                    startSprint = sprintTime + penaltySprint;
                }
            }
            else
            {
                if ()
                {

                }
                speed = 1;
            }

            //MOVEMENT                   ////
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
            //END MOVEMENT               ////

            //Shooting
            if (IsKeyDown(KeyboardKey.KEY_SPACE) && GetTime() - shotTime > .75f && capacity > 0)
            {
                if (totalBullets < 0)
                {
                    totalBullets = 0;
                }
                bullets[totalBullets].transform = bulletSpawner.GlobalTransform;
                totalBullets++;
                capacity--;
                shotTime = GetTime();
                fire = true;
                phase = GetTime();
            }
            //Reload
            if (IsKeyDown(KeyboardKey.KEY_R))
            {
                capacity += bull;
            }

            //Speeding up the bullets
            if (IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON))
            {
                bulletSpeed += 200;
            }
            //Slowing down the bullets
            if (IsMouseButtonPressed(MouseButton.MOUSE_RIGHT_BUTTON))
            {
                bulletSpeed -= 200;
            }
            //Random 
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
            if (IsKeyPressed(KeyboardKey.KEY_I))
            {
                turretObject.SetRotate(0);
            }

            //Bulet Target Overlap Check
            for (int i = 0; i < totalBullets; i++)
            {
                for (int x = 0; x < currentTargets; x++)
                {
                    if (bulletTest[i].Overlaps(targets[x]))
                    {
                        if (bullets[i].deathTime > 1f)
                        {
                            Score += bullets[i].deathTime;
                        }
                        else {
                            Score++;
                        }
                        for (int t = i; t < bull - 1; t++)
                        {
                            bullets[t] = bullets[t + 1];
                        }
                        targets[x] = new Circle();
                        currentTargets--;
                        totalBullets--;
                    }
                }
            }
            //Bullet Crate Collision
            for (int i = 0; i < totalCrates; i++)
            {
                if (crates[i].Overlap(outerTank))//CheckCollisionRecs(new Rectangle(outerTank.x, outerTank.y, outerTank.x, outerTank.y), new Rectangle(crates[i].x, crates[i].y, crates[i].w, crates[i].l)))
                {
                    capacity += bull;
                    crates[i] = new Box();
                    totalCrates--;
                }
                for (int x = 0; x < bulletTest.Length; x++)
                {
                    if (bulletTest[x].Overlaps(crates[i])&&(bulletTest[x].x != 0 && crates[i].x != 0))
                    {
                        if (bullets[i].deathTime > 1f)
                        {
                            Score -= bullets[i].deathTime;
                        }
                        else
                        {
                            Score--;
                        }
                        for (int t = x; t < bull - 1; t++)
                        {
                            bullets[t] = bullets[t + 1];
                        }
                        crates[i] = new Box();
                        totalCrates--;
                        totalBullets--;
                    }
                }
            }
            //Crates Crates overlap..Probably Don't need right now
            for (int i = 0; i < totalCrates; i++)
            {
                for (int x = i + 1; x < totalCrates; x++)
                {
                    if (crates[i].Overlap(crates[x]))
                    {
                        crates[x].x = rand.Next(10, GetScreenWidth() - crate.width - 10);
                        crates[x].y = rand.Next(10, GetScreenHeight() - crate.height - 10);
                    }
                }
            }
            //Death Time Bullets
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
            //Moving of targets if needed
            for (int i = 0; i < currentTargets; i++)
            {
                bool c = false;
                if (targets[i].x == 0 && targets[i].y == 0)
                {
                    for (int x = i; x < targets.Length; x++)
                    {
                        if (targets[x].radius != 0)
                        {
                            c = true;
                            break;
                        }
                    }
                    if (c)
                    {
                        for (int x = i; x < targets.Length; x++)
                        {
                            if (x < targets.Length - 1)
                            {
                                targets[x] = targets[x + 1]; targets[x + 1] = new Circle();
                            }
                        }
                    }
                }
            }
            //Moving of crates if needed
            for (int i = 0; i < crates.Length; i++)
            {
                bool c = false;
                if (crates[i].x == 0 && crates[i].y == 0)
                {
                    for (int x = i; x < crates.Length; x++)
                    {
                        if (crates[x].l != 0 && crates[x].w != 0)
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
                                crates[x] = crates[x + 1]; crates[x + 1] = new Box();
                            }
                        }
                    }
                }
            }

            tankObject.Update(deltaTime);
            lastTime = currentTime;
        }
        //Draws every object in game
        public void Draw()
        {
            BeginDrawing();
            ClearBackground(Color.WHITE);

            if (fire && GetTime() - phase > .015f)
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

            for (int i = 0; i < totalCrates; i++)
            {
                DrawTexture(crate, (int)crates[i].x, (int)crates[i].y, Color.WHITE);
                //DrawRectangleLines((int)crates[i].x, (int)crates[i].y, (int)crates[i].w, (int)crates[i].l, Color.BLACK);
            }

            for (int i = 0; i < totalBullets; i++)
            {
                float rotation = (float)Math.Atan2(bullets[i].transform.m4, bullets[i].transform.m1);
                DrawTextureEx(bulletSprite, new Vector2(bullets[i].transform.m3, bullets[i].transform.m6), rotation * (float)(180.0f / Math.PI) + 90, 1, Color.WHITE);

                bulletTest[i].x = bullets[i].transform.m3 - (((float)(Math.Sqrt(bulletSprite.height*bulletSprite.height+bulletSprite.width*bulletSprite.width)-Math.Atan2(bulletSprite.width,bulletSprite.height)*5)*bullets[i].transform.m1)/2);
                bulletTest[i].y = bullets[i].transform.m6 - (((float)(Math.Sqrt(bulletSprite.height*bulletSprite.height+bulletSprite.width*bulletSprite.width)-Math.Atan2(bulletSprite.width,bulletSprite.height)*5)*bullets[i].transform.m4)/2);
                
                bulletTest[i].radius = bulletSprite.height / 2f;

                //DrawCircleLines((int)bulletTest[i].x, (int)bulletTest[i].y, bulletTest[i].radius, Color.BLACK);

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

            for(int i = 0; i < currentTargets; i++)
            {
                DrawTexture(targetSprite,(int)targets[i].x-targetSprite.width/2,(int)targets[i].y- targetSprite.height/2,Color.WHITE);
                //DrawCircleLines((int)targets[i].x,(int)targets[i].y,(int)targets[i].radius, Color.BLUE);
            }

            //DrawRectangleLines((int)tank.min.x, (int)tank.min.y, (int)(tank.max.x - tank.min.x), (int)(tank.max.y - tank.min.y), Color.BLACK);
            //DrawRectangleLines((int)tankOuter.min.x, (int)tankOuter.min.y, (int)tankOuter.max.x, (int)tankOuter.max.y, Color.BLACK);
            //DrawRectangleLines((int)outerTank.x, (int)outerTank.y, (int)outerTank.w, (int)outerTank.l, Color.BLACK);
            //DrawRectanglePro(new Rectangle(tankObject.GlobalTransform.m3, tankObject.GlobalTransform.m6, tankSprite.Width, tankSprite.Height), new Vector2(tankSprite.Width / 2f, tankSprite.Height / 2f), (float)Math.Atan2(tankSprite.GlobalTransform.m4, tankSprite.GlobalTransform.m1) * (float)(180.0f / Math.PI) + 90, Color.RED);
            
            tankObject.Draw();
            DrawText(fps.ToString(), 10, 10, 12, Color.RED);
            DrawText(capacity.ToString(), 10, GetScreenHeight() - 20, 20, Color.RED);
            DrawText("Score: "+Score.ToString(),GetScreenWidth()-150,10, 20, Color.RED);
            EndDrawing();
        }
    }
}
