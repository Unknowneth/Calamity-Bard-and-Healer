using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using ThoriumMod.Projectiles.Scythe;
using CalamityMod.Particles;

namespace CalamityBardHealer.Projectiles
{
	public class ColdShoulder : ScythePro
	{
		public override void SafeSetStaticDefaults() {
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 4, base.Projectile.type);
		}
		public override void SafeSetDefaults() {
			base.Projectile.Size = new Vector2(146f);
			base.Projectile.ArmorPenetration = 10;
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
		public override void PostAI() {
			if(Main.myPlayer == base.Projectile.owner && (base.Projectile.ai[2] > 0f ? (int)base.Projectile.ai[2] : base.Projectile.timeLeft) % 7 == 0) {
				Vector2 shootDir = Main.rand.NextFloat(MathHelper.TwoPi).ToRotationVector2();
				int p = Projectile.NewProjectile(base.Projectile.GetSource_FromAI(), base.Projectile.Center + shootDir * 73f, shootDir * 4f, ModContent.ProjectileType<Projectiles.ColdShoulderIcicle>(), base.Projectile.damage / 2, base.Projectile.knockBack, base.Projectile.owner);
				NetMessage.SendData(27, -1, -1, null, p);
			}
		}
		public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			GeneralParticleHandler.SpawnParticle(new MediumMistParticle(target.Center, Vector2.Normalize(target.Center - base.Projectile.Center), Color.Cyan, Color.Violet, Main.rand.NextFloat(0.9f, 1.2f), 160f, Main.rand.NextFloat(0.03f, -0.03f)));
			target.AddBuff(BuffID.Frostburn2, 180);
		}
		public override bool PreDraw(ref Color lightColor) {
			lightColor *= MathHelper.Lerp(1f, 0f, (float)base.Projectile.alpha / 255f);
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, lightColor, base.Projectile.rotation, texture.Size() / 2, base.Projectile.scale, base.Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			lightColor = Color.Cyan * MathHelper.Lerp(0.15f, 0f, (float)base.Projectile.alpha / 255f);
			lightColor.A = 0;
			float rotOff = MathHelper.PiOver4 * (2 - base.Projectile.spriteDirection);
			texture = (Texture2D)ModContent.Request<Texture2D>("CalamityBardHealer/Projectiles/Slash_3");
			Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, lightColor, base.Projectile.rotation - rotOff, texture.Size() / 2, base.Projectile.scale * 1.85f, base.Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, lightColor, base.Projectile.rotation - rotOff + MathHelper.Pi, texture.Size() / 2, base.Projectile.scale * 1.85f, base.Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			texture = (Texture2D)ModContent.Request<Texture2D>("CalamityBardHealer/Projectiles/Slash_2");
			lightColor = Color.Violet * MathHelper.Lerp(0.35f, 0f, (float)base.Projectile.alpha / 255f);
			lightColor.A = 0;
			Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, lightColor, base.Projectile.rotation - rotOff, texture.Size() / 2, base.Projectile.scale * 2.15f, base.Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, lightColor, base.Projectile.rotation - rotOff + MathHelper.Pi, texture.Size() / 2, base.Projectile.scale * 2.15f, base.Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			return false;
		}
	}
}