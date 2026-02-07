using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalamityBardHealer.Projectiles
{
	public class IceCreamMachine : ModProjectile
	{
		public override void SetDefaults() {
			Projectile.width = 62;
			Projectile.height = 62;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 6000;
		}
		public override void AI() {
			Player player = Main.player[Projectile.owner];
			if(player.dead || player.armor[0].type != ModContent.ItemType<Items.DaedalusCowl>() || player.ownedProjectileCounts[Type] > 1) {
				Projectile.Kill();
				return;
			}
			if(Projectile.Hitbox.Contains(Main.MouseWorld.ToPoint()) && Projectile.Distance(Main.LocalPlayer.MountedCenter) < 160f && !Main.LocalPlayer.dead) {
				if(Main.LocalPlayer.statLife < Main.LocalPlayer.statLifeMax2 && Main.LocalPlayer.controlUseTile && Main.LocalPlayer.GetModPlayer<ThorlamityPlayer>().iceCreamCoolDown <= 0f) {
					HealerHelper.HealPlayer(Main.player[Projectile.owner], Main.LocalPlayer, (int)MathHelper.Max(player.GetModPlayer<ThoriumMod.ThoriumPlayer>().healBonus * 9, 1), 60);
					Main.LocalPlayer.GetModPlayer<ThorlamityPlayer>().iceCreamCoolDown = 10800;
					NetMessage.SendData(16, -1, -1, null, Main.myPlayer);
				}
				else if(Main.LocalPlayer.GetModPlayer<ThorlamityPlayer>().iceCreamCoolDown > 0) {
					int secondsLeft = Main.LocalPlayer.GetModPlayer<ThorlamityPlayer>().iceCreamCoolDown / 60;
					int minutesLeft = secondsLeft / 60;
					secondsLeft -= minutesLeft * 60;
					Main.instance.MouseText(minutesLeft + "m " + secondsLeft + "s");
				}
				else {
					Main.LocalPlayer.noThrow = 2;
					Main.LocalPlayer.cursorItemIconEnabled = true;
					Main.LocalPlayer.cursorItemIconID = 4026;
				}
			}
			if(Projectile.ai[2] == 1f) {
				if(Projectile.ai[1] > -5f) Projectile.ai[1]--;
			}
			else if(Projectile.ai[2] == 2f) {
				if(Projectile.ai[1] < 5f) Projectile.ai[1]++;
				else Projectile.ai[2]++;
			}
			else if(Projectile.ai[1] > 0f) Projectile.ai[1]--;
			else if(Projectile.ai[1] < 0f) Projectile.ai[1]++;
			if(Projectile.velocity.Y < 16f) Projectile.velocity.Y += 0.4f;
		}
		public override bool OnTileCollide(Vector2 oldVelocity) {
			if(Projectile.ai[2] < 2f && Projectile.velocity.Y != oldVelocity.Y) {
				Projectile.velocity.Y = -1f;
				Projectile.ai[2]++;
			}
			if(Projectile.ai[2] == 1f) Terraria.Audio.SoundEngine.PlaySound(Terraria.ID.SoundID.DD2_DefenseTowerSpawn, Projectile.Center);
			return false;
		}
		public override bool PreDraw(ref Color lightColor) {
			if(Main.player[Projectile.owner].ownedProjectileCounts[Type] > 1) return false;
			float rotation = MathHelper.ToRadians(Projectile.ai[1] * 2f);
			float xOffset = Projectile.ai[2] > 0f ? rotation < 0f ? -21f : 21f : 0f;
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Main.LocalPlayer.GetModPlayer<ThorlamityPlayer>().iceCreamCoolDown > 0 ? Texture : GlowTexture);
			Main.EntitySpriteDraw(texture, new Vector2(Projectile.Center.X + xOffset, Projectile.Bottom.Y) - Main.screenPosition, null, lightColor, rotation, new Vector2(texture.Width / 2 + xOffset, texture.Height), Projectile.scale, SpriteEffects.None, 0);
			return false;
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) {
			fallThrough = false;
			return true;
		}
		public override bool? CanDamage() => false;
	}
}