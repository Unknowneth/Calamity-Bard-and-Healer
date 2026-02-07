using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CalamityBardHealer.Projectiles
{
	public class Whirlwind : ModProjectile
	{
		public override string Texture => "Terraria/Images/Projectile_657";
		public override void SetStaticDefaults() {
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 6, base.Projectile.type);
		}
		public override void SetDefaults() {
			Projectile.width = Projectile.height = 90;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 60;
			Projectile.tileCollide = false;
			Projectile.alpha = 255;
			Projectile.penetrate = -1;
			Projectile.DamageType = ModLoader.GetMod("ThoriumMod").Find<DamageClass>("HealerDamage");
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
		}
		public override void AI() {
			if(Projectile.timeLeft < 15) Projectile.alpha += 17;
			else if(Projectile.alpha > 0) Projectile.alpha -= 51;
			if(Projectile.velocity.X != 0) Projectile.direction = Projectile.velocity.X > 0 ? 1 : -1;
			Projectile.spriteDirection = Projectile.direction;
			Projectile.scale += 0.1f;
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			float fade = MathHelper.Lerp(0.15f, 0f, (float)Projectile.alpha / 255f);
			for(int k = 0; k < 25; k++) {
				Color glowColor = Color.Lerp(new Color(204, 255, 255, 255), new Color(204, 255, 255, 0), (float)k / 25f) * ((float)k / 25f) * fade;
				Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, glowColor, Main.GlobalTimeWrappedHourly * Projectile.spriteDirection * 3f * MathHelper.TwoPi / (k / 25f + 1) + MathHelper.ToRadians(k * 15f * Projectile.spriteDirection), texture.Size() * 0.5f, Projectile.scale * 0.05f * k, Projectile.spriteDirection > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			}
			return false;
		}
		public override void ModifyDamageHitbox(ref Rectangle hitbox) {
			float size = Projectile.scale * Projectile.Size.Length() * 0.25f;
			hitbox = new Rectangle((int)(Projectile.Center.X - size), (int)(Projectile.Center.Y - size), (int)(size * 2f), (int)(size * 2f));
		}
		public override bool? CanHitNPC(NPC target) => target.Distance(Projectile.Center) > Projectile.scale * Projectile.Size.Length() * 0.25f ? false : null;
		public override bool ShouldUpdatePosition() => false;
	}
}