using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using PuzzleBobbleHell.Objects;
using Microsoft.Xna.Framework.Content;

namespace PuzzleBobbleHell.Manager
{
    public class BubbleManager
    {
        // ? Manager
        private ContentManager contentManager;

        // ? Texture2D
        //private Texture2D _bubblePlacholder;

        // ? Scene Objects
        private Bubble[,] _tableBubble;

        public BubbleManager()
        {
            _tableBubble = new Bubble[(int)Singleton.Instance.BUBBLE_SIZE.X, (int)Singleton.Instance.BUBBLE_SIZE.Y];
            Initiate();
        }

        public void LoadContent(ContentManager Content)
        {
            contentManager = new ContentManager(Content.ServiceProvider, Content.RootDirectory);

            for (int tmpY = 0; tmpY < Singleton.Instance.BUBBLE_SIZE.X; tmpY++)
            {
                for (int tmpX = 0; tmpX < Singleton.Instance.BUBBLE_SIZE.Y; tmpX++)
                {
                    _tableBubble[tmpY,tmpX].LoadContent(Content);
                }
            }
        }
        public void UnloadContent()
        {

        }

        public void Update(GameTime gameTime)
        {
            // ? Check where the Bubble land
            if (Singleton.Instance.isShooting)
            {
                // ? Set the bubble in array



                // ? Check the condition and go boom boom.

            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // ? Draw Shootable Bubble
            if (Singleton.Instance.isShooting)
            {
                // ? Play some animation
            }

            // ? Draw Bubble on the board
            for (int tmpY = 0; tmpY < Singleton.Instance.BUBBLE_SIZE.X; tmpY++)
            {
                for (int tmpX = 0; tmpX < Singleton.Instance.BUBBLE_SIZE.Y; tmpX++)
                {
                    _tableBubble[tmpY,tmpX].Draw(spriteBatch);
                }
            }
        }

        public void Initiate()
        {
            GenerateBubble(0, (int)Singleton.Instance.BUBBLE_SIZE.X - 3);
        }

        public void GenerateBubble(int start, int end)
        {
            System.Random rnd = new System.Random();

            bool isOdd = false;
            bool isEmpty = false;
            for (int tmpY = 0; tmpY < Singleton.Instance.BUBBLE_SIZE.X; tmpY++)
            {
                isEmpty = (tmpY < start || tmpY >= end - 1) ? true : false;
                isOdd = (tmpY % 2 == 1) ? true : false;

                for (int tmpX = 0; tmpX < Singleton.Instance.BUBBLE_SIZE.Y; tmpX++)
                {
                    if (isOdd && tmpX == Singleton.Instance.BUBBLE_SIZE.Y - 1)
                        isEmpty = true;
                    string randomColor = (isEmpty) ? "Black" : Singleton.Instance.BubbleColor[rnd.Next(6)];
                    _tableBubble[tmpY,tmpX] = new Bubble(tmpX, tmpY, isOdd, randomColor, isEmpty);
                }
            }
        }
    }
}
