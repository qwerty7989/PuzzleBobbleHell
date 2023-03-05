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
            _tableBubble = new Bubble[Singleton.Instance.BUBBLE_SIZE_ROW, Singleton.Instance.BUBBLE_SIZE_COLUMN];
            Initiate();
        }

        public void LoadContent(ContentManager Content)
        {
            contentManager = new ContentManager(Content.ServiceProvider, Content.RootDirectory);

            for (int tmpY = 0; tmpY < Singleton.Instance.BUBBLE_SIZE_ROW; tmpY++)
            {
                for (int tmpX = 0; tmpX < Singleton.Instance.BUBBLE_SIZE_COLUMN; tmpX++)
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

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int tmpY = 0; tmpY < Singleton.Instance.BUBBLE_SIZE_ROW; tmpY++)
            {
                for (int tmpX = 0; tmpX < Singleton.Instance.BUBBLE_SIZE_COLUMN; tmpX++)
                {
                    _tableBubble[tmpY,tmpX].Draw(spriteBatch);
                }
            }
        }

        public void Initiate()
        {
            GenerateBubble(0, 7);
        }

        public void GenerateBubble(int start, int end)
        {
            System.Random rnd = new System.Random();

            bool isOdd = false;
            bool isEmpty = false;
            for (int tmpY = 0; tmpY < Singleton.Instance.BUBBLE_SIZE_ROW; tmpY++)
            {
                isEmpty = (tmpY < start || tmpY >= end - 1) ? true : false;
                isOdd = (tmpY % 2 == 1) ? true : false;

                for (int tmpX = 0; tmpX < Singleton.Instance.BUBBLE_SIZE_COLUMN; tmpX++)
                {
                    string randomColor = (isEmpty) ? "Black" : Singleton.Instance.BubbleColor[rnd.Next(6)];
                    _tableBubble[tmpY,tmpX] = new Bubble(tmpX, tmpY, isOdd, randomColor, isEmpty);
                }
            }
        }
    }
}
