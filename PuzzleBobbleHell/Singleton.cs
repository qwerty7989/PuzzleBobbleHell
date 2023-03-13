using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PuzzleBobbleHell.Manager;
using PuzzleBobbleHell.Objects;

namespace PuzzleBobbleHell
{
    /// <summary>
    ///
    /// <example>
    /// For example:
    /// <code>
    /// finalScore = Singleton.Instance.Score;
    /// </code>
    /// Result in accessing the Score variable in Singleton Instance.
    /// And this instance is the only one throughout the game cycle.
    /// </example>
    /// </summary>
    public class Singleton
    {
        // ? System-related
        public int heightScreen = 1080;
        public int widthScreen = 1920;
        public bool isExitGame = false;
        public double gameTicksInMilliSec = 87f;
        public double swapDelayInMilliSec = 213f;

        // ? Graphic Manager
        public GraphicsDeviceManager graphicsDeviceManager;

        // ? ContentManager
        public ContentManager contentManager;

        // ? SceneManager
        public SceneManager sceneManager = new SceneManager();


        // ? PlayScene
        public int score = 0;
        public int mainStage = 1; // ? The main stage number (1)-1
        public int subStage = 1; // ? The sub stage number 1-(1)
        public bool isShooting = false;
        public Bubble shootingBubble;
        public string[] BubbleColor = {
            "Blue",
            "Cyan",
            "Green",
            "Red",
            "Yellow"
        };
        public Cannon cannon;
        public int CANNON_CARTRIDGE_SIZE = 4;
        public int CANNON_SPECIAL_CARTRIDGE_SIZE = 3;
        public Vector2 BUBBLE_SIZE = new Vector2(9, 11);
        public Vector2 BUBBLE_START_POS = new Vector2(603, 20);
        public Vector2 BUBBLE_MARGIN= new Vector2(64, 64);
        public int BUBBLE_ODD_ROW_MARGIN = 32;
        public Vector2 GAME_SCREEN_SIZE = new Vector2(750,1080);
        public Vector2 GAME_SCREEN_POSITION = new Vector2(585, 0);
        public Vector2 HUD_LEFT_SCREEN_SIZE = new Vector2(585,1080);
        public Vector2 HUD_LEFT_SCREEN_POSITION = new Vector2(0, 0);
        public Vector2 HUD_RIGHT_SCREEN_SIZE = new Vector2(585,1080);
        public Vector2 HUD_RIGHT_SCREEN_POSITION = new Vector2(1335, 0);
        public int bounceBorderMagin = 32;

        // ? Singleton Stuff
        private static Singleton instance;
        public static Singleton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Singleton();
                }
                return instance;
            }
        }
    }
}
