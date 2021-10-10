using System;
using System.Collections.Generic;
using LilyPath;
using Microsoft.Xna.Framework;

namespace GJP2021.Sources.Paint
{
    public class PaintCircle
    {
        public static readonly Color Red = new(230, 0, 0);
        public static readonly Dictionary<PaintColors, Color> ColorMap = new();

        private readonly Vector2 _center;
        private readonly float _radius;
        private readonly float _r;
        private readonly float _g;
        private readonly float _b;
        private readonly float _dropDuration;
        private readonly float _fadeDuration;
        private readonly bool _fade;
        private float _alpha;
        private float _currentDuration;
        private float _currentRadius;

        static PaintCircle()
        {
            ColorMap.Add(PaintColors.BLUE, new Color(66, 191, 232));
            ColorMap.Add(PaintColors.GREEN, new Color(168, 202, 88));
            ColorMap.Add(PaintColors.ORANGE, new Color(235, 136, 45));
            ColorMap.Add(PaintColors.PINK, new Color(255, 94, 135));
            ColorMap.Add(PaintColors.PURPLE, new Color(141, 51, 184));
            ColorMap.Add(PaintColors.RED, new Color(211, 24, 28));
            ColorMap.Add(PaintColors.YELLOW, new Color(252, 192, 47));
        }

        public PaintCircle(float x, float y, float radius, Color color, float dropDuration = 0.05f,
            float fadeDuration = 5, bool fade = true)
        {
            _center = new Vector2(x, y);
            _radius = radius;
            _alpha = 1;
            _r = color.R / 255F;
            _g = color.G / 255F;
            _b = color.B / 255F;
            _dropDuration = dropDuration;
            _fadeDuration = fadeDuration;
            _fade = fade;
            _currentDuration = 0;
        }

        public bool IsDone()
        {
            return _currentDuration >= _fadeDuration;
        }

        public void Update(GameTime gameTime)
        {
            _currentDuration += (float) gameTime.ElapsedGameTime.TotalSeconds;
            if (_fade)
            {
                _alpha = Math.Max((_fadeDuration - _currentDuration) / _fadeDuration, 0);
            }
            _currentRadius = _radius * Math.Min(1, _currentDuration / _dropDuration);
        }

        public void Draw(DrawBatch batch)
        {
            var color = new Color(_r, _g, _b, _alpha);
            // batch.DrawCircle(_center, _currentRadius, color, Color.White); 
            // batch.FillCircle(_center, _currentRadius, color);
            batch.FillCircle(new SolidColorBrush(color), _center, _currentRadius);
        }
    }
}