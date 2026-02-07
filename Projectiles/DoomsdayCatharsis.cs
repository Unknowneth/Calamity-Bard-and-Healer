using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CalamityBardHealer.Projectiles
{
	public class DoomsdayCatharsis : ModProjectile
	{
		public override string Texture => "CalamityBardHealer/Items/DoomsdayCatharsis";
		public override void SetDefaults() {
			Projectile.width = 76;
			Projectile.height = 46;
			Projectile.aiStyle = -1;
			Projectile.tileCollide = false;
			Projectile.alpha = 255;
			Projectile.timeLeft = 180;
			Projectile.scale = 2f;
		}
		public override void AI() {
			if(Projectile.timeLeft < 15) Projectile.alpha += 17;
			else if(Projectile.alpha > 0) Projectile.alpha -= 17;
			Player player = Main.player[Projectile.owner];
			Projectile.Center = player.Center + new Vector2(Projectile.ai[1], Projectile.ai[2]);
			if(Main.myPlayer == player.whoAmI && Projectile.timeLeft > 80 && Projectile.timeLeft < 100 && Projectile.timeLeft % 5 == 0) {
				int p = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), Projectile.Center + Vector2.Normalize(Projectile.velocity) * Projectile.width / 2, Projectile.velocity, ModContent.ProjectileType<Projectiles.DoomsdayToot>(), Projectile.damage, Projectile.knockBack, player.whoAmI, Projectile.ai[0]);
				NetMessage.SendData(27, -1, -1, null, p);
			}
			Projectile.rotation = Projectile.velocity.ToRotation();
			if(Projectile.rotation < -MathHelper.PiOver2 || Projectile.rotation > MathHelper.PiOver2) {
				Projectile.rotation += MathHelper.Pi;
				Projectile.spriteDirection = -1;
			}
			else Projectile.spriteDirection = 1;
		}
		public override bool PreDraw(ref Color lightColor) {
			lightColor *= MathHelper.Lerp(1f, 0f, (float)Projectile.alpha / 255f);
			Vector2 scaleMultiplier = Vector2.One;
			if(Projectile.timeLeft > 120) scaleMultiplier = new Vector2(MathHelper.SmoothStep(0.8f, 1f, (float)(Projectile.timeLeft - 135) / 45f), MathHelper.SmoothStep(1.2f, 1f, (float)(Projectile.timeLeft - 120) / 60f));
			else if(Projectile.timeLeft > 90) scaleMultiplier = new Vector2(MathHelper.SmoothStep(1.2f, 0.8f, (float)(Projectile.timeLeft - 90) / 45f), MathHelper.SmoothStep(0.8f, 1.2f, (float)(Projectile.timeLeft - 90) / 30f));
			else if(Projectile.timeLeft > 60) scaleMultiplier = new Vector2(MathHelper.SmoothStep(1f, 1.2f, (float)(Projectile.timeLeft - 45) / 45f), MathHelper.SmoothStep(1f, 0.8f, (float)(Projectile.timeLeft - 60) / 30f));
			Vector2 center = Projectile.Center - Main.screenPosition;
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			if(Main.myPlayer == Projectile.owner) center.Y += MathHelper.SmoothStep(0f, Main.screenHeight / 2 - Projectile.ai[2] + texture.Height, MathHelper.Max(0, Projectile.timeLeft - 150) / 30f);
			Main.EntitySpriteDraw(texture, center, null, lightColor, MathHelper.SmoothStep(Projectile.rotation, MathHelper.PiOver2 * Projectile.spriteDirection - MathHelper.PiOver2, MathHelper.Max(0, Projectile.timeLeft - 150) / 30f), texture.Size() / 2f, scaleMultiplier * Projectile.scale, Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			if(Projectile.timeLeft < 80) return false;
			lightColor = Color.Gold * MathHelper.Lerp(1f, 0f, (float)Projectile.alpha / 255f);
			if(Projectile.timeLeft > 100) lightColor *= MathHelper.SmoothStep(1f, 0f, (float)(Projectile.timeLeft - 100) / 80f);
			else lightColor *= MathHelper.SmoothStep(0f, 1f, (float)(Projectile.timeLeft - 80) / 20f);
			lightColor.A = 0;
			Main.EntitySpriteDraw(texture, center, null, lightColor, MathHelper.SmoothStep(Projectile.rotation, MathHelper.PiOver2 * Projectile.spriteDirection - MathHelper.PiOver2, MathHelper.Max(0, Projectile.timeLeft - 150) / 30f), texture.Size() / 2f, scaleMultiplier * Projectile.scale, Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			return false;
		}
	}
}