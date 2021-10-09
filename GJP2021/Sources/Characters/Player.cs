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
        public Vector2 Position;
        private float _health = 100F;
        private const float MaxHealth = 100F;
        private static Texture2D GetHealthTexture() => Kolori.Instance.TextureMap["healthbar"];
        private PaintColors _playerColor;
        private readonly PaintCircles _paintCircles;
        private readonly PaintPeriodicSpawner _periodicPaintSpawner;
        private readonly Random _randomGenerator = new();

        private Player(float x, float y, float speed, float maxAcceleration, Vector2 bounds)
        {
            Position = new Vector2(x, y);
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

        public void DrawHealth(SpriteBatch spriteBatch)
        {
            var texture = GetHealthTexture();
            const int x = 16;
            var y = Kolori.Instance.GetWindowHeight() - 92 - 16;

            var colorIndex = Array.IndexOf(Enum.GetValues(_playerColor.GetType()), _playerColor);
            var colorOffset = colorIndex * 38;

            //Empty Health Bar
            spriteBatch.Draw(texture, new Vector2(x, y), new Rectangle(0, 0, 288, 88), Color.White);
            
            //Bucket fluid
            spriteBatch.Draw(texture, new Vector2(x + 40, y + 12), new Rectangle(238, 88 + colorOffset, 26, 36), Color.White);
            
            //Health
            var healthPercent = (int) Math.Floor(238F * (_health / MaxHealth));
            spriteBatch.Draw(texture, new Vector2(x + 46, y + 46), new Rectangle(0, 88 + colorOffset, healthPercent, 38), Color.White);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(GetTexture(), Position, Color.White);
            DrawHealth(spriteBatch);
        }
        
      public void Update(GameTime gameTime)
        {
            var texture = GetTexture();
            var x = Position.X + texture.Width / 2F;
            var y = Position.Y + texture.Height / 2F;
            _paintCircles.Update(gameTime);

            _periodicPaintSpawner.Update(gameTime, _paintCircles, new Vector2(x, y));

            HandleInput();

            Position.Y = Math.Clamp(Position.Y, 0, _bounds.Y - 32F);
            Position.X = Math.Clamp(Position.X, 0, _bounds.X - 32F);

        }

        private void HandleInput()
        {
            var keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.A))
            {
                Position.X -= _speed;
            }

            if (keyState.IsKeyDown(Keys.S))
            {
                Position.Y += _speed;
            }

            if (keyState.IsKeyDown(Keys.D))
            {
                Position.X += _speed;
            }

            if (keyState.IsKeyDown(Keys.W))
            {
                Position.Y -= _speed;
            }
        }
        
        private Texture2D GetTexture() => Kolori.Instance.TextureMap["player_" + _playerColor.ToString().ToLower()];
        
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