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
    public class MenuScene : GameScene
    {
        private ContentManager contentManager;

        private Button playButton;
        private Button optionsButton;
        private Button creditsButton;
        private Button exitButton;
        private Button musicButton;

        private Image logoImage;
        private Image backgroundImage;

        private Rectangle cursorRectangle;
        private Texture2D cursorTexture;
        private MouseState previousMouseState;

        public MenuScene()
        {
            cursorRectangle = new Rectangle(0, 0, 100, 100);
        }

        public void LoadContent(ContentManager Content)
        {
            contentManager = new ContentManager(Content.ServiceProvider, Content.RootDirectory);

            backgroundImage = new Image(new Rectangle(0, 0, Singleton.Instance.widthScreen, Singleton.Instance.heightScreen), contentManager.Load<Texture2D>("MenuScene/background"));
            playButton = new Button(new Rectangle((Singleton.Instance.widthScreen / 2) - 150, 500, 300, 75), contentManager.Load<Texture2D>("MenuScene/play_button"));
            creditsButton = new Button(new Rectangle((Singleton.Instance.widthScreen / 2) - 150, 600, 300, 75), contentManager.Load<Texture2D>("MenuScene/credits_button"));
            optionsButton = new Button(new Rectangle((Singleton.Instance.widthScreen / 2) - 150, 700, 300, 75), contentManager.Load<Texture2D>("MenuScene/options_button"));
            musicButton = new Button(new Rectangle(50, 50, 50, 50), contentManager.Load<Texture2D>("MenuScene/music_button"));
            exitButton = new Button(new Rectangle(Singleton.Instance.widthScreen - 100, 50, 50, 50), contentManager.Load<Texture2D>("MenuScene/exit_button"));
            logoImage = new Image(new Rectangle((Singleton.Instance.widthScreen / 2) - 315, 100, 630, 350), contentManager.Load<Texture2D>("MenuScene/logo"));
            cursorTexture = contentManager.Load<Texture2D>("MenuScene/cursor");

            /* Subscribe Event */
            playButton.OnClicked += PlayButtonAction;
            creditsButton.OnClicked += CreditsButtonAction;
            optionsButton.OnClicked += OptionsButtonAction;
            musicButton.OnClicked += MusicButtonAction;
            exitButton.OnClicked += ExitButtonAction;
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
            playButton.Update(gameTime);
            creditsButton.Update(gameTime);
            optionsButton.Update(gameTime);
            musicButton.Update(gameTime);
            exitButton.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // TODO: draw as list
            backgroundImage.Draw(spriteBatch);
            playButton.Draw(spriteBatch);
            creditsButton.Draw(spriteBatch);
            optionsButton.Draw(spriteBatch);
            musicButton.Draw(spriteBatch);
            exitButton.Draw(spriteBatch);
            logoImage.Draw(spriteBatch);

            spriteBatch.Draw(cursorTexture, cursorRectangle, Color.White);
        }

        private void PlayButtonAction()
        {
            // TODO: add feedback

            Singleton.Instance.sceneManager.changeScene(Manager.SceneManager.SceneName.EndStageScene);
        }

        private void CreditsButtonAction()
        {
            // TODO: add feedback

            Singleton.Instance.sceneManager.changeScene(Manager.SceneManager.SceneName.CreditScene);
        }

        private void OptionsButtonAction()
        {
            // TODO: add feedback

            Singleton.Instance.sceneManager.changeScene(Manager.SceneManager.SceneName.SettingScene);
        }

        private void MusicButtonAction()
        {
            // TODO: add code to handle music button click
        }

        private void ExitButtonAction()
        {
            // TODO: add code to handle exit button click
        }
    }
}
