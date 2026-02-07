using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles;
using ThoriumMod.Projectiles.Bard;

namespace CalamityBardHealer.Projectiles
{
	public class FeralKeytar : BardProjectile
	{
		public override string Texture => "CalamityBardHealer/Projectiles/ScuffedKeytar";
		public override string GlowTexture => "Terraria/Images/Extra_98";
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[base.Projectile.type] = 22;
			ProjectileID.Sets.TrailingMode[base.Projectile.type] = 2;
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 9, base.Projectile.type);
			mor.Call("addElementProj", 12, base.Projectile.type);
		}
		public override BardInstrumentType InstrumentType => BardInstrumentType.Electronic;
		public override void SetBardDefaults() {
			base.Projectile.width = 48;
			base.Projectile.height = 48;
			base.Projectile.aiStyle = -1;
			base.Projectile.hostile = false;
			base.Projectile.friendly = true;
			base.Projectile.DamageType = BardDamage.Instance;
			base.Projectile.tileCollide = false;
			base.Projectile.penetrate = -1;
			base.Projectile.usesLocalNPCImmunity = true;
			base.Projectile.localNPCHitCooldown = 7;
			base.Projectile.timeLeft = 300;
			base.Projectile.extraUpdates = 2;
			base.Projectile.alpha = 255;
			base.Projectile.ArmorPenetration = 30;
		}
		public override void AI() {
			if(base.Projectile.timeLeft < 17) base.Projectile.alpha += 15;
			else if(base.Projectile.alpha > 0) base.Projectile.alpha -= 15;
			base.Projectile.ai[1] += base.Projectile.ai[0];
			if(base.Projectile.ai[1] < -9f) {
				base.Projectile.ai[0] = -base.Projectile.ai[0];
				base.Projectile.ai[1] = -9f;
			}
			else if(base.Projectile.ai[1] > 9f) {
				base.Projectile.ai[0] = -base.Projectile.ai[0];
				base.Projectile.ai[1] = 9f;
			}
			base.Projectile.velocity = base.Projectile.velocity.RotatedBy(MathHelper.ToRadians(base.Projectile.ai[1]) * 0.4f);
			base.Projectile.rotation = base.Projectile.velocity.ToRotation();
		}
		public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			Projectile.damage -= 7;
			if(Projectile.damage < 0) Projectile.damage = 1;
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(GlowTexture);
			float fade = MathHelper.Lerp(1f, 0f, (int)base.Projectile.alpha / 255f) * (MathHelper.Min(15f, (float)base.Projectile.timeLeft) / 15f);
			if(base.Projectile.velocity != Vector2.Zero) for(int h = -2; h <= 2; h++) for(int i = 0; i < base.Projectile.oldPos.Length; i++) {
				lightColor = Color.Lerp(Color.Pink, Color.Red, (float)i / (float)base.Projectile.oldPos.Length);
				lightColor.A = 0;
				lightColor *= fade;
				Main.EntitySpriteDraw(texture, base.Projectile.oldPos[i] + base.Projectile.Size * 0.5f + Vector2.UnitY.RotatedBy(base.Projectile.ai[2]) * h * 8f - Main.screenPosition, new Rectangle(31, 0, 10, texture.Height), lightColor, base.Projectile.oldRot[i] + MathHelper.PiOver2, new Vector2(10, texture.Height) * 0.5f, new Vector2(0.4f, 0.6f) * base.Projectile.scale, SpriteEffects.None, 0);
			}
			texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			lightColor = Color.Red * fade;
			lightColor.A = 0;
			Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, lightColor, base.Projectile.ai[2], texture.Size() * 0.5f, base.Projectile.scale, SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, new Color(125, 125, 125, 0) * fade, base.Projectile.ai[2], texture.Size() * 0.5f, base.Projectile.scale, SpriteEffects.None, 0);
			return false;
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			bool trailHit = false;
			for(int i = 0; i < base.Projectile.oldPos.Length; i++) trailHit |= targetHitbox.Intersects(new Rectangle((int)base.Projectile.oldPos[i].X, (int)base.Projectile.oldPos[i].Y, projHitbox.Width, projHitbox.Height));
			return projHitbox.Intersects(targetHitbox) || trailHit;
		}
	}
}