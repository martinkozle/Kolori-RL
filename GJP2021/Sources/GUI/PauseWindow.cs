using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GJP2021.Sources.GUI
{
    public class WindowWidget
    {
        private readonly List<Button> _buttons;
        private readonly Func<int> _x;
        private readonly Func<int> _y;
        private readonly Texture2D _texture;

        public WindowWidget(Func<int> x, Func<int> y, Texture2D texture, List<Button> buttons)
        {
            _x = x;
            _y = y;
            _texture = texture;
            _buttons = buttons;
        }
        
        public void Update()
        {
            if (_texture == null)
            {
                return;
            }

            foreach (var button in _buttons)
            {
                button.Update();
            }
        }

        public void Draw()
        {
            if (_texture == null)
            {
                return;
            }

            Kolori.Instance.SpriteBatch.Draw(_texture, GetPosition(), Color.White);
                
            foreach (var button in _buttons)
            {
                button.Draw();
            }
        }

        private Vector2 GetPosition() => new(_x.Invoke(), _y.Invoke());

        public static WindowWidgetBuilder Builder() => new();

        public class WindowWidgetBuilder
        {
            private Texture2D _texture;
            public Func<int> X { get; private set; }
            public Func<int> Y { get; private set; }
            private readonly List<Button> _buttons;

            internal WindowWidgetBuilder()
            {
                X = () => 0;
                Y = () => 0;
                _buttons = new List<Button>();
            }

            public WindowWidgetBuilder SetPosition(Func<int> x, Func<int> y)
            {
                X = x;
                Y = y;
                return this;
            }

            public WindowWidgetBuilder SetPosition(int x, int y)
            {
                X = () => x;
                Y = () => y;
                return this;
            }

            public WindowWidgetBuilder SetPosition(Vector2 position)
            {
                var (x, y) = position;
                X = () => (int)x;
                Y = () => (int)y;
                return this;
            }

            public WindowWidgetBuilder CenterHorizontally(Func<int> boundaryWidth)
            {
                var oldX = X;
                X = () => oldX.Invoke() + (boundaryWidth.Invoke() - _texture.Width) / 2;
                return this;
            }

            public WindowWidgetBuilder CenterHorizontally(int boundaryWidth)
            {
                var oldX = X;
                X = () => oldX.Invoke() + (boundaryWidth - _texture.Width) / 2;
                return this;
            }

            public WindowWidgetBuilder CenterVertically(Func<int> boundaryHeight)
            {
                var oldY = Y;
                Y = () => oldY.Invoke() + (boundaryHeight.Invoke() - _texture.Height) / 2;
                return this;
            }

            public WindowWidgetBuilder CenterVertically(int boundaryHeight)
            {
                var oldY = Y;
                Y = () => oldY.Invoke() + (boundaryHeight - _texture.Height) / 2;
                return this;
            }

            public WindowWidgetBuilder SetTexture(Texture2D texture)
            {
                _texture = texture;
                return this;
            }

            public WindowWidgetBuilder AddButton(Button button)
            {
                _buttons.Add(button);
                return this;
            }

            public WindowWidget Build() => new(X, Y, _texture, _buttons);
        }
    }
}