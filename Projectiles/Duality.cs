using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using ThoriumMod.Projectiles.Scythe;

namespace CalamityBardHealer.Projectiles
{
	public class Duality : ScythePro
	{
		public override void SafeSetStaticDefaults() {
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 9, base.Projectile.type);
			mor.Call("addElementProj", 12, base.Projectile.type);
		}
		public override void SafeSetDefaults() {
			base.Projectile.Size = new Vector2(146f);
			base.Projectile.scale = 1.1f;
		}
		public override bool PreAI() {
			if(base.Projectile.ai[0] != 0f) {
				Player player = Main.player[base.Projectile.owner];
				player.heldProj = base.Projectile.whoAmI;
				player.ChangeDir(base.Projectile.velocity.X > 0 ? -1 : 1);
				base.Projectile.spriteDirection = player.direction;
				base.Projectile.rotation += base.rotationSpeed * base.Projectile.spriteDirection * 2.5f;
				if(base.Projectile.ai[1] - base.Projectile.ai[2] > base.Projectile.timeLeft) base.Projectile.timeLeft++;
				float attackTime = ++base.Projectile.ai[2] / base.Projectile.ai[1];
				base.Projectile.Center = player.MountedCenter + (Vector2.UnitX.RotatedBy(attackTime * MathHelper.TwoPi) * new Vector2(base.Projectile.velocity.Length(), base.Projectile.velocity.Length() * base.Projectile.ai[0] * base.Projectile.spriteDirection)).RotatedBy(base.Projectile.velocity.ToRotation()) - base.Projectile.velocity;
				if(base.Projectile.rotation > MathHelper.Pi) {
					SoundEngine.PlaySound(SoundID.Item1, base.Projectile.Center);
					base.Projectile.rotation -= MathHelper.TwoPi;
				}
				else if(base.Projectile.rotation < -MathHelper.Pi) {
					SoundEngine.PlaySound(SoundID.Item1, base.Projectile.Center);
					base.Projectile.rotation += MathHelper.TwoPi;
				}
				return false;
			}
			return true;
		}
		public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			float point = 0f;
			float size = base.Projectile.Size.Length();
			Vector2 rotation = Vector2.UnitY.RotatedBy(Projectile.rotation) * -size;
			if(Collision.CheckAABBvLineCollision(target.Hitbox.TopLeft(), target.Hitbox.Size(), base.Projectile.Center, base.Projectile.Center + rotation, size, ref point)) target.AddBuff(153, 120);
			else if(Collision.CheckAABBvLineCollision(target.Hitbox.TopLeft(), target.Hitbox.Size(), base.Projectile.Center, base.Projectile.Center - rotation, size, ref point)) Main.player[base.Projectile.owner].AddBuff(151, 60);
		}
		public override bool PreDraw(ref Color lightColor) {
			lightColor *= MathHelper.Lerp(1f, 0f, (float)base.Projectile.alpha / 255f);
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, lightColor, base.Projectile.rotation + MathHelper.PiOver4 * base.Projectile.spriteDirection, texture.Size() / 2, base.Projectile.scale, base.Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			lightColor = (base.Projectile.spriteDirection > 0 ? Color.Purple : Color.Red) * MathHelper.Lerp(0.25f, 0f, (float)base.Projectile.alpha / 255f);
			lightColor.A = 0;
			texture = (Texture2D)ModContent.Request<Texture2D>("CalamityBardHealer/Projectiles/Slash_3");
			Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, lightColor, base.Projectile.rotation, texture.Size() / 2, base.Projectile.scale * 1.75f, base.Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, lightColor, base.Projectile.rotation, texture.Size() / 2, base.Projectile.scale * 1.75f, base.Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			lightColor = (base.Projectile.spriteDirection < 0 ? Color.Purple : Color.Red) * MathHelper.Lerp(0.25f, 0f, (float)base.Projectile.alpha / 255f);
			lightColor.A = 0;
			Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, lightColor, base.Projectile.rotation + MathHelper.Pi, texture.Size() / 2, base.Projectile.scale * 1.75f, base.Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, lightColor, base.Projectile.rotation + MathHelper.Pi, texture.Size() / 2, base.Projectile.scale * 1.75f, base.Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			return false;
		}
	}
}