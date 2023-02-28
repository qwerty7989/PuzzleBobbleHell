using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using PuzzleBobbleHell.Scenes; // ! Import every file from ./Scenes folders
using Microsoft.Xna.Framework.Content;

namespace PuzzleBobbleHell.Manager
{
	public class SceneManager
	{
		private GameScene currentGameScene;
		public enum SceneName
		{
			// ? Each Scene files are referenced here.
			MenuScene,
			SettingScene,
			CreditScene,
			PlayScene,
			LoadingScene,
			StoryScene,
			EndStageScene,
			EndGameScene,
		}
		public SceneManager()
		{
			currentGameScene= new PlaceholderScene();
		}
		public void changeScene(SceneName sceneName)
		{
			switch (sceneName)
			{
				case SceneName.MenuScene:
					currentGameScene = new MenuScene();
					break;
				case SceneName.SettingScene:
					currentGameScene = new SettingScene();
					break;
				case SceneName.CreditScene:
					currentGameScene = new CreditScene();
					break;
				case SceneName.PlayScene:
					currentGameScene = new PlayScene();
					break;
				case SceneName.LoadingScene:
					currentGameScene = new LoadingScene();
					break;
				case SceneName.StoryScene:
					currentGameScene = new StoryScene();
					break;
				case SceneName.EndStageScene:
					currentGameScene = new EndStageScene();
					break;
				case SceneName.EndGameScene:
					currentGameScene = new EndGameScene();
					break;
			}
			LoadContent(Singleton.Instance.contentManager);
		}
		public void LoadContent(ContentManager Content)
		{
			currentGameScene.LoadContent(Content);
		}
		public void UnloadContent()
		{
			currentGameScene.UnloadContent();
		}
		public void Update(GameTime gameTime)
		{
			currentGameScene.Update(gameTime);
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			currentGameScene.Draw(spriteBatch);
		}
	}
}
