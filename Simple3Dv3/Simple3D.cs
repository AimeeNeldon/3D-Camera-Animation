/*
 * Basic steps to add animated objects:
 * 1- Create a new project in (or add your existing project to) SkinningSampleWindows solution
 * 2- Add SkinnedModelWindows to project references
 * 3- Add SkinnedModelPipeline to content references
 * 4- Add your FBX file to the contents
 * 5- In Properties for the FBX, select SkinnedModelProcessor as content processor
 * 6- Uncomment that part of the code with //ANIMATION
 * */
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
//ANIMATION
using SkinnedModel;

namespace Simple3D
{
    //basic class for all objects
    public class GameObject
    {

        public Model model = null;
        public Vector3 position = new Vector3(0, 0, 0);
        public Vector3 rotation = new Vector3(0, 0, 0);
        public float scale = 1;
        public bool visible = true;
        public Vector3 speed = new Vector3(0, 0, 0);
        public bool animated = false;
        //ANIMATION
        public AnimationPlayer animationPlayer;
        public AnimationClip clip;

        public GameObject()
        {
        }

        //ANIMATION
        //initialize animation. call this if animated object
        public void InitAnim()
        {
            animated = true;
            // Look up our custom skinning information.
            SkinningData skinningData = model.Tag as SkinningData;
            if (skinningData == null)
                throw new InvalidOperationException
                    ("This model does not contain a SkinningData tag.");
            // Create an animation player, and start decoding an animation clip.
            animationPlayer = new AnimationPlayer(skinningData);
            clip = skinningData.AnimationClips["Take 001"];
            animationPlayer.StartClip(clip);
        }

        //update object 
        public void Update(GameTime gameTime)
        {
            if (visible)
                position += speed;

            //ANIMATION
            if(animated)
                animationPlayer.Update(gameTime.ElapsedGameTime, true, Matrix.Identity);
        }

        //Draw object
        public void Draw(Matrix projection, Matrix view)
        {
            if (visible)
            {
                //World transformation is composed of
                //three matrices multiplied in the correct order:
                //Rotation * Scale * Translation
                Matrix world = Matrix.CreateFromYawPitchRoll(rotation.Y, rotation.X, rotation.Z) *
                    Matrix.CreateScale(scale) *
                    Matrix.CreateTranslation(position);

                if (animated)
                {
                    //ANIMATION
                    //get skeleton
                    Matrix[] bones = animationPlayer.GetSkinTransforms();
                    // Render the skinned mesh.
                    foreach (ModelMesh mesh in model.Meshes)
                    {
                        foreach (SkinnedEffect effect in mesh.Effects)
                        {
                            effect.EnableDefaultLighting();
                            effect.View = view;
                            effect.Projection = projection;
                            effect.World = world;
                            effect.SetBoneTransforms(bones);
                            effect.SpecularColor = new Vector3(0.25f);
                            effect.SpecularPower = 16;
                        }

                        mesh.Draw();
                    }
                }
                else
                {
                    foreach (ModelMesh mesh in model.Meshes)
                    {
                        foreach (BasicEffect effect in mesh.Effects)
                        {
                            effect.EnableDefaultLighting();
                            effect.PreferPerPixelLighting = true;
                            effect.World = world;
                            effect.Projection = projection;
                            effect.View = view;
                        }
                        mesh.Draw();
                    }
                }
            }
        }
    }

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Simple3D : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;


        //*********************************************//
        //********************SCORE *******************//
        //score objects
        GameObject score = new GameObject();
        GameObject score2 = new GameObject();
        GameObject score3 = new GameObject();
        GameObject score4 = new GameObject();
        GameObject score5 = new GameObject();

        public int scoretotal = 0;
        public bool turnaround = false;
        public bool turnaround2 = false;
        public bool turnaround3 = false;

        GameObject bullet = new GameObject();
        //ANIMATION

        //********************* ENEMY ********************//

        // GameObject[] enemy = new GameObject[5];
        GameObject enemy = new GameObject();
        GameObject enemy2 = new GameObject();
        GameObject enemy3 = new GameObject();
        GameObject enemy4 = new GameObject();
        GameObject enemy5 = new GameObject();
        
        GameObject wall = new GameObject();
        GameObject wall2 = new GameObject();
        GameObject wall3 = new GameObject();
        GameObject wall4 = new GameObject();
        GameObject wall5 = new GameObject();
        GameObject wall6 = new GameObject();
        GameObject wall7 = new GameObject();

        //********************* CAMERA ********************//
        Vector3 cameraPosition = new Vector3(50.0f, 5.0f, 0.0f);
        Vector3 cameraLookAtRelative = new Vector3(0.0f, 5.0f, -50.0f);
        float cameraRotation = 0;
        Matrix cameraProjectionMatrix;
        Matrix cameraViewMatrix;
        Vector3 cspeed = new Vector3(0, 0, -1);
        SpriteFont Font1;
        Vector2 FontPos;
        // bool movefoward = true;


        public Simple3D()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>

        protected override void LoadContent()
        {

            //********************************************//
            //********************SCORE*******************//
            score.model = Content.Load<Model>("Gun");
            score.position = new Vector3(10, 0, -80);
            score.visible = true;

            score2.model = Content.Load<Model>("Gun");
            score2.position = new Vector3(20, 0, -60);
            score2.visible = true;

            score3.model = Content.Load<Model>("Gun");
            score3.position = new Vector3(0, 0, -100);
            score3.visible = true;

            score4.model = Content.Load<Model>("Gun");
            score4.position = new Vector3(40, 0, -100);
            score4.visible = true;

            score5.model = Content.Load<Model>("Gun");
            score5.position = new Vector3(20, 0, -10);
            score5.visible = true;

            //load bullet
            bullet.model = Content.Load<Model>("x2");
            bullet.rotation = new Vector3(-90, 0, 0);
            bullet.scale = 0.05f;
            bullet.visible = false;

            //ANIMATION

            //********************************************//
            //********************ENEMY*******************//

            //public enemy[] enemiesArray = new enemy[5];
            enemy.model = Content.Load<Model>("Dude");
            enemy.position = new Vector3(100, 0, -100);
            enemy.scale = 0.1f;
            enemy.InitAnim();

            enemy2.model = Content.Load<Model>("Dude");
            enemy2.position = new Vector3(50, 0, -150);
            enemy2.scale = 0.1f;
            enemy2.InitAnim();

            enemy3.model = Content.Load<Model>("Dude");
            enemy3.position = new Vector3(80, 0, -120);
            enemy3.scale = 0.1f;
            enemy3.InitAnim();

            enemy4.model = Content.Load<Model>("Dude");
            enemy4.position = new Vector3(70, 0, -180);
            enemy4.scale = 0.1f;
            enemy4.InitAnim();

            enemy5.model = Content.Load<Model>("Dude");
            enemy5.position = new Vector3(90, 0, -160);
            enemy5.scale = 0.1f;
            enemy5.InitAnim();

            //ANIMATION
            //********************************************//
            //********************WALL*******************//
            wall.model = Content.Load<Model>("Line");
            wall.position = new Vector3(0, 0, -100);
            wall.visible = true;
            wall2.model = Content.Load<Model>("Line");
            wall2.position = new Vector3(100, 0, -100);
            wall2.visible = true;
            wall3.model = Content.Load<Model>("Line");
            wall3.position = new Vector3(0, 0, -150);
            wall3.visible = true;
            wall4.model = Content.Load<Model>("Line");
            wall4.position = new Vector3(100, 0, -110);
            wall4.visible = true;
            wall5.model = Content.Load<Model>("Line2");
            wall5.position = new Vector3(55, -15, -150);
            wall5.visible = true;
            wall6.model = Content.Load<Model>("Line2");
            wall6.position = new Vector3(140, -15, -150);
            wall6.visible = true;
            wall7.model = Content.Load<Model>("Line2");
            wall7.position = new Vector3(160, -15, -110);
            wall7.visible = true;

            //set up camera
            UpdateCamera();

            //*********************************************//
            //******************** TEXT *******************//
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Font1 = Content.Load<SpriteFont>("SpriteFont1");

            // TODO: Load your game content here            
            //FontPos = new Vector2(60, 10);
        }

        /// <summary>
        /// update view and projection matices. call any time camera moves
        /// </summary>
        protected void UpdateCamera()
        {
            Matrix rotationMatrix = Matrix.CreateRotationY(cameraRotation);
            Vector3 cameraLookAtRelativeXformed = Vector3.Transform(cameraLookAtRelative, rotationMatrix);
            Vector3 cameraLookAt = cameraPosition + cameraLookAtRelativeXformed;
            cameraViewMatrix = Matrix.CreateLookAt(
                cameraPosition,
                cameraLookAt,
                Vector3.Up);

            cameraProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45.0f),
                (float)graphics.GraphicsDevice.Viewport.Width /
                (float)graphics.GraphicsDevice.Viewport.Height,
                1.0f,
                10000.0f);
        }


        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

#if !XBOX
            KeyboardState keyboardState = Keyboard.GetState();

            //end program
            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            //camera control
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                cameraRotation += 0.01f;
                UpdateCamera();
            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                cameraRotation -= 0.01f;
                UpdateCamera();
            }
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                Matrix forwardMovement = Matrix.CreateRotationY(cameraRotation);
                Vector3 v = Vector3.Transform(cspeed, forwardMovement);
                cameraPosition.Z += v.Z;
                cameraPosition.X += v.X;
                UpdateCamera();
            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                Matrix forwardMovement = Matrix.CreateRotationY(cameraRotation);
                Vector3 v = Vector3.Transform(-1*cspeed, forwardMovement);
                cameraPosition.Z += v.Z;
                cameraPosition.X += v.X;
                UpdateCamera();
            }

            //***************************************************//
            //********************FIRE BULLETS*******************//
            if (!bullet.visible && keyboardState.IsKeyDown(Keys.Space))
            {
                bullet.position = cameraPosition;
                bullet.position -= new Vector3(0, 1, 0);
                bullet.speed = Vector3.Transform(new Vector3(0, 0, -1), Matrix.CreateRotationY(cameraRotation));
                bullet.visible = true;
            }
            //move bullet if fired
            bullet.Update(gameTime);
            if (Vector3.Distance(bullet.position, cameraPosition) > 100)
            {
                bullet.visible = false;
            }

            
            //********************************************//
            //********************SCORE*******************//

            if (Vector3.Distance(score.position, cameraPosition) < 10)
            {
                scoretotal = scoretotal + 1;
                score.visible = false;
            }
            if (Vector3.Distance(score2.position, cameraPosition) < 10)
            {
                scoretotal = scoretotal + 1;
                score2.visible = false;
            }
            if (Vector3.Distance(score3.position, cameraPosition) < 10)
            {
                scoretotal = scoretotal + 1;
                score3.visible = false;
            }
            if (Vector3.Distance(score4.position, cameraPosition) < 10)
            {
                scoretotal = scoretotal + 1;
                score4.visible = false;
            }
            if (Vector3.Distance(score5.position, cameraPosition) < 10)
            {
                scoretotal = scoretotal + 1;
                score5.visible = false;
            }

            //*************************************************//
            //********************ENEMY WALL*******************//

            
            if (enemy3.position.X == wall.position.X)
            {
                turnaround3 = true;
            }
            if (enemy2.position.X == wall.position.X)
            {
                turnaround2 = true;
            }
            if (enemy.position.X == wall.position.X)
            {
                turnaround = true;
            }

            //ELSE
            if (enemy3.position.X == wall2.position.X)
            {
                turnaround3 = false;
            }
            if (enemy2.position.X == wall2.position.X)
            {
                turnaround2 = false;
            }
            if (enemy.position.X == wall2.position.X)
            {
                turnaround = false;
            }



            //******** CAMERA ***************//
            //if (Vector3.Distance(enemy.position, cameraPosition) < 30)
            //{
            //  cameraPosition.X = cameraPosition.X=-10;

            //}
            //for (int i = 0; i < 400;i++){
            //   if (cameraPosition.X == wall.position.X + i)
            // {
            //   movefoward = false;
            //}
            //else
            //  movefoward = true;

            //}


            //********************************************//
            //********************ENEMY*******************//

            if (turnaround == true)
            {
                enemy.position.X = enemy.position.X + 1;
                enemy4.position.X = enemy.position.X - 1;
                enemy5.position.X = enemy.position.X - 1;
            }
            if (turnaround2 == true)
            {
                enemy2.position.X = enemy2.position.X + 1;
            }
            if (turnaround3 == true)
            {
                enemy3.position.X = enemy3.position.X + 1;
            }

            //ELSE
            if (turnaround == false)
            {
                enemy.position.X = enemy.position.X - 1;
                enemy4.position.X = enemy.position.X - 1;
                enemy5.position.X = enemy.position.X - 1;
            }
            if (turnaround2 == false)
            {
                enemy2.position.X = enemy2.position.X - 1;
            }
            if (turnaround3 == false)
            {
                enemy3.position.X = enemy3.position.X - 1;
            }

            //UPDATE
            enemy.Update(gameTime);
            enemy2.Update(gameTime);
            enemy3.Update(gameTime);
            enemy4.Update(gameTime);
            enemy5.Update(gameTime);
#endif

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            graphics.GraphicsDevice.Clear(Color.Navy);

            // TEXT reset
            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            GraphicsDevice.DepthStencilState = DepthStencilState.None;
            GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;

            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

           // spriteBatch.Begin();

           // spriteBatch.DrawString(Font1, "Hello World", FontPos, Color.LightGreen);

            //spriteBatch.End();
 

            //*************************************************//
            //********************DRAW SCORE*******************//
            score.Draw(cameraProjectionMatrix, cameraViewMatrix);
            score2.Draw(cameraProjectionMatrix, cameraViewMatrix);
            score3.Draw(cameraProjectionMatrix, cameraViewMatrix);
            score4.Draw(cameraProjectionMatrix, cameraViewMatrix);
            score5.Draw(cameraProjectionMatrix, cameraViewMatrix);

            //draw bullet
            bullet.Draw(cameraProjectionMatrix, cameraViewMatrix);

            //ANIMATION
            //draw enemy (rotation * scale * translation )
       
            //for(int j=0; j < 5; j++)
           // {
               // int i=0;
            enemy.Draw(cameraProjectionMatrix, cameraViewMatrix);
            enemy2.Draw(cameraProjectionMatrix, cameraViewMatrix);
            enemy3.Draw(cameraProjectionMatrix, cameraViewMatrix);
            enemy4.Draw(cameraProjectionMatrix, cameraViewMatrix);
            enemy5.Draw(cameraProjectionMatrix, cameraViewMatrix);
           // }

            wall.Draw(cameraProjectionMatrix, cameraViewMatrix);
            wall2.Draw(cameraProjectionMatrix, cameraViewMatrix);
            wall3.Draw(cameraProjectionMatrix, cameraViewMatrix);
            wall4.Draw(cameraProjectionMatrix, cameraViewMatrix);
            wall5.Draw(cameraProjectionMatrix, cameraViewMatrix);
            wall6.Draw(cameraProjectionMatrix, cameraViewMatrix);
            wall7.Draw(cameraProjectionMatrix, cameraViewMatrix);

            //*******************************************//
            //********************TEXT*******************//
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.Additive);
            string str = "Score: ";
            str += scoretotal.ToString();
            spriteBatch.DrawString(Font1, str, Vector2.Zero, Color.White);
            spriteBatch.End(); 
    
            base.Draw(gameTime);
        }

    }
}
