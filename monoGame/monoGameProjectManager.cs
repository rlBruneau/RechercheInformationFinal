using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using monoGame.Actors;
using monoGame.Actors.Movables;
using monoGame.Actors.Movables.Players;
using monoGame.Services;
using monoGame.SoundManagers;
using monoGame.Sprites;
using monoGame.States;
using System.Collections.Generic;

namespace monoGame
{
    public class monoGameProjectManager : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public static int WindowWidth;
        public static int WindowHeight;
        private StateManager StateManager { get; } = StateManager.Instance;

        public monoGameProjectManager()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
            WindowWidth = GraphicsDevice.Viewport.Bounds.Width;
            WindowHeight = GraphicsDevice.Viewport.Bounds.Height;

            StateManager.SetCurrentState(StateEnum.GAME);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            SpriteService.Instance.LoadInitialTexture(Content, new Textures[] { Textures.MarioLuiji, Textures.RedBackground, Textures.MarioTiles });
            SpriteService.Instance.LoadGeneralSprite();
            SoundManager.Instance.Content = Content;
            SoundManager.Instance.LoadInitialSoundEffects();
            SoundManager.Instance.LoadSongs(new List<Musics>()
            {
                Musics.Mario
            });
            SoundManager.Instance.SetSong(Musics.Mario, true);
            
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            //StateManager.CurrentState.Update(gameTime);
            // TODO: Add your update logic here
            InputManager.Instance.KeyboardInput();
            this.StateManager.CurrentState.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //StateManager.CurrentState.Draw(gameTime, _spriteBatch);
            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            this.StateManager.CurrentState.Draw(gameTime, _spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
