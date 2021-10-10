using System.Collections.Generic;
using GJP2021.Sources.Characters;
using GJP2021.Sources.Paint;

namespace GJP2021.Sources.Abilities
{
    public abstract class IAbility
    {
        public static readonly Dictionary<PaintColors, IAbility> Abilities = new();

        static IAbility()
        {
            Abilities.Add(PaintColors.RED, BurstAbility.Instance);
        }
        
        protected abstract void Use(Player player, PaintCircles paintCircles);
        public abstract bool CanUse(Player player);
        public abstract int PaintCost { get; }

        public void TryUse(Player player, PaintCircles paintCircles)
        {
            if (CanUse(player))
            {
                Use(player, paintCircles);
            }
        }
    }
    
}