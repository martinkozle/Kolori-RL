using System;
using System.Diagnostics;
using GJP2021.Content.Resources.Textures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GJP2021.Sources.Characters
{
    public class PaintBucket
    {

        private Vector2 _position;
        private PaintColors _color;
        private Random _randomGenerator;
        public bool MarkedForDeleteion;
        private Kolori _game;

        public PaintBucket(Vector2 position, Random randomGenerator, Kolori game)
        {
            _game = game;
            _position = position;
            _randomGenerator = randomGenerator;
            _color = (PaintColors) Enum.GetValues(typeof(PaintColors))
                .GetValue(
                    randomGenerator.Next(
                        Enum.GetValues(typeof(PaintColors)).Length)
                );
            MarkedForDeleteion = false;
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
            var (x, y) = new Vector2(Math.Abs(_position.X - playerPos.X), Math.Abs(_position.Y - playerPos.Y));
            if (x < 20 && y < 20)
            {
                MarkedForDeleteion = true;
            }
        }

        public void Draw(GameTime gameTime)
        {
            _game.SpriteBatch.Draw(GetTexture(), _position,Color.White);
        }
    }
}