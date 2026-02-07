using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityBardHealer.Projectiles
{
	public class AstralFlames : ModProjectile
	{
		public override string Texture => "Terraria/Images/Extra_98";
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 2, base.Projectile.type);
			mor.Call("addElementProj", 14, base.Projectile.type);
		}
		public override void SetDefaults() {
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.DamageType = ModLoader.GetMod("ThoriumMod").Find<DamageClass>("HealerDamage");
			Projectile.extraUpdates = 1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			Projectile.timeLeft = 60;
		}
		public override void AI() {
			Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(6f) * Projectile.ai[0]);
			Projectile.rotation = Projectile.velocity.ToRotation();
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			bool trailHit = false;
			for(int i = 0; i < base.Projectile.oldPos.Length; i++) trailHit |= targetHitbox.Intersects(new Rectangle((int)base.Projectile.oldPos[i].X, (int)base.Projectile.oldPos[i].Y, projHitbox.Width, projHitbox.Height));
			return projHitbox.Intersects(targetHitbox) || trailHit;
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			float fade = MathHelper.Lerp(1f, 0f, (int)Projectile.alpha / 255f) * (MathHelper.Min(Projectile.timeLeft, 5) / 5);
			if(Projectile.velocity != Vector2.Zero) for(int i = 0; i < Projectile.oldPos.Length; i++) {
				lightColor = Color.Lerp(Color.Orange, Color.Cyan, (float)i / (float)Projectile.oldPos.Length) with {A = 100} * fade;
				Main.EntitySpriteDraw(texture, Projectile.oldPos[i] + new Vector2(Projectile.width, Projectile.height) / 2 - Main.screenPosition, null, lightColor, Projectile.oldRot[i] + MathHelper.PiOver2, texture.Size() * 0.5f, new Vector2(MathHelper.Lerp(Projectile.scale * 0.65f, 0.05f, (float)i / (float)Projectile.oldPos.Length), MathHelper.Lerp(Projectile.scale * 0.65f, 0.35f, (float)i / (float)Projectile.oldPos.Length)), SpriteEffects.None, 0);
			}
			return false;
		}
	}
}