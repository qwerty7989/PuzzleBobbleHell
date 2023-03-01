using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PuzzleBobbleHell.Manager;
using PuzzleBobbleHell.Objects;

namespace PuzzleBobbleHell.Scenes
{
    public class PlayScene : GameScene
    {
        private ContentManager contentManager;

        private Texture2D _rectTexture;

        // ? Scene Objects
        private Cannon _cannon;

        // ? Background
        private Vector2 gameBackgroundSize;
        private Vector2 gameForegroundSize;
        private Vector2 HUDBackgroundSize;

        // ? Gameplay variable
        private int playerHealth = 3; // ? Starting health with 3 hearts.
        private int bossHealth = -1; // ? -1, mean there's no boss existed.

        public PlayScene()
        {
            _cannon = new Cannon();
            gameBackgroundSize = new Vector2(1280, 1080);
            gameForegroundSize = new Vector2(768, 1048);
            HUDBackgroundSize = new Vector2(640, 1080);
        }

        public void LoadContent(ContentManager Content)
        {
            contentManager = new ContentManager(Content.ServiceProvider, Content.RootDirectory);

            // TODO: use this.Content to load your game content here

            // ? Shapes
            _rectTexture = new Texture2D(Singleton.Instance.graphicsDeviceManager.GraphicsDevice, 1, 1);
            Color[] data = new Color[1 * 1];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.White;
            _rectTexture.SetData(data);

            // ? Textures

            // ? Fonts
            //_fontTerminal = this.contentManager.Load<SpriteFont>("Fonts/Terminal");

            // ? Sounds
            //_exampleSound = content.Load<SoundEffect>("Audios/ExampleSound").CreateInstance();
            // ? Effects

            // ? Load Objects Content
            _cannon.LoadContent(Content);
        }

        public void UnloadContent()
        {
            contentManager.Unload();
        }

        public void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here

            KeyboardState keyboardState = Keyboard.GetState();

            // ? Exit game with Escape
            if (keyboardState.IsKeyDown(Keys.Escape))
                Singleton.Instance.isExitGame = true;

            // ? Update Objects
            _cannon.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // TODO: Add your drawing code here

            // ? Draw Background
            spriteBatch.Draw(_rectTexture, new Vector2(1280, 0), null, Color.DimGray, 0f, Vector2.Zero, HUDBackgroundSize, SpriteEffects.None, 0);
            spriteBatch.Draw(_rectTexture, new Vector2(0, 0), null, Color.OldLace, 0f, Vector2.Zero, gameBackgroundSize, SpriteEffects.None, 0);
            spriteBatch.Draw(_rectTexture, new Vector2(256, 16), null, Color.LightGray, 0f, Vector2.Zero, gameForegroundSize, SpriteEffects.None, 0);

            // ? Draw Sprite
            //spriteBatch.Draw(Texture2D, Vector2, XNA.Color);


            // ? Draw Objects
            _cannon.Draw(spriteBatch);
        }
    }
}
