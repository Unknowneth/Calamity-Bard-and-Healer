using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalamityBardHealer.Projectiles
{
	public class ArcticReinforcement : ModProjectile
	{
		public override void SetDefaults() {
			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.aiStyle = -1;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 2;
			DrawHeldProjInFrontOfHeldItemAndArms = true;
		}
		public override void AI() {
			Player player = Main.player[Projectile.owner];
			float attackTime = --Projectile.ai[0] / Projectile.ai[1];
			if(Projectile.ai[0] < 1) {
				player.compositeFrontArm.enabled = false;
				Projectile.Kill();
			}
			else {
				Projectile.rotation = MathHelper.SmoothStep(MathHelper.PiOver4, -MathHelper.PiOver2, attackTime) * player.direction;
				Projectile.timeLeft = 2;
				player.heldProj = Projectile.whoAmI;
				player.compositeFrontArm.enabled = true;
				player.compositeFrontArm.rotation = -MathHelper.SmoothStep(0f, MathHelper.PiOver2 + MathHelper.PiOver4, attackTime) * player.direction;
				if(Projectile.ai[2] > 480f) player.compositeFrontArm.rotation -= MathHelper.PiOver2 * player.direction;
				player.compositeFrontArm.stretch = (attackTime > 0.75f || attackTime < 0.25f) ? Player.CompositeArmStretchAmount.Quarter : (attackTime > 0.66f || attackTime < 0.33f) ? Player.CompositeArmStretchAmount.ThreeQuarters : Player.CompositeArmStretchAmount.Full;
			}
			Projectile.Center = player.MountedCenter - player.Directions * 4f + (Projectile.rotation + (Projectile.ai[2] > 480f ? -MathHelper.PiOver4 : MathHelper.PiOver4) * player.direction).ToRotationVector2() * (MathHelper.SmoothStep(12f, 16f, Vector2.UnitX.RotatedBy(attackTime * MathHelper.Pi).Y) + (Projectile.ai[2] > 480f ? 6f : 2f)) * player.direction;
			Projectile.spriteDirection = player.direction;
		}
		public override bool? CanDamage() => false;
		public override bool ShouldUpdatePosition() => false;
	}
}