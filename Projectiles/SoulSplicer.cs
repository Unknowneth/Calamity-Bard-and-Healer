using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Projectiles.Scythe;

namespace CalamityBardHealer.Projectiles
{
	public class SoulSplicer : ScythePro
	{
		public override void SafeSetStaticDefaults() {
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 2, base.Projectile.type);
			mor.Call("addElementProj", 9, base.Projectile.type);
		}
		public override void SafeSetDefaults() {
			base.Projectile.Size = new Vector2(138f);
			base.Projectile.scale = 1.15f;
			base.Projectile.extraUpdates = 1;
			base.Projectile.ArmorPenetration = 10;
			this.dustCount = 2;
			this.dustType = 135;
			this.dustOffset = new Vector2(-12f, 36f);
		}
		public override void ModifyDust(Dust dust, Vector2 position, int scytheIndex) {
			dust.noGravity = true;
			dust.alpha = 100;
			dust.scale *= 1.5f;
		}
		public override bool PreDraw(ref Color lightColor) {
			lightColor *= MathHelper.Lerp(1f, 0f, (float)base.Projectile.alpha / 255f);
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, lightColor, base.Projectile.rotation, texture.Size() / 2, base.Projectile.scale, base.Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			float rotOff = MathHelper.PiOver4 * (2 - base.Projectile.spriteDirection);
			texture = (Texture2D)ModContent.Request<Texture2D>("CalamityBardHealer/Projectiles/Slash_3");
			lightColor = new Color(157, 248, 234, 0) * MathHelper.Lerp(0.25f, 0f, (float)base.Projectile.alpha / 255f);
			Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, lightColor, base.Projectile.rotation - rotOff, texture.Size() / 2, base.Projectile.scale * 1.5f, base.Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, lightColor, base.Projectile.rotation - rotOff + MathHelper.Pi, texture.Size() / 2, base.Projectile.scale * 1.5f, base.Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			lightColor = new Color(111, 169, 241, 0) * MathHelper.Lerp(0.25f, 0f, (float)base.Projectile.alpha / 255f);
			Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, lightColor, base.Projectile.rotation - rotOff, texture.Size() / 2, base.Projectile.scale * 1.65f, base.Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, lightColor, base.Projectile.rotation - rotOff + MathHelper.Pi, texture.Size() / 2, base.Projectile.scale * 1.65f, base.Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			return false;
		}
		public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			target.AddBuff(144, 120);
			target.AddBuff(323, 120);
		}
	}
}