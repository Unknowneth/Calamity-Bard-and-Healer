using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles;
using ThoriumMod.Projectiles.Bard;

namespace CalamityBardHealer.Projectiles
{
	public class WulfrumSoundWave : BardProjectile
	{
		public override string Texture => "CalamityMod/Particles/HollowCircleHardEdge";
		public override BardInstrumentType InstrumentType => BardInstrumentType.Electronic;
		public override void SetStaticDefaults() {
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 6, base.Projectile.type);
		}
		public override void SetBardDefaults() {
			base.Projectile.width = 1;
			base.Projectile.height = 1;
			base.Projectile.aiStyle = -1;
			base.Projectile.friendly = true;
			base.Projectile.tileCollide = false;
			base.Projectile.DamageType = BardDamage.Instance;
			base.Projectile.penetrate = -1;
			base.Projectile.extraUpdates = 1;
			base.Projectile.timeLeft = 60;
			base.Projectile.alpha = 255;
		}
		public override void AI() {
			if(Collision.SolidCollision(Projectile.Center, 0, 0) && base.Projectile.timeLeft > 15) base.Projectile.timeLeft = 15;
			if(base.Projectile.timeLeft < 15) base.Projectile.alpha += 17;
			else if(base.Projectile.alpha > 0) base.Projectile.alpha -= 17;
			base.Projectile.velocity *= 1.01f;
			base.Projectile.scale *= 1.03f;
			base.Projectile.rotation = base.Projectile.velocity.ToRotation();
		}
		public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			if(ModLoader.TryGetMod("ThoriumMod", out Mod thorium)) target.AddBuff(thorium.Find<ModBuff>("Tuned").Type, 60);
			Projectile.damage -= 5;
			if(Projectile.damage < 0) Projectile.damage = 1;
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			float point = 0f;
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), base.Projectile.Center - Vector2.Normalize(base.Projectile.velocity).RotatedBy(MathHelper.PiOver2) * 16f * base.Projectile.scale, base.Projectile.Center + Vector2.Normalize(base.Projectile.velocity).RotatedBy(MathHelper.PiOver2) * 16f * base.Projectile.scale, 16f * base.Projectile.scale, ref point);
		}
		public override bool PreDraw(ref Color lightColor) {
			lightColor = Color.Lime * MathHelper.Lerp(1f, 0f, (float)base.Projectile.alpha / 255f);
			lightColor.A = 0;
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, lightColor, base.Projectile.rotation, texture.Size() * 0.5f, new Vector2(16f / texture.Width, 32f / texture.Height) * base.Projectile.scale, SpriteEffects.None, 0);
			return false;
		}
	}
}