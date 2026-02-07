using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalamityBardHealer.Projectiles
{
	public class ViolinceBow : ModProjectile
	{
		public override void SetStaticDefaults() => Terraria.ID.ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
		public override void SetDefaults() {
			Projectile.width = 38;
			Projectile.height = 20;
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
				if(Projectile.ai[0] < Projectile.ai[1] * 0.5f) Projectile.rotation = MathHelper.Lerp(MathHelper.PiOver4 * 0.5f * player.direction * player.gravDir, 0f, (Projectile.ai[0] / Projectile.ai[1]) * 2f);
				Projectile.timeLeft = 2;
				player.heldProj = Projectile.whoAmI;
				player.compositeFrontArm.enabled = true;
				player.compositeFrontArm.rotation = -(MathHelper.PiOver2 + Projectile.rotation * player.direction * player.gravDir) * player.direction;
				player.compositeFrontArm.stretch = (attackTime > 0.75f || attackTime < 0.25f) ? Player.CompositeArmStretchAmount.Quarter : (attackTime > 0.66f || attackTime < 0.33f) ? Player.CompositeArmStretchAmount.ThreeQuarters : Player.CompositeArmStretchAmount.Full;
			}
			Projectile.Center = player.MountedCenter + new Vector2(MathHelper.SmoothStep(16f, 24f, Vector2.UnitX.RotatedBy(attackTime * MathHelper.Pi).Y), -8) * player.Directions;
			Projectile.spriteDirection = player.direction;
		}
		public override bool? CanDamage() => false;
		public override bool ShouldUpdatePosition() => false;
	}
}