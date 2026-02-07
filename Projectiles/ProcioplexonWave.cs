using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.Particles;
using CalamityEntropy;
using CalamityEntropy.Content.Particles;

namespace CalamityBardHealer.Projectiles
{
	[ExtendsFromMod("CalamityEntropy")]
	[JITWhenModsEnabled("CalamityEntropy")]
	public class ProcioplexonWave : ModProjectile
	{
		public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod("CalamityEntropy");
		public override string Texture => "CalamityMod/Particles/SemiCircularSmear";
		public override void SetStaticDefaults() {
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 3, base.Projectile.type);
		}
		public override void SetDefaults() {
			Projectile.width = 360;
			Projectile.height = 360;
			Projectile.aiStyle = -1;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.DamageType = ModLoader.GetMod("ThoriumMod").Find<DamageClass>("HealerDamage");
			Projectile.ArmorPenetration = 100;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
			Projectile.extraUpdates = 1;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 180;
		}
		public override void AI() => GeneralParticleHandler.SpawnParticle(new HeavySmokeParticle(Projectile.Center + Projectile.velocity * 0.5f, Projectile.velocity * 0.5f, Color.SlateBlue, 5, Main.rand.NextFloat(1.8f, 2.1f) * Projectile.scale * 2f, 0.5f, Main.rand.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2), true, 0f, true));
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			CalamityEntropy.CalamityEntropy.Instance.screenShakeAmp = 2f;
			EParticle.NewParticle(new AbyssalLine(), target.Center, Vector2.Zero, Color.White, 1f, 1f, true, BlendState.Additive, CEUtils.randomRot(), -1);
			if(ModLoader.TryGetMod("CalamityEntropy", out Mod entropy)) target.AddBuff(entropy.Find<ModBuff>("LifeOppress").Type, 600);
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			lightColor = Color.SlateBlue;
			lightColor.A = 0;
			for(int i = 0; i < 10; i++) Main.EntitySpriteDraw(texture, Projectile.Center - Projectile.velocity.SafeNormalize(Projectile.oldVelocity) * i - Main.screenPosition, null, lightColor, Projectile.velocity.ToRotation(), texture.Size() / 2, Projectile.scale * 2f * (1f - i * 0.1f), SpriteEffects.FlipHorizontally, 0);
			return false;
		}
	}
}