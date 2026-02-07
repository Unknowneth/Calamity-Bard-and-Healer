using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using ThoriumMod.Projectiles.Scythe;

namespace CalamityBardHealer.Projectiles
{
	public class Trinity : ScythePro
	{
		public override string Texture => "CalamityBardHealer/Items/Trinity";
		public override void SafeSetStaticDefaults() {
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 2, base.Projectile.type);
			mor.Call("addElementProj", 14, base.Projectile.type);
		}
		public override void SafeSetDefaults() {
			base.Projectile.Size = new Vector2(144f);
			base.Projectile.scale = 1.1f;
			base.scytheCount = 3;
			base.rotationSpeed *= 1.5f;
		}
		public override bool PreAI() {
			if(base.Projectile.ai[0] != 0f) {
				Player player = Main.player[base.Projectile.owner];
				player.heldProj = base.Projectile.whoAmI;
				player.ChangeDir(base.Projectile.velocity.X > 0 ? -1 : 1);
				base.Projectile.spriteDirection = player.direction;
				base.Projectile.rotation += base.rotationSpeed * base.Projectile.spriteDirection * 2f;
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
		public override void OnFirstHit(NPC target, NPC.HitInfo hit, int damageDone) {
			if(Main.myPlayer == base.Projectile.owner) for(int i = 0; i < 3; i++) {
				int p = Projectile.NewProjectile(base.Projectile.GetSource_OnHit(target), target.Center, (base.Projectile.rotation + i * MathHelper.TwoPi / 3f).ToRotationVector2() * base.Projectile.direction * 12f, ModContent.ProjectileType<Projectiles.AstralFlames>(), base.Projectile.damage, base.Projectile.knockBack, base.Projectile.owner, base.Projectile.direction);
				NetMessage.SendData(27, -1, -1, null, p);
			}
		}
		public override bool PreDraw(ref Color lightColor) {
			lightColor = Color.White * MathHelper.Lerp(1f, 0f, (float)base.Projectile.alpha / 255f);
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			for(int i = 0; i < 3; i++) Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, lightColor, base.Projectile.rotation + MathHelper.TwoPi * (float)i / 3f * base.Projectile.spriteDirection, new Vector2(base.Projectile.spriteDirection < 0 ? texture.Width - 4 : 4, texture.Height - 4), base.Projectile.scale, base.Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			lightColor = Color.Orange * MathHelper.Lerp(0.5f, 0f, (float)base.Projectile.alpha / 255f);
			lightColor.A = 0;
			texture = (Texture2D)ModContent.Request<Texture2D>("CalamityBardHealer/Projectiles/Slash_1");
			for(int i = 0; i < 3; i++) Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, lightColor, base.Projectile.rotation + (MathHelper.PiOver4 * -0.4f + MathHelper.TwoPi * (float)i / 3f) * base.Projectile.spriteDirection, texture.Size() / 2, base.Projectile.scale * 1.75f, base.Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			lightColor = Color.Cyan * MathHelper.Lerp(0.5f, 0f, (float)base.Projectile.alpha / 255f);
			lightColor.A = 0;
			texture = (Texture2D)ModContent.Request<Texture2D>("CalamityBardHealer/Projectiles/Slash_2");
			for(int i = 0; i < 3; i++) Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, lightColor, base.Projectile.rotation + (MathHelper.PiOver4 * -0.4f + MathHelper.TwoPi * (float)i / 3f) * base.Projectile.spriteDirection, texture.Size() / 2, base.Projectile.scale * 1.6f, base.Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			return false;
		}
	}
}