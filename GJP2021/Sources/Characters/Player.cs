using System;
using System.Collections.Generic;
using Apos.Shapes;
using GJP2021.Content.Resources.Textures;
using GJP2021.Sources.Paint;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GJP2021.Sources.Characters
{
    public class Player
    {
        

        private readonly float _speed;
        private readonly float _maxAcceleration;
        private readonly Vector2 _bounds;
        private Vector2 _position;
        private PaintColors _playerColor;
        private Texture2D _playerTexture2D;
        private readonly PaintCircles _paintCircles;
        private readonly PaintPeriodicSpawner _periodicPaintSpawner;
        private readonly Random _randomGenerator = new Random();

        private Player(float x, float y, float speed, float maxAcceleration, Vector2 bounds)
        {
            _position = new Vector2(x, y);
            _speed = speed;
            _maxAcceleration = maxAcceleration;
            _bounds = bounds;
            _paintCircles = new PaintCircles();
            _periodicPaintSpawner =
                new PaintPeriodicSpawner(PaintCircle.Red, new Color(128, 64, 32), 25, 5, 20, 0.05F, 0.1F, 5);
            _playerColor = (PaintColors) Enum.GetValues(typeof(PaintColors))
                .GetValue(
                    _randomGenerator.Next(
                        Enum.GetValues(typeof(PaintColors)).Length)
                );
            GetTexture();
        }

        public Vector2 GetPos()
        {
            return _position;
        }

        public void Update(GameTime gameTime)
        {
            var texture = GetTexture();
            var x = _position.X + texture.Width / 2F;
            var y = _position.Y + texture.Height / 2F;
            _paintCircles.Update(gameTime);
            _periodicPaintSpawner.Update(gameTime, _paintCircles, new Vector2(x, y));

            HandleInput();

            _position.Y = Math.Clamp(_position.Y, 0, _bounds.Y - 32F);
            _position.X = Math.Clamp(_position.X, 0, _bounds.X - 32F);
        }

        private void HandleInput()
        {
            var keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.A))
            {
                _position.X -= _speed;
            }

            if (keyState.IsKeyDown(Keys.S))
            {
                _position.Y += _speed;
            }

            if (keyState.IsKeyDown(Keys.D))
            {
                _position.X += _speed;
            }

            if (keyState.IsKeyDown(Keys.W))
            {
                _position.Y -= _speed;
            }
        }

        public void DrawPositioned(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(GetTexture(), _position, Color.White);
        }

        private Texture2D GetTexture()
        {
            _playerTexture2D = _playerColor switch
            {
                PaintColors.BLUE => Kolori.TextureMap["player_blue"],
                PaintColors.RED => Kolori.TextureMap["player_red"],
                PaintColors.GREEN => Kolori.TextureMap["player_green"],
                PaintColors.ORANGE => Kolori.TextureMap["player_orange"],
                PaintColors.PINK => Kolori.TextureMap["player_pink"],
                PaintColors.PURPLE => Kolori.TextureMap["player_purple"],
                PaintColors.YELLOW => Kolori.TextureMap["player_yellow"],
                _ => _playerTexture2D
            };
            return _playerTexture2D;
        }

        public void setColor(PaintColors playerColor)
        {
            _playerColor = playerColor;
            GetTexture();
        }

        public void DrawShapeBatch(ShapeBatch batch)
        {
            _paintCircles.Draw(batch);
        }

        public static PlayerBuilder Builder()
        {
            return new ();
        }

        public class PlayerBuilder
        {
            private float _x;
            private float _y;
            private float _speed;
            private float _maxAcceleration;
            private Vector2 _bounds;

            internal PlayerBuilder()
            {
                _x = 0;
                _y = 0;
                _speed = 0f;
                _maxAcceleration = 0f;
            }

            public PlayerBuilder SetPosition(float x, float y)
            {
                _x = x;
                _y = y;
                return this;
            }

            public PlayerBuilder SetMaxAcceleration(float maxAcceleration)
            {
                _maxAcceleration = maxAcceleration;
                return this;
            }

            public PlayerBuilder SetSpeed(float speed)
            {
                _speed = speed;
                return this;
            }

            public PlayerBuilder SetBounds(Vector2 bounds)
            {
                _bounds = bounds;
                return this;
            }

            public Player Build()
            {
                return new (_x, _y, _speed, _maxAcceleration, _bounds);
            }
        }
    }
}