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
        public Vector2 Position;
        public float Rotation;
        public Vector2 Origin;
        public Vector2 Scale;
        public Vector2 cannonSize;

        public bool isUsingSpecialAmmo;
        public int specialAmmoIndex;

        public Cannon()
        {
            cannonSize = new Vector2(128,149);
            int posX = (int)((Singleton.Instance.GAME_SCREEN_SIZE.X/2f) + Singleton.Instance.GAME_SCREEN_POSITION.X);
            int posY = 769 + (int)(cannonSize.Y*73/100f);
            Position = new Vector2(posX, posY);
            Rotation = 0f;

            Initiate();

            // ? Special Ammo
            isUsingSpecialAmmo = false;
            specialAmmoIndex = 0;
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
                // ? Shooting!
                if (!Singleton.Instance.isShooting && keyboardState.IsKeyDown(Keys.Space))
                {
                    Singleton.Instance.isShooting = true;
                    Singleton.Instance.shootingBubble = _currentBubble;
                }


                // ? Rotate to the left
                if (keyboardState.IsKeyDown(Keys.Left) && Rotation > -(5*MathHelper.Pi/12))
                    Rotation -= MathHelper.Pi / 180;

                // ? Rotate to the right
                if (keyboardState.IsKeyDown(Keys.Right) && Rotation < (5*MathHelper.Pi/12))
                    Rotation += MathHelper.Pi / 180;


                // ? Choose Special Ammo
                if (isUsingSpecialAmmo)
                {
                    if (keyboardState.IsKeyDown(Keys.C))
                    {
                        specialAmmoIndex %= Singleton.Instance.CANNON_SPECIAL_CARTRIDGE_SIZE;
                        _currentBubble = _ammoSpecialBubble[specialAmmoIndex++];
                    }
                    else if (keyboardState.IsKeyDown(Keys.X))
                    {
                        _currentBubble = _ammoBubble[0];
                        isUsingSpecialAmmo = false;
                    }
                }
                else if (keyboardState.IsKeyDown(Keys.C))
                {
                    isUsingSpecialAmmo = true;
                }


                // ? Swap Normal ammo
                // ? Swapping while holding Special ammo is not allowed!
                if (keyboardState.IsKeyDown(Keys.Z) && !isUsingSpecialAmmo)
                {
                    Bubble tmpBubble = _ammoBubble[0];
                    _ammoBubble[0] = _ammoBubble[1];
                    _ammoBubble[1] = tmpBubble;

                    _currentBubble = _ammoBubble[0];
                }
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
        }

        public void CannonUI(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_placeholderTexture, Position, null, Color.White, Rotation, new Vector2(1/2f,73/100f), cannonSize, SpriteEffects.None, 0f);

            // ? Load cannon with sprite
            //spriteBatch.Draw(_cannon, Position, null, Color.White, Rotation, Origin, 1f, SpriteEffects.None, 0f);
        }

        public void GuidingLine(SpriteBatch spriteBatch)
        {
            double newX = System.Math.Sin(Rotation) * (cannonSize.Y*73/100f) + Position.X;
            double offsetY = (Rotation == 0) ? 0 : (cannonSize.Y*73/100f);
            double newY = (1-System.Math.Cos(Rotation)) * (cannonSize.Y*73/100f) + Position.Y - offsetY;

            // ? Find the make-sense length of cursor line

            int diffCenter = (int)(Singleton.Instance.GAME_SCREEN_SIZE.X + Singleton.Instance.GAME_SCREEN_POSITION.X - Position.X);
            int cursorLength = (int)((diffCenter/System.Math.Sin(System.Math.Abs(Rotation))) - (cannonSize.Y*73/100f));
            spriteBatch.Draw(_placeholderTexture, new Vector2((int)newX, (int)newY), null, Color.Aqua, Rotation, new Vector2(1/2f,1f), new Vector2(16, cursorLength), SpriteEffects.None, 0f);

            double currentDegree = (Rotation*180)/MathHelper.Pi;
            if (currentDegree < -23 || currentDegree > 23)
            {
                double extendX = newX + System.Math.Sin(Rotation) * (cursorLength);
                double extendY = newY - ((System.Math.Cos(Rotation)) * cursorLength);
                int bounceCursorLength = (int)(Singleton.Instance.GAME_SCREEN_SIZE.X/System.Math.Sin(System.Math.Abs(-Rotation)));
                spriteBatch.Draw(_placeholderTexture, new Vector2((int)extendX, (int)extendY), null, Color.Chartreuse, -Rotation, new Vector2(1/2f,1f), new Vector2(16, bounceCursorLength), SpriteEffects.None, 0f);
            }
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
                string randomColor = Singleton.Instance.BubbleColor[rnd.Next(6)];
                _ammoBubble[tmpX] = new Bubble(randomColor);
            }

            // ? Random special ammo
            for (int tmpX = 0; tmpX < Singleton.Instance.CANNON_SPECIAL_CARTRIDGE_SIZE; tmpX++)
            {
                string randomColor = Singleton.Instance.BubbleColor[rnd.Next(6)];
                _ammoSpecialBubble[tmpX] = new Bubble(randomColor);
            }

            _currentBubble = _ammoBubble[0];
        }
    }
}