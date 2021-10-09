using System;
using System.Collections.Generic;
using Apos.Shapes;
using GJP2021.Sources.Characters;
using Microsoft.Xna.Framework;

namespace GJP2021.Sources.Paint
{
    public class PaintCircle
    {
        public static readonly Color Red = new(230, 0, 0);
        public static readonly Dictionary<Player.PlayerColor, Color> ColorMap = new();

        private readonly Vector2 _center;
        private readonly float _radius;
        private readonly float _r;
        private readonly float _g;
        private readonly float _b;
        private readonly float _dropDuration;
        private readonly float _fadeDuration;
        private float _alpha;
        private float _currentDuration;
        private float _currentRadius;

        static PaintCircle()
        {
            ColorMap.Add(Player.PlayerColor.RED, new Color(230, 10, 0));
        }
        
        public PaintCircle(float x, float y, float radius, Color color, float dropDuration = 0.05f, float fadeDuration = 5)
        {
            _center = new Vector2(x, y);
            _radius = radius;
            _alpha = 1;
            _r = color.R / 255F;
            _g = color.G / 255F;
            _b = color.B / 255F;
            _dropDuration = dropDuration;
            _fadeDuration = fadeDuration;
            _currentDuration = 0;
        }

        public PaintCircle(float x, float y, float radius, float r, float g, float b) : 
            this(x, y, radius, new Color(r, g, b, 1)) { }

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

        public void Draw(ShapeBatch batch)
        {
            var color = new Color(_r, _g, _b, _alpha);
            batch.DrawCircle(_center, _currentRadius, color, color);
        }
    }
}