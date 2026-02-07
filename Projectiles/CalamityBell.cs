using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CalamityBardHealer.Projectiles
{
	public class CalamityBell : ModProjectile
	{
		public override void SetDefaults() {
			Projectile.width = 40;
			Projectile.height = 46;
			Projectile.aiStyle = -1;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 2;
			Projectile.light = 1f;
		}
		public override void AI() {
			Player player = Main.player[Projectile.owner];
			if(player.HasBuff(ModContent.BuffType<Buffs.CalamityBell>())) Projectile.timeLeft = 2;
			else {
				Projectile.Kill();
				return;
			}
			Projectile.Center = player.Top - Vector2.UnitY * Projectile.height;
			if(Main.myPlayer == player.whoAmI) {
				if(player.itemAnimation > 0 || player.itemTime > 0 || player.channel) if(++Projectile.localAI[0] > 600) {
					Projectile.localAI[0] = 0;
					Projectile.localAI[1]++;
				}
				if(Projectile.localAI[1] > 0f) if(Projectile.localAI[1] > 60f) {
					int z = Projectile.NewProjectile(player.GetSource_Misc("God Slayer Deathsinger's Cowl"), Projectile.Center, player.velocity, ModContent.ProjectileType<Projectiles.CalamityBellRing>(), 7500, 10f, player.whoAmI, 20f);
					NetMessage.SendData(27, -1, -1, null, z);
					Projectile.localAI[1] = 0f;
				}
				else Projectile.localAI[1]++;
			}
			if(Projectile.localAI[0] > 480f) {
				if(Projectile.localAI[2] < 60f) Projectile.localAI[2]++;
			}
			else if(Projectile.localAI[1] == 0f && Projectile.localAI[2] > 0f) Projectile.localAI[2]--;
		}
		public override bool PreDraw(ref Color lightColor) {
			Vector2 center = Projectile.Center - Main.screenPosition;
			if(Projectile.localAI[1] < 40f) center.Y -= MathHelper.SmoothStep(0f, Projectile.width, Projectile.localAI[1] / 40f);
			else if(Projectile.localAI[1] < 55f) center.Y -= Projectile.width;
			else center.Y -= MathHelper.SmoothStep(Projectile.width, 0f, (Projectile.localAI[1] - 55f) / 5f);
			float rotation = MathHelper.Lerp(Vector2.UnitX.RotatedBy(Main.GlobalTimeWrappedHourly * MathHelper.Pi).Y * MathHelper.PiOver4 * 0.2f, 0f, Projectile.localAI[1] / 60f);
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			if(Projectile.localAI[2] > 0f) for(int i = 0; i < 4; i++) Main.EntitySpriteDraw(texture, center + Vector2.UnitY.RotatedBy(MathHelper.PiOver2 * i) * MathHelper.SmoothStep(4f, 8f, Vector2.UnitY.RotatedBy(Projectile.localAI[1] / 60f * MathHelper.Pi).X), null, new Color(255, 0, 255, 0) * (Projectile.localAI[2] / 60f) * (Projectile.localAI[2] / 60f), rotation, texture.Size() / 2f, 1f, SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, center, null, Color.White, rotation, texture.Size() / 2f, 1f, SpriteEffects.None, 0);
			return false;
		}
		public override bool ShouldUpdatePosition() => false;
	}
}