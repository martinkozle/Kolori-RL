using GJP2021.Sources.Characters;
using GJP2021.Sources.GameStates;

namespace GJP2021.Sources.Abilities
{
    class DashAbility : Ability
    {
        protected override PaintColors AbilityColor => PaintColors.GREEN;
        public override float PaintCost => 5F;
        public static readonly DashAbility Instance = new();

        protected override bool Use(Player player, IngameState gameState)
        {
            Kolori.Instance.SoundMap["speedup_ability"].Play();
            player.SetSpeedBoost(3, 1);
            return true;
        }
    }
}