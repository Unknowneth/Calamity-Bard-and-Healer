using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles;
using ThoriumMod.Projectiles.Bard;
using Terraria.GameContent.Drawing;

namespace CalamityBardHealer.Projectiles
{
	public class InfestedCastanet : BardProjectile
	{
		public override BardInstrumentType InstrumentType => BardInstrumentType.Percussion;
		public override void SetStaticDefaults() => Main.projFrames[Type] = 3;
		public override void SetBardDefaults() {
			base.Projectile.width = 30;
			base.Projectile.height = 36;
			base.Projectile.aiStyle = -1;
			base.Projectile.hostile = false;
			base.Projectile.friendly = true;
			base.Projectile.DamageType = BardDamage.Instance;
			base.Projectile.tileCollide = false;
			base.Projectile.penetrate = -1;
			base.Projectile.usesLocalNPCImmunity = true;
			base.Projectile.localNPCHitCooldown = -1;
			base.Projectile.timeLeft = 2;
		}
		public override void AI() {
			if(base.Projectile.ai[1] > 0f) {
				base.Projectile.timeLeft = (int)base.Projectile.ai[1];
				base.Projectile.ai[1] *= -1f;
			}
			Player player = Main.player[base.Projectile.owner];
			base.Projectile.spriteDirection = player.direction;
			base.Projectile.rotation = base.Projectile.velocity.ToRotation();
			if(player.direction == -1) base.Projectile.rotation += MathHelper.Pi;
			if(base.Projectile.ai[0] == 1f) {
				player.compositeBackArm.enabled = true;
				player.compositeBackArm.rotation = base.Projectile.rotation - MathHelper.PiOver2 * base.Projectile.spriteDirection;
				player.compositeBackArm.stretch = (Player.CompositeArmStretchAmount)(1 - Projectile.frame);
				base.Projectile.Center = player.GetBackHandPosition(player.compositeBackArm.stretch, player.compositeBackArm.rotation) + Vector2.Normalize(base.Projectile.velocity) * 10f;
				player.compositeBackArm.stretch = Player.CompositeArmStretchAmount.Quarter;
			}
			else {
				DrawHeldProjInFrontOfHeldItemAndArms = true;
				base.Projectile.hide = true;
				player.heldProj = base.Projectile.whoAmI;
				player.compositeFrontArm.enabled = true;
				player.compositeFrontArm.rotation = base.Projectile.rotation - MathHelper.PiOver2 * base.Projectile.spriteDirection;
				player.compositeFrontArm.stretch = (Player.CompositeArmStretchAmount)(1 - Projectile.frame);
				base.Projectile.Center = player.GetFrontHandPosition(player.compositeFrontArm.stretch, player.compositeFrontArm.rotation) + Vector2.Normalize(base.Projectile.velocity) * 10f;
			}
			if(base.Projectile.timeLeft == -base.Projectile.ai[1] / 2) {
				for(int i = 0; i < 15; i++) Main.dust[Dust.NewDust(base.Projectile.position + Vector2.Normalize(base.Projectile.velocity) * 30f, base.Projectile.width, base.Projectile.height, 41, Projectile.velocity.X * 4f, Projectile.velocity.Y * 4f, 100, default(Color), 1f)].noGravity = true;
				for(int i = -1; i <= 1; i++) ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.ChlorophyteLeafCrystalShot, new ParticleOrchestraSettings { PositionInWorld = base.Projectile.Center + Vector2.Normalize(base.Projectile.velocity) * 20f + Vector2.Normalize(base.Projectile.velocity).RotatedBy(MathHelper.PiOver4 * 0.2f * i) * 10f, MovementVector = Vector2.Normalize(base.Projectile.velocity).RotatedBy(MathHelper.PiOver4 * 0.2f * i) * (6f - System.Math.Abs(i) * 2f), UniqueInfoPiece = (byte)(Main.rgbToHsl(Color.Blue).X * 255f)});
			}
			Projectile.frame = (int)MathHelper.Lerp(Main.projFrames[Type], 0f, (float)base.Projectile.timeLeft / -base.Projectile.ai[1]);
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			float point = 0f;
			bool hit = new Rectangle((int)(Projectile.Center.X - projHitbox.Width * 0.5f), (int)(Projectile.Center.Y - projHitbox.Height * 0.5f), projHitbox.Width, projHitbox.Height).Intersects(targetHitbox);
			Vector2 offset = Vector2.Normalize(base.Projectile.velocity);
			Vector2 center = base.Projectile.Center + offset * 20f;
			if(base.Projectile.timeLeft == -base.Projectile.ai[1] / 2) for(int i = -1; i <= 1; i++) hit |= Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), center, center + offset.RotatedBy(MathHelper.PiOver4 * 0.2f * i) * (64f - System.Math.Abs(i) * 8f), 2 * Projectile.scale, ref point);
			return hit;
		}
		public override bool ShouldUpdatePosition() => false;
	}
}