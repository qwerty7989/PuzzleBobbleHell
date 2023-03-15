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
using System.Linq.Expressions;


namespace PuzzleBobbleHell.Scenes
{
    public class StoryScene : GameScene
    {
        private ContentManager contentManager;

        private Image backgroundImage;

        private Button messageBox;
        private SpriteFont font;

        private Rectangle cursorRectangle;
        private Texture2D cursorTexture;
        private MouseState previousMouseState;

        private MouseState _currentMouseState;
        private MouseState _previousMouseState;

        private int cntDialog = 0;
        private String _Dialog = "";

        public StoryScene()
        {
            cursorRectangle = new Rectangle(0, 0, 100, 100);
        }

        public void LoadContent(ContentManager Content)
        {
            contentManager = new ContentManager(Content.ServiceProvider, Content.RootDirectory);

            backgroundImage = new Image(new Rectangle(0, 0, Singleton.Instance.widthScreen, Singleton.Instance.heightScreen), contentManager.Load<Texture2D>("MenuScene/background"));
            messageBox = new Button(new Rectangle(30, Singleton.Instance.heightScreen - (Singleton.Instance.heightScreen / 4), Singleton.Instance.widthScreen - 60, 210), contentManager.Load<Texture2D>("StoryScene/bgTextBox"));
            font = contentManager.Load<SpriteFont>("StoryScene/File");

            cursorTexture = contentManager.Load<Texture2D>("MenuScene/cursor");
            _currentMouseState = Mouse.GetState();
            _previousMouseState = _currentMouseState;

            //event
            messageBox.OnClicked += updateDialog;
        }
        public void UnloadContent()
        {
            contentManager.Unload();
        }

        public void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here

            messageBox.Update(gameTime);

            /* Cursor Handle */
            MouseState currentMouseState = Mouse.GetState();
            cursorRectangle.X = currentMouseState.X;
            cursorRectangle.Y = currentMouseState.Y;
            previousMouseState = currentMouseState;

            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // TODO: Add your drawing code here
            backgroundImage.Draw(spriteBatch);

            spriteBatch.Draw(cursorTexture, cursorRectangle, Color.White);
            messageBox.Draw(spriteBatch);
            spriteBatch.DrawString(font, _Dialog, new Vector2(100, Singleton.Instance.heightScreen - (Singleton.Instance.heightScreen / 4) + 40), Color.White);

        }

        private void updateDialog()
        {
            if (_previousMouseState.LeftButton == ButtonState.Released && _currentMouseState.LeftButton == ButtonState.Pressed)
            {
                cntDialog += 1;
                switch (cntDialog)
                {
                    case 1:
                        _Dialog = "In a village far away";
                        break;
                    case 2:
                        _Dialog = "There was a legendary pirate.";
                        break;
                    case 3:
                        _Dialog = "legendary pirate died our adventures of our lives and the waiting family at home.";
                        break;
                    case 4:
                        _Dialog = "A single mother in an era when pirates were few and far between.";
                        break;
                    case 5:
                        _Dialog = "The wife left her two young children in an old house by the sea.";
                        break;
                    case 6:
                        _Dialog = "The little boy grew up with a love for his two brothers.";
                        break;
                    case 7:
                        _Dialog = "The eldest does everything from working on a pirate crew, selling goods, or doing other miscellaneous work to support his little sister.";
                        break;
                    case 8:
                        _Dialog = "But then one day.";
                        break;
                    case 9:
                        _Dialog = "His sister disappeared from the house when he returned from work outside.";
                        break;
                    case 10:
                        _Dialog = "There was only a trace of something terrifying disappearing into the vast sea.";
                        break;
                    case 11:
                        _Dialog = "The protagonist decides to run to his pirate friend.";
                        break;
                    case 12:
                        _Dialog = "He and his friend steal two pirate ships.";
                        break;
                    case 13:
                        _Dialog = "The memories of his father's existence were still vivid. The warm hand that held his hand while driving the big boat.";
                        break;
                    case 14:
                        _Dialog = "He and his friend steal two pirate ships.";
                        break;
                    case 15:
                        _Dialog = "And the proud smile that was sent to him after he had tried firing the cannon, the weapon of the proud pirates, on the colossal ship.";
                        break;
                    case 16:
                        _Dialog = "The protagonist sails in the same direction as the trace of the monster. Along the way, there are many obstacles...";
                        break;
                    default:
                        Singleton.Instance.sceneManager.changeScene(Manager.SceneManager.SceneName.LoadingScene);
                        break;
                }
            }
        }

    }
}
