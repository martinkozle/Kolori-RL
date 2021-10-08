﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GJP2021.Sources.GUI
{
    public class Button
    {
        private readonly Texture2D _normalTexture;
        private readonly Texture2D _hoveredTexture;
        private readonly Texture2D _pressedTexture;
        private readonly SoundEffect _pressSound;
        private readonly SoundEffect _releaseSound;
        private readonly Action _action;
        private readonly Func<int> _x;
        private readonly Func<int> _y;
        private Texture2D _currentTexture;

        private Button(Func<int> x, Func<int> y, Texture2D normalTexture, Texture2D hoveredTexture, Texture2D pressedTexture, SoundEffect pressSound, SoundEffect releaseSound, Action action)
        {
            _x = x;
            _y = y;
            _normalTexture = normalTexture;
            _hoveredTexture = hoveredTexture;
            _pressedTexture = pressedTexture;
            _currentTexture = _normalTexture;
            _pressSound = pressSound;
            _releaseSound = releaseSound;
            _action = action;
        }

        public void Update()
        {
            var mouseState = Mouse.GetState();
            if (Utils.IsInsideBox(mouseState.Position, GetPosition(), new Vector2(_currentTexture.Width, _currentTexture.Height)))
            {
                if (mouseState.LeftButton == ButtonState.Released)
                {
                    if (_currentTexture == _pressedTexture) {
                        _releaseSound.Play();
                        Click();
                    }
                    _currentTexture = _hoveredTexture;
                } else if (_currentTexture == _hoveredTexture)
                {
                    _pressSound.Play();
                    _currentTexture = _pressedTexture;
                }
            }
            else
            {
                if (_currentTexture == _pressedTexture)
                {
                    _releaseSound.Play();
                }
                _currentTexture = _normalTexture;
            }
        }
        
        public void DrawPositioned(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_currentTexture, GetPosition(), Color.White);
        }

        private Vector2 GetPosition()
        {
            return new(_x.Invoke(), _y.Invoke());
        }

        public void Click()
        {
            _action.Invoke();
        }

        public static ButtonBuilder Builder()
        {
            return new();
        }

        public class ButtonBuilder
        {
            private Texture2D _normalTexture;
            private Texture2D _hoveredTexture;
            private Texture2D _pressedTexture;
            private SoundEffect _pressSound;
            private SoundEffect _releaseSound;
            private Action _action;
            private Func<int> _x;
            private Func<int> _y;

            internal ButtonBuilder()
            {
                _x = () => 0;
                _y = () => 0;
                _normalTexture = Kolori.TextureMap["button_normal"];
                _hoveredTexture = Kolori.TextureMap["button_hover"];
                _pressedTexture = Kolori.TextureMap["button_pressed"];
                _pressSound = null;
                _releaseSound = null;
                _action = () => {};
            }
            
            public ButtonBuilder SetPosition(Func<int> x, Func<int> y)
            {
                _x = x;
                _y = y;
                return this;
            }

            public ButtonBuilder SetPosition(int x, int y)
            {
                _x = () => x;
                _y = () => y;
                return this;
            }
            
            public ButtonBuilder CenterHorizontally(Func<int> boundaryWidth)
            {
                _x = () => (boundaryWidth.Invoke() - _normalTexture.Width) / 2;
                return this;
            }

            public ButtonBuilder CenterHorizontally(int boundaryWidth)
            {
                _x = () => (boundaryWidth - _normalTexture.Width) / 2;
                return this;
            }

            public ButtonBuilder CenterVertically(Func<int> boundaryHeight)
            {
                _y = () => (boundaryHeight.Invoke() - _normalTexture.Height) / 2;
                return this;
            }

            public ButtonBuilder CenterVertically(int boundaryHeight)
            {
                _y = () => (boundaryHeight - _normalTexture.Height) / 2;
                return this;
            }
            
            public ButtonBuilder SetSound(string soundName)
            {
                if (!Kolori.SoundMap.ContainsKey(soundName + "_release") ||
                    !Kolori.SoundMap.ContainsKey(soundName + "_press")) return this;
                _pressSound = Kolori.SoundMap[soundName + "_press"];
                _releaseSound = Kolori.SoundMap[soundName + "_release"];

                return this;
            }

            public ButtonBuilder SetTexture(string textureName)
            {
                if (!Kolori.TextureMap.ContainsKey(textureName + "_normal") ||
                    !Kolori.TextureMap.ContainsKey(textureName + "_hover") ||
                    !Kolori.TextureMap.ContainsKey(textureName + "_pressed")) return this;
                _normalTexture = Kolori.TextureMap[textureName + "_normal"];
                _hoveredTexture = Kolori.TextureMap[textureName + "_hover"];
                _pressedTexture = Kolori.TextureMap[textureName + "_pressed"];

                return this;
            }

            public ButtonBuilder SetAction(Action action)
            {
                _action = action;
                return this;
            }

            public Button Build()
            {
                return new(_x, _y, _normalTexture, _hoveredTexture, _pressedTexture, _pressSound, _releaseSound, _action);
            }

        }
    }
}