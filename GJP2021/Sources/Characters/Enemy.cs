using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace GJP2021.Sources.Characters
{
    public class Enemy
    {
        private Vector2 _position;
        private readonly float _speed;
        private readonly Kolori _game;
        public bool markedForDelete;

        public Enemy(Vector2 position, float speed, Kolori game)
        {
            _speed = speed;
            _position = position;
            _game = game;
            markedForDelete = false;
        }

        public void Update(GameTime gameTime, float playerPosX, float playerPosY)
        {
            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds * 60;
            var width = Math.Abs(_position.X - playerPosX);
            var height = Math.Abs(_position.Y - playerPosY);
            var h = (float) Math.Sqrt(Math.Pow(width, 2) + Math.Pow(height, 2));
            if (Math.Abs(playerPosX - _position.X) < 20 && Math.Abs(playerPosY - _position.Y) < 20)
            {
                markedForDelete = true;
            }

            if (playerPosX > _position.X)
            {
                _position.X += _speed * (width / h) * delta;
            }
            //this.yPos += this.speed * (width / (width + height));
            //this.xPos += this.speed * (height / (width + height));

            else if (playerPosX < _position.X)
            {
                _position.X -= _speed * (width / h) * delta;
            }

//
            if (playerPosY > _position.Y)
            {
                _position.Y += _speed * (height / h) * delta;
            }
//
            else if (playerPosY < _position.Y)
            {
                _position.Y -= _speed * (height / h) * delta;
            }
        }

        public void Draw(GameTime gameTime)
        {
            _game.SpriteBatch.Draw(Kolori.TextureMap["eraser"], _position, null,
                Color.White);
        }
    }
}