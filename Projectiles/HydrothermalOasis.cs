using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.Particles;

namespace CalamityBardHealer.Projectiles
{
	public class HydrothermalOasis : ModProjectile
	{
		public override string Texture => "ThoriumMod/Empty";
		public override void SetDefaults() {
			Projectile.width = Projectile.height = 1;
			Projectile.aiStyle = -1;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 180;
		}
		public override void AI() {
			if(Projectile.velocity != Vector2.Zero) if(Projectile.velocity.Length() < 1f) Projectile.velocity = Vector2.Zero;
			else Projectile.velocity *= 0.95f;
			if(Projectile.ai[0] < 60f) Projectile.ai[0]++;
			else foreach(Player target in Main.ActivePlayers) if(Projectile.Distance(target.MountedCenter) < 160f && !target.dead && Projectile.localAI[2] <= 0f && target.statLife < target.statLifeMax2 && HealerHelper.HealPlayer(Main.player[Projectile.owner], target, 1, 180)) Projectile.localAI[2] = 20f;
			if(Projectile.localAI[2] > 0) Projectile.localAI[2]--;
			GeneralParticleHandler.SpawnParticle(new HeavySmokeParticle(Projectile.Center, Projectile.velocity * 0.5f, Color.Lerp(Color.DarkOrange, Color.DarkBlue, System.Math.Abs(Vector2.UnitY.RotatedBy(MathHelper.TwoPi * (float)Projectile.timeLeft / 30f).X)), 15, Projectile.ai[0] / 12f, 0.6f, Main.rand.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4) * 0.3f, true, 0f, true));
		}
		public override bool? CanDamage() => false;
	}
}