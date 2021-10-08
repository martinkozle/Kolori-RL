using System;
using Microsoft.Xna.Framework;

namespace GJP2021.Sources.Characters
{
    public class Enemy
    {
        private float _xPos;
        private float _yPos;
        private float _speed;
        private readonly Kolori _game;

        public Enemy(float xPos, float yPos, float speed, Kolori game)
        {
            _speed = speed;
            _xPos = xPos;
            _yPos = yPos;
            _game = game;
        }

        public void Update(GameTime gameTime, float playerPosX, float playerPosY)
        {
            var delta = gameTime.ElapsedGameTime.Milliseconds/1000F *60;
            var width = Math.Abs(_xPos - playerPosX);
            var height = Math.Abs(_yPos - playerPosY);
            var h = (float)Math.Sqrt(Math.Pow(width, 2) + Math.Pow(height, 2));
            if (playerPosX == _xPos && playerPosY == _yPos)
            {
                return;
            }
            if (playerPosX > _xPos)
            {
                _xPos = _xPos + _speed * (width / h)*delta;
            }
            //this.yPos += this.speed * (width / (width + height));
            //this.xPos += this.speed * (height / (width + height));

            else if (playerPosX < _xPos)
            {
                _xPos = _xPos - _speed * (width / h)*delta;
            }

//
            if (playerPosY > _yPos)
            {
                _yPos += _speed * (height / h)*delta;
            }
//
            else if (playerPosY < _yPos)
            {
                _yPos -= _speed * (height / h)*delta;
            }

            //this.xPos = MathHelper.Lerp(this.xPos, playerPosX, this.lerp);
            //this.yPos = MathHelper.Lerp(this.yPos, playerPosY, this.lerp);
        }

        public void Draw(GameTime gameTime)
        {
            _game.SpriteBatch.Draw(Kolori.TextureMap["eraser"], new Vector2(_xPos, _yPos), null,
                Color.White);
        }
        
    }
}