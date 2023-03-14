using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using PuzzleBobbleHell.Objects;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System.Linq;

namespace PuzzleBobbleHell.Manager
{
    public class BubbleManager
    {
        // ? Manager
        private ContentManager contentManager;

        // ? Texture2D
        //private Texture2D _bubblePlacholder;

        // ? Scene Objects
        private List<Bubble> listBubble;
        public string[] BubbleColorList;

        private List<List<string[]>> stageList = new List<List<string[]>>();
        private List<string[]> stage1_1 = new List<string[]>(){
            new string[]{"R", "R", "Y", "Y", "B", "B", "G", "G", "B", "B", "G", "G"},
            new string[]{"R", "R", "Y", "Y", "B", "B", "G", "B", "B", "G", "G"},
            new string[]{"B", "B", "G", "G", "R", "R", "Y", "Y", "B", "B", "G", "G"},
            new string[]{"B", "G", "G", "R", "R", "Y", "Y", "B", "B", "G", "G"},
            new string[]{"X", "X", "X", "X", "X", "X", "X", "X", "X", "X", "X", "X"},
            new string[]{"X", "X", "X", "X", "X", "X", "X", "X", "X", "X", "X"},
            new string[]{"X", "X", "X", "X", "X", "X", "X", "X", "X", "X", "X", "X"},
            new string[]{"X", "X", "X", "X", "X", "X", "X", "X", "X", "X", "X"},
            new string[]{"X", "X", "X", "X", "X", "X", "X", "X", "X", "X", "X", "X"},
            new string[]{"X", "X", "X", "X", "X", "X", "X", "X", "X", "X", "X"}
        };

        public BubbleManager()
        {
            listBubble = new List<Bubble>();
            BubbleColorList = new List<string>(Singleton.Instance.BUBBLE_COLOR_DIC.Values).ToArray();
            stageList.Add(stage1_1);
            GenerateBubbles();
        }

        public void LoadContent(ContentManager Content)
        {
            contentManager = new ContentManager(Content.ServiceProvider, Content.RootDirectory);

            foreach (Bubble bubble in listBubble)
            {
                bubble.LoadContent(Content);
            }
        }

        public void UnloadContent()
        {

        }

        public void Update(GameTime gameTime)
        {
            if (Singleton.Instance.isShooting)
            {
                Singleton.Instance._shootingBubble.Position.X += Singleton.Instance._shootingBubble.Velocity.X;
                Singleton.Instance._shootingBubble.Position.Y += Singleton.Instance._shootingBubble.Velocity.Y;

                // ? Bounce the Bubble against the Border.
                if (Singleton.Instance._shootingBubble.Position.X - Singleton.Instance.BUBBLE_GRID_MARGIN / 2 < Singleton.Instance.BUBBLE_START_POS.X)
                { // ? Left Border
                    Singleton.Instance._shootingBubble.Position.X = Singleton.Instance.BUBBLE_START_POS.X + Singleton.Instance.BUBBLE_GRID_MARGIN / 2;
                    Singleton.Instance._shootingBubble.Velocity.X *= -1;
                }
                else if (Singleton.Instance._shootingBubble.Position.X + Singleton.Instance.BUBBLE_GRID_MARGIN / 2 > Singleton.Instance.GAME_SCREEN_POSITION.X + Singleton.Instance.GAME_SCREEN_SIZE.X)
                { // ? Right Border
                    Singleton.Instance._shootingBubble.Position.X = (Singleton.Instance.GAME_SCREEN_POSITION.X + Singleton.Instance.GAME_SCREEN_SIZE.X) - Singleton.Instance.BUBBLE_GRID_MARGIN / 2;
                    Singleton.Instance._shootingBubble.Velocity.X *= -1;
                }

                // ? Top Border
                if (Singleton.Instance._shootingBubble.Position.Y - Singleton.Instance.BUBBLE_GRID_MARGIN / 2 < Singleton.Instance.BUBBLE_START_POS.Y)
                {
                    Bubble closestBubble = FindClosestBubble(Singleton.Instance._shootingBubble);
                    handleCollision(closestBubble);
                }

                // ? Collide with other Bubble
                for (int i = 0; i < listBubble.Count; i++)
                {
                    Bubble bubble = listBubble[i];
                    if (bubble.isActive && CheckCollides(Singleton.Instance._shootingBubble, bubble))
                    {
                        Bubble closestBubble = FindClosestBubble(Singleton.Instance._shootingBubble);
                        if (closestBubble == null)
                        {
                            // ? Game Over!
                        }

                        if (closestBubble != null) {
                            handleCollision(closestBubble);
                        }
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Bubble bubble in listBubble)
            {
                if (bubble.isActive)
                {
                    bubble.Draw(spriteBatch);
                }
            }

            if (Singleton.Instance.isShooting)
            {
                Singleton.Instance._shootingBubble.DrawShooting(spriteBatch);
            }
            else
            {
                Singleton.Instance._shootingBubble.DrawShooting(spriteBatch);
            }
        }

        public double DegreeToRadian(double degree)
        {
            return (degree * MathHelper.Pi) / 180;
        }

        public Vector2 RotatePoint(double posX, double posY, double angle)
        {
            double sin = System.Math.Sin(angle);
            double cos = System.Math.Cos(angle);

            posX = (float)((posX * cos) - (posY * sin));
            posY = (float)((posX * sin) + (posY * cos));
            return new Vector2((float)posX, (float)posY);
        }

        public int RandomNumber(int start, int range)
        {
            System.Random rnd = new System.Random();
            return rnd.Next(range) + start;
        }

        public double CalculateDistance(Bubble shootingBubble, Bubble bubbleOnGrid)
        {
            double distanceX = shootingBubble.Position.X - bubbleOnGrid.Position.X;
            double distanceY = shootingBubble.Position.Y - bubbleOnGrid.Position.Y;
            return System.Math.Sqrt((distanceX * distanceX) + (distanceY * distanceY));
        }

        public bool CheckCollides(Bubble shootingBubble, Bubble bubbleOnGrid)
        {
            return CalculateDistance(shootingBubble, bubbleOnGrid) < shootingBubble.radius + bubbleOnGrid.radius;
        }

        public Bubble FindClosestBubble(Bubble shootingBubble, bool activeState = false)
        {
            List<Bubble> closestBubbles = listBubble.FindAll(bubble => bubble.isActive == activeState && CheckCollides(shootingBubble, bubble));

            if (closestBubbles.Count == 0)
            {
                return null;
            }

            List<KeyValuePair<double, Bubble>> closestBubblesWithDistance = new List<KeyValuePair<double, Bubble>>();
            closestBubbles.ForEach(bubbleOnGrid =>
            {
                closestBubblesWithDistance.Add(new KeyValuePair<double, Bubble>(CalculateDistance(shootingBubble, bubbleOnGrid), bubbleOnGrid));
            });
            closestBubblesWithDistance.Sort((x, y) => x.Key.CompareTo(y.Key));

            return closestBubblesWithDistance[0].Value;
        }

        public List<Bubble> FindNeighborBubbles(Bubble bubbleOnGrid)
        {
            List<Bubble> neighbors = new List<Bubble>();

            Vector2[] directions = {
              RotatePoint(Singleton.Instance.BUBBLE_GRID_MARGIN, 0, 0), // right
              RotatePoint(Singleton.Instance.BUBBLE_GRID_MARGIN, 0, DegreeToRadian(60)), // up-right
              RotatePoint(Singleton.Instance.BUBBLE_GRID_MARGIN, 0, DegreeToRadian(120)), // up-left
              RotatePoint(Singleton.Instance.BUBBLE_GRID_MARGIN, 0, DegreeToRadian(180)), // left
              RotatePoint(Singleton.Instance.BUBBLE_GRID_MARGIN, 0, DegreeToRadian(240)), // down-left
              RotatePoint(Singleton.Instance.BUBBLE_GRID_MARGIN, 0, DegreeToRadian(300)) // down-right
            };

            for (int i = 0; i < directions.Length; i++)
            {
                Vector2 direction = directions[i];

                double posX = bubbleOnGrid.Position.X + direction.X;
                double posY = bubbleOnGrid.Position.Y + direction.Y;
                Bubble newBubble = new Bubble(posX, posY, (int)bubbleOnGrid.radius);
                Bubble neighbor = FindClosestBubble(newBubble, true);
                if (neighbor != null && neighbor != bubbleOnGrid && !neighbors.Contains(neighbor))
                {
                    neighbors.Add(neighbor);
                }
            }
            return neighbors;
        }

        public void removeMatch(Bubble targetBubble)
        {
            List<Bubble> matchedBubbles = new List<Bubble>();
            matchedBubbles.Add(targetBubble);

            listBubble.ForEach(bubble => bubble.isProcessed = false);
            targetBubble.isProcessed = true;

            List<Bubble> neighbors = FindNeighborBubbles(targetBubble);
            for (int i = 0; i < neighbors.Count; i++)
            {
                Bubble neighbor = neighbors[i];

                if (!neighbor.isProcessed)
                {
                    neighbor.isProcessed = true;

                    if (neighbor.colorBubble.Equals(targetBubble.colorBubble))
                    {
                        matchedBubbles.Add(neighbor);
                        neighbors = Enumerable.Concat(neighbors, FindNeighborBubbles(neighbor)).ToList();
                    }
                }
            }

            if (matchedBubbles.Count >= 3)
            {
                matchedBubbles.ForEach(bubble => bubble.isActive = false);
            }
        }

        public void dropFloatingBubbles()
        {
            List<Bubble> activeBubbles = listBubble.FindAll(bubble => bubble.isActive);
            activeBubbles.ForEach(bubble => bubble.isProcessed = false);

            List<Bubble> neighbors = activeBubbles.FindAll(bubble => bubble.Position.Y - Singleton.Instance.BUBBLE_GRID_MARGIN <= Singleton.Instance.BUBBLE_START_POS.Y);

            // process all bubbles that form a chain with the ceiling bubbles
            for (int i = 0; i < neighbors.Count; i++)
            {
                Bubble neighbor = neighbors[i];

                if (!neighbor.isProcessed)
                {
                    neighbor.isProcessed = true;
                    neighbors = Enumerable.Concat(neighbors, FindNeighborBubbles(neighbor)).ToList();
                }
            }

            // any bubble that is not isProcessed doesn't touch the ceiling
            activeBubbles.FindAll(bubble => !bubble.isProcessed).ForEach(bubble =>
            {
                bubble.isActive = false;
            });
        }

        public int stageIndexToListIndex()
        {
            int listIndex = ((Singleton.Instance.MAIN_STAGE - 1) * Singleton.Instance.SUB_STAGE_AMOUNT) + (Singleton.Instance.SUB_STAGE - 1);
            return listIndex;
        }

        public void GenerateBubbles()
        {
            List<string[]> currentStage = stageList[stageIndexToListIndex()];
            for (int row = 0; row < Singleton.Instance.BUBBLE_SIZE.Y; row++)
            {
                for (int col = 0; col < Singleton.Instance.BUBBLE_SIZE.X; col++)
                {
                    string color = currentStage[row][col];
                    if (color == "X")
                    {
                        CreateBubble(col * Singleton.Instance.BUBBLE_GRID_MARGIN, row * Singleton.Instance.BUBBLE_GRID_MARGIN, Singleton.Instance.BUBBLE_COLOR_DIC[color]);
                    }
                    else
                    {
                        CreateBubble(col * Singleton.Instance.BUBBLE_GRID_MARGIN, row * Singleton.Instance.BUBBLE_GRID_MARGIN, Singleton.Instance.BUBBLE_COLOR_DIC[color]);
                    }
                }
            }
        }

        public void CreateBubble(double x, double y, string color)
        {
            int row = (int)System.Math.Floor(y / Singleton.Instance.BUBBLE_GRID_MARGIN);
            int col = (int)System.Math.Floor(x / Singleton.Instance.BUBBLE_GRID_MARGIN);

            int startX = (row % 2 == 0) ? 0 : (int)(Singleton.Instance.BUBBLE_GRID_MARGIN / 2);
            int center = Singleton.Instance.BUBBLE_GRID_MARGIN / 2;

            double posX = Singleton.Instance.BUBBLE_START_POS.X + ((Singleton.Instance.BUBBLE_GRID_MARGIN + Singleton.Instance.BUBBLE_GAP) * col) + startX;
            double posY = Singleton.Instance.BUBBLE_START_POS.Y + ((Singleton.Instance.BUBBLE_GRID_MARGIN + Singleton.Instance.BUBBLE_GAP - 4) * row);

            bool isActive = (color != "Black") ? true : false;
            listBubble.Add(new Bubble(posX, posY, Singleton.Instance.BUBBLE_GRID_MARGIN / 2, color, isActive));
        }

        // ? Cannon
        public void getNewBubble()
        {
            Singleton.Instance._shootingBubble.Position.X = Singleton.Instance._shootingBubble.originalPos.X;
            Singleton.Instance._shootingBubble.Position.Y = Singleton.Instance._shootingBubble.originalPos.Y;
            Singleton.Instance._shootingBubble.Velocity.X = Singleton.Instance._shootingBubble.Velocity.Y = 0;

            Singleton.Instance._shootingBubble.colorBubble = BubbleColorList[RandomNumber(0, Singleton.Instance.BUBBLE_COLOR_DIC.Count-1)];
        }

        public void handleCollision(Bubble bubbleOnGrid)
        {
            bubbleOnGrid.colorBubble = Singleton.Instance._shootingBubble.colorBubble;
            bubbleOnGrid.isActive = true;
            getNewBubble();
            removeMatch(bubbleOnGrid);
            dropFloatingBubbles();
        }
    }
}
