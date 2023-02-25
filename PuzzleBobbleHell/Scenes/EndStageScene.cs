using System;
using System.Drawing;
using System.Numerics;
using System.Reflection.Metadata;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PuzzleBobbleHell.Manager;

namespace PuzzleBobbleHell.Scenes
{
    public class EndStageScene : GameScene
    {
        private ContentManager contentManager;
        private int playerHealth = 3; // ? Starting health with 3 hearts.
        private int bossHealth = -1; // ? -1, mean there's no boss existed. 
        private SpriteFont font; // ? Declare a variable to hold the font

        private SceneManager _sceneManager;

        public EndStageScene()
        {

        }
        public void LoadContent(ContentManager Content)
        {
            contentManager = new ContentManager(Content.ServiceProvider, Content.RootDirectory);

            // TODO: Load the font in LoadContent method
            font = contentManager.Load<SpriteFont>("Fonts/MyFont");

            // TODO: use this.Content to load your game content here
            /*
            // ? Textures
            _line = this.contentManager.Load<Texture2D>("Line");
            _cross = this.contentManager.Load<Texture2D>("Cross");
            _circle = this.contentManager.Load<Texture2D>("Circle");
            
            // ? Fonts
            _fontTerminal = this.contentManager.Load<SpriteFont>("Fonts/Terminal");
            
            // ? Sounds
			_exampleSound = content.Load<SoundEffect>("Audios/ExampleSound").CreateInstance();
            // ? Effects
            */
        }

        public void UnloadContent()
        {
            contentManager.Unload();
        }

        public void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                _sceneManager.changeScene(SceneManager.SceneName.EndStageScene);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // TODO: Add your drawing code here
            spriteBatch.DrawString(font, "Congratulations! You have completed the game!", new Microsoft.Xna.Framework.Vector2(0, 0), Microsoft.Xna.Framework.Color.Black);
        }
    }
}
