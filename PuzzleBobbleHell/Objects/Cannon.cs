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
        private Bubble[] _ammoBubble;

        // ? Variables
        public Vector2 Position;
        public float Rotation;
        public Vector2 Origin;
        public Vector2 Scale;

        public Cannon()
        {
            // ? 640,960
            // ? 704,960
            Position = new Vector2(576 + 64, 768 + 192);
            Rotation = 0f;

            _ammoBubble = new Bubble[Singleton.Instance.CANNON_CARTRIDGE_SIZE];
            Initiate();
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
            _cannon = this.contentManager.Load<Texture2D>("PlayScene/Cannon");
            Origin = new Vector2(_cannon.Width / 2f, _cannon.Height / 4f * 3f); // ? Center of Width, 3/4 of Height
            for (int tmpX = 0; tmpX < Singleton.Instance.CANNON_CARTRIDGE_SIZE; tmpX++)
            {
                _ammoBubble[tmpX].LoadContent(Content);
            }

            // ? Font
            testingFont = contentManager.Load<SpriteFont>("PlayScene/MyFont");
        }
        public void UnloadContent()
        {

        }
        public void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            // ? Rotate to the left
            if (keyboardState.IsKeyDown(Keys.Left) && Rotation > -(5*MathHelper.Pi/12))
                Rotation -= MathHelper.Pi / 180;

            // ? Rotate to the right
            if (keyboardState.IsKeyDown(Keys.Right) && Rotation < (5*MathHelper.Pi/12))
                Rotation += MathHelper.Pi / 180;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(_cannon, Position, null, Color.White, Rotation, Origin, 1f, SpriteEffects.None, 0f);
            CartridgeUI(spriteBatch);
            GuidingLine(spriteBatch);
            TestingCannon(spriteBatch);
        }

        public void TestingCannon(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_placeholderTexture, Position, null, Color.White, Rotation, new Vector2(1/2f,3/4f), new Vector2(96, 192), SpriteEffects.None, 0f);
        }

        public void GuidingLine(SpriteBatch spriteBatch)
        {
            double rawRadian = Rotation;
            double currentDegree = (Rotation*180)/MathHelper.Pi;
            double newX = System.Math.Sin(rawRadian) * 144 + Position.X;
            double offsetY = (rawRadian == 0) ? 0 : 144;
            double newY = (1-System.Math.Cos(rawRadian)) * 144 + Position.Y - offsetY;

            spriteBatch.DrawString(testingFont, "Rotation: " + currentDegree, new Vector2(800, 1000), Color.Black);
            spriteBatch.DrawString(testingFont, "Radians: " + rawRadian, new Vector2(800, 1025), Color.Black);
            spriteBatch.DrawString(testingFont, "New X: " + newX, new Vector2(800, 950), Color.Black);
            spriteBatch.DrawString(testingFont, "New Y: " + newY, new Vector2(800, 975), Color.Black);

            int diffCenter = (int)(Singleton.Instance.GAME_SCREEN_SIZE.X + Singleton.Instance.GAME_SCREEN_POSITION.X - Position.X);
            int cursorLength = (int)(diffCenter/System.Math.Sin(System.Math.Abs(rawRadian)));
            cursorLength -= 144;
            spriteBatch.Draw(_placeholderTexture, new Vector2((int)newX, (int)newY), null, Color.Aqua, Rotation, new Vector2(1/2f,1f), new Vector2(16, cursorLength), SpriteEffects.None, 0f);

            if (currentDegree < -23 || currentDegree > 23)
            {
                double extendX = newX + System.Math.Sin(rawRadian) * (cursorLength);
                double extendY = newY - ((System.Math.Cos(rawRadian)) * cursorLength);
                int bounceCursorLength = (int)(Singleton.Instance.GAME_SCREEN_SIZE.X/System.Math.Sin(System.Math.Abs(-rawRadian)));
                spriteBatch.Draw(_placeholderTexture, new Vector2((int)extendX, (int)extendY), null, Color.Chartreuse, -Rotation, new Vector2(1/2f,1f), new Vector2(16, bounceCursorLength), SpriteEffects.None, 0f);
            }
        }

        public void CartridgeUI(SpriteBatch spriteBatch)
        {
            for (int tmpX = 0; tmpX < Singleton.Instance.CANNON_CARTRIDGE_SIZE; tmpX++)
            {
                _ammoBubble[tmpX].DrawAmmo(spriteBatch, new Vector2(274 + (tmpX * 64), 960));
            }
        }

        public void Initiate()
        {
            System.Random rnd = new System.Random();
            for (int tmpX = 0; tmpX < Singleton.Instance.CANNON_CARTRIDGE_SIZE; tmpX++)
            {
                string randomColor = Singleton.Instance.BubbleColor[rnd.Next(6)];
                _ammoBubble[tmpX] = new Bubble(randomColor);
            }
        }
    }
}