using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GJP2021.Sources.Characters;
using GJP2021.Sources.GUI;
using GJP2021.Sources.Paint;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

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
        private int _numberOfEnemies = 1;
        public List<PaintBucket> PaintBuckets;
        public Projectiles Projectiles;
        public bool TimeScaleActive = false;
        public float TimeScale = 1;
        public float TimeScaleDuration = 0;
        private PaintSpawner _enemyDeathPaintSpawner;
        private static Song _song;

        private List<Vector2> _enemySpawnPoint;
        private WindowWidget _pauseWindow;

        private void spawnEnemy()
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
        }

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
                return;
            }

            if (TimeScaleActive)
            {
                TimeScaleDuration += (float) gameTime.ElapsedGameTime.TotalSeconds;
                if (TimeScaleDuration > 3)
                {
                    TimeScaleActive = false;
                    TimeScale = 1;
                    TimeScaleDuration = 0;
                }
            }

            if (gameTime.TotalGameTime.TotalSeconds - _lastSpawnEnemy >= 10)
            {
                for (var i = 0; i < _numberOfEnemies + 1; i++)
                {
                    spawnEnemy();
                }

                if (_numberOfEnemies != 8)
                {
                    _numberOfEnemies += 1;
                }

                _lastSpawnEnemy = (float) gameTime.TotalGameTime.TotalSeconds;
            }

            if (gameTime.TotalGameTime.TotalSeconds - _lastSpawnBucket >= 10)
            {
                PaintBuckets.Add(new PaintBucket(
                    new Vector2(
                        _randomGenerator.Next(40, Kolori.Instance.GetWindowWidth() - 40),
                        _randomGenerator.Next(40, Kolori.Instance.GetWindowHeight() - 40)
                    ),
                    _randomGenerator)
                );


                _lastSpawnBucket = (float) gameTime.TotalGameTime.TotalSeconds;
            }

            foreach (var enemy in Enemies.Where(enemy => enemy.MarkedAsKilled))
            {
                _player.Score += 1;
                _player.Heal(10F);
            }

            foreach (var enemy in Enemies.Where(enemy => enemy.MarkedForDeletion))
            {
                for (var i = 0; i < 3; ++i)
                {
                    PaintCircles.Add(_enemyDeathPaintSpawner.SpawnCircle(enemy.Position));
                }
            }

            Enemies.RemoveAll(el => el.MarkedForDeletion);
            foreach (var enemy in Enemies)
            {
                enemy.Update(gameTime, _player, PaintCircles, TimeScale);
            }

            _player.HandleAbility(this);
            Projectiles.Update(gameTime, this, PaintCircles, TimeScale);

            PaintCircles.Update(gameTime, TimeScale);
            foreach (var bucket in PaintBuckets)
            {
                bucket.Update(gameTime, _player.Position);
                if (!bucket.MarkedForDeletion) continue;
                _player.TrailColor = bucket.GetPaintBucketColor();
            }

            _player.Update(gameTime, PaintCircles);

            PaintBuckets.RemoveAll(pb => pb.MarkedForDeletion);
        }

        public void Draw(GameTime gameTime)
        {
            if (!_player.IsAlive())
            {
                Kolori.Instance.GameStateManager.SetGameState(GameOverState.Instance);
                GameOverState.Instance.SetFinalScore(_player.Score);
                Kolori.Instance.SoundMap["death_screen"].Play();
                Initialize();
            }

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

            _player.DrawDisplay();

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
            _song = Kolori.Instance.SongMap["bgm_loop"];
            MediaPlayer.Play(_song);
            MediaPlayer.IsRepeating = true;

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
                    _randomGenerator.Next(40, Kolori.Instance.GetWindowWidth() + 40),
                    _randomGenerator.Next(40, Kolori.Instance.GetWindowHeight() + 40)
                ),
                _randomGenerator)
            );
            _enemyDeathPaintSpawner = new PaintSpawner(Color.White, new Color(0, 0, 0), 35, 10, 15, 0.2F, 2);
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
            _numberOfEnemies = 0;

            spawnEnemy();
        }
    }
}