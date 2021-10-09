using System;
using System.Collections.Generic;
using Apos.Shapes;
using GJP2021.Sources.Paint;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GJP2021.Sources.Characters
{
    public class Player
    {
        private float _speedX, _speedY;
        private readonly float _maxSpeed, _acceleration, _dragCoefficient, _dragConstant;
        private readonly Vector2 _bounds;
        public Vector2 Position;
        private float _health = 100F;
        private const float MaxHealth = 100F;
        private static Texture2D GetHealthTexture() => Kolori.Instance.TextureMap["healthbar"];
        private PaintColors _playerColor;
        private readonly PaintPeriodicSpawner _periodicPaintSpawner;
        private readonly Random _randomGenerator = new();

        private Player(float x, float y, float maxSpeed, float acceleration, Vector2 bounds)
        {
            Position = new Vector2(x, y);
            _maxSpeed = maxSpeed;
            _acceleration = acceleration;
            _bounds = bounds;
            _speedX = 0;
            _speedY = 0;
            _dragCoefficient = 0.5f;
            _dragConstant = 80;
            _periodicPaintSpawner =
                new PaintPeriodicSpawner(PaintCircle.Red, new Color(128, 64, 32), 35, 10, 30, 0.05F, 0.1F, 120);
            _playerColor = (PaintColors) Enum.GetValues(typeof(PaintColors))
                .GetValue(
                    _randomGenerator.Next(
                        Enum.GetValues(typeof(PaintColors)).Length)
                );
        }

        public Vector2 GetSpeedVector()
        {
            return new Vector2(_speedX, _speedY);
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
            var texture = GetTexture();
            spriteBatch.Draw(texture, Position - new Vector2(texture.Width / 2F, texture.Height / 2F),
                Color.White);
        }

        public void Update(GameTime gameTime, PaintCircles paintCircles)
        {
            var delta = (float) gameTime.ElapsedGameTime.TotalSeconds;
            _periodicPaintSpawner.Update(gameTime, paintCircles, Position);

            HandleAcceleration(gameTime);

            Position.X += _speedX * delta;
            Position.Y += _speedY * delta;

            var w = GetTexture().Width / 2F;
            var h = GetTexture().Height / 2F;

            if (Position.Y <= h || Position.Y >= _bounds.Y - h)
            {
                _speedY = 0;
            }

            if (Position.X <= w || Position.X >= _bounds.X - w)
            {
                _speedX = 0;
            }


            Position.Y = Math.Clamp(Position.Y, w, _bounds.Y - w);
            Position.X = Math.Clamp(Position.X, h, _bounds.X - h);
        }

        private void HandleAcceleration(GameTime gameTime)
        {
            var delta = (float) gameTime.ElapsedGameTime.TotalSeconds;
            var keyState = Keyboard.GetState();
            var a = keyState.IsKeyDown(Keys.A);
            var w = keyState.IsKeyDown(Keys.W);
            var s = keyState.IsKeyDown(Keys.S);
            var d = keyState.IsKeyDown(Keys.D);
            var mx = Convert.ToInt32(d) - Convert.ToInt32(a);
            var my = Convert.ToInt32(s) - Convert.ToInt32(w);
            var diagonalBias = 1F;
            if (mx != 0 && my != 0)
            {
                diagonalBias = 1 / (float) Math.Sqrt(2);
            }

            var ax = _acceleration * mx * diagonalBias;
            var ay = _acceleration * my * diagonalBias;

            if (mx == 0)
            {
                _speedX -= _speedX * (1 - _dragCoefficient) * delta;
                switch (_speedX)
                {
                    case > 0:
                        _speedX = Math.Max(0, _speedX - _dragConstant * delta);
                        break;
                    case < 0:
                        _speedX = Math.Min(0, _speedX + _dragConstant * delta);
                        break;
                }
            }

            if (my == 0)
            {
                _speedY -= _speedY * (1 - _dragCoefficient) * delta;
                switch (_speedY)
                {
                    case > 0:
                        _speedY = Math.Max(0, _speedY - _dragConstant * delta);
                        break;
                    case < 0:
                        _speedY = Math.Min(0, _speedY + _dragConstant * delta);
                        break;
                }
            }

            _speedX += ax * delta;
            _speedY += ay * delta;
            var biasX = Math.Abs(_speedX) / (float) Math.Sqrt(_speedX * _speedX + _speedY * _speedY);
            var biasY = Math.Abs(_speedY) / (float) Math.Sqrt(_speedX * _speedX + _speedY * _speedY);
            _speedX = Math.Clamp(_speedX, -_maxSpeed * biasX, _maxSpeed * biasX);
            _speedY = Math.Clamp(_speedY, -_maxSpeed * biasY, _maxSpeed * biasY);
        }
        
        private Texture2D GetTexture() => Kolori.Instance.TextureMap["player_" + _playerColor.ToString().ToLower()];
        
        public void SetColor(PaintColors playerColor)
        {
            _playerColor = playerColor;
        }

        public static PlayerBuilder Builder()
        {
            return new();
        }

        public class PlayerBuilder
        {
            private float _x;
            private float _y;
            private float _maxSpeed, _acceleration;
            private Vector2 _bounds;

            internal PlayerBuilder()
            {
                _x = 0;
                _y = 0;
                _maxSpeed = 0;
                _acceleration = 0;
            }

            public PlayerBuilder SetPosition(float x, float y)
            {
                _x = x;
                _y = y;
                return this;
            }

            public PlayerBuilder SetAcceleration(float acceleration)
            {
                _acceleration = acceleration;
                return this;
            }

            public PlayerBuilder SetMaxSpeed(float maxSpeed)
            {
                _maxSpeed = maxSpeed;
                return this;
            }

            public PlayerBuilder SetBounds(Vector2 bounds)
            {
                _bounds = bounds;
                return this;
            }

            public Player Build()
            {
                return new(_x, _y, _maxSpeed, _acceleration, _bounds);
            }
        }
    }
}