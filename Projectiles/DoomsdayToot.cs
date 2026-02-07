using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles;
using ThoriumMod.Projectiles.Bard;
using CalamityMod.Particles;

namespace CalamityBardHealer.Projectiles
{
	public class DoomsdayToot : BardProjectile
	{
		public override string Texture => "CalamityBardHealer/Projectiles/Slash_1";
		public override string GlowTexture => "CalamityBardHealer/Projectiles/Slash_2";
		public override BardInstrumentType InstrumentType => BardInstrumentType.Brass;
		public override void SetStaticDefaults() {
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 2, base.Projectile.type);
			mor.Call("addElementProj", 12, base.Projectile.type);
			mor.Call("addElementProj", 14, base.Projectile.type);
		}
		public override void SetBardDefaults() {
			base.Projectile.width = 90;
			base.Projectile.height = 90;
			base.Projectile.aiStyle = -1;
			base.Projectile.hostile = false;
			base.Projectile.friendly = true;
			base.Projectile.DamageType = BardDamage.Instance;
			base.Projectile.ArmorPenetration = 35;
			base.Projectile.penetrate = -1;
			base.Projectile.usesLocalNPCImmunity = true;
			base.Projectile.localNPCHitCooldown = 60;
			base.Projectile.extraUpdates = 3;
			base.Projectile.tileCollide = false;
			base.Projectile.timeLeft = 180;
		}
		public override void AI() {
			if(base.Projectile.FindTargetWithinRange(Projectile.ai[0]) is not null) base.Projectile.velocity = Vector2.Normalize(Vector2.Lerp(Vector2.Normalize(base.Projectile.velocity), Vector2.Normalize(base.Projectile.FindTargetWithinRange(Projectile.ai[0]).Center - base.Projectile.Center), MathHelper.Min(++base.Projectile.ai[1] / 60f, 1f) * 0.3f)) * base.Projectile.velocity.Length();
			GeneralParticleHandler.SpawnParticle(new HeavySmokeParticle(base.Projectile.Center + base.Projectile.velocity * 0.5f, base.Projectile.velocity * 0.5f, Color.DarkOrange, 5, Main.rand.NextFloat(1.2f, 1.5f) * base.Projectile.scale, 0.8f, Main.rand.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2), true, 0f, true));
		}
		public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) target.AddBuff(calamity.Find<ModBuff>("Dragonfire").Type, 240);
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(GlowTexture);
			lightColor = Color.DarkOrange * 0.01f;
			lightColor.A = 0;
			lightColor *= base.Projectile.timeLeft * 0.05f;
			texture = (Texture2D)ModContent.Request<Texture2D>(GlowTexture);
			Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, lightColor, base.Projectile.velocity.ToRotation(), texture.Size() / 2, base.Projectile.scale * 0.9f, SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, lightColor, base.Projectile.velocity.ToRotation(), texture.Size() / 2, base.Projectile.scale * 0.9f, SpriteEffects.FlipVertically, 0);
			lightColor = Color.Gold * 0.01f;
			lightColor.A = 0;
			lightColor *= base.Projectile.timeLeft * 0.05f;
			texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, lightColor, base.Projectile.velocity.ToRotation(), texture.Size() / 2, base.Projectile.scale * 1.1f, SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, lightColor, base.Projectile.velocity.ToRotation(), texture.Size() / 2, base.Projectile.scale * 1.1f, SpriteEffects.FlipVertically, 0);
			return false;
		}
	}
}