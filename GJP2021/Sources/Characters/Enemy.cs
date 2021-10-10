using System;
using GJP2021.Sources.Paint;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GJP2021.Sources.Characters
{
    public class Enemy
    {
        public Vector2 Position;
        private readonly float _speed;
        public bool MarkedForDeletion;
        public bool MarkedAsKilled;
        private readonly PaintPeriodicSpawner _periodicPaintSpawner;


        public Enemy(Vector2 position, float speed)
        {
            _speed = speed;
            Position = position;
            _periodicPaintSpawner =
                new PaintPeriodicSpawner(new Color(255, 255, 255), new Color(0, 0, 0), 0, 20, 20, 0.05F, 0.1F, 30,
                    false);

            MarkedForDeletion = false;
            MarkedAsKilled = false;
        }

        public void Update(GameTime gameTime, Player player, PaintCircles paintCircles, float timeScale)
        {
            var (playerPosX, playerPosY) = player.Position;
            _periodicPaintSpawner.Update(gameTime, paintCircles, Position);
            var delta = (float) gameTime.ElapsedGameTime.TotalSeconds * timeScale;
            var width = Math.Abs(Position.X - playerPosX);
            var height = Math.Abs(Position.Y - playerPosY);
            var h = (float) Math.Sqrt(Math.Pow(width, 2) + Math.Pow(height, 2));
            if (Math.Abs(playerPosX - Position.X) < 20 && Math.Abs(playerPosY - Position.Y) < 20)
            {
                player.Damage(10F);
                MarkedForDeletion = true;
            }

            if (playerPosX > Position.X)
            {
                Position.X += _speed * (width / h) * delta;
            }

            else if (playerPosX < Position.X)
            {
                Position.X -= _speed * (width / h) * delta;
            }

            if (playerPosY > Position.Y)
            {
                Position.Y += _speed * (height / h) * delta;
            }
            else if (playerPosY < Position.Y)
            {
                Position.Y -= _speed * (height / h) * delta;
            }
        }

        public void Kill()
        {
            MarkedForDeletion = true;
            MarkedAsKilled = true;
        }

        public void Draw(GameTime gameTime)
        {
            var texture = GetTexture();
            Kolori.Instance.SpriteBatch.Draw(GetTexture(),
                Position - new Vector2(texture.Width / 2F, texture.Height / 2F), null,
                Color.White);
        }

        private static Texture2D GetTexture()
        {
            return Kolori.Instance.TextureMap["eraser"];
        }
    }
}