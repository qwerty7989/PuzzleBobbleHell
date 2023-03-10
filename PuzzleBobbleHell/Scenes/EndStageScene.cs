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
        int Score = Singleton.Instance.Score;

        private ContentManager contentManager;

        private Button playButton;
        private Button nextButton;

        private Image backgroundImage;
        private Image starfishImage;

        private SpriteFont textFont;
        private string paragraph;
        private string score;

        private Rectangle cursorRectangle;
        private Texture2D cursorTexture;
        private MouseState previousMouseState;

        public EndStageScene()
        {
            cursorRectangle = new Rectangle(0, 0, 100, 100);

            paragraph = "SCORE";
            score = Score.ToString();
        }

        public void LoadContent(ContentManager Content)
        {
            contentManager = new ContentManager(Content.ServiceProvider, Content.RootDirectory);

            backgroundImage = new Image(new Rectangle(0, 0, Singleton.Instance.widthScreen, Singleton.Instance.heightScreen), contentManager.Load<Texture2D>("EndStageScene/background1"));

            playButton = new Button(new Rectangle((Singleton.Instance.widthScreen / 2) -200, 600, 75, 75), contentManager.Load<Texture2D>("MenuScene/play_button"));
            nextButton = new Button(new Rectangle((Singleton.Instance.widthScreen / 2) -100, 600, 300, 75), contentManager.Load<Texture2D>("MenuScene/play_button"));
            textFont = contentManager.Load<SpriteFont>("Font/Pixel");
            cursorTexture = contentManager.Load<Texture2D>("MenuScene/cursor");

            /* Subscribe Event */
            playButton.OnClicked += PlayButtonAction;
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
            playButton.Update(gameTime);
            nextButton.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            backgroundImage.Draw(spriteBatch);

            Texture2D starfishTexture;
            if (int.TryParse(score.Replace(",", ""), out int scoreValue))
            {
                if (scoreValue < 5000)
                    starfishTexture = contentManager.Load<Texture2D>("EndStageScene/starfish0");
                else if (scoreValue < 8000)
                    starfishTexture = contentManager.Load<Texture2D>("EndStageScene/starfish1");
                else if (scoreValue < 10000)
                    starfishTexture = contentManager.Load<Texture2D>("EndStageScene/starfish2");
                else
                    starfishTexture = contentManager.Load<Texture2D>("EndStageScene/starfish3");
            }
            else
            {
                starfishTexture = contentManager.Load<Texture2D>("EndStageScene/starfish2");
            }

            starfishImage = new Image(new Rectangle((Singleton.Instance.widthScreen / 2) - 250, 180, 500, 250), starfishTexture);
            starfishImage.Draw(spriteBatch);

            playButton.Draw(spriteBatch);
            nextButton.Draw(spriteBatch);

            Vector2 paragraphPosition = new Vector2((Singleton.Instance.widthScreen / 2) - (textFont.MeasureString(paragraph).X / 2), (Singleton.Instance.heightScreen / 2) - (textFont.MeasureString(paragraph).Y / 2));
            spriteBatch.DrawString(textFont, paragraph, paragraphPosition, Color.White);

            Vector2 scorePosition = new Vector2((Singleton.Instance.widthScreen / 2) - (textFont.MeasureString(score).X / 2), (Singleton.Instance.heightScreen / 2) + 50);
            spriteBatch.DrawString(textFont, score, scorePosition, Color.White);

            spriteBatch.Draw(cursorTexture, cursorRectangle, Color.White);
        }

        private void PlayButtonAction()
        {
            // TODO: add feedback

            Singleton.Instance.sceneManager.changeScene(Manager.SceneManager.SceneName.MenuScene);
        }

        private void NextButtonAction()
        {
            // TODO: add feedback

            Singleton.Instance.sceneManager.changeScene(Manager.SceneManager.SceneName.EndGameScene);
        }
    }
}
