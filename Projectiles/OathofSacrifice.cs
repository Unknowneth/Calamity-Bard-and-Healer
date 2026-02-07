using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Utilities;
using ThoriumMod.Buffs.Healer;

namespace CalamityBardHealer.Projectiles
{
	public class OathofSacrifice : ModProjectile
	{
		public override void SetDefaults() {
			Projectile.width = Projectile.height = 1;
			Projectile.aiStyle = -1;
			Projectile.tileCollide = false;
		}
		public override void AI() {
			Projectile.timeLeft = 2;
			Player player = Main.player[Projectile.owner];
			if(Main.myPlayer == Projectile.owner) {
				Projectile.ai[1] = player.channel && player.HeldItem.type == ModContent.ItemType<Items.OathofSacrifice>() ? 1f : 0f;
				Projectile.Center = player.MountedCenter;
				NetMessage.SendData(27, -1, -1, null, Projectile.whoAmI);
			}
			else foreach(Player target in Main.ActivePlayers) if(target.Distance(Projectile.Center) < Projectile.ai[0] / 30f * 204f && target.whoAmI != player.whoAmI) if(target.dead) {
				target.respawnTimer = 0;
				target.statLife = (int)((float)target.statLifeMax2 * 0.1f);
				target.DoSpecialTeleport(Projectile.Center, ThoriumTeleportType.DeathGazersGlass, true);
				target.SetImmune(180, 0, false, 127, "", default(Color), false, false, false);
			}
			else if(target.statLife < target.statLifeMax2) HealerHelper.HealPlayer(player, target, 1, 600);
			if(Projectile.ai[0] < 30f && Projectile.ai[1] == 1f) Projectile.ai[0]++;
			else if(Projectile.ai[0] > 0f && Projectile.ai[1] == 0f) if(--Projectile.ai[0] < 0f) Projectile.Kill();
		}
		public override bool PreDraw(ref Color lightColor) {
			lightColor = new Color(255, 0, 0, 0);
			float scale = Projectile.scale * MathHelper.Clamp(Projectile.ai[0] / 30f, 0f, 1f);
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Main.GlobalTimeWrappedHourly * MathHelper.PiOver2, texture.Size() * 0.5f, scale, SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Main.GlobalTimeWrappedHourly * -MathHelper.PiOver2, texture.Size() * 0.5f, scale * 0.42f, SpriteEffects.None, 0);
			texture = (Texture2D)ModContent.Request<Texture2D>(GlowTexture);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Main.GlobalTimeWrappedHourly * -MathHelper.PiOver2, texture.Size() * 0.5f, scale, SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Main.GlobalTimeWrappedHourly * MathHelper.PiOver2, texture.Size() * 0.5f, scale * 0.42f, SpriteEffects.None, 0);
			return false;
		}
		public override bool ShouldUpdatePosition() => false;
		public override bool? CanDamage() => false;
	}
}