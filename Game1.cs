using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio; 
using Microsoft.Xna.Framework.Input;
using RC_Framework;

namespace GPT_Assignment.MacOS
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont llFont, mmFont, ssFont;
        KeyboardState ks, pks;

        ScrollBackGround background;
        Rectangle backgroundDest;
        Rectangle backgroundSource;

        Sprite3 playerSprite;
        Sprite3 kittyCat;
        Sprite3 crazyDog;           // the crazy dog shows up in the second level 

        Texture2D kittyCatTex;
        Texture2D backgroundTex;
        Texture2D yarnTex; 
        Texture2D playerTex;
        Texture2D doggyTex;

        Random rnd = new Random(); 

        string kcTexPath;
        string bbTexPath;
        string playerTexPath;
        string doggyTexPath;



        int state;                  // state changes depending on transition/level

        bool showHH = false;        // toggle bool for help
        bool showBB = false;        // show bounding boxes
        bool showII = false;        // level info and health metrics

        Vector2 kittyCatVec;        // position vector for cat sprite on splash screen
        Vector2 titleVec;           // position vector for title 
        Vector2 sloganVec;          // not important but nice to have
        Vector2 instructVec;        // tell the user what to do 
        Vector2 levelIntroVec;      // position for the level intro text
        Vector2 playerVec;          // the position vector for the small kitty cat
        Vector2[] playerFrames;     // array containing all the frames positions for cat
        Vector2 doggyVec;           // the position vector for the crazy dog
        Vector2[] doggyFrames;      // array containing all the frames positions for crazy dog

        TextRenderable levelText;    // renderable text
        Color ltCol;                 // color for the renderable text


        bool playerJump;            // whether the small kitty cat is jumping or not
        float playerJumpSpeed;      // the speed at which the kitty cat can jump
        Vector2 prevPos;            // store the initial position of the small kitty cat


        SpriteList foodBowls;       // the small kitty cat needs to get bowls of food
        SpriteList yarnBalls;       // the small kitty cat has to capture balls of yarn 
        SpriteList ciggieList;      // have a ciggie? 
        SpriteList sprinklers;      // frame based water splashes - the small kitty cat
                                    // needs to avoid these

        int treasureScore = 0;      // this will be incremented when the cat does

        SoundEffect success;        // moving sound
        SoundEffect kaching;        // when the cat collides with food bowls, yarn balls
        SoundEffect yowlin;         // if the cat hits the dog or a ciggy
        SoundEffect splash;         // splash sound if the sprinkler hits the ground
        SoundEffect jumpo;          // jumping sound when the cat jumps
        SoundEffect barko;          // the dog's bark
        SoundEffect easterEgg;      // just having some fun here - don't be offended

        LimitSound jumpoSound;
        LimitSound barkoSound;
        LimitSound successSound;
        LimitSound kachingSound;
        LimitSound yowlinSound;
        LimitSound eeSound;

        bool colWBB;                 // status variable as to whether collision occurred

        int maxYarnBalls = 25;      


        int points = 0;
        int lives = 2;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
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
            spriteBatch = new SpriteBatch(GraphicsDevice);
            LineBatch.init(GraphicsDevice);

            // load all fonts for the splash screen and levels 
            llFont = Content.Load<SpriteFont>("Large");
            mmFont = Content.Load<SpriteFont>("Medium");
            ssFont = Content.Load<SpriteFont>("Small");

            // load  textures from local content directory
            kcTexPath = @"/Users/jfc/Desktop/CrazyCat_Assignment_GPT/GPT_Assignment/GPT_Assignment/Content/kittyCat.png";
            kittyCatTex = Util.texFromFile(GraphicsDevice, kcTexPath);

            bbTexPath = @"/Users/jfc/Desktop/CrazyCat_Assignment_GPT/GPT_Assignment/GPT_Assignment/Content/Background.png";
            backgroundTex = Util.texFromFile(GraphicsDevice, bbTexPath);

            playerTexPath = @"/Users/jfc/Desktop/CrazyCat_Assignment_GPT/GPT_Assignment/GPT_Assignment/Content/KittyCatFrames.png";
            playerTex = Util.texFromFile(GraphicsDevice, playerTexPath);

            doggyTexPath = @"/Users/jfc/Desktop/CrazyCat_Assignment_GPT/GPT_Assignment/GPT_Assignment/CrazyDogFrames.png";
            doggyTex = Util.texFromFile(GraphicsDevice, doggyTexPath);

            yarnTex = Content.Load<Texture2D>("Yarn");

            // position vector for the cat sprite on main splash screen 
            kittyCatVec = new Vector2(275, 50);
            titleVec = new Vector2(100, 300);
            sloganVec = new Vector2(290, 400);
            instructVec = new Vector2(315, 425);
            levelIntroVec = new Vector2(250, 200);
            playerVec = new Vector2(600, 400);
            doggyVec = new Vector2(0, 400);


            // make the cat sprite for the main splash screen 
            kittyCat = new Sprite3(true, kittyCatTex, kittyCatVec.X, kittyCatVec.Y);

            // prepare the crazy dog for level 2
            crazyDog = new Sprite3(false, doggyTex, doggyVec.X, doggyVec.Y);




            // color for the level text and alpha value
            ltCol = new Color(0, 255, 0, 1);
            // level text initialization 
            levelText = new TextRenderable("Blabla", levelIntroVec, llFont, Color.Red);


            // source and destination rectangles for the scrolling background
            backgroundDest = new Rectangle(0, 0, 800, 600);
            backgroundSource = new Rectangle(0, 0, 800, 600);

            // draw background 
            background = new ScrollBackGround(backgroundTex, backgroundDest, backgroundSource, 5.0f, 2);

            // player vector and player texture  

            playerSprite = new Sprite3(true, playerTex, playerVec.X, playerVec.Y);

            // initialize frame vector
            playerFrames = new Vector2[10];

            // frame vector values 1280x128

            playerFrames[0].X = 0; playerFrames[0].Y = 0;
            playerFrames[1].X = 1; playerFrames[1].Y = 0;
            playerFrames[2].X = 2; playerFrames[2].Y = 0;
            playerFrames[3].X = 3; playerFrames[3].Y = 0;
            playerFrames[4].X = 4; playerFrames[4].Y = 0;
            playerFrames[5].X = 5; playerFrames[5].Y = 0;
            playerFrames[6].X = 6; playerFrames[6].Y = 0;
            playerFrames[7].X = 7; playerFrames[7].Y = 0;
            playerFrames[8].X = 8; playerFrames[8].Y = 0;
            playerFrames[9].X = 9; playerFrames[9].Y = 0;

            // set all the parameters for the cute little kitty cat
            playerSprite.setWidthHeight(64, 64);
            playerSprite.setWidthHeightOfTex(1280, 128);
            playerSprite.setXframes(10);
            playerSprite.setYframes(0);
            playerSprite.setBB(0, 0, 128, 128);
            playerSprite.setAnimationSequence(playerFrames, 0, 9, 15);
            playerSprite.animationStart();

            // the parameters for the player sprite jumping
            playerJump = false;
            playerJumpSpeed = 0;

            // store the initial positions of the player sprite
            prevPos.Y = playerVec.Y;
            prevPos.X = playerVec.X;

            // initialize sounds for the game
            jumpo = Content.Load<SoundEffect>("Sounds/Boing");
            jumpoSound = new LimitSound(jumpo, 1);

            success = Content.Load<SoundEffect>("Sounds/Success");
            successSound = new LimitSound(success, 1);

            barko = Content.Load<SoundEffect>("Sounds/Growl");
            barkoSound = new LimitSound(barko, 1);

            kaching = Content.Load<SoundEffect>("Sounds/Kaching");
            kachingSound = new LimitSound(kaching, 1);

            yowlin = Content.Load<SoundEffect>("Sounds/Yowl");
            yowlinSound = new LimitSound(yowlin, 1);

            easterEgg = Content.Load<SoundEffect>("Sounds/OhWhatever");
            eeSound = new LimitSound(easterEgg, 1);


            /// yarn ball sprite list
            /// 
            /// 
            /// 
            ///     
            /// 
            /// 

            yarnBalls = new SpriteList();

            Sprite3 yb; 

            for (int i=0; i < maxYarnBalls; i++)
            {
                // temporarily set to doggyVec for positioning - change later?
                yb = new Sprite3(true, yarnTex, doggyVec.X + i * 50, doggyVec.Y - i * 50);
                yb.setDeltaSpeed(new Vector2((float)(1 + rnd.NextDouble()), 0)); 
                yb.setWidthHeight(32, 32);
                yb.setWidthHeightOfTex(32, 32);
                yb.setHSoffset(new Vector2(32, 32)); 
                yb.setBBToTexture(); 
                yb.setMoveSpeed(2.1f);

                yarnBalls.addSpriteReuse(yb); 
            }
        



            // initialize frame vector for dog
            doggyFrames = new Vector2[8];


            doggyFrames[0].X = 0; doggyFrames[0].Y = 0;
            doggyFrames[1].X = 1; doggyFrames[1].Y = 0;
            doggyFrames[2].X = 2; doggyFrames[2].Y = 0;
            doggyFrames[3].X = 3; doggyFrames[3].Y = 0;
            doggyFrames[4].X = 4; doggyFrames[4].Y = 0;
            doggyFrames[5].X = 5; doggyFrames[5].Y = 0;
            doggyFrames[6].X = 6; doggyFrames[6].Y = 0;
            doggyFrames[7].X = 7; doggyFrames[7].Y = 0;

            crazyDog.setWidthHeight(64, 64);
            crazyDog.setWidthHeightOfTex(1024, 128);
            crazyDog.setXframes(8);
            crazyDog.setYframes(0);
            crazyDog.setBB(0, 0, 128, 128);
            crazyDog.setAnimationSequence(playerFrames, 0, 7, 15);
            crazyDog.animationStart();



     


        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // retrieve keyboard state 
            pks = ks;
            ks = Keyboard.GetState();


            // states and their affiliated keys are marked here

            if (ks.IsKeyDown(Keys.Q)) state = 0;  // default state
            if (ks.IsKeyDown(Keys.S)) state = 1;  // transition state between default and L1
            if (ks.IsKeyDown(Keys.D1)) state = 2;  // level 1
            if (ks.IsKeyDown(Keys.D2)) state = 3;  // level 2

            // for testing purposes - remove later - this changes to GAME OVER

            if (ks.IsKeyDown(Keys.T)) state = 6;

            // show information about the level and health metrics
            if (ks.IsKeyDown(Keys.I) && pks.IsKeyUp(Keys.I)) showII = !showII;
            // show the help bar so the user knows what keys to press
            if (ks.IsKeyDown(Keys.H) && pks.IsKeyUp(Keys.H)) showHH = !showHH;



            // affiliated update routines to each state

            if (state == 0) UpdateSS0(gameTime);   // reverts back to default state 
            if (state == 1) UpdateSS1(gameTime);   // moves the splash screen off screen 
            if (state == 2) UpdateSS2(gameTime);   // change to level 1 
            if (state == 3) UpdateSS3(gameTime);   // change to level 2


            // state 4 and 5 are player ready states

            if (state == 4) UpdateL1(gameTime);
            if (state == 5) UpdateL2(gameTime);

            // for testing purposes - remove later
            if (state == 6) UpdateEE(gameTime);
            if (state == 7) UpdateEE(gameTime);

         

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);


            if (state == 0) { DrawSS0(gameTime); } // draw default screen - splash screen
            if (state == 1) { DrawSS0(gameTime); }


            // draw the splash screens for each level 
            if (state == 2) { DrawSS2(gameTime); }
            if (state == 3) { DrawSS3(gameTime); }


            // this is the actual level one for the game 
            if (state == 4) {  DrawL1(gameTime); }

            // actual level two for the game 
            if (state == 5) { DrawL2(gameTime); }

            // toggle the help screen for the user 
            if (showHH) { DrawHH(gameTime); }

            // toggle the information screen for the user
            if (showII) { DrawII(gameTime); }


            // once the user has passed both levels
            // then game over is declared 
            if (state == 6) { DrawEE(gameTime); }
            if (state == 7) { DrawSurprise(gameTime);  }

            base.Draw(gameTime);
        }


        /*** 
         *** 
         *** Stores all the important components of the current
         *** computer game, such as the load content, timer and
         *** draw methods, for each level in the current computer
         *** game.         
         ***  
         *** 
         ***/

        public void UpdateSS0(GameTime gameTime) {
            // reset back to the original coordinates
            kittyCatVec.X = 275;
            kittyCatVec.Y = 50;

            titleVec.X = 100;
            sloganVec.X = 290;
            instructVec.X = 315;
            kittyCat.setPos(kittyCatVec.X, kittyCatVec.Y);
        }


        public void UpdateSS1(GameTime gameTime)
        {
            // transition timer between splash screen and level 1
            kittyCatVec.X++;
            titleVec.X++;
            sloganVec.X++;
            instructVec.X++;
            kittyCat.setPos(kittyCatVec.X, kittyCatVec.Y);

            // transitition boolean 
            if (kittyCatVec.X > 900) state = 2;

        }
        public void UpdateSS2(GameTime gameTime) {
            // Level 1 text should fade and then transition 
            ltCol.G--;

            // once the color fades out then switch state to level 1 game
            if (ltCol.G == 0) state = 4;


        }
        public void UpdateSS3(GameTime gameTime)
        {
            // fade out the title for the level
            ltCol.G--;

            // reset points and lives
            points = 0;
            lives = 2; 
            // once the color fades out then switch state to level 2 game
            if (ltCol.G == 0) state = 5;

        }
        public void UpdateHH(GameTime gameTime)
        {

        }
        public void DrawSS0(GameTime gameTime)
        {
            // This draws the splash screen for the computer game
            spriteBatch.Begin();
            kittyCat.Draw(spriteBatch);
            spriteBatch.DrawString(llFont, "Cat Adventures", titleVec, Color.OrangeRed);
            spriteBatch.DrawString(mmFont, "It's a hard life being a cat...", sloganVec, Color.OrangeRed);
            spriteBatch.DrawString(mmFont, "Press S to continue...", instructVec, Color.OrangeRed);
            spriteBatch.End();

        }

        public void LoadSS2()
        {
            // set values for the first level
            levelText.text = "Level 1";
            levelText.colour = ltCol;
        }

        public void DrawSS2(GameTime gameTime) {
            LoadSS2();
            // This is the 1st level splash screen
            spriteBatch.Begin();
            levelText.Draw(spriteBatch);
            spriteBatch.End();
        }

        public void LoadL1()
        {
            // LoadContent for Level 1 
            //playerSprite = new Sprite3(true, kcTexxx, playerVec.X, playerVec.Y); 



        }


        public void DrawL1(GameTime gameTime)
        {
            // now the user can actually play the game - this is level 1
            spriteBatch.Begin();
            background.Draw(spriteBatch);
            playerSprite.Draw(spriteBatch);
            yarnBalls.Draw(spriteBatch); 

            // draw the bounding box if the user needs it 
            if (showBB) playerSprite.drawBB(spriteBatch, Color.Red);

            spriteBatch.End();
        }

        public void UpdateL1(GameTime gameTime)
        {

    

            ks = Keyboard.GetState();

          
            yarnBalls.moveDeltaXY();


            // collision test 

            for (int i=0; i < yarnBalls.count(); i++)
            {
                Sprite3 s = yarnBalls.getSprite(i);

                 bool coll = playerSprite.collision(s);

                if (coll)
                {
                
                    kachingSound.playSoundIfOk(); 
                    s.setActive(false);
                    points++; 
                }
               

            }

           


            // used to move the kitty cat to the left 
            if (ks.IsKeyDown(Keys.Left))
            {
                playerSprite.moveByDeltaXY();
                playerSprite.animationTick(gameTime);
                successSound.playSoundIfOk();
                playerSprite.active = true;
                background.Update(gameTime);
            }

            if (ks.IsKeyDown(Keys.Right))
            {
                // NOTE: the move right functionality does not work currently
                background.Update(gameTime);

            }

            if (ks.IsKeyDown(Keys.B))
            {
                showBB = !showBB;
            }

            /// increases the player's jump speed - this is a secret function
            if (ks.IsKeyDown(Keys.Z))
            {
                playerJumpSpeed++;
            }

            // decreases the player's jump speed - this is another secret function
            if (ks.IsKeyDown(Keys.X))
            {
                playerJumpSpeed--;
            }

            // this gives the small kitty cat the ability to jump around the screen 
            if (playerJump)
            {
                playerVec.Y += playerJumpSpeed;
                playerJumpSpeed += 1;
                playerSprite.setPosY(playerVec.Y);

                if (playerVec.Y >= prevPos.Y)
                {
                    playerSprite.setPosY(prevPos.Y);
                    playerJump = false;
                }

            } else {
                if (ks.IsKeyDown(Keys.Up))
                {
                    playerJump = true;
                    jumpoSound.playSoundIfOk();
                    playerJumpSpeed = -14;
                    playerSprite.setPosY(playerVec.Y);
                }

            }

            // transition over to level 2 
            if (points == 5) state = 3;

        }

        public void UpdateL2(GameTime gameTime)
        {

            // the crazy dog will be running towards the right and will
            // appear randomly on the screen 

            crazyDog.visible = true;
            crazyDog.moveByDeltaXY();
            crazyDog.animationTick(gameTime);
            crazyDog.active = true;

            doggyVec.X++;
            crazyDog.setPosX(doggyVec.X);

            if (crazyDog.getPosX() > 800) doggyVec.X = 0;

            if (doggyVec.X == 20 || doggyVec.X == 400) barkoSound.playSoundIfOk();


            // check whether the player sprite has clashed with the crazy dog
            colWBB = playerSprite.collision(crazyDog);

            // play the sound if this is true 
            if (colWBB == true && doggyVec.X == playerVec.X)
            {
                yowlinSound.playSoundIfOk();
                lives--;   
            }


                // play the kaching sound if the player sprite passes over 
                // the dog and also does not colide

                if (colWBB == false && doggyVec.X == playerVec.X)
                {
                    kachingSound.playSoundIfOk();
                    points++; 
                   
                }






                ks = Keyboard.GetState();

                // this is really bad practice to repeat code from another method
                // but have no time right now to implement something better 
                if (ks.IsKeyDown(Keys.Left))
                {
                    playerSprite.moveByDeltaXY();
                    playerSprite.animationTick(gameTime);
                    successSound.playSoundIfOk();
                    playerSprite.active = true;
                    background.Update(gameTime);
                }


                if (ks.IsKeyDown(Keys.Right))
                {
                    // NOTE: the move right functionality does not work currently
                    background.Update(gameTime);

                }

                if (ks.IsKeyDown(Keys.B))
                {
                    showBB = !showBB;
                }

                /// increases the player's jump speed - this is a secret function
                if (ks.IsKeyDown(Keys.Z))
                {
                    playerJumpSpeed++;
                }

                // decreases the player's jump speed - this is another secret function
                if (ks.IsKeyDown(Keys.X))
                {
                    playerJumpSpeed--;
                }

                // this gives the small kitty cat the ability to jump around the screen 
                if (playerJump)
                {
                    playerVec.Y += playerJumpSpeed;
                    playerJumpSpeed += 1;
                    playerSprite.setPosY(playerVec.Y);

                    if (playerVec.Y >= prevPos.Y)
                    {
                        playerSprite.setPosY(prevPos.Y);
                        playerJump = false;
                    }

                }
                else
                {
                    if (ks.IsKeyDown(Keys.Up))
                    {
                        playerJump = true;
                        playerJumpSpeed = -14;
                        jumpoSound.playSoundIfOk();
                        playerSprite.setPosY(playerVec.Y);
                    }

                }


            // transition to end of game if user reaches 5 pts
            if (points == 5) state = 6;
            if (lives == 0) state = 7; 



            }

            public void DrawL2(GameTime gameTime)
            {
                spriteBatch.Begin();
                background.Draw(spriteBatch);
                crazyDog.Draw(spriteBatch);
                playerSprite.Draw(spriteBatch);


                // draw the bounding box if the user needs it 
                if (showBB)
                {
                    playerSprite.drawBB(spriteBatch, Color.Red);
                    crazyDog.drawBB(spriteBatch, Color.Red);
                }


                spriteBatch.End();

            }

            public void LoadSS3()
            {
                // set values for level 2
                levelText.text = "Level 2";
                levelText.colour = ltCol;
            }



            public void DrawSS3(GameTime gameTime)
            {
                // This is the 2nd level splash screen
                LoadSS3();


                spriteBatch.Begin();
                levelText.Draw(spriteBatch);
                spriteBatch.End();
            }


            public void DrawHH(GameTime gameTime)
            {
                // draws the help screen which can be toggled 
                spriteBatch.Begin();
                spriteBatch.DrawString(ssFont, "KEYBOARD OPTIONS", new Vector2(50, 50), Color.Gold);
                spriteBatch.DrawString(ssFont, "H - toggle help", new Vector2(50, 65), Color.Gold);
                spriteBatch.DrawString(ssFont, "S - start game", new Vector2(50, 80), Color.Gold);
                spriteBatch.DrawString(ssFont, "1/2 - levels 1 or 2", new Vector2(50, 95), Color.Gold);
                spriteBatch.DrawString(ssFont, "Right - move right", new Vector2(50, 110), Color.Gold);
                spriteBatch.DrawString(ssFont, "Left - move left", new Vector2(50, 125), Color.Gold);
                spriteBatch.DrawString(ssFont, "Up - jump", new Vector2(50, 140), Color.Gold);
                spriteBatch.DrawString(ssFont, "Space - pounce", new Vector2(50, 155), Color.Gold);
                spriteBatch.DrawString(ssFont, "B - show bounding boxes", new Vector2(50, 170), Color.Gold);
                spriteBatch.End();
            }

            public void DrawII(GameTime gameTime)
            {
                // show information if the user requests it
                spriteBatch.Begin();
                spriteBatch.DrawString(ssFont, levelText.text, new Vector2(700, 50), Color.White);
                spriteBatch.DrawString(ssFont, "Lives: " + lives, new Vector2(700, 65), Color.White);
                spriteBatch.DrawString(ssFont, "Points: " + points, new Vector2(700, 80), Color.White);
                spriteBatch.End();

            }

            public void LoadEE()
            {
                ltCol.R = 255;
                ltCol.G = 255;
                ltCol.B = 255;
            }


            public void UpdateEE(GameTime gameTime)
            {
                // is it better to decrement the alpha or the rgb values?
                ltCol.R--;
                ltCol.G--;
                ltCol.B--;
                ltCol.A--;

                if (lives == 0 && ltCol.R==0) eeSound.playSoundIfOk();
                if (ltCol.R == 0)  state = 0;  showII = false; 

            }
            
            public void DrawSurprise(GameTime gameTime)
            {
   
                spriteBatch.Begin();
                spriteBatch.DrawString(llFont, "You lost!", new Vector2(175, 200), ltCol);
                spriteBatch.End();

            }


            public void DrawEE(GameTime gameTime)
            {
            //LoadEE(); 
            // this is the game over splash screen when the user 
            // successfully passes all the levels 

            // change vector to standard vector - maybe titleVec? 

                spriteBatch.Begin();
                spriteBatch.DrawString(llFont, "Game Over!", new Vector2(175, 200), ltCol);
                spriteBatch.End();

            }

        }
    }