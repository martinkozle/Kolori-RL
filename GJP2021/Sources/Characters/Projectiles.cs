using System.Collections.Generic;
using GJP2021.Sources.GameStates;
using GJP2021.Sources.Paint;
using Microsoft.Xna.Framework;

namespace GJP2021.Sources.Characters
{
    public class Projectiles
    {
        private readonly List<Projectile> _projectiles;

        public Projectiles()
        {
            _projectiles = new List<Projectile>();
        }

        public void Update(GameTime gameTime, IngameState gameState, PaintCircles paintCircles, float timeScale)
        {
            foreach (var projectile in _projectiles)
            {
                projectile.Update(gameTime, gameState, paintCircles, timeScale);
            }

            _projectiles.RemoveAll(p => p.IsDone());
        }

        public void Add(Projectile projectile)
        {
            _projectiles.Add(projectile);
        }
    }
}