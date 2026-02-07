using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Utilities;

namespace CalamityBardHealer.Projectiles
{
	public class StarBirthStar : ModProjectile
	{
		public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod("CatalystMod");
		public override string Texture => "CalamityBardHealer/Projectiles/Sparkle";
		public override string GlowTexture => "CatalystMod/Assets/Glow";
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
			if(Projectile.timeLeft < 17) Projectile.alpha += 15;
			else if(Projectile.alpha > 0) Projectile.alpha -= 15;
			if(Projectile.ai[2] < 0f) {
				Projectile.velocity = Vector2.Zero;
				if(Projectile.timeLeft > 17) Projectile.timeLeft = 17;
			}
			else if(Projectile.ai[2] == 0f && Main.myPlayer == Projectile.owner) {
				float maxRange = 320f;
				for(int i = 0; i < Main.maxPlayers; i++) if(i != Projectile.owner && !Main.player[i].dead && Main.player[i].active && Collision.CanHitLine(Main.MouseWorld, 0, 0, Main.player[i].Center, 0, 0) && Main.player[i].Distance(Main.MouseWorld) < maxRange && Main.player[i].statLife < Main.player[i].statLifeMax2 && Main.player[i].team == Main.player[Projectile.owner].team) {
					maxRange = Main.player[i].Distance(Main.MouseWorld);
					Projectile.ai[2] = i + 1;
				}
				if(Projectile.ai[2] > 0f) NetMessage.SendData(27, -1, -1, null, Projectile.whoAmI);
			}
			else if(Projectile.Hitbox.Intersects(Main.player[(int)Projectile.ai[2] - 1].Hitbox)) {
				if(HealerHelper.HealPlayer(Main.player[Projectile.owner], Main.player[(int)Projectile.ai[2] - 1], 7, 120) && Main.myPlayer == Projectile.owner) {
					Projectile.ai[2] = -1f;
					NetMessage.SendData(27, -1, -1, null, Projectile.whoAmI);
				}
			}
			else {
				Projectile.velocity += Vector2.Normalize(Main.player[(int)Projectile.ai[2] - 1].Center - Projectile.Center) * 0.23f;
				Projectile.velocity *= 0.97f;
			}
		}
		public override bool PreDraw(ref Color lightColor) {
			float fade = MathHelper.Lerp(1f, 0f, (float)Projectile.alpha / 255f);
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(GlowTexture);
			Vector2 origin = texture.Size() / 2f;
			int trailLength = Projectile.oldPos.Length;
			for(int i = 0; i < trailLength; i++) Main.spriteBatch.Draw(texture, Projectile.oldPos[i] + Projectile.Size * 0.5f - Main.screenPosition, null, new Color(220, 95, 210, 0) * (1f - 1f / (float)trailLength * (float)i) * fade * 0.5f, Projectile.rotation, origin, Projectile.scale * (1f - 1f / (float)trailLength * (float)i) * fade, 0, 0f);
			texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			origin = texture.Size() / 2f;
			lightColor = new Color(255, 233, 2, 0) * fade; 
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, origin, Projectile.scale * fade * 0.5f, SpriteEffects.None, 0);
			return false;
		}
	}
}