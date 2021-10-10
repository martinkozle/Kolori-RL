using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GJP2021.Sources.Characters
{
    public class PaintBucket
    {
        private Vector2 _position;
        private PaintColors _color;
        private Random _randomGenerator;
        public bool MarkedForDeletion;

        public PaintBucket(Vector2 position, Random randomGenerator)
        {
            _position = position;
            _randomGenerator = randomGenerator;
            _color = new List<PaintColors>{
                PaintColors.BLUE,
                PaintColors.GREEN,
                PaintColors.PURPLE,
                PaintColors.RED,
                PaintColors.YELLOW
            }[randomGenerator.Next(5)];
            MarkedForDeletion = false;
        }

        private Texture2D GetTexture()
        {
            return Kolori.Instance.TextureMap[_color.ToString().ToLower() + "_bucket"];
        }

        public PaintColors GetPaintBucketColor()
        {
            return _color;
        }

        public void Update(GameTime gameTime, Vector2 playerPos)
        {
            var (playerX, playerY) = playerPos;
            var (x, y) = new Vector2(Math.Abs(_position.X - playerX), Math.Abs(_position.Y - playerY));
            if (x < 20 && y < 20)
            {
                MarkedForDeletion = true;
            }
        }

        public void Draw(GameTime gameTime)
        {
            var texture = GetTexture();
            Kolori.Instance.SpriteBatch.Draw(texture, _position - new Vector2(texture.Width / 2F, texture.Height / 2F),
                Color.White);
        }
    }
}