using System.Collections.Generic;
using Apos.Shapes;
using System;
using GJP2021.Content.Resources.Textures;
using GJP2021.Sources.Characters;
using GJP2021.Sources.GameStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GJP2021.Sources
{
    public class Kolori : Game
    {
        public readonly GraphicsDeviceManager Graphics;
        public SpriteBatch SpriteBatch;
        public ShapeBatch ShapeBatch;
        public SpriteFont SpriteFont;
        public GameStateManager GameStateManager;
        public Dictionary<string, Texture2D> TextureMap { get; } = new();
        public Dictionary<string, SoundEffect> SoundMap { get; } = new();
        public static Kolori Instance { get; private set; }

        public Kolori()
        {
            Instance = this;
            Content.RootDirectory = "Content/Resources";
            Graphics = new GraphicsDeviceManager(this);
            IsMouseVisible = true;
            Window.AllowUserResizing = false;
            Window.Title = "Kolori";
        }

        protected override void Initialize()
        {
            base.Initialize();
            GameStateManager = new GameStateManager(this);
            Graphics.IsFullScreen = false;
            Graphics.PreferredBackBufferWidth = 800;
            Graphics.PreferredBackBufferHeight = 800;
            Graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            ShapeBatch = new ShapeBatch(GraphicsDevice, Content);
            SpriteFont = Content.Load<SpriteFont>("Fonts/lunchds");

            TextureMap.Add("start_button_normal", Content.Load<Texture2D>("Textures/Buttons/start_button_normal"));
            TextureMap.Add("start_button_hover", Content.Load<Texture2D>("Textures/Buttons/start_button_hover"));
            TextureMap.Add("start_button_pressed", Content.Load<Texture2D>("Textures/Buttons/start_button_pressed"));

            TextureMap.Add("exit_button_normal", Content.Load<Texture2D>("Textures/Buttons/exit_button_normal"));
            TextureMap.Add("exit_button_hover", Content.Load<Texture2D>("Textures/Buttons/exit_button_hover"));
            TextureMap.Add("exit_button_pressed", Content.Load<Texture2D>("Textures/Buttons/exit_button_pressed"));

            TextureMap.Add("logo", Content.Load<Texture2D>("Textures/kolori"));
            TextureMap.Add("healthbar", Content.Load<Texture2D>("Textures/healthbar"));

            TextureMap.Add("eraser", Content.Load<Texture2D>("Textures/eraser"));
            TextureMap.Add("blue_bucket", Content.Load<Texture2D>("Textures/Buckets/blue_bucket"));
            TextureMap.Add("green_bucket", Content.Load<Texture2D>("Textures/Buckets/green_bucket"));
            TextureMap.Add("orange_bucket", Content.Load<Texture2D>("Textures/Buckets/orange_bucket"));
            TextureMap.Add("pink_bucket", Content.Load<Texture2D>("Textures/Buckets/pink_bucket"));
            TextureMap.Add("purple_bucket", Content.Load<Texture2D>("Textures/Buckets/purple_bucket"));
            TextureMap.Add("red_bucket", Content.Load<Texture2D>("Textures/Buckets/red_bucket"));
            TextureMap.Add("yellow_bucket", Content.Load<Texture2D>("Textures/Buckets/yellow_bucket"));

            foreach (var color in Enum.GetNames(typeof(PaintColors)))
            {
                TextureMap.Add("player_" + color.ToLower(),
                    Content.Load<Texture2D>("Textures/Player/player_" + color.ToLower()));
            }

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