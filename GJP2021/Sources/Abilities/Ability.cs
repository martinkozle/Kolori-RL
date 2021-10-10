using System.Collections.Generic;
using GJP2021.Sources.Characters;
using GJP2021.Sources.Paint;

namespace GJP2021.Sources.Abilities
{
    public abstract class Ability
    {
        public static readonly Dictionary<PaintColors, Ability> Abilities = new();

        protected abstract PaintColors AbilityColor { get; }

        static Ability()
        {
            Abilities.Add(PaintColors.RED, BurstAbility.Instance);
            Abilities.Add(PaintColors.PURPLE, TeleportAbility.Instance);
        }

        protected abstract bool Use(Player player, PaintCircles paintCircles);


        private protected bool CanUse(Player player)
        {
            return AbilityColor == player.TrailColor && player.Health >= PaintCost + 5;
        }

        public abstract float PaintCost { get; }

        public void TryUse(Player player, PaintCircles paintCircles)
        {
            if (CanUse(player) && Use(player, paintCircles))
            {
                player.Damage(PaintCost);
            }
        }
    }
    
}