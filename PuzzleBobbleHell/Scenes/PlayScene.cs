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
        private Texture2D _gameScreenBackground;
        private Texture2D _gameScreenWater;
        private Texture2D _leftHUDBackground;
        private Texture2D _rightHUDBackground;

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
            Singleton.Instance._cannon = _cannon;
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
            _leftHUDBackground = this.contentManager.Load<Texture2D>("PlayScene/LeftHUDBackground");
            _rightHUDBackground = this.contentManager.Load<Texture2D>("PlayScene/RightHUDBackground");
            _gameScreenBackground = this.contentManager.Load<Texture2D>("PlayScene/PlaySceneGameBackground");
            _gameScreenWater = this.contentManager.Load<Texture2D>("PlayScene/Water");

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
            _cannon.Update(gameTime);
            bubbleManager.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // TODO: Add your drawing code here

            // ? Draw Game Background
            spriteBatch.Draw(_gameScreenBackground, Singleton.Instance.GAME_SCREEN_POSITION, null, Color.White);
            spriteBatch.Draw(_gameScreenWater, new Vector2(Singleton.Instance.GAME_SCREEN_POSITION.X, 992), null, Color.White);

            // ? Draw Sprite
            //spriteBatch.Draw(Texture2D, Vector2, XNA.Color);

            // ? Draw Objects
            bubbleManager.Draw(spriteBatch);
            _cannon.Draw(spriteBatch);

            // ? Draw HUD Background
            spriteBatch.Draw(_leftHUDBackground, Singleton.Instance.HUD_LEFT_SCREEN_POSITION, null, Color.White);
            spriteBatch.Draw(_rightHUDBackground, Singleton.Instance.HUD_RIGHT_SCREEN_POSITION, null, Color.White);
        }
    }
}
