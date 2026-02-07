using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CalamityBardHealer.Projectiles
{
	public class HyphaeBatonSporeCloud : ModProjectile
	{
		public override void SetStaticDefaults() => Main.projFrames[Type] = 3;
		public override void SetDefaults() {
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.aiStyle = -1;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 180;
			Projectile.alpha = 255;
			Projectile.penetrate = -1;
			Projectile.DamageType = ThoriumMod.HealerDamage.Instance;
		}
		public override void AI() {
			if(++Projectile.frameCounter >= 9) Projectile.frameCounter = 0;
			Projectile.frame = Projectile.frameCounter / 3;
			Projectile.velocity *= 0.98f;
			if(Projectile.timeLeft > 15 && Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height)) Projectile.timeLeft = 15;
			else if(Projectile.timeLeft < 15) Projectile.alpha += 17;
			else if(Projectile.alpha > 0) Projectile.alpha -= 17;
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, texture.Height / Main.projFrames[Type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Type]), lightColor * Projectile.Opacity, Projectile.rotation, new Vector2(texture.Width) / 2, Projectile.scale, SpriteEffects.None, 0);
			return false;
		}
	}
}