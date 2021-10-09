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
        private Kolori _game;
        private static readonly Color BgColor = Color.White;
        private Player _player;
        private float _lastSpawnEnemy;
        private float _lastSpawnBucket;

        private Random _randomGenerator;
        private PaintCircles _paintCircles;


        private readonly List<Enemy> _enemies = new();
        private List<PaintBucket> _paintBuckets = new List<PaintBucket>();

        private List<Vector2> _enemySpawnPoint;

        public void Update(GameTime gameTime)
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed &&
                gameTime.TotalGameTime.TotalSeconds - _lastSpawnEnemy >= 0.1)
            {
                _enemies.Add(new Enemy(_enemySpawnPoint[_randomGenerator.Next(0, 4)], 130F, _game));
                _lastSpawnEnemy = (float) gameTime.TotalGameTime.TotalSeconds;
            }
            if (Mouse.GetState().RightButton == ButtonState.Pressed &&
                gameTime.TotalGameTime.TotalSeconds - _lastSpawnBucket >= 0.1)
            {
                _paintBuckets.Add(new PaintBucket(
                    new Vector2(
                        _randomGenerator.Next(0, _game.GetWindowHeight()),
                        _randomGenerator.Next(0, _game.GetWindowHeight())
                    ),
                    _randomGenerator,
                    _game)
                );
                _lastSpawnBucket = (float) gameTime.TotalGameTime.TotalSeconds;
            }

            _enemies.RemoveAll(el => el.MarkedForDelete);

            _player.Update(gameTime, _paintCircles);
            
            foreach (var enemy in _enemies)
            {
                enemy.Update(gameTime, _player.Position, _paintCircles);
            }
            
            _paintCircles.Update(gameTime);
            foreach (var bucket in _paintBuckets )
            {
                bucket.Update(gameTime,_player.Position);
                if (bucket.MarkedForDeleteion)
                {
                    _player.SetColor(bucket.GetPaintBucketColor());
                }
                
            }
            _player.Update(gameTime, _paintCircles);
            
            _paintBuckets.RemoveAll(pb => pb.MarkedForDeleteion);
            
        }

        public void Draw(GameTime gameTime)
        {
            _game.GraphicsDevice.Clear(BgColor);

            _paintCircles.Draw(_game.ShapeBatch);
            
            _game.SpriteBatch.Begin();
            
            foreach (var bucket in _paintBuckets)
            {
                bucket.Draw(gameTime);
            }

            foreach (var enemy in _enemies)
            {
                enemy.Draw(gameTime);
            }

            _player.Draw(_game.SpriteBatch);
            
            _player.DrawHealth(_game.SpriteBatch);
            
            _game.SpriteBatch.End();

            //_game.ShapeBatch.Begin();
            //_game.ShapeBatch.DrawLine(_player.Posi/tion, _player.Position + _player.GetSpeedVector(), 2, Color.Green,
            //    Color.Green);
            //_game.ShapeBatch.End();
        }

        public void Initialize(Kolori game)
        {
            _randomGenerator = new Random();
            _game = game;
            //_paintBucket=new PaintBucket(new Vector2(100,100),_randomGenerator,_game);
            _paintBuckets.Add(new PaintBucket(
                new Vector2(
                    _randomGenerator.Next(0, _game.GetWindowHeight()),
                    _randomGenerator.Next(0, _game.GetWindowHeight())
                ),
                _randomGenerator,
                _game)
            );
            _player = Player.Builder()
                .SetPosition(0, 0)
                .SetMaxSpeed(150f)
                .SetAcceleration(450f)
                .SetBounds(new Vector2(_game.GetWindowWidth(),
                    _game.GetWindowHeight()))
                .Build();
            _paintCircles = new PaintCircles();
            _enemySpawnPoint = new List<Vector2>
            {
                new(-50, -50),
                new(-50, _game.GetWindowHeight() + 50),
                new(_game.GetWindowWidth() + 50, -50),
                new(_game.GetWindowWidth() + 50, _game.GetWindowHeight() + 50)
            };

            _enemies.Add(new Enemy(_enemySpawnPoint[_randomGenerator.Next(0, 4)], 130F, _game));
        }
    }
}