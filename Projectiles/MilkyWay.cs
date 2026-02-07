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
	public class MilkyWay : ScythePro
	{
		public override void SafeSetStaticDefaults() {
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 8, base.Projectile.type);
			mor.Call("addElementProj", 14, base.Projectile.type);
		}
		public override void SafeSetDefaults() {
			base.Projectile.Size = new Vector2(216f);
			base.Projectile.idStaticNPCHitCooldown = 3;
			base.Projectile.ArmorPenetration = 35;
		}
		public override bool PreAI() {
			Player player = Main.player[base.Projectile.owner];
			if(Main.myPlayer == player.whoAmI && base.Projectile.timeLeft % 3 == 0) {
				int p = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), base.Projectile.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.MilkyWaySpur>(), base.Projectile.damage, base.Projectile.knockBack, player.whoAmI, base.Projectile.spriteDirection * 10f, base.Projectile.rotation - (base.Projectile.spriteDirection - 2) * MathHelper.PiOver4);
				NetMessage.SendData(27, -1, -1, null, p);
				p = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), base.Projectile.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.MilkyWaySpur>(), base.Projectile.damage, base.Projectile.knockBack, player.whoAmI, base.Projectile.spriteDirection * 10f, base.Projectile.rotation - (base.Projectile.spriteDirection - 2) * MathHelper.PiOver4 - MathHelper.Pi, 1f);
				NetMessage.SendData(27, -1, -1, null, p);
			}
			if(base.Projectile.ai[0] != 0f) {
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
			else base.Projectile.rotation = MathHelper.TwoPi * -base.Projectile.spriteDirection * (player.itemAnimation / (float)player.itemAnimationMax);
			return true;
		}
		public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) target.AddBuff(calamity.Find<ModBuff>("GodSlayerInferno").Type, 120);
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			for(int k = 0; k < 2; k++) for(int i = 3; i > 0; i--) {
				float rotOff = MathHelper.PiOver4 * (2 - base.Projectile.spriteDirection) + MathHelper.PiOver4 * (k == 1 ? 0.5f : -0.25f) * base.Projectile.spriteDirection;
				float scale = base.Projectile.scale * MathHelper.Lerp(3f, 2f, (float)i / 3f);
				lightColor = (base.Projectile.spriteDirection > 0 ? Color.Magenta : Color.Cyan) * MathHelper.Lerp(0.15f / i, 0f, (float)base.Projectile.alpha / 255f);
				lightColor.A = 0;
				texture = (Texture2D)ModContent.Request<Texture2D>("CalamityBardHealer/Projectiles/Slash_" + i);
				Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, lightColor, base.Projectile.rotation - rotOff, texture.Size() / 2, scale, base.Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
				Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, lightColor, base.Projectile.rotation - rotOff, texture.Size() / 2, scale, base.Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
				lightColor = (base.Projectile.spriteDirection < 0 ? Color.Magenta : Color.Cyan) * MathHelper.Lerp(0.15f / i, 0f, (float)base.Projectile.alpha / 255f);
				lightColor.A = 0;
				Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, lightColor, base.Projectile.rotation - rotOff + MathHelper.Pi, texture.Size() / 2, scale, base.Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
				Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, lightColor, base.Projectile.rotation - rotOff + MathHelper.Pi, texture.Size() / 2, scale, base.Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			}
			lightColor = Color.White * MathHelper.Lerp(1f, 0f, (float)base.Projectile.alpha / 255f);
			texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, lightColor, base.Projectile.rotation, texture.Size() / 2, base.Projectile.scale, base.Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			return false;
		}
	}
}