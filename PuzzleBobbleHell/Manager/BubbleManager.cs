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
        public Vector2 shootingBubblePosition;
        public double traverseLength;
        public double initialTraverseLength;

        private int bounceCount;
        private double bounceX;
        private double bounceY;
        private double bounceCursorLength;

        public BubbleManager()
        {
            _tableBubble = new Bubble[(int)Singleton.Instance.BUBBLE_SIZE.Y, (int)Singleton.Instance.BUBBLE_SIZE.X];
            traverseLength = 0f;
            initialTraverseLength = 0f;

            bounceCount = 0;
            bounceX = 0f;
            bounceY = 0f;
            bounceCursorLength = 5f;
            Initiate();
        }

        public void LoadContent(ContentManager Content)
        {
            contentManager = new ContentManager(Content.ServiceProvider, Content.RootDirectory);

            for (int tmpY = 0; tmpY < Singleton.Instance.BUBBLE_SIZE.Y; tmpY++)
            {
                for (int tmpX = 0; tmpX < Singleton.Instance.BUBBLE_SIZE.X; tmpX++)
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
                // ? Calculate Shooting Bubble Position
                //LandingPosition(Singleton.Instance.cannon.Rotation, Singleton.Instance.cannon.Position, Singleton.Instance.cannon.cannonLength);
                BubbleShooting(Singleton.Instance.cannon.Rotation, Singleton.Instance.cannon.Position, Singleton.Instance.cannon.cannonLength, Singleton.Instance.cannon.cursorInitialLength);

                // ? Set the bubble in array
                PlaceBubbleIntoTable();


                // ? Check the condition and go boom boom.

                IsShootingStop();
            }
            else
            {
                shootingBubblePosition = Singleton.Instance.cannon.Position;
                traverseLength = initialTraverseLength = 0f;
                bounceX = bounceY = 0f;
                bounceCount = 0;
                bounceCursorLength = 5f;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // ? Draw Shootable Bubble
            if (Singleton.Instance.isShooting)
            {
                // ? Play some animation
                Singleton.Instance.shootingBubble.DrawShooting(spriteBatch, shootingBubblePosition, Singleton.Instance.cannon.Rotation);
            }

            // ? Draw Bubble on the board
            for (int tmpY = 0; tmpY < Singleton.Instance.BUBBLE_SIZE.Y; tmpY++)
            {
                for (int tmpX = 0; tmpX < Singleton.Instance.BUBBLE_SIZE.X; tmpX++)
                {
                    _tableBubble[tmpY,tmpX].Draw(spriteBatch);
                }
            }
        }

        public void BubbleShooting(float Rotation, Vector2 Position, float cannonLength, double cursorInitialLength)
        {
            if (initialTraverseLength < cursorInitialLength)
            {
                initialTraverseLength += (Singleton.Instance.GAME_SCREEN_SIZE.Y/Singleton.Instance.gameTicksInMilliSec);
                double offsetX = System.Math.Sin(Rotation)*(cannonLength);
                double offsetY = -System.Math.Cos(Rotation)*(cannonLength);

                double newX = System.Math.Sin(Rotation)*(initialTraverseLength) + Position.X + offsetX;
                double newY = -System.Math.Cos(Rotation)*(initialTraverseLength) + Position.Y + offsetY;
                shootingBubblePosition = new Vector2((float)newX, (float)newY);

                bounceX = System.Math.Sin(Rotation) * cursorInitialLength + (System.Math.Sin(Rotation) * cannonLength + Position.X); // ? New base X of bounce line
                bounceY = -System.Math.Cos(Rotation) * cursorInitialLength + (-System.Math.Cos(Rotation) * cannonLength + Position.Y); // ? New base Y of bounce line
            }
            else // ? Bouncing Line
            {
                if (Rotation < -0.369 || Rotation > 0.369) // ? Bouncing
                {
                    bounceCursorLength = (Singleton.Instance.GAME_SCREEN_SIZE.X - (Singleton.Instance.bounceBorderMagin*2))/System.Math.Sin(System.Math.Abs(Rotation));
                    float angleSin = (bounceCount % 2 == 1) ? Rotation : -Rotation;
                    if (traverseLength < bounceCursorLength)
                    {
                        double newX = 0f, newY = 0f;
                        traverseLength += (Singleton.Instance.GAME_SCREEN_SIZE.Y/Singleton.Instance.gameTicksInMilliSec);
                        // ? Calculate bouncing line
                        // ? Bounce Cursor
                        newX = System.Math.Sin(angleSin) * traverseLength + bounceX;
                        newY = -System.Math.Cos(Rotation) * traverseLength + bounceY;
                        shootingBubblePosition = new Vector2((float)newX, (float)newY);
                    }
                    else
                    {
                        bounceX = System.Math.Sin(angleSin) * bounceCursorLength + bounceX;
                        bounceY = -System.Math.Cos(Rotation) * bounceCursorLength + bounceY;
                        bounceCount += 1;
                        traverseLength = 0f;
                    }
                }
            }
        }

        public void PlaceBubbleIntoTable()
        {
            for (int tmpY = (int)Singleton.Instance.BUBBLE_SIZE.Y - 1; tmpY > -1; tmpY--)
            {
                for (int tmpX = 0; tmpX < Singleton.Instance.BUBBLE_SIZE.X; tmpX++)
                {
                    if ((_tableBubble[tmpY, tmpX].isEmpty) || (tmpX == Singleton.Instance.BUBBLE_SIZE.X - 1 && tmpY % 2 == 1))
                        continue;

                    if (Singleton.Instance.shootingBubble.checkCollision(_tableBubble[tmpY, tmpX], shootingBubblePosition, _tableBubble))
                    {
                        Singleton.Instance.isShooting = false;
                        break;
                    }
                }
            }
        }

        public void IsShootingStop()
        {
            if (!((shootingBubblePosition.X > Singleton.Instance.GAME_SCREEN_POSITION.X && shootingBubblePosition.X < Singleton.Instance.GAME_SCREEN_POSITION.X + Singleton.Instance.GAME_SCREEN_SIZE.X) && (shootingBubblePosition.Y > Singleton.Instance.GAME_SCREEN_POSITION.Y - Singleton.Instance.bounceBorderMagin && shootingBubblePosition.Y < Singleton.Instance.GAME_SCREEN_POSITION.Y + Singleton.Instance.GAME_SCREEN_SIZE.Y)))
            {
                Singleton.Instance.isShooting = false;
            }
        }

        public void Initiate()
        {
            GenerateBubble(0, (int)Singleton.Instance.BUBBLE_SIZE.Y - 2);
        }

        public void GenerateBubble(int start, int end)
        {
            System.Random rnd = new System.Random();

            bool isOdd = false;
            bool isEmpty = false;
            for (int tmpY = 0; tmpY < Singleton.Instance.BUBBLE_SIZE.Y; tmpY++)
            {
                isEmpty = (tmpY < start || tmpY >= end - 1) ? true : false;
                isOdd = (tmpY % 2 == 0) ? true : false;

                for (int tmpX = 0; tmpX < Singleton.Instance.BUBBLE_SIZE.X; tmpX++)
                {
                    if (isOdd && tmpX == Singleton.Instance.BUBBLE_SIZE.X - 1)
                        isEmpty = true;
                    string randomColor = (isEmpty) ? "Black" : Singleton.Instance.BubbleColor[rnd.Next(Singleton.Instance.BubbleColor.Length)];
                    _tableBubble[tmpY,tmpX] = new Bubble(tmpX, tmpY, isOdd, randomColor, isEmpty);
                }
            }
        }
    }
}
