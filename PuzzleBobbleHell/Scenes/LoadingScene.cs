using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleBobbleHell.Scenes
{
	public class LoadingScene : GameScene
	{
		private ContentManager contentManager;
		public void LoadContent(ContentManager Content)
		{
			contentManager = new ContentManager(Content.ServiceProvider, Content.RootDirectory);
		}
		public void UnloadContent()
		{
			contentManager.Unload();
		}

		public void Update(GameTime gameTime)
		{
			// TODO: Add your update logic here

			// ? Change scene Example
			// ? Singleton.Instance.sceneManager.changeScene(Manager.SceneManager.SceneName.MenuScene);
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			// TODO: Add your drawing code here
		}
	}
}
