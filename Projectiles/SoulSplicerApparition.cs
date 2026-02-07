using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CalamityBardHealer.Projectiles
{
	public class SoulSplicerApparition : ModProjectile
	{
		public override void SetStaticDefaults() {
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 2, Projectile.type);
			mor.Call("addElementProj", 9, Projectile.type);
		}
		public override void SetDefaults() {
			Projectile.width = 138;
			Projectile.height = 138;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.DamageType = ThoriumMod.HealerDamage.Instance;
			Projectile.timeLeft = 120;
			Projectile.penetrate = -1;
			Projectile.extraUpdates = 1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
			Projectile.ArmorPenetration = 10;
		}
		public override void AI() {
			if(Projectile.velocity.X != 0) Projectile.direction = Projectile.velocity.X > 0 ? 1 : -1;
			Projectile.velocity *= 0.95f;
			if(Projectile.timeLeft < 51) Projectile.alpha += 5;
			Projectile.rotation += 0.4f * Projectile.direction * Projectile.Opacity;
			Projectile.spriteDirection = Projectile.direction;
		}
		public override bool PreDraw(ref Color lightColor) {
			lightColor *= MathHelper.Lerp(1f, 0f, (float)Projectile.alpha / 255f);
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, new Color(200, 200, 200, 0) * Projectile.Opacity, Projectile.rotation, texture.Size() / 2, Projectile.scale, Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			float rotOff = MathHelper.PiOver4 * (2 - Projectile.spriteDirection);
			texture = (Texture2D)ModContent.Request<Texture2D>("CalamityBardHealer/Projectiles/Slash_3");
			lightColor = new Color(157, 248, 234, 0) * MathHelper.Lerp(0.25f, 0f, (float)Projectile.alpha / 255f);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation - rotOff, texture.Size() / 2, Projectile.scale * 1.5f, Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation - rotOff + MathHelper.Pi, texture.Size() / 2, Projectile.scale * 1.5f, Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			lightColor = new Color(111, 169, 241, 0) * MathHelper.Lerp(0.25f, 0f, (float)Projectile.alpha / 255f);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation - rotOff, texture.Size() / 2, Projectile.scale * 1.65f, Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation - rotOff + MathHelper.Pi, texture.Size() / 2, Projectile.scale * 1.65f, Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			return false;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			target.AddBuff(144, 120);
			target.AddBuff(323, 120);
		}
	}
}