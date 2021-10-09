using System;
using System.Collections.Generic;
using GJP2021.Sources.Characters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GJP2021.Sources.GameStates
{
    public class IngameState : IGameState
    {
        public static readonly IngameState Instance = new ();
        private Kolori _game;
        private static readonly Color BgColor = Color.White;
        private Player _player;
        private float _lastSpawnEnemy;
        private float _lastSpawnBucket;

        private Random _randomGenerator;

        //private PaintBucket _paintBucket;
        private List<PaintBucket> _paintBuckets = new List<PaintBucket>();

        private readonly List<Enemy> _enemies = new ();

        private List<Vector2> _enemySpawnPoint;

        public void Update(GameTime gameTime)
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed &&
                gameTime.TotalGameTime.TotalSeconds - _lastSpawnEnemy >= 0.1)
            {
                _enemies.Add(new Enemy(_enemySpawnPoint[_randomGenerator.Next(0, 4)], 2.5F, _game));
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

            foreach (var enemy in _enemies)
            {
                enemy.Update(gameTime, _player.Position);
            }
            foreach (var bucket in _paintBuckets )
            {
                bucket.Update(gameTime,_player.Position);
                if (bucket.MarkedForDeleteion)
                {
                    _player.setColor(bucket.GetPaintBucketColor());
                }
                
            }
            _player.Update(gameTime);
            
            _paintBuckets.RemoveAll(pb => pb.MarkedForDeleteion);
            
        }

        public void Draw(GameTime gameTime)
        {
            _game.GraphicsDevice.Clear(BgColor);

            _player.DrawShapeBatch(_game.ShapeBatch);

            _game.SpriteBatch.Begin();

            foreach (var enemy in _enemies)
            {
                enemy.Draw(gameTime);
            }

            _player.Draw(_game.SpriteBatch);

            foreach (var bucket in _paintBuckets)
            {
                bucket.Draw(gameTime);
            }
            
            _game.SpriteBatch.End();
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
                .SetSpeed(2f)
                .SetMaxAcceleration(6f)
                .SetBounds(new Vector2(_game.GetWindowWidth(),
                    _game.GetWindowHeight()))
                .Build();
            _enemySpawnPoint = new List<Vector2>
            {
                new ( - 50, -50),
                new ( - 50, _game.GetWindowHeight() + 50),
                new (_game.GetWindowWidth()+50, -50),
                new (_game.GetWindowWidth()+50, _game.GetWindowHeight() + 50)
            };

            _enemies.Add(new Enemy(_enemySpawnPoint[_randomGenerator.Next(0, 4)], 2.5F, _game));
        }
    }
}