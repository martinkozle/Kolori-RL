using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GJP2021.Sources.Characters
{
    internal class Player
    {
        public enum PlayerColor
        {
            BLUE,
            GREEN,
            ORANGE,
            PINK,
            PURPLE,
            RED,
            YELLOW
        }

        private readonly float _speed;
        private readonly float _maxAccel;
        private readonly Vector2 _bounds;
        private Vector2 _position;
        private readonly Action _action;
        private PlayerColor _playerColor = PlayerColor.RED;

        private Player(float x, float y, Action action, float speed, float maxAccel, Vector2 bounds)
        {
            _position = new Vector2(x, y);
            _action = action;
            _speed = speed;
            _maxAccel = maxAccel;
            _bounds = bounds;
        }

        public void Update(GameTime gameTime)
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

            if (_position.Y >= _bounds.Y - 32f)
                _position.Y = _bounds.Y - 32f;
            if (_position.X >= _bounds.X - 32f)
                _position.X = _bounds.X - 32f;
            if (_position.Y <= 0)
                _position.Y = 0;
            if (_position.X <= 0)
                _position.X = 0;
        }

        public void DrawPositioned(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Kolori.TextureMap["player_" + _playerColor.ToString().ToLower()], _position, Color.White);
        }
        public static PlayerBuilder Builder()
        {
            return new();
        }
        public class PlayerBuilder
        {
            private float _x;
            private float _y;
            private float _speed;
            private float _maxAccel;
            private Vector2 _bounds;
            private Action _action;

            internal PlayerBuilder()
            {
                _x = 0;
                _y = 0;
                _speed = 0f;
                _maxAccel = 0f;
                _action = () => { };
            }

            public PlayerBuilder SetPosition(float x, float y)
            {
                _x = x;
                _y = y;
                return this;
            }

            public PlayerBuilder SetMaxAcceleration(float maxAccel)
            {
                _maxAccel = maxAccel;
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

            public PlayerBuilder SetAction(Action action)
            {
                _action = action;
                return this;
            }
            
            public Player Build()
            {
                return new(_x, _y, _action, _speed, _maxAccel, _bounds);
            }
        }
    }
}
