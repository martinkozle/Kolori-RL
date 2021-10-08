using System.Collections.Generic;
using GJP2021.Sources.GameStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GJP2021.Sources
{
    public class Kolori : Game
    {
        public GraphicsDeviceManager Graphics;
        public SpriteBatch SpriteBatch;
        public SpriteFont SpriteFont;
        public GameStateManager GameStateManager;
        public static Dictionary<string, Texture2D> TextureMap { get; } = new();
        public static Dictionary<string, SoundEffect> SoundMap { get; } = new();

        public Kolori()
        {
            Content.RootDirectory = "Content/Resources";
            Graphics = new GraphicsDeviceManager(this);
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            Window.Title = "Kolori";
        }

        protected override void Initialize()
        {
            base.Initialize();
            GameStateManager = new GameStateManager(this);
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            SpriteFont = Content.Load<SpriteFont>("Fonts/lunchds");

            TextureMap.Add("button_normal", Content.Load<Texture2D>("Textures/button_normal"));
            TextureMap.Add("button_hover", Content.Load<Texture2D>("Textures/button_hover"));
            TextureMap.Add("button_pressed", Content.Load<Texture2D>("Textures/button_pressed"));

            SoundMap.Add("button_press", Content.Load<SoundEffect>("Sounds/button_press"));
            SoundMap.Add("button_release", Content.Load<SoundEffect>("Sounds/button_release"));
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            GameStateManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GameStateManager.Draw(gameTime);
            
            base.Draw(gameTime);
        }

        public int GetWindowWidth()
        {
            return Window.ClientBounds.Width;
        }

        public int GetWindowHeight()
        {
            return Window.ClientBounds.Height;
        }
        
    }
}