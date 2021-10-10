using System.Collections.Generic;
using System;
using GJP2021.Content;
using GJP2021.Sources.GameStates;
using LilyPath;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GJP2021.Sources
{
    public class Kolori : Game
    {
        public readonly GraphicsDeviceManager Graphics;
        public SpriteBatch SpriteBatch;
        public DrawBatch DrawBatch;
        public GameStateManager GameStateManager;
        public Dictionary<string, Texture2D> TextureMap { get; } = new();
        public Dictionary<string, SoundEffect> SoundMap { get; } = new();
        public Dictionary<string, Song> SongMap { get; } = new();
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
            GameStateManager = new GameStateManager();
            Graphics.IsFullScreen = false;
            Graphics.PreferredBackBufferWidth = 800;
            Graphics.PreferredBackBufferHeight = 800;
            Graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            DrawBatch = new DrawBatch(GraphicsDevice);

            TextureMap.Add("start_button_normal", Content.Load<Texture2D>("Textures/GUI/Buttons/start_button_normal"));
            TextureMap.Add("start_button_hover", Content.Load<Texture2D>("Textures/GUI/Buttons/start_button_hover"));
            TextureMap.Add("start_button_pressed", Content.Load<Texture2D>("Textures/GUI/Buttons/start_button_pressed"));
            TextureMap.Add("restart_button_normal", Content.Load<Texture2D>("Textures/GUI/Buttons/restart_button_normal"));
            TextureMap.Add("restart_button_hover", Content.Load<Texture2D>("Textures/GUI/Buttons/restart_button_hover"));
            TextureMap.Add("restart_button_pressed", Content.Load<Texture2D>("Textures/GUI/Buttons/restart_button_pressed"));
            TextureMap.Add("resume_button_normal", Content.Load<Texture2D>("Textures/GUI/Buttons/resume_button_normal"));
            TextureMap.Add("resume_button_hover", Content.Load<Texture2D>("Textures/GUI/Buttons/resume_button_hover"));
            TextureMap.Add("resume_button_pressed", Content.Load<Texture2D>("Textures/GUI/Buttons/resume_button_pressed"));
            TextureMap.Add("exit_button_normal", Content.Load<Texture2D>("Textures/GUI/Buttons/exit_button_normal"));
            TextureMap.Add("exit_button_hover", Content.Load<Texture2D>("Textures/GUI/Buttons/exit_button_hover"));
            TextureMap.Add("exit_button_pressed", Content.Load<Texture2D>("Textures/GUI/Buttons/exit_button_pressed"));
            TextureMap.Add("menu_button_normal", Content.Load<Texture2D>("Textures/GUI/Buttons/menu_button_normal"));
            TextureMap.Add("menu_button_hover", Content.Load<Texture2D>("Textures/GUI/Buttons/menu_button_hover"));
            TextureMap.Add("menu_button_pressed", Content.Load<Texture2D>("Textures/GUI/Buttons/menu_button_pressed"));

            TextureMap.Add("health_bar", Content.Load<Texture2D>("Textures/GUI/health_bar"));
            TextureMap.Add("pause_window", Content.Load<Texture2D>("Textures/GUI/pause_window"));
            TextureMap.Add("logo", Content.Load<Texture2D>("Textures/GUI/kolori"));
            TextureMap.Add("game_over", Content.Load<Texture2D>("Textures/GUI/game_over"));
            TextureMap.Add("ability_icons", Content.Load<Texture2D>("Textures/GUI/ability_icons"));

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
            SoundMap.Add("teleport_ability", Content.Load<SoundEffect>("Sounds/teleport_ability"));
            SoundMap.Add("burst_ability_1", Content.Load<SoundEffect>("Sounds/burst_ability_1"));
            SoundMap.Add("burst_ability_2", Content.Load<SoundEffect>("Sounds/burst_ability_2"));
            SoundMap.Add("timestop_ability", Content.Load<SoundEffect>("Sounds/timestop_ability"));
            SoundMap.Add("shoot_ability", Content.Load<SoundEffect>("Sounds/shoot_ability"));
            SoundMap.Add("speedup_ability", Content.Load<SoundEffect>("Sounds/speedup_ability"));
            SoundMap.Add("player_move", Content.Load<SoundEffect>("Sounds/player_move"));
            SoundMap.Add("death_screen", Content.Load<SoundEffect>("Sounds/death_screen"));
            SoundMap.Add("pause_screen", Content.Load<SoundEffect>("Sounds/pause"));

            SongMap.Add("bgm_loop", Content.Load<Song>("Sounds/bgm_loop"));
            SongMap.Add("bgm_start", Content.Load<Song>("Sounds/bgm_start"));

            Font.Initialize(SpriteBatch, Content);
            Font.LoadSizes("Fonts/lunchds", new[] {12, 16, 24, 32, 48, 72});
        }

        protected override void Update(GameTime gameTime)
        {
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