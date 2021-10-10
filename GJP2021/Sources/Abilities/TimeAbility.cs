using GJP2021.Sources.Characters;
using GJP2021.Sources.GameStates;

namespace GJP2021.Sources.Abilities
{
    public class TimeAbility : Ability
    {
        protected override PaintColors AbilityColor => PaintColors.YELLOW;
        public override float PaintCost => 10;
        public static readonly TimeAbility Instance = new();

        protected override bool Use(Player player, IngameState gameState)
        {
            gameState.TimeScaleActive = true;
            gameState.TimeScale = 0.5F;
            gameState.TimeScaleDuration = 0;
            Kolori.Instance.SoundMap["timestop_ability"].Play();
            return true;
        }

        
    }
}