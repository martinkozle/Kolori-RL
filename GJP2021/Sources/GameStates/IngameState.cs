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
        private Random _randomGenerator;

        private readonly List<Enemy> _enemies = new ();

        private List<Vector2> _enemySpawnPoint;

        public void Update(GameTime gameTime)
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed &&
                gameTime.TotalGameTime.TotalSeconds - _lastSpawnEnemy >= 0.1)
            {
                _enemies.Add(new Enemy(_enemySpawnPoint[_randomGenerator.Next(0, 4)], 2.5F, _game));
                _lastSpawnEnemy = (float)gameTime.TotalGameTime.TotalSeconds;
            }

            _enemies.RemoveAll(el => el.MarkedForDelete);

            foreach (var enemy in _enemies)
            {
                enemy.Update(gameTime, _player.GetPos().X, _player.GetPos().Y);
            }

            _player.Update(gameTime);
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

            _player.DrawPositioned(_game.SpriteBatch);

            _game.SpriteBatch.End();
        }

        public void Initialize(Kolori game)
        {
            _randomGenerator = new Random();
            _game = game;
            _player = Player.Builder()
                .SetPosition(0, 0)
                .SetSpeed(2f)
                .SetMaxAcceleration(6f)
                .SetBounds(new Vector2(_game.GetWindowWidth(),
                    _game.GetWindowHeight()))
                .Build();
            _enemySpawnPoint = new List<Vector2>
            {
                new(-50, -50), 
                new(-50, _game.GetWindowHeight()+50),
                new(_game.GetWindowWidth()+50, -50),
                new(_game.GetWindowWidth()+50, _game.GetWindowHeight()+50)
            };

            _enemies.Add(new Enemy(_enemySpawnPoint[_randomGenerator.Next(0, 4)], 2.5F, _game));
        }
    }
}