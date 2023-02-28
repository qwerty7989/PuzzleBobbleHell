using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using PuzzleBobbleHell.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

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
		public int heightScreen = 900;
		public int widthScreen = 1600;

		// ? ContentManager
		public ContentManager contentManager;

		// ? SceneManager
		public SceneManager sceneManager = new SceneManager();


		// ? PlayScene
		public int Score = 0;
		public int mainStage = 1; // ? The main stage number (1)-1
		public int subStage = 1; // ? The sub stage number 1-(1)


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
