using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.Particles;

namespace CalamityBardHealer.Projectiles
{
	public class TidalWave : ModProjectile
	{
		public override string Texture => "CalamityMod/Particles/SemiCircularSmear";
		public override void SetStaticDefaults() {
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 3, base.Projectile.type);
		}
		public override void SetDefaults() {
			Projectile.width = 180;
			Projectile.height = 180;
			Projectile.aiStyle = -1;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.DamageType = ModLoader.GetMod("ThoriumMod").Find<DamageClass>("HealerDamage");
			Projectile.ArmorPenetration = 40;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
			Projectile.extraUpdates = 1;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 180;
		}
		public override void AI() => GeneralParticleHandler.SpawnParticle(new HeavySmokeParticle(Projectile.Center + Projectile.velocity * 0.5f, Projectile.velocity * 0.5f, Color.Aquamarine, 5, Main.rand.NextFloat(1.8f, 2.1f) * Projectile.scale, 0.5f, Main.rand.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2), true, 0f, true));
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) target.AddBuff(calamity.Find<ModBuff>("CrushDepth").Type, 240);
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			lightColor = Color.Aquamarine;
			lightColor.A = 0;
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.velocity.ToRotation(), texture.Size() / 2, Projectile.scale, SpriteEffects.FlipHorizontally, 0);
			return false;
		}
	}
}