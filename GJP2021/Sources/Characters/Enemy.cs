using System;
using GJP2021.Sources.Paint;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GJP2021.Sources.Characters
{
    public class Enemy
    {
        private Vector2 _position;
        private readonly float _speed;
        public bool MarkedForDelete;
        private readonly PaintPeriodicSpawner _periodicPaintSpawner;


        public Enemy(Vector2 position, float speed)
        {
            _speed = speed;
            _position = position;
            _periodicPaintSpawner =
                new PaintPeriodicSpawner(new Color(255, 255, 255), new Color(0, 0, 0), 0, 20, 20, 0.05F, 0.1F, 120);

            MarkedForDelete = false;
        }

        public void Update(GameTime gameTime, Vector2 playerPos, PaintCircles paintCircles)
        {
            var (x, y) = playerPos;
            Update(gameTime, x, y, paintCircles);
        }

        public void Update(GameTime gameTime, float playerPosX, float playerPosY, PaintCircles paintCircles)
        {
            _periodicPaintSpawner.Update(gameTime, paintCircles, _position);
            var delta = (float) gameTime.ElapsedGameTime.TotalSeconds;
            var width = Math.Abs(_position.X - playerPosX);
            var height = Math.Abs(_position.Y - playerPosY);
            var h = (float) Math.Sqrt(Math.Pow(width, 2) + Math.Pow(height, 2));
            if (Math.Abs(playerPosX - _position.X) < 20 && Math.Abs(playerPosY - _position.Y) < 20)
            {
                MarkedForDelete = true;
            }

            if (playerPosX > _position.X)
            {
                _position.X += _speed * (width / h) * delta;
            }

            else if (playerPosX < _position.X)
            {
                _position.X -= _speed * (width / h) * delta;
            }

            if (playerPosY > _position.Y)
            {
                _position.Y += _speed * (height / h) * delta;
            }
            else if (playerPosY < _position.Y)
            {
                _position.Y -= _speed * (height / h) * delta;
            }
        }

        public void Draw(GameTime gameTime)
        {
            var texture = GetTexture();
            Kolori.Instance.SpriteBatch.Draw(GetTexture(), _position - new Vector2(texture.Width / 2F, texture.Height / 2F), null,
                Color.White);
        }

        private Texture2D GetTexture()
        {
            return Kolori.Instance.TextureMap["eraser"];
        }
    }
}