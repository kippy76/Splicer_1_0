using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Splicer
{
    class View
    {
        private Model model;
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private ContentManager contentManager;
        private Vector3 screenScale;
        private Matrix screenResTransform;
        private Vector2 baseScreenRes;
        private float horScale, vertScale;
        private SpriteFont spriteFontS, spriteFontL;
        private int screenW, screenH;
        private Texture2D mouseIcon;
        int mouseX, mouseY;

        public View(Model model, GraphicsDeviceManager gfx, SpriteBatch sb, ContentManager cm, int screenW, int screenH)
        {
            this.model = model;
            this.graphics = gfx;
            this.spriteBatch = sb;
            this.contentManager = cm;
            this.screenW = screenW;
            this.screenH = screenH;
            model.screenWidth = gfx.PreferredBackBufferWidth;
            model.screenHeight = graphics.PreferredBackBufferHeight;            
        }

        public void initGFX()
        {
            // Graphics created in 1024 x 768 - create scaler for spritebatch to carry out transform if XNA falls back to any other            
            baseScreenRes = new Vector2(this.screenW, this.screenH);
            horScale = (float)graphics.GraphicsDevice.Viewport.Width / baseScreenRes.X;
            vertScale = (float)graphics.GraphicsDevice.Viewport.Height / baseScreenRes.Y;
            screenScale = new Vector3(horScale, vertScale, 1);
            screenResTransform = Matrix.CreateScale(screenScale);
            mouseIcon = this.contentManager.Load<Texture2D>("mouse");            
        }

        public void render(int mouseX, int mouseY)
        {
            this.mouseX = mouseX;
            this.mouseY = mouseY;
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, null, null, screenResTransform);
            spriteFontS = contentManager.Load<SpriteFont>("spriteFontS");
            spriteFontL = contentManager.Load<SpriteFont>("spriteFontL");
            // Draw background
            graphics.GraphicsDevice.Clear(Color.White);
            renderBackdrop();
            if ((model.gameState == (int)Splicer.Model.GameStates.PLAY) || (model.gameState == (int)Splicer.Model.GameStates.LEVELSTART))
            {
                // Render timer background
                int renderWidthPixels = model.screenWidth - 40;
                if (model.timer.duration != 0)
                {
                    renderWidthPixels = (int)((float)renderWidthPixels * ((1 / (float)model.timer.duration) * (float)model.timer.secondsRemaining()));
                }
                renderTimerOutline(18, model.screenHeight - 20 - 40 - 2, model.screenWidth - 35, 44);
                renderTimer(renderWidthPixels);                
            }
            if (model.gameState == (int)Splicer.Model.GameStates.PLAY)
            {
                renderPlasmid();
                renderSelectedPlasmidBricks();
                renderDrawers();
                renderGoaltext();              
                renderButton();
            }
            if (model.gameState == (int)Splicer.Model.GameStates.LEVELSTART)
            {
                renderLevelStart();
            }
            if (model.gameState == (int)Splicer.Model.GameStates.GAMEOVER)
            {
                renderGameOver();
            }
            renderScore();
            renderMouse();
            spriteBatch.End();
        }

        private void renderBackdrop()
        {
            Texture2D texture = contentManager.Load<Texture2D>("Backdrop");
             spriteBatch.Draw(
                     texture,
                     new Vector2(0, 0),
                     null,
                     Color.White,
                    0f,
                    new Vector2(0, 0),
                    1f,
                     SpriteEffects.None,
                  1.0f);
        }

        private void renderGoaltext()
        {
            int tx = (model.screenWidth / 2) - 140;
            int ty = (model.screenHeight / 2) - 140;
            string goaltext = model.solution.getGoalTextForLevel(model.level);
            goaltext = boundText(goaltext, 280, true);
            spriteBatch.DrawString(spriteFontL, goaltext, new Vector2(tx, ty), Color.Blue);
        }

        private void renderSelectedPlasmidBricks()
        {
            int bricksInSolution = model.solution.brickCountForLevel(model.level);
            int currentBrickNumber = 1;
            int currentPlasmidLocation = 1;
            int segmentsPerBrick = 1;
            bool doubleUp = (12 / (float)bricksInSolution) % 1 == 0 ? false : true;
            double angle, X, Y;
            foreach (Brick thisBrick in model.pallet.getAllSelectedBricks())
            {
                segmentsPerBrick = doubleUp ? ((currentBrickNumber > ((float)bricksInSolution - (bricksInSolution * ((12 / (float)bricksInSolution) % 1)))) ? 2 : 1) : 12 / bricksInSolution;
                for (int dup = segmentsPerBrick; dup > 0; dup--)
                {
                    angle = ((2 * (float)Math.PI) / 12) * (currentPlasmidLocation - 1);
                    X = ((double)model.screenWidth - 500) / 2;
                    Y = ((double)model.screenHeight - 500) / 2;
                    X += ((double)250) * (Math.Sin(angle) + 1);
                    Y += ((double)250) * (1 - (Math.Cos(angle)));
                    Texture2D texture = contentManager.Load<Texture2D>("plasmid" + (currentBrickNumber % 13));
                    Rectangle dest;
                    dest.X = (int)Math.Round(X, 0);
                    dest.Y = (int)Math.Round(Y, 0);
                    spriteBatch.Draw(
                         texture,
                         new Vector2(dest.X, dest.Y),
                         null,
                         Color.White,
                        (float)angle,
                        new Vector2(0, 0),
                        1f,
                         SpriteEffects.None,
                      0.1f);
                    currentPlasmidLocation++;
                }
                currentBrickNumber++;
            }
        }

        private void renderDrawers()
        {
            int drawerNo = 1;
            float X, Y;
            X = Y = 20;
            foreach (string drawer in model.pallet.pallet.Keys)
            {
                // Drawer
                Rectangle dest;
                string fileAdd = model.activeDrawer == drawerNo ? "on" : "off";
                Texture2D texture = contentManager.Load<Texture2D>("drawer" + fileAdd);
                dest.X = (int)Math.Round(X, 0);
                dest.Y = (int)Math.Round(Y, 0);
                spriteBatch.Draw(
                     texture,
                     new Vector2(dest.X, dest.Y),
                     null,
                     Color.White,
                    0f,
                    new Vector2(0, 0),
                    1f,
                     SpriteEffects.None,
                  0.1f);
                // Render text overlay
                string drawerText = drawer;
                int w = (int)spriteFontL.MeasureString(drawerText).X;
                int h = (int)spriteFontL.MeasureString(drawerText).Y;
                int x = 20 + ((100 - w) / 2);
                int y = 20 + ((drawerNo -1 )* 50) + ((50 - h) / 2);
                x = x < 0 ? 0 : x;
                y = y < 0 ? 0 : y;
                if (model.activeDrawer == drawerNo)
                {
                    spriteBatch.DrawString(spriteFontL, drawerText, new Vector2(x, y), Color.LightSteelBlue);
                }
                else
                {
                    spriteBatch.DrawString(spriteFontL, drawerText, new Vector2(x, y), Color.LightGray);
                }
                Y += texture.Height;
                // Drawer Contents?
                if (model.activeDrawer == drawerNo)
                {
                    renderDrawerContents(drawer);
                }
                drawerNo++;
            }
        }

        private void renderDrawerContents(string drawerName)
        {
            int X = 125;
            int Y = 20;
            int itemCount = model.pallet.pallet[drawerName].Count;
            int itemIdx = 0;
            bool brickSelected;
            for (; itemCount > 0; itemCount--)
            {
                string textToRender = " " + boundText(model.pallet.pallet[drawerName][itemIdx].name, 150, false);
                brickSelected = model.pallet.pallet[drawerName][itemIdx].picked;
                int textHeight = 50;
                if (brickSelected)
                {
                    renderRectangle(X, Y, X + 150, Y + textHeight, Color.DarkRed, Color.GreenYellow);
                }
                else
                {
                    renderRectangle(X, Y, X + 150, Y + textHeight, Color.DarkRed);
                }              
                    spriteBatch.DrawString(spriteFontS, textToRender, new Vector2(X, Y), Color.DarkRed);              
                Y += textHeight;
                itemIdx++;
            }
        }

        private void renderButton()
        {
            Rectangle dest;
            Texture2D texture = contentManager.Load<Texture2D>("go");
            dest.X = model.screenWidth - texture.Width - 10;
            dest.Y = (model.screenHeight - texture.Height) / 2;
            spriteBatch.Draw(
                   texture,
                   new Vector2(dest.X, dest.Y),
                   null,
                   Color.White,
                  0f,
                  new Vector2(0, 0),
                  1f,
                   SpriteEffects.None,
                0.1f);
        }

        private void renderPlasmid()
        {
            Texture2D texture = contentManager.Load<Texture2D>("plasmid");
            Rectangle dest = new Rectangle();
            dest.X = (model.screenWidth - 500) / 2;
            dest.Y = (model.screenHeight - 500) / 2;
            dest.Width = texture.Width;
            dest.Height = texture.Height;
            spriteBatch.Draw(
                        texture,
                        new Vector2(dest.X, dest.Y),
                        null,
                        Color.White,
                       0,
                       new Vector2(0, 0),
                       1f,
                        SpriteEffects.None,
                     0.8f);
        }

        private void renderTimer(int renderWidthPixels)
        {
            Texture2D texture = contentManager.Load<Texture2D>("timer");
            int timerWidth = model.screenWidth - 40;
            Rectangle dest;
            dest.X = 20;
            dest.Y = model.screenHeight - 20 - texture.Height;
            for (int thisPix = 0; thisPix < renderWidthPixels; thisPix++)
            {
                spriteBatch.Draw(
                    texture,
                    new Vector2(dest.X, dest.Y),
                    null,
                    Color.White,
                   0f,
                   new Vector2(0, 0),
                   1f,
                    SpriteEffects.None,
                 0.05f);
                dest.X = dest.X + 1;
            }
        }

        private void renderTimerOutline(int x, int y, int width, int height)
        {
            renderRectangle(x, y, x + width - 1, y + height - 1, Color.Red, Color.Red);
        }

        private void renderRectangle(int x1, int y1, int x2, int y2, Color lineColour)
        {
            renderRectangle(x1, y1, x2, y2, false, lineColour, Color.White);
        }

        private void renderRectangle(int x1, int y1, int x2, int y2, Color lineColour, Color fillColour)
        {
            renderRectangle(x1, y1, x2, y2, true, lineColour, fillColour);
        }

        private void renderRectangle(int x1, int y1, int x2, int y2, bool filled, Color lineColour, Color fillColour)
        {
            int width = x2 - x1;
            int height = y2 - y1;
            Texture2D texture = new Texture2D(graphics.GraphicsDevice, width, height, false, SurfaceFormat.Color);
            Color[] colourArray = new Color[width * height];
            for (int row = 0; row < height; row++)
            {
                for (int pix = 0; pix < width; pix++)
                {
                    if ((pix == 0) || (pix == width - 1) || (row == 0) || (row == height - 1))
                    {
                        colourArray[row * width + pix] = lineColour;
                    }
                    else
                    {
                        if (filled)
                        {
                            colourArray[row * width + pix] = fillColour;
                        }
                    }
                }
            }
            texture.SetData(colourArray);
            spriteBatch.Draw(texture, new Vector2(x1, y1), null, Color.White, 0.0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0.1f);
        }

        private void renderLevelStart()
        {
            string renderText = "";
            if (model.level > 1)
            {
                renderText += "Well Done!\n\n";
            }
            renderText += "Prepare For\n   Level " + model.level.ToString();
            int x = (model.screenWidth / 2) - 40;
            int y = (model.screenHeight / 2) - 40;
            spriteBatch.DrawString(spriteFontL, renderText, new Vector2(x, y), Color.BlueViolet);
        }

        private void renderGameOver()
        {
            List<string> textLines = new List<string>() ;
            int y;
            int ygap;
            if (model.solution.brickCountForLevel(model.level) == -1)
            {
                textLines.Add("Congratulations!");
                textLines.Add("You have Completed All The Levels");
            }
            textLines.Add("GAME OVER");
            textLines.Add("You Scored:");
            textLines.Add(model.score.ToString());
            if (model.score >= model.hiscore)
            {
                textLines.Add("A NEW HI-SCORE!");
            }
            y = (model.screenHeight / 2) - 100;
             ygap = 5 + (int)spriteFontL.MeasureString(textLines[0]).Y;
             int x;
             foreach (string line in textLines)
             {
                 x = (model.screenWidth / 2)  - (int)((spriteFontL.MeasureString(line).X) / 2);
                 spriteBatch.DrawString(spriteFontL, line, new Vector2(x, y), Color.BlueViolet);
                 y += ygap;
             }
        }

        private void renderScore()
        {
            string renderText = "SCORE : " + model.score + "   HISCORE : " + model.hiscore;
            int x = (model.screenWidth / 2) - (int)((spriteFontL.MeasureString(renderText).X)/2);
            int y = model.screenHeight  - 55;
            spriteBatch.DrawString(spriteFontL, renderText, new Vector2(x, y), Color.White);
        }

        private String boundText(String text, int boxWidth, bool large)
        {
            String currentLine = String.Empty;
            String result = String.Empty;
            String[] wordArray = text.Split(' ');
            SpriteFont sf = large ? spriteFontL : spriteFontS;
            foreach (String word in wordArray)
            {
                if (sf.MeasureString(currentLine + word).Length() > boxWidth)
                {
                    result += currentLine + "\n ";
                    currentLine = String.Empty;
                }
                currentLine += (word + ' ');
            }
            return result + currentLine;
        }

        private void renderMouse()
        {
            spriteBatch.Draw(mouseIcon, new Vector2(mouseX, mouseY), null, Color.White, 0.0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0.01f);            
        }

    }
}
