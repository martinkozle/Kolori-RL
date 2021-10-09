using System;
using System.Collections.Generic;
using GJP2021.Sources.Characters;
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
        private PaintCircles _paintCircles;
        private List<Enemy> _enemies = new();
        private List<PaintBucket> _paintBuckets = new();

        private List<Vector2> _enemySpawnPoint;

        public void Update(GameTime gameTime)
        {
            if (!_player.IsAlive())
            {
                Kolori.Instance.GameStateManager.SetGameState(GameOverState.Instance);
                Initialize();
            }

            if (Mouse.GetState().LeftButton == ButtonState.Pressed &&
                gameTime.TotalGameTime.TotalSeconds - _lastSpawnEnemy >= 0.1)
            {
                _enemies.Add(new Enemy(_enemySpawnPoint[_randomGenerator.Next(0, 4)], 200F));
                _lastSpawnEnemy = (float) gameTime.TotalGameTime.TotalSeconds;
            }

            if (Mouse.GetState().RightButton == ButtonState.Pressed &&
                gameTime.TotalGameTime.TotalSeconds - _lastSpawnBucket >= 0.1)
            {
                _paintBuckets.Add(new PaintBucket(
                    new Vector2(
                        _randomGenerator.Next(0, Kolori.Instance.GetWindowHeight()),
                        _randomGenerator.Next(0, Kolori.Instance.GetWindowHeight())
                    ),
                    _randomGenerator)
                );
                _lastSpawnBucket = (float) gameTime.TotalGameTime.TotalSeconds;
            }

            _enemies.RemoveAll(el => el.MarkedForDeletion);

            foreach (var enemy in _enemies)
            {
                enemy.Update(gameTime, _player, _paintCircles);
            }

            _paintCircles.Update(gameTime);
            foreach (var bucket in _paintBuckets)
            {
                bucket.Update(gameTime, _player.Position);
                if (bucket.MarkedForDeletion)
                {
                    _player.SetColor(bucket.GetPaintBucketColor());
                    _player.Score += 1;
                }
            }

            _player.Update(gameTime, _paintCircles);

            _paintBuckets.RemoveAll(pb => pb.MarkedForDeletion);
        }

        public void Draw(GameTime gameTime)
        {
            Kolori.Instance.GraphicsDevice.Clear(BgColor);

            _paintCircles.Draw(Kolori.Instance.DrawBatch);

            Kolori.Instance.SpriteBatch.Begin();

            foreach (var bucket in _paintBuckets)
            {
                bucket.Draw(gameTime);
            }

            foreach (var enemy in _enemies)
            {
                enemy.Draw(gameTime);
            }

            _player.Draw(Kolori.Instance.SpriteBatch);

            _player.DrawHealth(Kolori.Instance.SpriteBatch);

            Kolori.Instance.SpriteBatch.End();

            //Kolori.Instance.ShapeBatch.Begin();
            //Kolori.Instance.ShapeBatch.DrawLine(_player.Position, _player.Position + _player.GetSpeedVector(), 2, Color.Green,
            //    Color.Green);
            //Kolori.Instance.ShapeBatch.End();
        }
        
        public void Initialize()
        {
            _enemies = new List<Enemy>(); 
            _paintBuckets = new List<PaintBucket>();
            _lastSpawnEnemy = 0;
            _lastSpawnBucket = 0;
            _randomGenerator = new Random();
            //_paintBucket=new PaintBucket(new Vector2(100,100),_randomGenerator,Kolori.Instance);
            _paintBuckets.Add(new PaintBucket(
                new Vector2(
                    _randomGenerator.Next(0, Kolori.Instance.GetWindowHeight()),
                    _randomGenerator.Next(0, Kolori.Instance.GetWindowHeight())
                ),
                _randomGenerator)
            );
            _player = Player.Builder()
                .SetPosition(Kolori.Instance.GetWindowWidth()/2F,Kolori.Instance.GetWindowHeight()/2F )
                .SetMaxSpeed(225f)
                .SetAcceleration(450f)
                .SetBounds(new Vector2(Kolori.Instance.GetWindowWidth(),
                    Kolori.Instance.GetWindowHeight()))
                .Build();
            _paintCircles = new PaintCircles();
            _enemySpawnPoint = new List<Vector2>
            {
                new(-50, -50),
                new(-50, Kolori.Instance.GetWindowHeight() + 50),
                new(Kolori.Instance.GetWindowWidth() + 50, -50),
                new(Kolori.Instance.GetWindowWidth() + 50, Kolori.Instance.GetWindowHeight() + 50)
            };

            _enemies.Add(new Enemy(_enemySpawnPoint[_randomGenerator.Next(0, 4)], 200F));
        }
    }
}