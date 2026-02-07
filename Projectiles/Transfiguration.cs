using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using ThoriumMod;
using ThoriumMod.Buffs.Healer;
using System.IO;

namespace CalamityBardHealer.Projectiles
{
	public class Transfiguration : ModProjectile
	{
		public override string Texture => "CalamityBardHealer/Projectiles/Sparkle";
		public override string GlowTexture => "CalamityBardHealer/Projectiles/Ray";
		public override void SetDefaults() {
			Projectile.width = Projectile.height = 1;
			Projectile.aiStyle = -1;
			Projectile.tileCollide = false;
		}
		public override void AI() {
			Player player = Main.player[Projectile.owner];
			if(Main.myPlayer == player.whoAmI && player.channel && player.HeldItem.type == ModContent.ItemType<Items.Transfiguration>() && !player.dead) {
				if(Projectile.ai[1] < 60f) Projectile.ai[1]++;
				if(Projectile.ai[1] > 40f && player.itemTime == player.itemTimeMax - 1) Projectile.ai[2] = 1f;
				Projectile.Center = Vector2.SmoothStep(player.MountedCenter + Vector2.Normalize(Main.MouseWorld - player.MountedCenter) * (player.HeldItem.Size.Length() - 16f), Main.MouseWorld, MathHelper.Min(Projectile.ai[1], 40f) / 40f); 
				Projectile.ai[0] = 1f;
				NetMessage.SendData(27, -1, -1, null, Projectile.whoAmI);
			}
			if(Projectile.ai[2] > 0f) {
				float point = 675f;
				foreach(Player target in Main.ActivePlayers) if(Collision.CheckAABBvLineCollision(target.Hitbox.TopLeft(), target.Hitbox.Size(), Projectile.Center, Projectile.Center * Vector2.UnitY * point, MathHelper.Lerp(22f, 65f, Projectile.Distance(target.MountedCenter) / point), ref point) && !target.dead && target.statLife < target.statLifeMax2 && target.whoAmI != player.whoAmI) HealerHelper.HealPlayer(Main.player[Projectile.owner], target, 7, 60);
				Projectile.ai[2] = 0f;
			}
			if(--Projectile.ai[0] == 0f) Projectile.timeLeft = 15;
		}
		public override bool PreDraw(ref Color lightColor) {
			lightColor = Color.Gold * MathHelper.Min(1f, (float)Projectile.timeLeft / 15f);
			lightColor.A = 0;
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			float scale = Projectile.scale + MathHelper.Max(Projectile.ai[1] - 40f, 0f) / 20f;
			float rotation = (Main.GlobalTimeWrappedHourly * MathHelper.Pi).ToRotationVector2().ToRotation() * 2f;
			if(rotation > MathHelper.Pi) scale += Vector2.UnitX.RotatedBy(rotation - MathHelper.Pi).Y * 0.25f;
			for(int i = 1; i <= 10; i++) Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor * (1f / i), rotation, texture.Size() * 0.5f, scale * i * 0.2f, SpriteEffects.None, 0);
			lightColor *= 0.01f;
			texture = (Texture2D)ModContent.Request<Texture2D>(GlowTexture);
			for(int i = 0; i < (int)MathHelper.Max(Projectile.ai[1] - 40f, 0f) * 5; i++) Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, MathHelper.PiOver2, new Vector2(1, 8), Projectile.scale * new Vector2(MathHelper.Lerp(0.5f, 25f, (float)i / 100f), MathHelper.Lerp(0.4f, 4f, (float)i / 100f)), SpriteEffects.None, 0);
			return false;
		}
		public override bool ShouldUpdatePosition() => false;
		public override bool? CanDamage() => false;
	}
}