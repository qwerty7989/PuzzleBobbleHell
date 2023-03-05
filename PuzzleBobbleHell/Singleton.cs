using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PuzzleBobbleHell.Manager;

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
        public string[] BubbleColor = {
            "Blue",
            "Cyan",
            "Green",
            "Pink",
            "Red",
            "Yellow"
        };
        public int CANNON_CARTRIDGE_SIZE = 3;
        public int BUBBLE_SIZE_ROW = 10;
        public int BUBBLE_SIZE_COLUMN = 11;
        public int BUBBLE_SIZE_WIDTH = 704;
        public int BUBBLE_SIZE_HEIGHT = 512;
        public int BUBBLE_START_X = 272;
        public int BUBBLE_START_Y = 20;
        public int BUBBLE_MARGIN_WIDTH = 64;
        public int BUBBLE_MARGIN_HEIGHT = 64;
        public int BUBBLE_ODD_ROW_MARGIN = 32;
        public Vector2 GAME_SCREEN_SIZE = new Vector2(768,1048);
        public Vector2 GAME_SCREEN_POSITION = new Vector2(256, 16);
        public Vector2 HUD_SCREEN_SIZE = new Vector2(640,1080);
        public Vector2 HUD_SCREEN_POSITION = new Vector2(1280, 0);
        public Vector2 BACKGROUND_SCREEN_SIZE = new Vector2(1280,1080);
        public Vector2 BACKGROUND_SCREEN_POSITION = new Vector2(0, 0);


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
