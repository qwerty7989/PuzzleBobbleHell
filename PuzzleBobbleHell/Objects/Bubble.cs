using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PuzzleBobbleHell.Manager;

namespace PuzzleBobbleHell.Objects
{
    public class Bubble : GameObject
   {
        private ContentManager contentManager;

        protected string colorBubble;
        protected Texture2D _bubbleColor;

        public Vector2 Position;
        public float Rotation;
        public Vector2 Origin;
        public Vector2 Scale;
        public string bubbleType;
        private bool isOdd;
        private bool isEmpty;
        private int arrX;
        private int arrY;
        private bool isShootable;

        // ? Ammo Bubble
        public Bubble(string randomColor)
        {
            colorBubble = randomColor;
            isShootable = true;
        }

        // ? Bubble
        public Bubble(int tmpX, int tmpY, bool isOdd, string randomColor, bool isEmpty)
        {
            int posX = Singleton.Instance.BUBBLE_START_X + (Singleton.Instance.BUBBLE_MARGIN_WIDTH * tmpX);
            int posY = Singleton.Instance.BUBBLE_START_Y + (Singleton.Instance.BUBBLE_MARGIN_HEIGHT * tmpY);

            posX += (isOdd) ? Singleton.Instance.BUBBLE_ODD_ROW_MARGIN : 0;

            Position = new Vector2(posX, posY);
            this.arrX = tmpX;
            this.arrY = tmpY;
            this.isOdd = isOdd;
            colorBubble = randomColor;
            this.isEmpty = isEmpty;
            isShootable = false;
        }

        public void LoadContent(ContentManager Content)
        {
            contentManager = new ContentManager(Content.ServiceProvider, Content.RootDirectory);

            // ? Load Bubble Color
            string filePath = "PlayScene/Bubble" + colorBubble;
            _bubbleColor = this.contentManager.Load<Texture2D>(filePath);
        }

        public void UnloadContent()
        {

        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isEmpty)
            {
                spriteBatch.Draw(_bubbleColor, Position, null, Color.LightGray);
            }
            else
            {
                spriteBatch.Draw(_bubbleColor, Position, null, Color.LightGray);
            }
        }

        public void DrawAmmo(SpriteBatch spriteBatch, Vector2 AmmoPosition)
        {
            spriteBatch.Draw(_bubbleColor, AmmoPosition, null, Color.LightGray);
        }
    }
}