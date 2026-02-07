using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Projectiles.Scythe;

namespace CalamityBardHealer.Projectiles
{
	public class SARS : ScythePro
	{
		public override void SafeSetStaticDefaults() {
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 11, base.Projectile.type);
		}
		public override void SafeSetDefaults() {
			base.Projectile.Size = new Vector2(142f);
			base.Projectile.scale = 1.25f;
			base.Projectile.ArmorPenetration = 20;
			this.dustCount = 2;
			this.dustType = 89;
			this.dustOffset = new Vector2(-8f, 36f);
		}
		public override void ModifyDust(Dust dust, Vector2 position, int scytheIndex) {
			dust.scale = 1.6f;
			dust.noGravity = true;
			dust.alpha = 100;
		}
		public override void SafeAI() {
			if(Main.myPlayer == base.Projectile.owner && base.Projectile.timeLeft == 1) {
				Vector2 shootDir = Main.rand.NextFloat(MathHelper.TwoPi).ToRotationVector2();
				int p = Projectile.NewProjectile(base.Projectile.GetSource_FromAI(), base.Projectile.Center + shootDir * 71f, shootDir * 8f, ModContent.ProjectileType<Projectiles.PlagueTrace>(), base.Projectile.damage, base.Projectile.knockBack, base.Projectile.owner, (Main.rand.NextBool(2) ? 0.001f : -0.001f) * Main.rand.Next(1, 10));
				NetMessage.SendData(27, -1, -1, null, p);
			}
		}
		public override void OnFirstHit(NPC target, NPC.HitInfo hit, int damageDone) {
			if(Main.myPlayer == base.Projectile.owner) {
				int p = Projectile.NewProjectile(base.Projectile.GetSource_OnHit(target), target.Center, Main.rand.NextFloat(MathHelper.TwoPi).ToRotationVector2() * 8f, ModContent.ProjectileType<Projectiles.PlagueTrace>(), base.Projectile.damage, base.Projectile.knockBack, base.Projectile.owner, (Main.rand.NextBool(2) ? 0.001f : -0.001f) * Main.rand.Next(3, 10));
				NetMessage.SendData(27, -1, -1, null, p);
			}
		}
		public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) target.AddBuff(calamity.Find<ModBuff>("Plague").Type, 120);
		}
		public override bool PreDraw(ref Color lightColor) {
			lightColor *= MathHelper.Lerp(1f, 0f, (float)base.Projectile.alpha / 255f);
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, lightColor, base.Projectile.rotation, texture.Size() / 2, base.Projectile.scale, base.Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			lightColor = Color.White * MathHelper.Lerp(0.1f, 0f, (float)base.Projectile.alpha / 255f);
			texture = (Texture2D)ModContent.Request<Texture2D>(GlowTexture);
			Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, lightColor, base.Projectile.rotation, texture.Size() / 2, base.Projectile.scale, base.Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			float rotOff = MathHelper.PiOver4 * (2 - base.Projectile.spriteDirection);
			lightColor = new Color(231, 220, 90, 0) * MathHelper.Lerp(0.1f, 0f, (float)base.Projectile.alpha / 255f);
			texture = (Texture2D)ModContent.Request<Texture2D>("CalamityBardHealer/Projectiles/Slash_3");
			Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, lightColor, base.Projectile.rotation - rotOff, texture.Size() / 2, base.Projectile.scale * 1.85f, base.Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, lightColor, base.Projectile.rotation - rotOff + MathHelper.Pi, texture.Size() / 2, base.Projectile.scale * 1.85f, base.Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			lightColor = new Color(154, 186, 74, 0) * MathHelper.Lerp(0.1f, 0f, (float)base.Projectile.alpha / 255f);
			texture = (Texture2D)ModContent.Request<Texture2D>("CalamityBardHealer/Projectiles/Slash_1");
			Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, lightColor, base.Projectile.rotation - rotOff, texture.Size() / 2, base.Projectile.scale * 1.85f, base.Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, lightColor, base.Projectile.rotation - rotOff + MathHelper.Pi, texture.Size() / 2, base.Projectile.scale * 1.85f, base.Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			return false;
		}
	}
}