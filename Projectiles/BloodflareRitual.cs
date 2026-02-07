using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CalamityBardHealer.Projectiles
{
	public class BloodflareRitual : ModProjectile
	{
		public override void SetDefaults() {
			Projectile.width = 1;
			Projectile.height = 1;
			Projectile.aiStyle = -1;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 300;
			Projectile.alpha = 255;
		}
		public override void AI() {
			if(Main.myPlayer != Projectile.owner && Projectile.Distance(Main.LocalPlayer.Center) <= MathHelper.Lerp(256f, 0f, base.Projectile.alpha / 255f) && --Projectile.localAI[0] <= 0f && Main.LocalPlayer.statLife < Main.LocalPlayer.statLifeMax2) {
				if(HealerHelper.HealPlayer(Main.player[Projectile.owner], Main.LocalPlayer, 1, 60)) Projectile.localAI[0] = 30f;
				NetMessage.SendData(16, -1, -1, null, Main.myPlayer);
			}
			if(Projectile.timeLeft < 17) base.Projectile.alpha += 15;
			else if(Projectile.alpha > 0) base.Projectile.alpha -= 15;
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, new Color(255, 0, 0, 0) * MathHelper.Lerp(1f, 0f, (float)base.Projectile.alpha / 255f), Main.GlobalTimeWrappedHourly * MathHelper.PiOver4, texture.Size() / 2, MathHelper.Lerp(1f, 0f, (float)Projectile.alpha / 255f), SpriteEffects.None, 0);
			return false;
		}
		public override bool ShouldUpdatePosition() => false;
	}
}