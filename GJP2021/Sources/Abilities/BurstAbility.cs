using System;
using System.Linq;
using GJP2021.Sources.Characters;
using GJP2021.Sources.GameStates;
using GJP2021.Sources.Paint;
using Microsoft.Xna.Framework;

namespace GJP2021.Sources.Abilities
{
    public class BurstAbility : Ability
    {
        public static readonly BurstAbility Instance = new();

        public override float PaintCost => 20;

        private readonly PaintSpawner _paintSpawner = new(
            PaintCircle.ColorMap[PaintColors.RED],
            new Color(64, 64, 64),
            150,
            30,
            70,
            0.5F,
            30);

        private readonly Random _random = new();

        private BurstAbility()
        {
        }

        protected override PaintColors AbilityColor => PaintColors.RED;

        protected override bool Use(Player player, IngameState gameState)
        {
            _paintSpawner.SetColor(PaintCircle.ColorMap[player.TrailColor]);
            var max = 16 + _random.Next(17);
            Kolori.Instance.SoundMap["burst_ability_" + (_random.Next(2) + 1)].Play();
            for (var i = 0; i < max; ++i)
            {
                gameState.PaintCircles.Add(_paintSpawner.SpawnCircle(player.Position));
            }

            foreach (var enemy in gameState.Enemies.Where(enemy =>
                Vector2.Distance(enemy.Position, player.Position) < 150))
            {
                enemy.Kill();
            }

            return true;
        }
    }
}