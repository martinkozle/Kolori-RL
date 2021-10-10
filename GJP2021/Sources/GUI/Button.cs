using System;
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
        public Texture2D CurrentTexture { get; private set; }

        private Button(Func<int> x, Func<int> y, Texture2D normalTexture, Texture2D hoveredTexture,
            Texture2D pressedTexture, SoundEffect pressSound, SoundEffect releaseSound, Action action)
        {
            _x = x;
            _y = y;
            _normalTexture = normalTexture;
            _hoveredTexture = hoveredTexture;
            _pressedTexture = pressedTexture;
            CurrentTexture = _normalTexture;
            _pressSound = pressSound;
            _releaseSound = releaseSound;
            _action = action;
        }

        public void Update()
        {
            if (CurrentTexture == null)
            {
                return;
            }

            var mouseState = Mouse.GetState();
            if (Utils.IsInsideBox(mouseState.Position, GetPosition(),
                new Vector2(CurrentTexture.Width, CurrentTexture.Height)))
            {
                if (mouseState.LeftButton == ButtonState.Released)
                {
                    if (CurrentTexture == _pressedTexture)
                    {
                        _releaseSound.Play();
                        Click();
                    }

                    CurrentTexture = _hoveredTexture;
                }
                else if (CurrentTexture == _hoveredTexture)
                {
                    _pressSound.Play();
                    CurrentTexture = _pressedTexture;
                }
            }
            else
            {
                if (CurrentTexture == _pressedTexture)
                {
                    _releaseSound.Play();
                }

                CurrentTexture = _normalTexture;
            }
        }

        public void Draw()
        {
            if (CurrentTexture == null)
            {
                return;
            }

            Kolori.Instance.SpriteBatch.Draw(CurrentTexture, GetPosition(), Color.White);
        }

        private Vector2 GetPosition()
        {
            return new(_x.Invoke(), _y.Invoke());
        }

        public void Click()
        {
            _action.Invoke();
        }

        public static ButtonBuilder Builder() => new();

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
                _pressSound = null;
                _releaseSound = null;
                _action = () => { };
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
            public ButtonBuilder SetPosition(Vector2 position)
            {
                var (x, y) = position;
                _x = () => (int)x;
                _y = () => (int)y;
                return this;
            }
            
            public ButtonBuilder CenterHorizontally(Func<int> boundaryWidth)
            {
                var oldX = _x;
                _x = () => oldX.Invoke() + (boundaryWidth.Invoke() - _normalTexture.Width) / 2;
                return this;
            }

            public ButtonBuilder CenterHorizontally(int boundaryWidth)
            {
                var oldX = _x;
                _x = () => oldX.Invoke() + (boundaryWidth - _normalTexture.Width) / 2;
                return this;
            }

            public ButtonBuilder CenterVertically(Func<int> boundaryHeight)
            {
                var oldY = _y;
                _y = () => oldY.Invoke() + (boundaryHeight.Invoke() - _normalTexture.Height) / 2;
                return this;
            }

            public ButtonBuilder CenterVertically(int boundaryHeight)
            {
                var oldY = _y;
                _y = () => oldY.Invoke() + (boundaryHeight - _normalTexture.Height) / 2;
                return this;
            }

            public ButtonBuilder SetSound(string soundName)
            {
                if (!Kolori.Instance.SoundMap.ContainsKey(soundName + "_release") ||
                    !Kolori.Instance.SoundMap.ContainsKey(soundName + "_press")) return this;
                _pressSound = Kolori.Instance.SoundMap[soundName + "_press"];
                _releaseSound = Kolori.Instance.SoundMap[soundName + "_release"];

                return this;
            }

            public ButtonBuilder SetTexture(string textureName)
            {
                if (!Kolori.Instance.TextureMap.ContainsKey(textureName + "_button_normal") ||
                    !Kolori.Instance.TextureMap.ContainsKey(textureName + "_button_hover") ||
                    !Kolori.Instance.TextureMap.ContainsKey(textureName + "_button_pressed")) return this;
                _normalTexture = Kolori.Instance.TextureMap[textureName + "_button_normal"];
                _hoveredTexture = Kolori.Instance.TextureMap[textureName + "_button_hover"];
                _pressedTexture = Kolori.Instance.TextureMap[textureName + "_button_pressed"];

                return this;
            }

            public ButtonBuilder SetAction(Action action)
            {
                _action = action;
                return this;
            }

            public Button Build()
            {
                return new(_x, _y, _normalTexture, _hoveredTexture, _pressedTexture, _pressSound, _releaseSound,
                    _action);
            }

        }
    }
}