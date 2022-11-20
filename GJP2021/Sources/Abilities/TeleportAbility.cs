using GJP2021.Sources.Characters;
using GJP2021.Sources.GameStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GJP2021.Sources.Abilities
{
    public class TeleportAbility : Ability
    {
        protected override PaintColors AbilityColor => PaintColors.PURPLE;
        public override float PaintCost => 10;

        public static readonly TeleportAbility Instance = new();

        protected override bool Use(Player player, IngameState gameState)
        {
            var mousePos = Mouse.GetState().Position;
            var (mouseX, mouseY) = Mouse.GetState().Position;
            if (!Utils.IsInsideBox(mousePos, mouseX, mouseY, Kolori.Instance.GetWindowWidth(), Kolori.Instance.GetWindowHeight()))
            {
                return false;
            }

            Kolori.Instance.SoundMap["teleport_ability"].Play();
            player.Position = new Vector2(mouseX, mouseY);
            player.SetSpeed(0, 0);
            return true;
        }
    }
}