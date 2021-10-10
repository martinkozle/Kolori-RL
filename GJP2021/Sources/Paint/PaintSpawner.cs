using System;
using Microsoft.Xna.Framework;

namespace GJP2021.Sources.Paint
{
    public class PaintSpawner
    {
        private Color _color;
        private readonly float _maxDistance, _minSize, _maxSize, _dropDuration, _fadeDuration;
        private readonly bool _fade;
        private readonly Random _random;
        private readonly Color _colorRanges;

        public PaintSpawner(Color color, Color colorRanges, float maxDistance, float minSize, float maxSize,
            float dropDuration = 0.2f,
            float fadeDuration = 5, bool fade = true)
        {
            _color = color;
            _maxDistance = maxDistance;
            _minSize = minSize;
            _maxSize = maxSize;
            _dropDuration = dropDuration;
            _fadeDuration = fadeDuration;
            _random = new Random();
            _colorRanges = colorRanges;
            _fade = fade;
        }

        public void SetColor(Color color)
        {
            _color = color;
        }

        public PaintCircle SpawnCircle(float x, float y)
        {
            var angle = (float) _random.NextDouble() * 2 * Math.PI;
            var distance = (float) _random.NextDouble() * _maxDistance;
            var xOffset = (float) Math.Cos(angle) * distance;
            var yOffset = (float) Math.Sin(angle) * distance;
            var radius = (float) _random.NextDouble() * (_maxSize - _minSize) + _minSize;
            var r = Math.Clamp(_color.R + _random.Next(_colorRanges.R + 1) - _colorRanges.R / 2, 0, 255);
            var g = Math.Clamp(_color.G + _random.Next(_colorRanges.G + 1) - _colorRanges.G / 2, 0, 255);
            var b = Math.Clamp(_color.B + _random.Next(_colorRanges.B + 1) - _colorRanges.B / 2, 0, 255);
            var color = new Color(r, g, b);
            return new PaintCircle(x + xOffset, y + yOffset, radius, color, _dropDuration, _fadeDuration, _fade);
        }
    }
}