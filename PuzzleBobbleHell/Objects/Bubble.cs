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

        public string colorBubble { get; }
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
        public Bubble(int tmpX, int tmpY, bool isOdd, string colorBubble, bool isEmpty)
        {
            int posX = (int)(Singleton.Instance.BUBBLE_START_POS.X + (Singleton.Instance.BUBBLE_MARGIN.X * tmpX));
            int posY = (int)(Singleton.Instance.BUBBLE_START_POS.Y + (Singleton.Instance.BUBBLE_MARGIN.Y * tmpY));

            posX += (isOdd) ? Singleton.Instance.BUBBLE_ODD_ROW_MARGIN : 0;

            Position = new Vector2(posX, posY);
            this.arrX = tmpX;
            this.arrY = tmpY;
            this.isOdd = isOdd;
            this.colorBubble = colorBubble;
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
                spriteBatch.Draw(_bubbleColor, Position, null, Color.LightGray, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
            else
            {
                spriteBatch.Draw(_bubbleColor, Position, null, Color.LightGray, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
        }

        public void DrawAmmo(SpriteBatch spriteBatch, Vector2 AmmoPosition)
        {
            spriteBatch.Draw(_bubbleColor, AmmoPosition, null, Color.LightGray, 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);
        }

        public void DrawShooting(SpriteBatch spriteBatch, Vector2 AmmoPosition, float Rotation)
        {
            spriteBatch.Draw(_bubbleColor, AmmoPosition, null, Color.LightGray, Rotation, new Vector2((_bubbleColor.Width) / 2f, (_bubbleColor.Height) / 2f), 1f, SpriteEffects.None, 0f);
        }
    }
}