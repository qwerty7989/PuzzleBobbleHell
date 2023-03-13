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

        public string colorBubble { set; get; }
        protected Texture2D _bubbleColor;

        public Vector2 Position;
        public float Rotation;
        public Vector2 Origin;
        public Vector2 Scale;
        public string bubbleType;
        public bool isEmpty { set; get; }
        private bool isOdd;
        private int arrX;
        private int arrY;
        private bool isShootable;

        // ? Check Collision stuff
        private Bubble lastBubble;
        private Bubble closestBubble;
        private double closestDistance;
        private bool doneShooting;

        // ? Ammo Bubble
        public Bubble(string randomColor)
        {
            colorBubble = randomColor;
            isShootable = true;
            closestDistance = 99999;
            doneShooting = false;
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
                //spriteBatch.Draw(_bubbleColor, Position, null, Color.LightGray, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
            else
            {
                spriteBatch.Draw(_bubbleColor, Position, null, Color.LightGray, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
        }

        public bool checkCollision(Bubble bubbleOnTable, Vector2 shootingBubblePosition, Bubble[,] tableBubble)
        {
            double centerXShooting = shootingBubblePosition.X + (_bubbleColor.Width / 2.0);
            double centerYShooting = shootingBubblePosition.Y + (_bubbleColor.Height / 2.0);
            double centerX = bubbleOnTable.Position.X + (_bubbleColor.Width / 2.0);
            double centerY = bubbleOnTable.Position.Y + (_bubbleColor.Height / 2.0);

            double distance = System.Math.Sqrt(System.Math.Pow((centerX-centerXShooting), 2) + System.Math.Pow((centerY-centerYShooting), 2));
            if (distance < _bubbleColor.Width*2/4.0 && !doneShooting)
            {
                if (centerXShooting >= centerX)
                {
                    if (bubbleOnTable.isOdd)
                    {
                        lastBubble = tableBubble[bubbleOnTable.arrY+1,bubbleOnTable.arrX+1];
                    }
                    else
                    {
                        if (bubbleOnTable.arrX == Singleton.Instance.BUBBLE_SIZE.X - 1)
                            lastBubble = tableBubble[bubbleOnTable.arrY+1,bubbleOnTable.arrX-1];
                        else
                            lastBubble = tableBubble[bubbleOnTable.arrY+1,bubbleOnTable.arrX];
                    }
                    doneShooting = true;
                }
                else // centerXShooting < centerX
                {
                    if (bubbleOnTable.isOdd)
                    {
                        lastBubble = tableBubble[bubbleOnTable.arrY+1,bubbleOnTable.arrX];
                    }
                    else
                    {
                        if (bubbleOnTable.arrX == 0)
                            lastBubble = tableBubble[bubbleOnTable.arrY+1,bubbleOnTable.arrX];
                        else
                            lastBubble = tableBubble[bubbleOnTable.arrY+1,bubbleOnTable.arrX-1];

                    }
                    doneShooting = true;
                }
                closestDistance = distance;
                lastBubble._bubbleColor = Singleton.Instance.shootingBubble._bubbleColor;
                lastBubble.isEmpty = false;
                doneShooting = false;
                return true;
            }

            return false;
        }

        public void CopyBubble(Bubble shootingBubble)
        {
            colorBubble = shootingBubble.colorBubble;
            isEmpty = false;
            isShootable = false;
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