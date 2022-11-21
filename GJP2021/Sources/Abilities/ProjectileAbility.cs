using System;
using GJP2021.Sources.Characters;
using GJP2021.Sources.GameStates;
using GJP2021.Sources.Paint;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GJP2021.Sources.Abilities
{
    class ProjectileAbility : Ability
    {
        protected override PaintColors AbilityColor => PaintColors.BLUE;
        public override float PaintCost => 10;

        public static readonly ProjectileAbility Instance = new();

        protected override bool Use(Player player, IngameState gameState)
        {
            Kolori.Instance.SoundMap["shoot_ability"].Play();
            var (mouseX, mouseY) = Mouse.GetState().Position;
            var (a, b) = (new Vector2(mouseX, mouseY) - player.Position);
            var angle = (float)Math.Atan2(b, a);
            gameState.Projectiles.Add(new Projectile(
                PaintCircle.ColorMap[player.TrailColor],
                player.Position,
                300F,
                angle,
                2F));
            return true;
        }
    }
}