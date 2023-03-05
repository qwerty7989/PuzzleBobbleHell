using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PuzzleBobbleHell.Objects;

namespace PuzzleBobbleHell.Scenes
{
    public class EndStageScene : GameScene
    {

        private ContentManager contentManager;

        private Button nextButton;

        private Image backgroundImage;

        private Rectangle cursorRectangle;
        private Texture2D cursorTexture;
        private MouseState previousMouseState;

        public EndStageScene()
        {
            cursorRectangle = new Rectangle(0, 0, 100, 100);
        }

        public void LoadContent(ContentManager Content)
        {
            contentManager = new ContentManager(Content.ServiceProvider, Content.RootDirectory);

            backgroundImage = new Image(new Rectangle(0, 0, Singleton.Instance.widthScreen, Singleton.Instance.heightScreen), contentManager.Load<Texture2D>("MenuScene/background"));
            nextButton = new Button(new Rectangle((Singleton.Instance.widthScreen / 2) - 150, 200, 300, 75), contentManager.Load<Texture2D>("MenuScene/play_button"));
            cursorTexture = contentManager.Load<Texture2D>("MenuScene/cursor");

            /* Subscribe Event */
            nextButton.OnClicked += NextButtonAction;
        }

        public void UnloadContent()
        {
            contentManager.Unload();
        }

        public void Update(GameTime gameTime)
        {
            /* Cursor Handle */
            MouseState currentMouseState = Mouse.GetState();
            cursorRectangle.X = currentMouseState.X;
            cursorRectangle.Y = currentMouseState.Y;
            previousMouseState = currentMouseState;

            // TODO: update as list
            nextButton.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // TODO: draw as list
            backgroundImage.Draw(spriteBatch);
            nextButton.Draw(spriteBatch);

            spriteBatch.Draw(cursorTexture, cursorRectangle, Color.White);
        }

        private void NextButtonAction()
        {
            // TODO: add feedback

            Singleton.Instance.sceneManager.changeScene(Manager.SceneManager.SceneName.EndGameScene);
        }
    }
}
