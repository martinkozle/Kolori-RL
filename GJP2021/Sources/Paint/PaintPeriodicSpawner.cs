using Microsoft.Xna.Framework;

namespace GJP2021.Sources.Paint
{
    public class PaintPeriodicSpawner
    {
        private readonly PaintSpawner _paintSpawner;
        private readonly float _period;
        private float _timeSinceLastSpawn;
        
        public PaintPeriodicSpawner(Color color, Color colorRanges, float maxDistance, float minSize, float maxSize, float period,
            float dropDuration, float fadeDuration)
        {
            _period = period;
            _paintSpawner = new PaintSpawner(color, colorRanges, maxDistance, minSize, maxSize, dropDuration, fadeDuration);
            _timeSinceLastSpawn = 0;
        }

        public void SetColor(Color color)
        {
            _paintSpawner.SetColor(color);
        }

        public void Update(GameTime gameTime, PaintCircles paintCircles, float x, float y)
        {
            _timeSinceLastSpawn += (float) gameTime.ElapsedGameTime.TotalSeconds;
            if (_timeSinceLastSpawn < _period) return;
            _timeSinceLastSpawn -= _period;
            var paintCircle = _paintSpawner.SpawnCircle(x, y);
            paintCircles.Add(paintCircle);
        }

        public void Update(GameTime gameTime, PaintCircles paintCircles, Vector2 position)
        {
            var (x, y) = position;
            Update(gameTime, paintCircles, x, y);
        }
    }
}