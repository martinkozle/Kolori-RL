using System;
using Microsoft.Xna.Framework;

namespace GJP2021.Sources.Paint
{
    public class PaintSpawner
    {
        private Color _color;
        private readonly float _maxDistance, _minSize, _maxSize, _dropDuration, _fadeDuration;
        private readonly Random _random;

        public PaintSpawner(Color color, float maxDistance, float minSize, float maxSize, float dropDuration = 0.2f,
            float fadeDuration = 5)
        {
            _color = color;
            _maxDistance = maxDistance;
            _minSize = minSize;
            _maxSize = maxSize;
            _dropDuration = dropDuration;
            _fadeDuration = fadeDuration;
            _random = new Random();
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
            return new PaintCircle(x + xOffset, y + yOffset, radius, _color, _dropDuration, _fadeDuration);
        }
    }
}