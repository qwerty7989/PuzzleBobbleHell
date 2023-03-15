using System.Collections.Generic;
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
        protected Texture2D _baseCannon;
        protected Texture2D _cannonHolder;
        protected Texture2D _placeholderTexture;

        // ? Font
        protected SpriteFont testingFont;

        // ? Objects
        private Bubble[] _ammoBubble;
        private Bubble[] _ammoSpecialBubble;

        // ? Variables
        public Vector2 Position;
        public Vector2 cannonSize;
        public float rotateOrigin;
        public int cursorSize;

        public Bubble currentBubble;
        public double minimumDegree;
        public double maximumDegree;
        public double shootingDegree;
        public int shootingDirection; // -1 Left, 1 Right
        public string[] BubbleColorList;

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
            float posX = (Singleton.Instance.GAME_SCREEN_SIZE.X/2f) + Singleton.Instance.GAME_SCREEN_POSITION.X;
            float posY = (870 + (cannonSize.Y*rotateOrigin));
            rotateOrigin = 73/100f;
            Position = new Vector2(posX, posY);

            shootingDegree = 0;
            minimumDegree = DegreeToRadian(-60);
            maximumDegree = DegreeToRadian(60);
            shootingDirection = 0;

            BubbleColorList = new List<string>(Singleton.Instance.BUBBLE_COLOR_DIC.Values).ToArray();
            currentBubble = new Bubble(posX, posY, Singleton.Instance.BUBBLE_GRID_MARGIN/2, BubbleColorList[RandomNumber(0, BubbleColorList.Length-1)], Singleton.Instance.BUBBLE_SPEED, 0, 0, Position);
            Singleton.Instance._shootingBubble = currentBubble;
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
            currentBubble.LoadContent(Content);
            _baseCannon = this.contentManager.Load<Texture2D>("PlayScene/BaseCannon");
            _cannonHolder = this.contentManager.Load<Texture2D>("PlayScene/CannonHolder");
            _cannon = this.contentManager.Load<Texture2D>("PlayScene/Cannon");
            //_cannon = this.contentManager.Load<Texture2D>("PlayScene/Cannon");
            //Origin = new Vector2(_cannon.Width / 2f, _cannon.Height / 4f * 3f); // ? Center of Width, 3/4 of Height
            //for (int tmpX = 0; tmpX < Singleton.Instance.CANNON_CARTRIDGE_SIZE; tmpX++) { _ammoBubble[tmpX].LoadContent(Content); }
            //for (int tmpX = 0; tmpX < Singleton.Instance.CANNON_SPECIAL_CARTRIDGE_SIZE; tmpX++) { _ammoSpecialBubble[tmpX].LoadContent(Content); }

            // ? Font
            testingFont = contentManager.Load<SpriteFont>("PlayScene/MyFont");
        }

        public void UnloadContent()
        {

        }

        public void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            shootingDegree = shootingDegree + DegreeToRadian(2) * shootingDirection;

            // ? Minimum/Maximum Degree
            if (shootingDegree < minimumDegree) {
                shootingDegree = minimumDegree;
            }
            else if (shootingDegree > maximumDegree) {
                shootingDegree = maximumDegree;
            }

            // ? Left or Right
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                shootingDirection = -1;
            }
            else if (keyboardState.IsKeyDown(Keys.Right))
            {
                shootingDirection = 1;
            }
            else
            {
                shootingDirection = 0;
            }

            if (keyboardState.IsKeyDown(Keys.Space) && Singleton.Instance._shootingBubble.Velocity.X.Equals(0) && Singleton.Instance._shootingBubble.Velocity.Y.Equals(0)) {
                Singleton.Instance._shootingBubble.Velocity.X = (float)(System.Math.Sin(shootingDegree) * Singleton.Instance.BUBBLE_SPEED);
                Singleton.Instance._shootingBubble.Velocity.Y = (float)(-System.Math.Cos(shootingDegree) * Singleton.Instance.BUBBLE_SPEED);
                Singleton.Instance.isShooting = true;
            }


        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // ?  Cannon and Aiming Line
            CannonUI(spriteBatch);
            GuidingLine(spriteBatch);

            //// ? Cartridge Ammo UI
            //CartridgeNormalAmmoUI(spriteBatch);
            //CartridgeSpecialAmmoUI(spriteBatch);
        }


        public double DegreeToRadian(double degree)
        {
            return (degree * MathHelper.Pi) / 180;
        }

        public int RandomNumber(int start, int range)
        {
            System.Random rnd = new System.Random();
            return rnd.Next(range) + start;
        }

        // reset the bubble to shoot to the bottom of the screen










        public void CannonUI(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(_placeholderTexture, Position, null, Color.White, (float)shootingDegree, new Vector2(1/2f,rotateOrigin), cannonSize, SpriteEffects.None, 0f);

            // ? Load cannon with sprite
            spriteBatch.Draw(_cannon, Position, null, Color.White, (float)shootingDegree, new Vector2(_cannon.Width/2f,_cannon.Height*rotateOrigin), 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(_baseCannon, new Vector2(816, 935), null, Color.White);
            spriteBatch.Draw(_cannonHolder, new Vector2((Singleton.Instance.GAME_SCREEN_SIZE.X/2) + Singleton.Instance.GAME_SCREEN_POSITION.X - _cannonHolder.Width/2, 864), null, Color.White);
        }

        public void GuidingLine(SpriteBatch spriteBatch)
        {
            double newX = System.Math.Sin(shootingDegree) * (cannonSize.Y*rotateOrigin) + Position.X;
            double newY = -System.Math.Cos(shootingDegree) * (cannonSize.Y*rotateOrigin) + Position.Y;

            // ? First initial cursor line
            double borderToCannonDistance = Singleton.Instance.GAME_SCREEN_SIZE.X + Singleton.Instance.GAME_SCREEN_POSITION.X - Position.X;

            // ! No margin offset value
            double cursorShowLength = borderToCannonDistance/System.Math.Sin(System.Math.Abs(shootingDegree)) - (cannonSize.Y*rotateOrigin);

            // ? Upper border
            // ! 0.369 ~= 21 in Degree
            if (shootingDegree > -0.369 && shootingDegree < 0.369)
            {
                if (shootingDegree.Equals(0))
                {
                    cursorShowLength = (Singleton.Instance.GAME_SCREEN_POSITION.Y + Position.Y);
                }
                else
                {
                    borderToCannonDistance = newY;
                    cursorShowLength = borderToCannonDistance/System.Math.Cos(System.Math.Abs(shootingDegree)) + (cannonSize.Y*rotateOrigin);
                }
            }

            // ? Draw First Cursor
            spriteBatch.Draw(_placeholderTexture, new Vector2((float)newX, (float)newY), null, Color.Red, (float)shootingDegree, new Vector2(1/2f,1f), new Vector2(cursorSize, (float)cursorShowLength), SpriteEffects.None, 0f);


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

        //public void Initiate()
        //{
        //    // ? Initite Bubble Array
        //    _ammoBubble = new Bubble[Singleton.Instance.CANNON_CARTRIDGE_SIZE];
        //    _ammoSpecialBubble = new Bubble[Singleton.Instance.CANNON_SPECIAL_CARTRIDGE_SIZE];

        //    // ? Random out 3 color ammo
        //    System.Random rnd = new System.Random();
        //    for (int tmpX = 0; tmpX < Singleton.Instance.CANNON_CARTRIDGE_SIZE; tmpX++)
        //    {
        //        string randomColor = Singleton.Instance.BubbleColor[rnd.Next(Singleton.Instance.BubbleColor.Length)];
        //        _ammoBubble[tmpX] = new Bubble(randomColor);
        //    }

        //    // ? Random special ammo
        //    for (int tmpX = 0; tmpX < Singleton.Instance.CANNON_SPECIAL_CARTRIDGE_SIZE; tmpX++)
        //    {
        //        string randomColor = Singleton.Instance.BubbleColor[rnd.Next(Singleton.Instance.BubbleColor.Length)];
        //        _ammoSpecialBubble[tmpX] = new Bubble(randomColor);
        //    }

        //    _currentBubble = _ammoBubble[0];
        //}
    }
}