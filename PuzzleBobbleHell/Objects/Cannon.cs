using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PuzzleBobbleHell.Manager;

namespace PuzzleBobbleHell.Objects
{
	public class Cannon : GameObject
	{
		private ContentManager contentManager;

		protected Texture2D _texture;

		public Vector2 Position;
		public float Rotation;
     	public Vector2 Origin;
		public Vector2 Scale;
		public string Name;

		public Cannon()
		{
			//Position = new Vector2(576, 768);
			Position = new Vector2(576 + 64, 768 + 192);
			Rotation = 0f;
			Origin = new Vector2(128 / 2f, 256 / 4f * 3f);
		}

		public void LoadContent(ContentManager Content)
		{
			contentManager = new ContentManager(Content.ServiceProvider, Content.RootDirectory);
			_texture = this.contentManager.Load<Texture2D>("PlayScene/Cannon");
		}
		public void UnloadContent()
		{

		}
		public void Update(GameTime gameTime)
		{
			KeyboardState keyboardState = Keyboard.GetState();

			// ? Rotate to the left
			if (keyboardState.IsKeyDown(Keys.Left) && Rotation > -(5*MathHelper.Pi/12))
				Rotation -= MathHelper.Pi / 128;

			// ? Rotate to the right
			if (keyboardState.IsKeyDown(Keys.Right) && Rotation < (5*MathHelper.Pi/12))
				Rotation += MathHelper.Pi / 128;
		}
		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(_texture, Position, null, Color.White, Rotation, Origin, 1f, SpriteEffects.None, 0f);
		}
	}
}