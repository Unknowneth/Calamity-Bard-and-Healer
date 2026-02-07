using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.Particles;

namespace CalamityBardHealer.Projectiles
{
	public class IrradiatedOasis : ModProjectile
	{
		public override string Texture => "ThoriumMod/Empty";
		public override void SetDefaults() {
			Projectile.width = Projectile.height = 1;
			Projectile.aiStyle = -1;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 600;
		}
		public override void AI() {
			if(Projectile.velocity != Vector2.Zero) if(Projectile.velocity.Length() < 1f) Projectile.velocity = Vector2.Zero;
			else Projectile.velocity *= 0.95f;
			if(Projectile.ai[0] < 60f) Projectile.ai[0]++;
			else foreach(Player target in Main.ActivePlayers) if(Projectile.Distance(target.MountedCenter) < 48f && !target.dead && Projectile.localAI[2] <= 0f && target.statLife < target.statLifeMax2) {
				if(HealerHelper.HealPlayer(Main.player[Projectile.owner], target, 11, 60)) Projectile.localAI[2] = 30f;
				GeneralParticleHandler.SpawnParticle(new MediumMistParticle(target.Center, Main.rand.NextVector2Circular(16f, 16f), new Color(133, 180, 49), new Color(140, 234, 87), Main.rand.NextFloat(0.9f, 1.2f), 48f, Main.rand.NextFloat(0.03f, -0.03f)));
			}
			if(Projectile.localAI[2] > 0) Projectile.localAI[2]--;
			GeneralParticleHandler.SpawnParticle(new HeavySmokeParticle(Projectile.Center, Projectile.velocity * 0.5f, Main.rand.NextBool() ? new Color(133, 180, 49) : new Color(140, 234, 87), 20, Projectile.ai[0] / 120f, 0.6f, Main.rand.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4) * 0.2f, true, 0f, true));
		}
		public override bool? CanDamage() => false;
	}
}