using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace CalamityBardHealer.Projectiles
{
	public class ScreamingClam : ModProjectile
	{
		public override void SetStaticDefaults() => Main.projFrames[Type] = 4;
		public override void SetDefaults() {
			Projectile.width = 32;
			Projectile.height = 38;
			Projectile.aiStyle = -1;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 2;
		}
		public override void AI() {
			Player player = Main.player[Projectile.owner];
			if(player.GetModPlayer<ThorlamityPlayer>().screamingClam && !player.dead) Projectile.timeLeft = 2;
			else {
				Projectile.Kill();
				return;
			}
			bool attack = (player.itemTime > 0 || player.channel);
			Projectile.spriteDirection = player.direction;
			Vector2 hoverPos = player.Top - Vector2.UnitY * Projectile.width;
			if(Projectile.Center != hoverPos && Vector2.Distance(Projectile.Center, hoverPos) < 1f) Projectile.Center = hoverPos;
			else if(Vector2.Distance(Projectile.Center, hoverPos) > 1f) Projectile.Center = Vector2.Lerp(Projectile.Center, hoverPos, 0.4f);
			if(Main.myPlayer == player.whoAmI) {
				Projectile.velocity = Vector2.Normalize(Main.MouseWorld - Projectile.Center);
				NetMessage.SendData(27, -1, -1, null, Projectile.whoAmI);
			}
			if(Projectile.velocity.X != 0f) Projectile.spriteDirection = Projectile.direction = Projectile.velocity.X > 0f ? 1 : -1;
			if(attack) {
				if(Projectile.ai[2] == 45f && Main.myPlayer == player.whoAmI) {
					int p = Projectile.NewProjectile(player.GetSource_Misc("Screaming Clam"), Projectile.Center, Projectile.velocity * 8f, ModContent.ProjectileType<Projectiles.ScreamingClamScream>(), Projectile.damage, 0f, player.whoAmI);
					NetMessage.SendData(27, -1, -1, null, p);
				}
				else if(Projectile.ai[2] > 30f && Projectile.ai[2] < 45f) {
					if(Projectile.frame <= Main.projFrames[Type] - 1) if(++Projectile.frameCounter > 3) {
						Projectile.frame++;
						Projectile.frameCounter = 0;
					}
				}
				else if(Projectile.ai[2] > 70f && Projectile.frame > 0) if(++Projectile.frameCounter > 4) {
					Projectile.frame--;
					Projectile.frameCounter = 0;
				}
				Projectile.ai[2]++;
			}
			else {
				if(Projectile.frame > 0) if(++Projectile.frameCounter > 4) {
					Projectile.frame--;
					Projectile.frameCounter = 0;
				}
				if(Projectile.ai[2] > 0f) Projectile.ai[2]--;
			}
			Projectile.rotation = Projectile.velocity.ToRotation();
			if(Projectile.ai[2] > 90f) Projectile.ai[2] = 0f;
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Projectile.type]), lightColor, Projectile.rotation, new Vector2(texture.Width, texture.Height / Main.projFrames[Projectile.type]) / 2, Projectile.scale, Projectile.spriteDirection < 0 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0);
			texture = (Texture2D)ModContent.Request<Texture2D>(GlowTexture);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Projectile.type]), Color.White, Projectile.rotation, new Vector2(texture.Width, texture.Height / Main.projFrames[Projectile.type]) / 2, Projectile.scale, Projectile.spriteDirection < 0 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0);
			return false;
		}
		public override bool? CanDamage() => false;
		public override bool ShouldUpdatePosition() => false;
	}
}