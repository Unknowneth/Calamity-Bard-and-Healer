using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Buffs.Healer;

namespace CalamityBardHealer.Projectiles
{
	public class TarragonHeartbeat : ModProjectile
	{
		public override string Texture => "CalamityMod/Particles/HighResHollowCircleHardEdge";
		public override void SetStaticDefaults() => Terraria.ID.ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2048;
		public override void SetDefaults() {
			Projectile.width = 1;
			Projectile.height = 1;
			Projectile.aiStyle = -1;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 40;
		}
		public override void AI() {
			if(Main.myPlayer != Projectile.owner && Projectile.Distance(Main.LocalPlayer.Center) <= MathHelper.SmoothStep(Projectile.ai[0] * 64f, 0f, Projectile.timeLeft * 0.025f) && !Main.LocalPlayer.dead && Projectile.localAI[0] == 0f) {
				int healAmount = (int)MathHelper.Max(Main.player[Projectile.owner].GetModPlayer<ThoriumPlayer>().healBonus, 1);
				if(Main.LocalPlayer.GetModPlayer<ThoriumPlayer>().shieldHealth < 50) {
					Main.LocalPlayer.GetModPlayer<ThoriumPlayer>().shieldHealth += healAmount;
					if(Main.LocalPlayer.GetModPlayer<ThoriumPlayer>().shieldHealth >= 50) Main.LocalPlayer.GetModPlayer<ThoriumPlayer>().shieldHealth = 50;
				}
				if(Main.LocalPlayer.statLife < Main.LocalPlayer.statLifeMax2) HealerHelper.HealPlayer(Main.player[Projectile.owner], Main.LocalPlayer, 1, 60);
				NetMessage.SendData(16, -1, -1, null, Main.myPlayer);
				Projectile.localAI[0]++;
			}
		}
		public override bool PreDraw(ref Color lightColor) {
			lightColor = Color.DarkGreen;
			lightColor.A = 0;
			lightColor *= Projectile.timeLeft * 0.025f;
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Main.rand.NextFloat(MathHelper.TwoPi), texture.Size() / 2, MathHelper.SmoothStep(Projectile.ai[0] / 16f, 0f, Projectile.timeLeft * 0.025f), SpriteEffects.None, 0);
			return false;
		}
		public override bool ShouldUpdatePosition() => false;
	}
}