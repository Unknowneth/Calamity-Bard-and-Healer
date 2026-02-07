using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using ThoriumMod.Projectiles.Scythe;
using System;

namespace CalamityBardHealer.Projectiles
{
	public class Syzygy : ScythePro
	{
		public override void SafeSetStaticDefaults() {
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 1, base.Projectile.type);
			mor.Call("addElementProj", 14, base.Projectile.type);
		}
		public override void SafeSetDefaults() {
			base.Projectile.Size = new Vector2(158f);
			base.Projectile.idStaticNPCHitCooldown = 6;
			base.Projectile.ArmorPenetration = 25;
		}
		public override bool PreAI() {
			Player player = Main.player[base.Projectile.owner];
			if(base.Projectile.ai[0] != 0f) {
				player.heldProj = base.Projectile.whoAmI;
				player.ChangeDir(base.Projectile.velocity.X > 0 ? -1 : 1);
				base.Projectile.spriteDirection = player.direction;
				base.Projectile.rotation += base.rotationSpeed * base.Projectile.spriteDirection * 2.5f;
				if(base.Projectile.ai[1] - base.Projectile.ai[2] > base.Projectile.timeLeft) base.Projectile.timeLeft++;
				float attackTime = ++base.Projectile.ai[2] / base.Projectile.ai[1];
				base.Projectile.Center = player.MountedCenter + (Vector2.UnitX.RotatedBy(attackTime * MathHelper.TwoPi) * new Vector2(base.Projectile.velocity.Length(), base.Projectile.velocity.Length() * base.Projectile.ai[0] * base.Projectile.spriteDirection)).RotatedBy(base.Projectile.velocity.ToRotation()) - base.Projectile.velocity;
				if(Main.myPlayer == player.whoAmI && base.Projectile.ai[2] % 6f == 0 && base.Projectile.ai[2] > 0f) {
					int p = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), base.Projectile.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.ElementalSyzygy>(), base.Projectile.damage, base.Projectile.knockBack, player.whoAmI, Main.rand.NextBool(2) ? -10f : 10f, Main.rand.NextFloat(MathHelper.TwoPi) - MathHelper.Pi, Main.rand.Next(1, 9) * 32f);
					NetMessage.SendData(27, -1, -1, null, p);
				}
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
			else base.Projectile.rotation = MathHelper.TwoPi * -base.Projectile.spriteDirection * (float)player.itemAnimation / (float)player.itemAnimationMax;
			return true;
		}
		public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) target.AddBuff(calamity.Find<ModBuff>("ElementalMix").Type, 120);
		}
		public override bool PreDraw(ref Color lightColor) {
			lightColor = Color.White * MathHelper.Lerp(1f, 0f, (float)base.Projectile.alpha / 255f);
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, lightColor, base.Projectile.rotation, texture.Size() / 2, base.Projectile.scale, base.Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			for(int i = 3; i > 0; i--) {
				float offset = MathHelper.PiOver4 / 3f * (i - 2) * base.Projectile.spriteDirection;
				float rotation = (base.Projectile.rotation - offset).ToRotationVector2().ToRotation();
				if(rotation < 0f) rotation += MathHelper.TwoPi;
				lightColor = Main.hslToRgb(Math.Abs(rotation / MathHelper.TwoPi) % 1f, 1f, 0.66f) * MathHelper.Min(0.25f, MathHelper.Lerp(1, 0f, (float)base.Projectile.alpha / 255f)) * 0.8f;
				lightColor.A = 0;
				rotation = (base.Projectile.rotation + MathHelper.Pi - offset).ToRotationVector2().ToRotation();
				if(rotation < 0f) rotation += MathHelper.TwoPi;
				Color glowColor = Main.hslToRgb(Math.Abs(rotation / MathHelper.TwoPi) % 1f, 1f, 0.66f) * MathHelper.Min(0.25f, MathHelper.Lerp(1, 0f, (float)base.Projectile.alpha / 255f)) * 0.8f;
				glowColor.A = 0;
				SpriteEffects spriteEffects = base.Projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
				texture = (Texture2D)ModContent.Request<Texture2D>("CalamityBardHealer/Projectiles/Slash_" + i);
				Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, lightColor, base.Projectile.rotation - MathHelper.PiOver4 * base.Projectile.spriteDirection - offset, texture.Size() * 0.5f, base.Projectile.scale * 2.275f, base.Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
				Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, glowColor, base.Projectile.rotation + MathHelper.PiOver4 * base.Projectile.spriteDirection * 3f - offset, texture.Size() * 0.5f, base.Projectile.scale * 2.275f, base.Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			}
			return false;
		}
	}
}