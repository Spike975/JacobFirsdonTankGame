using System;
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
    class Game
    {
        public static SceneObject tankObject = new SceneObject();
        public static SceneObject turretObject = new SceneObject();
        public static SpriteObject tankSprite = new SpriteObject();
        public static SpriteObject turretSprite = new SpriteObject();
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
            tankSprite.Load("resources/tankBlue_outline.png");
            // sprite is facing the wrong way... fix that here
            tankSprite.SetRotate( (float)(-90 *Math.PI / 180.0f));
            tankSprite.name = "Tank Sprite";
            // sets an offset for the base, so it rotates around the centre
            tankSprite.SetPosition(-(tankSprite.Width / 2.0f), tankSprite.Height / 2.0f);
            turretSprite.Load("resources/barrelBlue.png");
            turretSprite.SetRotate((float)(-90 * Math.PI / 180.0f));
            turretSprite.name = "Turret Sprite";
            // set the turret offset from the tank base
            turretSprite.SetPosition(0, turretSprite.Width / 2.0f);
            // set up the scene object hierarchy - parent the turret to the base,
            // then the base to the tank sceneObject
            turretObject.name = "Turret Object";
            tankObject.name = "Tank Object";
            turretObject.AddChild(turretSprite);
            tankObject.AddChild(tankSprite);
            tankObject.AddChild(turretObject);
            // having an empty object for the tank parent means we can set the
            // position/rotation of the tank without
            // affecting the offset of the base sprite
            tankObject.SetPosition((GetScreenWidth() / 2f), (GetScreenHeight() / 2f));
        }
        public void Shutdown()
        { }
        public void Update()
        {
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
               tankObject.LocalTransform.m2, 1) * deltaTime * 100;
                tankObject.Translate(facing.x, facing.y);
            }
            if (IsKeyDown(KeyboardKey.KEY_S))
            {
                Vector3 facing = new Vector3(
               tankObject.LocalTransform.m1,
               tankObject.LocalTransform.m2, 1) * deltaTime * -100;
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
            tankObject.Update(deltaTime);

            lastTime = currentTime;
        }
        public void Draw()
        {
            BeginDrawing();
            ClearBackground(Color.WHITE);
            DrawText(fps.ToString(), 10, 10, 12, Color.RED);

            tankObject.Draw();

            EndDrawing();
        }
    }
}
