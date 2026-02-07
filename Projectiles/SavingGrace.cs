using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Utilities;

namespace CalamityBardHealer.Projectiles
{
	public class SavingGrace : ModProjectile
	{
		public override string Texture => "CalamityBardHealer/Projectiles/Sparkle";
		public override string GlowTexture => "Terraria/Images/Extra_98";
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
		public override void SetDefaults() {
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.aiStyle = -1;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 300;
			Projectile.extraUpdates = 2;
			Projectile.alpha = 255;
		}
		public override void AI() {
			if(Projectile.alpha == 255) Projectile.spriteDirection = (int)Projectile.ai[0];
			if(Projectile.timeLeft < 17) Projectile.alpha += 15;
			else if(Projectile.alpha > 0) Projectile.alpha -= 15;
			if(Projectile.ai[0] != 0f) {
				Projectile.ai[1] += Projectile.ai[0];
				if(Projectile.ai[1] < -9f) {
					Projectile.ai[0] = -Projectile.ai[0];
					Projectile.ai[1] = -9f;
				}
				else if(Projectile.ai[1] > 9f) {
					Projectile.ai[0] = -Projectile.ai[0];
					Projectile.ai[1] = 9f;
				}
			}
			Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(Projectile.ai[1]) * 0.5f);
			if(Projectile.ai[2] < 0f) {
				Projectile.velocity = Vector2.Zero;
				if(Projectile.timeLeft > 17) Projectile.timeLeft = 17;
			}
			else if(Projectile.timeLeft > 295 && Projectile.ai[2] == 0f && Main.myPlayer == Projectile.owner) {
				float maxRange = 3200f;
				for(int i = 0; i < Main.maxPlayers; i++) if(i != Projectile.owner && !Main.player[i].dead && Main.player[i].active && Collision.CanHitLine(Main.MouseWorld, 0, 0, Main.player[i].Center, 0, 0) && Main.player[i].Distance(Main.MouseWorld) < maxRange && Main.player[i].statLife < Main.player[i].statLifeMax2 && Main.player[i].team == Main.player[Projectile.owner].team) {
					maxRange = Main.player[i].Distance(Main.MouseWorld);
					Projectile.ai[2] = i + 1;
				}
				if(Projectile.ai[2] > 0f) NetMessage.SendData(27, -1, -1, null, Projectile.whoAmI);
			}
			else if(Projectile.ai[2] > 0f) if(Projectile.Hitbox.Intersects(Main.player[(int)Projectile.ai[2] - 1].Hitbox)) {
				int healAmount = 24;
				if(Main.player[(int)Projectile.ai[2] - 1].statLife < Main.player[(int)Projectile.ai[2] - 1].statLifeMax2 / 10) healAmount = Main.player[(int)Projectile.ai[2] - 1].statLifeMax2 - Main.player[(int)Projectile.ai[2] - 1].statLife;
				if(HealerHelper.HealPlayer(Main.player[Projectile.owner], Main.player[(int)Projectile.ai[2] - 1], healAmount, 120) && Main.myPlayer == Projectile.owner) {
					Projectile.ai[2] = -1f;
					Projectile.netUpdate = true;
				}
			}
			else Projectile.velocity = Vector2.Normalize(Vector2.Lerp(Vector2.Normalize(Projectile.velocity), Vector2.Normalize(Main.player[(int)Projectile.ai[2] - 1].Center - Projectile.Center), 0.5f)) * Projectile.velocity.Length();
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Texture2D glowTexture = (Texture2D)ModContent.Request<Texture2D>(GlowTexture);
			lightColor = (Projectile.spriteDirection > 0 ? Color.Cyan : Color.Magenta) * 0.6f;
			lightColor.A = 0;
			for(int k = 1; k <= 2; k++) for(int i = 0; i < Projectile.oldPos.Length; i++) if(i == 0) Main.EntitySpriteDraw(texture, Projectile.oldPos[i] + new Vector2(Projectile.width, Projectile.height) * 0.5f - Main.screenPosition, null, (k == 1 ? lightColor * MathHelper.Lerp(2f, 0.2f, (float)i / (float)Projectile.oldPos.Length) : new Color(150, 150, 150, 0)), Main.GlobalTimeWrappedHourly * MathHelper.TwoPi * Projectile.spriteDirection, texture.Size() / 2, Projectile.scale * (k == 1 ? 1f : 0.3f), SpriteEffects.None, 0);
			else Main.EntitySpriteDraw(glowTexture, Projectile.oldPos[i] + new Vector2(Projectile.width, Projectile.height) * 0.5f - Main.screenPosition, null, (k == 1 ? lightColor * MathHelper.Lerp(1f, 0.1f, (float)i / (float)Projectile.oldPos.Length) : new Color(150, 150, 150, 0)), (Projectile.oldPos[i] - Projectile.oldPos[i - 1]).ToRotation() + MathHelper.PiOver2, glowTexture.Size() / 2, Projectile.scale * new Vector2(0.75f * (k > 1 ? MathHelper.Lerp(0.2f, 0.02f, (float)i / (float)Projectile.oldPos.Length) : 1f), 1.15f), SpriteEffects.None, 0);
			return false;
		}
	}
}