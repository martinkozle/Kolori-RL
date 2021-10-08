using System;
using System.Collections.Generic;
using System.Linq;
using GJP2021.Sources.Characters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GJP2021.Sources.GameStates
{
    public class IngameState : IGameState
    {
        public static readonly IngameState Instance = new ();
        private Kolori _game;
        private Player _player;
        private static readonly Color BgColor = new (115F/255F, 190F/255F, 211F/255F);
        private int lastSpawnEnemy = 0;

        private List<Enemy> _enemies = new List<Enemy>();
        //private Enemy _enemy;


        public void Update(GameTime gameTime)
        {
            //_enemy.Update(gameTime,Mouse.GetState().X,Mouse.GetState().Y);
            if (Mouse.GetState().LeftButton == ButtonState.Pressed &&
                gameTime.TotalGameTime.Seconds - lastSpawnEnemy >= 0.1)
            {
                _enemies.Add(new Enemy(Mouse.GetState().X+new Random().Next(100), Mouse.GetState().Y+new Random().Next(100), 2.5F, _game));
                lastSpawnEnemy = gameTime.TotalGameTime.Seconds;
            }

            foreach (var enemy in _enemies)
            {
                enemy.Update(gameTime, Mouse.GetState().X, Mouse.GetState().Y);
            }
            // _enemy.Update(gameTime,Mouse.GetState().X,Mouse.GetState().Y);

            _player.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            _game.GraphicsDevice.Clear(BgColor);

            _game.SpriteBatch.Begin();

            //_enemy.Draw(gameTime);
            foreach (var enemy in _enemies)
            {
                enemy.Draw(gameTime);
            }

            //_enemy.Draw(gameTime);
            //TODO DRAWING GOES HERE
            _player.DrawPositioned(_game.SpriteBatch);

            _game.SpriteBatch.End();
        }

        public void Initialize(Kolori game)
        {
            _game = game;
            //_enemy= new Enemy(100, 100, 10, _game);
            _enemies.Add(new Enemy(Mouse.GetState().X+new Random().Next(100), Mouse.GetState().Y+new Random().Next(100), 2.5F, _game));
            //_enemy=new Enemy(200,200,3F,game,0.025F);
            _player = Player.Builder()
                .SetPosition(0, 0)
                .SetSpeed(2f)
                .SetMaxAcceleration(6f)
                .SetBounds(new Vector2(_game.Graphics.PreferredBackBufferWidth,
                    _game.Graphics.PreferredBackBufferHeight))
                .Build();
        }
    }
}