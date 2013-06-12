using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Splicer
{
    public class Controller : Microsoft.Xna.Framework.Game
    {
        // BOOKMARK Screen Settings
        private static bool FULLSCREEN = false;
        private static int SCREENW = 1024;
        private static int SCREENH = 768;
        int DBL = 4;    // Delay between levels
        IO io;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Model model;
        View view;
        SoundPlayer soundPlayer;

        public Controller()
        {          
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = SCREENW;
            graphics.PreferredBackBufferHeight = SCREENH;
            graphics.IsFullScreen = FULLSCREEN;
            this.IsMouseVisible = false;    // We will render a custom mouse icon in-game
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            // Init sound
            soundPlayer = new SoundPlayer(Content);
            soundPlayer.addEffect("cut1");
            soundPlayer.setEffectVolume("cut1", 0.8f);
            soundPlayer.addEffect("cut2");
            soundPlayer.setEffectVolume("cut2", 0.8f);
            soundPlayer.addEffect("cut3");
            soundPlayer.setEffectVolume("cut3", 0.8f);
            soundPlayer.addEffect("wrong");
            soundPlayer.setEffectVolume("wrong", 0.8f);
            soundPlayer.addEffect("levelup");
            soundPlayer.setEffectVolume("levelup", 0.4f);
            soundPlayer.addEffect("gameover");
            soundPlayer.setEffectVolume("gameover", 1.0f);
        }

        protected override void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // Create MVC framework
            model = new Model();
            view = new View(model, graphics, spriteBatch, Content, SCREENW, SCREENH);
            io = new IO();
            model.timer.start(DBL);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            view.initGFX();
        }
        
        protected override void UnloadContent()
        {            
        }

        protected override void Update(GameTime gameTime)
        {
            if (model.gameState == (int)Model.GameStates.EXIT)
            {
                this.Exit();
            }
            if ((model.timer.expired()) && (model.gameState == (int)Model.GameStates.LEVELSTART))
            {
                model.timer.start(model.timeLimit);
                model.gameState = (int)Model.GameStates.PLAY;
            }
            if ((model.timer.expired()) && (model.gameState == (int)Model.GameStates.GAMEOVER))
            {
                model.timer.start(DBL);
                model.level = 1;                
                if (model.score > model.hiscore)
                {
                    model.hiscore = model.score;
                }
                model.score = 0;
                model.initLevel();
                model.gameState = (int)Model.GameStates.LEVELSTART;
            }
            if ((model.timer.expired()) && (model.gameState == (int)Model.GameStates.PLAY))
            {
                model.gameState = (int)Model.GameStates.GAMEOVER;
                model.timer.start(10);
                soundPlayer.playEffect("gameover", false);
            }
            if (io.pollKeys() == 27)
            {
                model.gameState = (int)Model.GameStates.EXIT;
            }
            if (io.mouseButtonsStateChange())
            {
                MouseState mouseState = io.pollMouse();
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    // Check for drawer selection...
                    if ((mouseState.X > 20) && (mouseState.X < 120) && (mouseState.Y > 20) && (mouseState.Y < (20 + 50 * model.pallet.pallet.Keys.Count)))
                    {
                        model.activeDrawer = 1 + ((mouseState.Y - 20) / 50);
                    }
                    // Check for drawer content selection...
                    if ((mouseState.X > 125) && (mouseState.X < 275) && (mouseState.Y > 20) && (mouseState.Y < (20 + 50 * model.pallet.pallet.ElementAt(model.activeDrawer-1).Value.Count)))
                    {
                        int drawerNo = model.activeDrawer - 1;
                        int itemNo = ((mouseState.Y - 20) / 50);
                        itemNo = itemNo >= model.pallet.pallet[model.pallet.getDrawerNames()[drawerNo]].Count ? -1 : itemNo;
                        if (itemNo != -1)
                        {
                            int selBrickId =  model.pallet.pallet[model.pallet.getDrawerNames()[drawerNo]][itemNo].id;
                            Brick selectedBrick = model.pallet.getBrick(selBrickId);
                            if ((!selectedBrick.picked) && (model.plasmid.restrictionSitesQuantity > model.pallet.getAllSelectedBricks().Count()))
                            {
                                selectedBrick.picked = true;
                                model.plasmid.sitesFilled = model.plasmid.sitesFilled + 1;
                                soundPlayer.playEffect("cut" + Random(1, 3), false);
                            }
                            else
                            {
                                if (selectedBrick.picked)
                                {
                                    selectedBrick.picked = false;
                                    model.plasmid.sitesFilled = model.plasmid.sitesFilled - 1;
                                    soundPlayer.playEffect("cut" + Random(1, 3), false);
                                }
                                else
                                {
                                    soundPlayer.playEffect("wrong", false);
                                }                                                    
                            }
                        }
                    }
                    // Check for translate press
                    if ((mouseState.X > model.screenWidth - 210) && (mouseState.X < model.screenWidth - 10) && (mouseState.Y > (model.screenHeight / 2) - 100) && (mouseState.Y < (model.screenHeight / 2) + 100))
                    {
                        // Check correct solution...
                        if (model.plasmid.restrictionSitesQuantity != model.plasmid.sitesFilled)
                        {                           
                            soundPlayer.playEffect("wrong", false);
                            model.scoreDown(5);
                        }
                        else
                        {
                            // verify is solution correct
                            if (model.pallet.getAllSelectedBrickIds().Except(model.solution.getBrickIdsForSolution(model.level)).Count() == 0)
                            {
                                // Level up, set model game state to LEVEL START and set model timer to 5 secs
                                model.gameState = (int)Splicer.Model.GameStates.LEVELSTART;
                                model.scoreUp(100);
                                model.levelUp();
                                model.timer.start(DBL);
                                model.timeLimit = (int)((float)model.timeLimit * 0.95f);
                                model.initLevel();
                                if (model.solution.brickCountForLevel(model.level) == -1)   // All levels completed!
                                {
                                    model.gameState = (int)Splicer.Model.GameStates.GAMEOVER;
                                }
                                soundPlayer.playEffect("levelup", false);
                            }
                            else
                            {
                                soundPlayer.playEffect("wrong", false);
                            }
                        }
                    }
                }
            }
           
          
            base.Update(gameTime);
        }

        private int Random(int min, int max)
        {
            max++;
            Random random = new Random();
            return random.Next(min, max);
        }

        protected override void Draw(GameTime gameTime)
        {
            view.render(io.pollMouse().X, io.pollMouse().Y);
            base.Draw(gameTime);
        }
    }
}
