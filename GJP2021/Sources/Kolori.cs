using System.Collections.Generic;
using System;
using GJP2021.Content;
using GJP2021.Sources.GameStates;
using LilyPath;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using NetMQ.Sockets;
using NetMQ;
using System.Diagnostics;
using System.Text.Json;
using Microsoft.Xna.Framework.Input;
using System.Threading;

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

        public readonly bool RL;

        private readonly ResponseSocket _RLServer;
        private GameTime _RLGameTime;
        private bool _RenderOnce = false;

        public Kolori(bool rl, int port)
        {
            RL = rl;
            if (RL)
            {
                _RLServer = new ResponseSocket($"tcp://*:{port}");
                UnlockFPS();
            }
            else
            {
                LockFPS(30);
            }

            Instance = this;
            Content.RootDirectory = "Content/Resources";
            Graphics = new GraphicsDeviceManager(this);
            IsMouseVisible = true;
            Window.AllowUserResizing = false;
            Window.Title = "Kolori";
        }

        private void LockFPS(int fps)
        {
            TargetElapsedTime = TimeSpan.FromSeconds(1.0 / fps);
            IsFixedTimeStep = true;
        }

        private void UnlockFPS()
        {
            IsFixedTimeStep = false;
        }

        ~Kolori()
        {
            _RLServer.Dispose();
        }

        protected override void Initialize()
        {
            base.Initialize();
            GameStateManager = new GameStateManager();
            Graphics.IsFullScreen = false;
            Graphics.PreferredBackBufferWidth = 800;
            Graphics.PreferredBackBufferHeight = 800;
            Graphics.ApplyChanges();
            _RLGameTime = new GameTime();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            DrawBatch = new DrawBatch(GraphicsDevice);

            TextureMap.Add(
                "start_button_normal",
                Content.Load<Texture2D>("Textures/GUI/Buttons/start_button_normal")
            );
            TextureMap.Add(
                "start_button_hover",
                Content.Load<Texture2D>("Textures/GUI/Buttons/start_button_hover")
            );
            TextureMap.Add(
                "start_button_pressed",
                Content.Load<Texture2D>("Textures/GUI/Buttons/start_button_pressed")
            );
            TextureMap.Add(
                "restart_button_normal",
                Content.Load<Texture2D>("Textures/GUI/Buttons/restart_button_normal")
            );
            TextureMap.Add(
                "restart_button_hover",
                Content.Load<Texture2D>("Textures/GUI/Buttons/restart_button_hover")
            );
            TextureMap.Add(
                "restart_button_pressed",
                Content.Load<Texture2D>("Textures/GUI/Buttons/restart_button_pressed")
            );
            TextureMap.Add(
                "resume_button_normal",
                Content.Load<Texture2D>("Textures/GUI/Buttons/resume_button_normal")
            );
            TextureMap.Add(
                "resume_button_hover",
                Content.Load<Texture2D>("Textures/GUI/Buttons/resume_button_hover")
            );
            TextureMap.Add(
                "resume_button_pressed",
                Content.Load<Texture2D>("Textures/GUI/Buttons/resume_button_pressed")
            );
            TextureMap.Add(
                "exit_button_normal",
                Content.Load<Texture2D>("Textures/GUI/Buttons/exit_button_normal")
            );
            TextureMap.Add(
                "exit_button_hover",
                Content.Load<Texture2D>("Textures/GUI/Buttons/exit_button_hover")
            );
            TextureMap.Add(
                "exit_button_pressed",
                Content.Load<Texture2D>("Textures/GUI/Buttons/exit_button_pressed")
            );
            TextureMap.Add(
                "menu_button_normal",
                Content.Load<Texture2D>("Textures/GUI/Buttons/menu_button_normal")
            );
            TextureMap.Add(
                "menu_button_hover",
                Content.Load<Texture2D>("Textures/GUI/Buttons/menu_button_hover")
            );
            TextureMap.Add(
                "menu_button_pressed",
                Content.Load<Texture2D>("Textures/GUI/Buttons/menu_button_pressed")
            );

            TextureMap.Add("health_bar", Content.Load<Texture2D>("Textures/GUI/health_bar"));
            TextureMap.Add("pause_window", Content.Load<Texture2D>("Textures/GUI/pause_window"));
            TextureMap.Add("logo", Content.Load<Texture2D>("Textures/GUI/kolori"));
            TextureMap.Add("game_over", Content.Load<Texture2D>("Textures/GUI/game_over"));
            TextureMap.Add("ability_icons", Content.Load<Texture2D>("Textures/GUI/ability_icons"));

            TextureMap.Add("eraser", Content.Load<Texture2D>("Textures/eraser"));
            TextureMap.Add("blue_bucket", Content.Load<Texture2D>("Textures/Buckets/blue_bucket"));
            TextureMap.Add(
                "green_bucket",
                Content.Load<Texture2D>("Textures/Buckets/green_bucket")
            );
            TextureMap.Add(
                "orange_bucket",
                Content.Load<Texture2D>("Textures/Buckets/orange_bucket")
            );
            TextureMap.Add("pink_bucket", Content.Load<Texture2D>("Textures/Buckets/pink_bucket"));
            TextureMap.Add(
                "purple_bucket",
                Content.Load<Texture2D>("Textures/Buckets/purple_bucket")
            );
            TextureMap.Add("red_bucket", Content.Load<Texture2D>("Textures/Buckets/red_bucket"));
            TextureMap.Add(
                "yellow_bucket",
                Content.Load<Texture2D>("Textures/Buckets/yellow_bucket")
            );

            foreach (var color in Enum.GetNames(typeof(PaintColors)))
            {
                TextureMap.Add(
                    "player_" + color.ToLower(),
                    Content.Load<Texture2D>("Textures/Player/player_" + color.ToLower())
                );
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
            Font.LoadSizes("Fonts/lunchds", new[] { 12, 16, 24, 32, 48, 72 });
        }

        public string ToJsonString(GameTime gameTime)
        {
            return JsonSerializer.Serialize(ToDict(gameTime));
        }

        public object ToDict(GameTime gameTime)
        {
            return new
            {
                elapsedGameTime = gameTime.ElapsedGameTime.TotalSeconds,
                totalGameTime = gameTime.TotalGameTime.TotalSeconds,
                IngameState = IngameState.Instance.ToDict()
            };
        }

        protected override void Update(GameTime gameTime)
        {
            if (RL)
            {
                if (_RenderOnce)
                {
                    _RenderOnce = false;
                    UnlockFPS();
                }
                byte[] messageBytes = _RLServer.ReceiveFrameBytes();
                var messageJson = JsonSerializer.Deserialize<JsonElement>(messageBytes);
                // Debug.WriteLine(
                //     $"Time: {gameTime.TotalGameTime.TotalSeconds} Message: {messageJson}"
                // );
                _RLGameTime.ElapsedGameTime = TimeSpan.FromSeconds(1.0 / 30.0);

                if (
                    messageJson.TryGetProperty("render_next", out var render) && render.GetBoolean()
                )
                {
                    _RenderOnce = true;
                    LockFPS(30);
                }
                else
                {
                    _RLGameTime.TotalGameTime += _RLGameTime.ElapsedGameTime;
                }
                gameTime = _RLGameTime;

                switch (messageJson.GetProperty("command").GetString())
                {
                    case "RESET":
                    {
                        Debug.WriteLine("RESET");
                        ResetRL();
                        _RLServer.SendFrame(ToJsonString(gameTime));
                        break;
                    }
                    case "UPDATE":
                    {
                        GameStateManager.Update(gameTime);
                        _RLServer.SendFrame(ToJsonString(gameTime));
                        break;
                    }
                    case "STEP":
                    {
                        // if (IngameState.Instance.Player.Health <= 0)
                        // {
                        //     Reset(gameTime);
                        //     _RLServer.SendFrame(ToJsonString(gameTime));
                        //     break;
                        // }

                        var move = messageJson.GetProperty("move").Deserialize<bool[]>();
                        var ability = messageJson.GetProperty("ability").GetBoolean();
                        // var mousePositionElement = messageJson.GetProperty("mouse_position");
                        // var mouseX = (float)mousePositionElement[0].GetDouble();
                        // var mouseY = (float)mousePositionElement[1].GetDouble();
                        // var mouseVector = new Vector2(mouseX, mouseY);
                        // Console.WriteLine(
                        //     $"move: {move[0]}, {move[1]}, {move[2]}, {move[3]} ability: {ability} mouse: {mouseVector}"
                        // );
                        var controls = IngameState.Instance.Controls;
                        controls.KeysDown.Clear();
                        if (move[0])
                        {
                            controls.KeysDown.Add(Keys.W);
                        }
                        if (move[1])
                        {
                            controls.KeysDown.Add(Keys.S);
                        }
                        if (move[2])
                        {
                            controls.KeysDown.Add(Keys.A);
                        }
                        if (move[3])
                        {
                            controls.KeysDown.Add(Keys.D);
                        }
                        if (ability)
                        {
                            controls.UseAbility = true;
                        }
                        // controls.MouseState = new MouseState(
                        //     (int)mouseX,
                        //     (int)mouseY,
                        //     0,
                        //     ButtonState.Released,
                        //     ButtonState.Released,
                        //     ButtonState.Released,
                        //     ButtonState.Released,
                        //     ButtonState.Released
                        // );
                        GameStateManager.Update(gameTime);
                        _RLServer.SendFrame(ToJsonString(gameTime));
                        break;
                    }
                }
            }
            else
            {
                GameStateManager.Update(gameTime);
            }

            base.Update(gameTime);
        }

        private void ResetRL()
        {
            GameStateManager.SetGameState(IngameState.Instance, reset: true);
            IngameState.Instance.Controls.RL = RL;
            _RLGameTime.TotalGameTime = TimeSpan.Zero;
            _RLGameTime.ElapsedGameTime = TimeSpan.Zero;
        }

        protected override void Draw(GameTime gameTime)
        {
            if (RL)
            {
                if (!_RenderOnce)
                {
                    return;
                }
                gameTime = _RLGameTime;
            }
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
