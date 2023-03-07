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
        // ? Manager
        private ContentManager contentManager;
        private BubbleManager bubbleManager;

        // ? Texture2D
        private Texture2D _gameBackgroundPlaceholder;

        // ? Scene Objects
        private Cannon _cannon;

        // ? Gameplay variable
        private int playerHealth = 3; // ? Starting health with 3 hearts.
        private int bossHealth = -1; // ? -1, mean there's no boss existed.

        public PlayScene()
        {
            // ? Initiate Bubble Manager
            bubbleManager = new BubbleManager();

            // ? Initiate Objects
            _cannon = new Cannon();
        }

        public void LoadContent(ContentManager Content)
        {
            contentManager = new ContentManager(Content.ServiceProvider, Content.RootDirectory);

            // ? Shapes
            _gameBackgroundPlaceholder = new Texture2D(Singleton.Instance.graphicsDeviceManager.GraphicsDevice, 1, 1);
            Color[] data = new Color[1 * 1];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.White;
            _gameBackgroundPlaceholder.SetData(data);

            // ? Textures
            //_bubblePlacholder = this.contentManager.Load<Texture2D>("PlayScene/BubbleRed");

            // ? Fonts
            //_fontTerminal = this.contentManager.Load<SpriteFont>("Fonts/Terminal");

            // ? Sounds
            //_exampleSound = content.Load<SoundEffect>("Audios/ExampleSound").CreateInstance();

            // ? Effects


            // ? Load Objects Content
            bubbleManager.LoadContent(Content);
            _cannon.LoadContent(Content);
        }

        public void UnloadContent()
        {
            contentManager.Unload();
            bubbleManager.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here

            KeyboardState keyboardState = Keyboard.GetState();

            // ? Exit game with Escape
            if (keyboardState.IsKeyDown(Keys.Escape))
                Singleton.Instance.isExitGame = true;

            // ? Update Objects
            bubbleManager.Update(gameTime);
            _cannon.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // TODO: Add your drawing code here

            // ? Draw Background
            spriteBatch.Draw(_gameBackgroundPlaceholder, Singleton.Instance.HUD_LEFT_SCREEN_POSITION, null, Color.OldLace, 0f, Vector2.Zero, Singleton.Instance.HUD_LEFT_SCREEN_SIZE, SpriteEffects.None, 0);
            spriteBatch.Draw(_gameBackgroundPlaceholder, Singleton.Instance.HUD_RIGHT_SCREEN_POSITION, null, Color.OldLace, 0f, Vector2.Zero, Singleton.Instance.HUD_RIGHT_SCREEN_SIZE, SpriteEffects.None, 0);
            spriteBatch.Draw(_gameBackgroundPlaceholder, Singleton.Instance.GAME_SCREEN_POSITION, null, Color.LightGray, 0f, Vector2.Zero, Singleton.Instance.GAME_SCREEN_SIZE, SpriteEffects.None, 0);

            // ? Draw Sprite
            //spriteBatch.Draw(Texture2D, Vector2, XNA.Color);


            // ? Draw Objects
            _cannon.Draw(spriteBatch);
            bubbleManager.Draw(spriteBatch);
        }
    }
}
