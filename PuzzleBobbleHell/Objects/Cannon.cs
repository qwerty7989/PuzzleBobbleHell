using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PuzzleBobbleHell.Manager;

namespace PuzzleBobbleHell.Objects
{
    public class Cannon : GameObject
    {
        private ContentManager contentManager;


        // ? Texture2D
        protected Texture2D _cannon;
        protected Texture2D _placeholderTexture;

        // ? Font
        protected SpriteFont testingFont;

        // ? Objects
        private Bubble _currentBubble;
        private Bubble[] _ammoBubble;
        private Bubble[] _ammoSpecialBubble;

        // ? Variables
        public Vector2 Position { set; get; }
        public float Rotation { set; get; }
        public Vector2 Origin;
        public Vector2 Scale;
        public Vector2 cannonSize;
        public float rotateOrigin;
        public int cursorSize;
        public float cannonLength { get; }
        public double cursorInitialLength { set; get; }
        public double cursorShowLength { set; get; }

        public float rotateRate;

        public double lastPressTime;
        public double nowPressTime;
        public double lastSwap;

        public bool isUsingSpecialAmmo;
        public int specialAmmoIndex;
        public bool loadNewAmmo;

        public Cannon()
        {
            cursorSize = 1;
            cannonSize = new Vector2(128,149);
            rotateOrigin = 73/100f;
            int posX = (int)((Singleton.Instance.GAME_SCREEN_SIZE.X/2f) + Singleton.Instance.GAME_SCREEN_POSITION.X);
            int posY = 769 + (int)(cannonSize.Y*rotateOrigin);
            Position = new Vector2(posX, posY);
            Rotation = 0f;
            cannonLength = cannonSize.Y*rotateOrigin;

            Initiate();

            // ? Special Ammo
            isUsingSpecialAmmo = false;
            specialAmmoIndex = 0;

            lastPressTime = 0f;
            lastSwap = 0f;

            rotateRate = 1.0f;
            loadNewAmmo = true;
        }

        public void LoadContent(ContentManager Content)
        {
            contentManager = new ContentManager(Content.ServiceProvider, Content.RootDirectory);

            // ? Shapes
            _placeholderTexture = new Texture2D(Singleton.Instance.graphicsDeviceManager.GraphicsDevice, 1, 1);
            Color[] data = new Color[1 * 1];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.White;
            _placeholderTexture.SetData(data);

            // ? Textures
            //_cannon = this.contentManager.Load<Texture2D>("PlayScene/Cannon");
            //Origin = new Vector2(_cannon.Width / 2f, _cannon.Height / 4f * 3f); // ? Center of Width, 3/4 of Height
            for (int tmpX = 0; tmpX < Singleton.Instance.CANNON_CARTRIDGE_SIZE; tmpX++) { _ammoBubble[tmpX].LoadContent(Content); }
            for (int tmpX = 0; tmpX < Singleton.Instance.CANNON_SPECIAL_CARTRIDGE_SIZE; tmpX++) { _ammoSpecialBubble[tmpX].LoadContent(Content); }

            // ? Font
            testingFont = contentManager.Load<SpriteFont>("PlayScene/MyFont");
        }

        public void UnloadContent()
        {

        }

        public void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (!Singleton.Instance.isShooting)
            {
                nowPressTime = (gameTime.TotalGameTime.Ticks / System.TimeSpan.TicksPerMillisecond);

                loadNewAmmo = true;

                // ? Shooting!
                if (!Singleton.Instance.isShooting && keyboardState.IsKeyDown(Keys.Space) && nowPressTime - lastPressTime > Singleton.Instance.gameTicksInMilliSec)
                {
                    Singleton.Instance.isShooting = true;
                    Singleton.Instance.shootingBubble = _currentBubble;

                    lastPressTime = nowPressTime;
                }


                // ? Rotate to the left
                if ((keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.Right)) && nowPressTime - lastPressTime > Singleton.Instance.gameTicksInMilliSec * rotateRate)
                {
                    rotateRate -= 0.054f;
                    if (rotateRate < 0.213f) rotateRate = 0.213f;


                    // ! 0.017 ~= 1 in Degree
                    if (keyboardState.IsKeyDown(Keys.Left) && Rotation > -(7*MathHelper.Pi/16))
                    {
                        Rotation -= 0.0085f;
                        lastPressTime = nowPressTime;
                    }
                    else if (keyboardState.IsKeyDown(Keys.Right) && Rotation < (7*MathHelper.Pi/16))
                    {
                        Rotation += 0.0085f;
                        lastPressTime = nowPressTime;
                    }
                }
                else if (nowPressTime - lastPressTime > 75f)
                {
                    if (rotateRate > 1f)
                        rotateRate = 1f;
                    else
                        rotateRate += 0.466f;
                }

                // ? Rotate to the right


                // ? Choose Special Ammo
                if (isUsingSpecialAmmo)
                {
                    if (keyboardState.IsKeyDown(Keys.C) && nowPressTime - lastSwap > Singleton.Instance.swapDelayInMilliSec)
                    {
                        specialAmmoIndex %= Singleton.Instance.CANNON_SPECIAL_CARTRIDGE_SIZE;
                        _currentBubble = _ammoSpecialBubble[specialAmmoIndex++];
                        lastSwap = nowPressTime;
                    }
                    else if (keyboardState.IsKeyDown(Keys.X) && nowPressTime - lastSwap > Singleton.Instance.swapDelayInMilliSec)
                    {
                        _currentBubble = _ammoBubble[0];
                        isUsingSpecialAmmo = false;
                        lastSwap = nowPressTime;
                    }
                }
                else if (keyboardState.IsKeyDown(Keys.C) && nowPressTime - lastSwap > Singleton.Instance.swapDelayInMilliSec)
                {
                    isUsingSpecialAmmo = true;
                    lastSwap = nowPressTime;
                }

                // ? Swap Normal ammo
                // ? Swapping while holding Special ammo is not allowed!
                if (keyboardState.IsKeyDown(Keys.Z) && !isUsingSpecialAmmo && nowPressTime - lastSwap > Singleton.Instance.swapDelayInMilliSec)
                {
                    Bubble tmpBubble = _ammoBubble[0];
                    _ammoBubble[0] = _ammoBubble[1];
                    _ammoBubble[1] = tmpBubble;

                    _currentBubble = _ammoBubble[0];
                    lastSwap = nowPressTime;
                }
            }
            else if (loadNewAmmo) // ? Loading the new ammo and stuff
            {
                for (int i = 0; i < Singleton.Instance.CANNON_CARTRIDGE_SIZE - 1; i++)
                {
                    _ammoBubble[i] = _ammoBubble[i+1];
                }

                System.Random rnd = new System.Random();
                string randomColor = Singleton.Instance.BubbleColor[rnd.Next(Singleton.Instance.BubbleColor.Length)];
                _ammoBubble[Singleton.Instance.CANNON_CARTRIDGE_SIZE - 1] = new Bubble(randomColor);
                _ammoBubble[Singleton.Instance.CANNON_CARTRIDGE_SIZE - 1].LoadContent(contentManager);


                _currentBubble = _ammoBubble[0];
                loadNewAmmo = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // ?  Cannon and Aiming Line
            CannonUI(spriteBatch);
            CurrentBubbleUI(spriteBatch);
            GuidingLine(spriteBatch);

            // ? Cartridge Ammo UI
            CartridgeNormalAmmoUI(spriteBatch);
            CartridgeSpecialAmmoUI(spriteBatch);

            // DEBUG
            spriteBatch.DrawString(testingFont, "Last time: " + lastPressTime, new Vector2(800, 1000), Color.Black);
            spriteBatch.DrawString(testingFont, "Now time: " + nowPressTime, new Vector2(800, 975), Color.Black);
            spriteBatch.DrawString(testingFont, "Elaspsed time: " + (nowPressTime-lastPressTime), new Vector2(800, 950), Color.Black);
        }

        public void CannonUI(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_placeholderTexture, Position, null, Color.White, Rotation, new Vector2(1/2f,rotateOrigin), cannonSize, SpriteEffects.None, 0f);

            // ? Load cannon with sprite
            //spriteBatch.Draw(_cannon, Position, null, Color.White, Rotation, Origin, 1f, SpriteEffects.None, 0f);
        }

        public void GuidingLine(SpriteBatch spriteBatch)
        {
            /*
                newX = sin(radian) * (cannon length to origin point) * (cannon postiion)
                newY = -cos(radian) * (cannon length to origin point) * (cannon postiion)
            */
            double newX = System.Math.Sin(Rotation) * (cannonSize.Y*rotateOrigin) + Position.X;
            double newY = -System.Math.Cos(Rotation) * (cannonSize.Y*rotateOrigin) + Position.Y;

            // ? First initial cursor line
            double borderToCannonDistance = Singleton.Instance.GAME_SCREEN_SIZE.X + Singleton.Instance.GAME_SCREEN_POSITION.X - Position.X;

            /*
                borderToCannonDistance * sin(Abs(radian)) - cannon size
            */
            // ! Margin offset value
            cursorInitialLength = (borderToCannonDistance- Singleton.Instance.bounceBorderMagin)/System.Math.Sin(System.Math.Abs(Rotation)) - (cannonSize.Y*rotateOrigin);

            // ! No margin offset value
            cursorShowLength = borderToCannonDistance/System.Math.Sin(System.Math.Abs(Rotation)) - (cannonSize.Y*rotateOrigin);

            // ? Upper border
            // ! 0.408 ~= 23 in Degree
            if (Rotation > -0.408 && Rotation < 0.408)
            {
                if (Rotation.Equals(0))
                {
                    cursorShowLength = (Singleton.Instance.GAME_SCREEN_POSITION.Y + Position.Y);
                }
                else
                {
                    borderToCannonDistance = newY;
                    cursorShowLength = borderToCannonDistance/System.Math.Cos(System.Math.Abs(Rotation)) + (cannonSize.Y*rotateOrigin);
                }
            }

            // ? Draw First Cursor
            spriteBatch.Draw(_placeholderTexture, new Vector2((int)newX, (int)newY), null, Color.Red, Rotation, new Vector2(1/2f,1f), new Vector2(cursorSize, (int)cursorShowLength), SpriteEffects.None, 0f);


            spriteBatch.Draw(_placeholderTexture, new Vector2((int)newX, (int)newY), null, Color.Green, Rotation, new Vector2(1/2f,1f), new Vector2(cursorSize, (int)cursorInitialLength), SpriteEffects.None, 0f);

            // ? Draw Bounce Cursor
            if (Rotation < -0.408 || Rotation > 0.408)
            {
                int amountOfBounce = 2;
                BounceCursor(spriteBatch, newX, newY, cursorShowLength, 0, amountOfBounce);
                BounceCursorMargin(spriteBatch, newX, newY, cursorInitialLength, 0, amountOfBounce);
            }

            // DEBUG
            spriteBatch.DrawString(testingFont, "Cursor: " + (cursorShowLength), new Vector2(800, 925), Color.Black);
            spriteBatch.DrawString(testingFont, "Angle: " + (Rotation), new Vector2(800, 900), Color.Black);
        }

        public void BounceCursor(SpriteBatch spriteBatch, double newX, double newY, double cursorInitialLength, int bounceCount, int bounceAmount)
        {
            if (!(newY > Singleton.Instance.GAME_SCREEN_POSITION.Y && newY < Singleton.Instance.GAME_SCREEN_POSITION.Y + Singleton.Instance.GAME_SCREEN_SIZE.Y))
                return;

            // ? For specific amount of bounce
            //if (bounceAmount == bounceCount)
            //    return;

            // ? Bounce Cursor
            double bounceX = 0f, bounceY = 0f;
            double bounceCursorLength = 0f;
            float angleSin = 0f, angleRotation = 0f;
            if (Rotation < -0.408 || Rotation > 0.408)
            {
                angleSin = (bounceCount % 2 == 0) ? Rotation : -Rotation;
                angleRotation = -angleSin;
                bounceX = System.Math.Sin(angleSin) * cursorInitialLength + newX;
                bounceY = -System.Math.Cos(Rotation) * cursorInitialLength + newY;
                bounceCursorLength = (Singleton.Instance.GAME_SCREEN_SIZE.X)/System.Math.Sin(System.Math.Abs(Rotation));
                spriteBatch.Draw(_placeholderTexture, new Vector2((int)bounceX, (int)bounceY), null, Color.Red, angleRotation, new Vector2(1/2f,1f), new Vector2(cursorSize, (int)bounceCursorLength), SpriteEffects.None, 0f);
            }

            BounceCursor(spriteBatch, bounceX, bounceY, bounceCursorLength, bounceCount + 1, bounceAmount);
        }

        public void BounceCursorMargin(SpriteBatch spriteBatch, double newX, double newY, double cursorInitialLength, int bounceCount, int bounceAmount)
        {
            if (!(newY > Singleton.Instance.GAME_SCREEN_POSITION.Y && newY < Singleton.Instance.GAME_SCREEN_POSITION.Y + Singleton.Instance.GAME_SCREEN_SIZE.Y))
                return;

            // ? For specific amount of bounce
            //if (bounceAmount == bounceCount)
            //    return;

            // ? Bounce Cursor
            double bounceX = 0f, bounceY = 0f;
            double bounceCursorLength = 0f;
            float angleSin = 0f, angleRotation = 0f;
            if (Rotation < -0.408 || Rotation > 0.408)
            {
                angleSin = (bounceCount % 2 == 0) ? Rotation : -Rotation;
                angleRotation = -angleSin;
                bounceX = System.Math.Sin(angleSin) * cursorInitialLength + newX;
                bounceY = -System.Math.Cos(Rotation) * cursorInitialLength + newY;
                bounceCursorLength = (Singleton.Instance.GAME_SCREEN_SIZE.X-(Singleton.Instance.bounceBorderMagin*2))/System.Math.Sin(System.Math.Abs(Rotation));
                spriteBatch.Draw(_placeholderTexture, new Vector2((int)bounceX, (int)bounceY), null, Color.Green, angleRotation, new Vector2(1/2f,1f), new Vector2(cursorSize, (int)bounceCursorLength), SpriteEffects.None, 0f);
            }

            BounceCursorMargin(spriteBatch, bounceX, bounceY, bounceCursorLength, bounceCount + 1, bounceAmount);
        }

        public void CurrentBubbleUI(SpriteBatch spriteBatch)
        {
            _currentBubble.DrawAmmo(spriteBatch, Position);
        }

        public void CartridgeNormalAmmoUI(SpriteBatch spriteBatch)
        {
            for (int tmpX = 1; tmpX < Singleton.Instance.CANNON_CARTRIDGE_SIZE; tmpX++)
            {
                // ? The order is 3 2 1 0
                _ammoBubble[Singleton.Instance.CANNON_CARTRIDGE_SIZE-tmpX].DrawAmmo(spriteBatch, new Vector2(Singleton.Instance.BUBBLE_START_POS.X + ((tmpX-1) * 56) + 9, 980));
            }
        }

        public void CartridgeSpecialAmmoUI(SpriteBatch spriteBatch)
        {
            for (int tmpX = 0; tmpX < Singleton.Instance.CANNON_SPECIAL_CARTRIDGE_SIZE; tmpX++)
            {
                _ammoSpecialBubble[tmpX].DrawAmmo(spriteBatch, new Vector2(Singleton.Instance.BUBBLE_START_POS.X + (tmpX * 56) + 521, 980));
            }
        }

        public void Initiate()
        {
            // ? Initite Bubble Array
            _ammoBubble = new Bubble[Singleton.Instance.CANNON_CARTRIDGE_SIZE];
            _ammoSpecialBubble = new Bubble[Singleton.Instance.CANNON_SPECIAL_CARTRIDGE_SIZE];

            // ? Random out 3 color ammo
            System.Random rnd = new System.Random();
            for (int tmpX = 0; tmpX < Singleton.Instance.CANNON_CARTRIDGE_SIZE; tmpX++)
            {
                string randomColor = Singleton.Instance.BubbleColor[rnd.Next(Singleton.Instance.BubbleColor.Length)];
                _ammoBubble[tmpX] = new Bubble(randomColor);
            }

            // ? Random special ammo
            for (int tmpX = 0; tmpX < Singleton.Instance.CANNON_SPECIAL_CARTRIDGE_SIZE; tmpX++)
            {
                string randomColor = Singleton.Instance.BubbleColor[rnd.Next(Singleton.Instance.BubbleColor.Length)];
                _ammoSpecialBubble[tmpX] = new Bubble(randomColor);
            }

            _currentBubble = _ammoBubble[0];
        }
    }
}