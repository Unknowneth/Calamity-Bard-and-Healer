using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Buffs.Healer;
using CalamityMod.Particles;

namespace CalamityBardHealer.Projectiles
{
	public class StarBirth : ModProjectile
	{
		public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod("CatalystMod");
		public override string Texture => "ThoriumMod/Empty";
		public override void SetDefaults() {
			Projectile.width = Projectile.height = 1;
			Projectile.aiStyle = -1;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 180;
		}
		public override void AI() {
			if(Projectile.velocity != Vector2.Zero) if(Projectile.velocity.Length() < 1f) Projectile.velocity = Vector2.Zero;
			else Projectile.velocity *= 0.97f;
			if(Projectile.ai[0] < 60f) Projectile.ai[0]++;
			else foreach(Player target in Main.ActivePlayers) if(Projectile.Distance(target.MountedCenter) < 240f && !target.dead && Projectile.localAI[2] <= 0f && target.statLife < target.statLifeMax2) {
				if(HealerHelper.HealPlayer(Main.player[Projectile.owner], target, 13, 120)) Projectile.localAI[2] = 20f;
				GeneralParticleHandler.SpawnParticle(new MediumMistParticle(target.Center, Main.rand.NextVector2Circular(16f, 16f), Color.Aquamarine, Color.Blue, Main.rand.NextFloat(0.9f, 1.2f), 160f, Main.rand.NextFloat(0.03f, -0.03f)));
			}
			if(Projectile.localAI[2] > 0) Projectile.localAI[2]--;
			if(Projectile.timeLeft > 80 && Projectile.timeLeft < 160 && Main.myPlayer == Projectile.owner && ++Projectile.localAI[1] > 15f) {
				int p = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center + Main.rand.NextVector2Circular(Projectile.ai[0], Projectile.ai[0]) * 2.5f, Vector2.Zero, ModContent.ProjectileType<Projectiles.StarBirthStar>(), 0, 0f, Projectile.owner);
				NetMessage.SendData(27, -1, -1, null, p);
				Projectile.localAI[1] = 0f;
			}
			int dustType = 6;
			if(ModLoader.TryGetMod("CatalystMod", out Mod catalyst)) dustType = catalyst.Find<ModDust>("MonoDust2").Type;
			if(Projectile.timeLeft % Projectile.MaxUpdates == 0) for(int i = 0; i < 2; i++) if(!Main.rand.NextBool(3)) {
				float mult = (float)i * 0.5f;
				Dust dust = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(Projectile.ai[0], Projectile.ai[0]) * 4f + Projectile.velocity * mult, dustType, null, 0, default(Color), 1f);
				dust.noGravity = true;
				dust.velocity *= 0.1f;
				dust.scale = 1.6f;
				dust.fadeIn = 0.1f;
				dust.alpha = 100;
				dust.color = (!Main.rand.NextBool(3) ? new Color(255, 233, 2, 50) : new Color(220, 95, 210, 50));
				if(Projectile.oldVelocity != Vector2.Zero) dust.velocity -= Vector2.Normalize(Projectile.oldVelocity);
			}
			GeneralParticleHandler.SpawnParticle(new HeavySmokeParticle(Projectile.Center, Projectile.velocity * 0.5f, Color.Lerp(Color.DarkViolet, Color.DarkBlue, Vector2.UnitX.RotatedBy(Projectile.timeLeft / 20f * MathHelper.Pi).X), 20, Projectile.ai[0] / 10f * 1.2f, 0.6f, Main.rand.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4) * 0.1f, true, 0f, true));
			GeneralParticleHandler.SpawnParticle(new HeavySmokeParticle(Projectile.Center, Projectile.velocity * 0.5f, Color.Lerp(Color.DarkOrange, Color.DarkGoldenrod, Vector2.UnitX.RotatedBy(Projectile.timeLeft / 30f * MathHelper.Pi).X), 20, Projectile.ai[0] / 12f * 1.2f, 0.4f, Main.rand.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4) * 0.3f, true, 0f, true));
		}
		public override bool? CanDamage() => false;
	}
}