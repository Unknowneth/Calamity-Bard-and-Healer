using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CalamityBardHealer.Projectiles
{
	public class Noisebringer : ModProjectile
	{
		public override string Texture => "CalamityMod/Particles/HollowCircleHardEdge";
		public override void SetStaticDefaults() {
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 6, base.Projectile.type);
			mor.Call("addElementProj", 11, base.Projectile.type);
		}
		public override void SetDefaults() {
			Projectile.width = 1;
			Projectile.height = 1;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.DamageType = ModLoader.GetMod("ThoriumMod").Find<DamageClass>("BardDamage");
			Projectile.penetrate = -1;
			Projectile.extraUpdates = 1;
			Projectile.timeLeft = 60;
			Projectile.alpha = 255;
		}
		public override void AI() {
			if(Collision.SolidCollision(Projectile.Center, 0, 0) && Projectile.timeLeft > 15) Projectile.timeLeft = 15;
			if(Projectile.timeLeft < 15) Projectile.alpha += 17;
			else if(Projectile.alpha > 0) Projectile.alpha -= 17;
			Projectile.velocity *= 1.01f;
			Projectile.scale *= 1.03f;
			Projectile.rotation = Projectile.velocity.ToRotation() + Projectile.ai[0];
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			if(ModLoader.TryGetMod("ThoriumMod", out Mod thorium)) target.AddBuff(thorium.Find<ModBuff>("Tuned").Type, 240);
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) target.AddBuff(calamity.Find<ModBuff>("Plague").Type, 240);
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			float point = 0f;
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center - Vector2.Normalize(Projectile.velocity).RotatedBy(MathHelper.PiOver2 + Projectile.ai[0]) * 16f * Projectile.scale, Projectile.Center + Vector2.Normalize(Projectile.velocity).RotatedBy(MathHelper.PiOver2 + Projectile.ai[0]) * 16f * Projectile.scale, 16f * Projectile.scale, ref point);
		}
		public override bool PreDraw(ref Color lightColor) {
			lightColor = new Color(231, 220, 90, 0) * MathHelper.Lerp(1f, 0f, (float)Projectile.alpha / 255f);
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, texture.Size() * 0.5f, new Vector2(16f / texture.Width, 32f / texture.Height) * Projectile.scale, SpriteEffects.None, 0);
			return false;
		}
	}
}