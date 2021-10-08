using System;
using Microsoft.Xna.Framework;

namespace GJP2021.Sources.Paint
{
    public class PaintCircle
    {
        public static readonly Color Red = new(255, 0, 0);

        private readonly Vector2 _center;
        private readonly float _radius, _r, _g, _b, _dropDuration, _fadeDuration;
        private float _alpha, _currentDuration, _currentRadius;

        public PaintCircle(float x, float y, float radius, Color color, float dropDuration = 0.05f,
            float fadeDuration = 5)
        {
            _center = new Vector2(x, y);
            _radius = radius;
            _alpha = 1;
            _r = color.R;
            _g = color.G;
            _b = color.B;
            _dropDuration = dropDuration;
            _fadeDuration = fadeDuration;
            _currentDuration = 0;
        }

        public PaintCircle(float x, float y, float radius, float r, float g, float b)
        {
            _center = new Vector2(x, y);
            _radius = radius;
            _currentRadius = 0;
            _alpha = 1;
            _r = r;
            _g = g;
            _b = b;
        }

        public bool IsDone()
        {
            return _currentDuration >= _fadeDuration;
        }

        public void Update(GameTime gameTime)
        {
            _currentDuration += (float) gameTime.ElapsedGameTime.TotalSeconds;
            _alpha = Math.Max((_fadeDuration - _currentDuration) / _fadeDuration, 0);
            _currentRadius = _radius * Math.Min(1, _currentDuration / _dropDuration);
        }

        public void Draw(Kolori game)
        {
            var color = new Color(_r, _g, _b, _alpha);
            game.ShapeBatch.DrawCircle(_center, _currentRadius, color, color);
        }
    }
}