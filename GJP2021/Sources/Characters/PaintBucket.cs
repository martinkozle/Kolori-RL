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
        private Texture2D _texture2D;
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
            setColor();
            MarkedForDeleteion = false;
        }

        private void setColor()
        {
            _texture2D = _color switch
            {
                PaintColors.BLUE => Kolori.TextureMap["blue_bucket"],
                PaintColors.RED => Kolori.TextureMap["red_bucket"],
                PaintColors.GREEN => Kolori.TextureMap["green_bucket"],
                PaintColors.ORANGE => Kolori.TextureMap["orange_bucket"],
                PaintColors.PINK => Kolori.TextureMap["pink_bucket"],
                PaintColors.PURPLE => Kolori.TextureMap["purple_bucket"],
                PaintColors.YELLOW => Kolori.TextureMap["yellow_bucket"],
                _ => _texture2D
            };
        }

        public PaintColors GetPaintBucketColor()
        {
            return _color;
        }

        public void Update(GameTime gameTime, Vector2 playerPos)
        {
            var distance = new Vector2(Math.Abs(_position.X - playerPos.X), Math.Abs(_position.Y - playerPos.Y));
            if (distance.X < 20 && distance.Y < 20)
            {
                MarkedForDeleteion = true;
            }
        }

        public void Draw(GameTime gameTime)
        {
            _game.SpriteBatch.Draw(_texture2D, _position,Color.White);
        }
    }
}