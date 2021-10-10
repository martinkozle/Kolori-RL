using System;
using System.Collections.Generic;
using System.Linq;
using GJP2021.Sources.Characters;
using GJP2021.Sources.GUI;
using GJP2021.Sources.Paint;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GJP2021.Sources.GameStates
{
    public class IngameState : IGameState
    {
        public static readonly IngameState Instance = new();

        private static readonly Color BgColor = Color.White;
        private Player _player;
        private float _lastSpawnEnemy;
        private float _lastSpawnBucket;

        private Random _randomGenerator;
        public PaintCircles PaintCircles;
        public List<Enemy> Enemies;
        public List<PaintBucket> PaintBuckets;
        public Projectiles Projectiles;

        private List<Vector2> _enemySpawnPoint;
        private WindowWidget _pauseWindow;

        public void Update(GameTime gameTime)
        {
            _player.HandlePause();

            if (_player.Paused)
            {
                _pauseWindow.Update();
                return;
            }

            if (!_player.IsAlive())
            {
                Kolori.Instance.GameStateManager.SetGameState(GameOverState.Instance);
                GameOverState.Instance.SetFinalScore(_player.Score);
                Initialize();
            }

            if (Mouse.GetState().LeftButton == ButtonState.Pressed &&
                gameTime.TotalGameTime.TotalSeconds - _lastSpawnEnemy >= 0.1)
            {
                var sideDecision = _randomGenerator.Next(4);
                switch (sideDecision)
                {
                    case 0:
                        Enemies.Add(new Enemy(new Vector2(
                                -20,
                                _randomGenerator.Next(Kolori.Instance.GetWindowHeight())),
                            200F));
                        break;
                    case 1:
                        Enemies.Add(new Enemy(new Vector2(
                                _randomGenerator.Next(Kolori.Instance.GetWindowWidth()),
                                Kolori.Instance.GetWindowHeight() + 20),
                            200F));
                        break;
                    case 2:
                        Enemies.Add(new Enemy(new Vector2(
                                Kolori.Instance.GetWindowWidth() + 20,
                                _randomGenerator.Next(Kolori.Instance.GetWindowHeight())),
                            200F));
                        break;
                    case 3:
                        Enemies.Add(new Enemy(new Vector2(
                                _randomGenerator.Next(Kolori.Instance.GetWindowWidth()),
                                -20),
                            200F));
                        break;
                }

                _lastSpawnEnemy = (float) gameTime.TotalGameTime.TotalSeconds;
            }

            if (Mouse.GetState().RightButton == ButtonState.Pressed &&
                gameTime.TotalGameTime.TotalSeconds - _lastSpawnBucket >= 0.1)
            {
                PaintBuckets.Add(new PaintBucket(
                    new Vector2(
                        _randomGenerator.Next(32, Kolori.Instance.GetWindowWidth()-32),
                        _randomGenerator.Next(32, Kolori.Instance.GetWindowHeight()-32)
                    ),
                    _randomGenerator)
                );
                _lastSpawnBucket = (float) gameTime.TotalGameTime.TotalSeconds;
            }

            foreach (var enemy in Enemies.Where(enemy => enemy.MarkedAsKilled))
            {
                _player.Heal(10F);
            }

            Enemies.RemoveAll(el => el.MarkedForDeletion);
            foreach (var enemy in Enemies)
            {
                enemy.Update(gameTime, _player, PaintCircles);
            }

            _player.HandleAbility(this);
            Projectiles.Update(gameTime, this, PaintCircles);

            PaintCircles.Update(gameTime);
            foreach (var bucket in PaintBuckets)
            {
                bucket.Update(gameTime, _player.Position);
                if (!bucket.MarkedForDeletion) continue;
                _player.TrailColor = bucket.GetPaintBucketColor();
                _player.Score += 1;
            }

            _player.Update(gameTime, PaintCircles);

            PaintBuckets.RemoveAll(pb => pb.MarkedForDeletion);
        }

        public void Draw(GameTime gameTime)
        {
            Kolori.Instance.GraphicsDevice.Clear(BgColor);

            PaintCircles.Draw(Kolori.Instance.DrawBatch);

            Kolori.Instance.SpriteBatch.Begin();

            foreach (var bucket in PaintBuckets)
            {
                bucket.Draw(gameTime);
            }

            foreach (var enemy in Enemies)
            {
                enemy.Draw(gameTime);
            }

            _player.Draw();

            _player.DrawHealth();

            if (_player.Paused)
            {
                _pauseWindow.Draw();
            }

            Kolori.Instance.SpriteBatch.End();

            //Kolori.Instance.ShapeBatch.Begin();
            //Kolori.Instance.ShapeBatch.DrawLine(_player.Position, _player.Position + _player.GetSpeedVector(), 2, Color.Green,
            //    Color.Green);
            //Kolori.Instance.ShapeBatch.End();
        }

        public void Initialize()
        {
            var windowTexture = Kolori.Instance.TextureMap["pause_window"];
            var windowBuilder = WindowWidget.Builder()
                .CenterHorizontally(Kolori.Instance.GetWindowWidth)
                .CenterVertically(Kolori.Instance.GetWindowHeight);

            var windowX = windowBuilder.X;
            var windowY = windowBuilder.Y;

            _pauseWindow = windowBuilder.SetTexture(windowTexture)
                .AddButton(
                    Button.Builder()
                        .SetPosition(() => windowX.Invoke(), () => windowY.Invoke() + 192)
                        .CenterHorizontally(windowTexture.Width)
                        .SetSound("button")
                        .SetTexture("resume")
                        .SetAction(() => _player.Paused = false)
                        .Build()
                )
                .AddButton(
                    Button.Builder()
                        .SetPosition(() => windowX.Invoke(), () => windowY.Invoke() + 270)
                        .CenterHorizontally(windowTexture.Width)
                        .SetSound("button")
                        .SetTexture("menu")
                        .SetAction(
                            () => { Kolori.Instance.GameStateManager.SetGameState(MenuState.Instance); })
                        .Build()
                )
                .Build();
            Enemies = new List<Enemy>();
            PaintBuckets = new List<PaintBucket>();
            Projectiles = new Projectiles();
            _lastSpawnEnemy = 0;
            _lastSpawnBucket = 0;
            _randomGenerator = new Random();
            //_paintBucket=new PaintBucket(new Vector2(100,100),_randomGenerator,Kolori.Instance);
            PaintBuckets.Add(new PaintBucket(
                new Vector2(
                    _randomGenerator.Next(0, Kolori.Instance.GetWindowHeight()),
                    _randomGenerator.Next(0, Kolori.Instance.GetWindowHeight())
                ),
                _randomGenerator)
            );
            _player = Player.Builder()
                .SetPosition(Kolori.Instance.GetWindowWidth() / 2F, Kolori.Instance.GetWindowHeight() / 2F)
                .SetMaxSpeed(225f)
                .SetAcceleration(450f)
                .SetBounds(new Vector2(Kolori.Instance.GetWindowWidth(),
                    Kolori.Instance.GetWindowHeight()))
                .Build();
            PaintCircles = new PaintCircles();
            _enemySpawnPoint = new List<Vector2>
            {
                new(-50, -50),
                new(-50, Kolori.Instance.GetWindowHeight() + 50),
                new(Kolori.Instance.GetWindowWidth() + 50, -50),
                new(Kolori.Instance.GetWindowWidth() + 50, Kolori.Instance.GetWindowHeight() + 50)
            };

            Enemies.Add(new Enemy(_enemySpawnPoint[_randomGenerator.Next(0, 4)], 200F));
        }
    }
}