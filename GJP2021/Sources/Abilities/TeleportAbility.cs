using GJP2021.Sources.Characters;
using GJP2021.Sources.Paint;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GJP2021.Sources.Abilities
{

    public class TeleportAbility : IAbility
    {
        protected override PaintColors AbilityColor => PaintColors.PURPLE;
        public override float PaintCost => 10;
        public static readonly TeleportAbility Instance = new();

        protected override bool Use(Player player, PaintCircles paintCircles)
        {
            var mousePos = Mouse.GetState().Position;
            var (mouseX, mouseY) = Mouse.GetState().Position;
            if (!Utils.IsInsideBox(mousePos, mouseX, mouseY, Kolori.Instance.GetWindowWidth(), Kolori.Instance.GetWindowHeight()))
            {
                return false;
            }
            player.Position = new Vector2(mouseX, mouseY);
            return true;
        }

    }
    
}