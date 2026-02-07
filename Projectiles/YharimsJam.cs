using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace CalamityBardHealer.Projectiles
{
	public class YharimsJam : ModProjectile
	{
		public override string Texture => "CalamityBardHealer/Items/YharimsJam";
		public override string GlowTexture => "Terraria/Images/Projectile_694";
		public override void SetDefaults() {
			Projectile.width = 38;
			Projectile.height = 44;
			Projectile.aiStyle = -1;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 2;
		}
		public override void AI() {
			Player player = Main.player[Projectile.owner];
			if(player.GetModPlayer<ThorlamityPlayer>().yharimsJam && !player.dead) Projectile.timeLeft = 2;
			else {
				Projectile.Kill();
				return;
			}
			if(Projectile.Center != player.Center && Vector2.Distance(Projectile.Center, player.Center) < 1f) Projectile.Center = player.Center;
			else if(Vector2.Distance(Projectile.Center, player.Center) > 1f) Projectile.Center = Vector2.Lerp(Projectile.Center, player.Center, 0.8f);
			if(Main.myPlayer == player.whoAmI) {
				for(int i = 0; i < Projectile.ai.Length; i++) Projectile.ai[i] = Vector2.Normalize(Main.MouseWorld - (Projectile.Center + Vector2.UnitY.RotatedBy(MathHelper.ToRadians(120f) * i) * 64f)).ToRotation();
				NetMessage.SendData(27, -1, -1, null, Projectile.whoAmI);
			}
			if((player.itemTime > 0 || player.channel) && Projectile.localAI[0] % 30f == 0f) {
				int selectAI = (int)(Projectile.localAI[0] / 30f);
				if(Main.myPlayer == player.whoAmI) {
					int p = Projectile.NewProjectile(player.GetSource_Misc("Yharims Jam"), Projectile.Center + Vector2.UnitY.RotatedBy(MathHelper.ToRadians(120f) * (float)selectAI) * 64f, Projectile.ai[selectAI].ToRotationVector2() * 15f, ModContent.ProjectileType<Projectiles.BurningBeat>(), Projectile.damage, 0f, player.whoAmI);
					NetMessage.SendData(27, -1, -1, null, p);
				}
				Projectile.localAI[0]++;
			}
			if(Projectile.localAI[0] % 30f != 0f) Projectile.localAI[0]++;
			if(Projectile.localAI[0] >= 90f) Projectile.localAI[0] = 0f;
		}
		public override bool PreDraw(ref Color lightColor) {
			int selectAI = (int)(Projectile.localAI[0] / 30f);
			float frameLerp = (Projectile.localAI[0] - selectAI * 30f) / 30f;
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			for(int i = 0; i < Projectile.ai.Length; i++) {
				float rotation = Projectile.ai[i];
				bool faceLeft = rotation < -MathHelper.PiOver2 || rotation > MathHelper.PiOver2;
				Main.EntitySpriteDraw(texture, Projectile.Center + Vector2.UnitY.RotatedBy(MathHelper.ToRadians(120f) * i) * 64f - (i == selectAI ? Projectile.ai[selectAI].ToRotationVector2() * Vector2.UnitX.RotatedBy(frameLerp * MathHelper.Pi).Y * 4f : Vector2.Zero) - Main.screenPosition, null, Color.White, rotation, texture.Size() / 2 + Vector2.UnitY * (faceLeft ? 4f : -4f), Projectile.scale, faceLeft ? SpriteEffects.FlipVertically : SpriteEffects.None, 0);
			}
			frameLerp *= 2f;
			if(Projectile.localAI[0] % 30f == 0f || frameLerp >= 1f) return false;
			texture = (Texture2D)ModContent.Request<Texture2D>(GlowTexture);
			for(int i = 0; i < 2; i++) Main.EntitySpriteDraw(texture, Projectile.Center + Vector2.UnitY.RotatedBy(MathHelper.ToRadians(120f) * selectAI) * 64f - Main.screenPosition, new Rectangle(0, texture.Height / Main.projFrames[694] * (int)MathHelper.Lerp(1f, Main.projFrames[694], frameLerp), texture.Width, texture.Height / Main.projFrames[694]), new Color(255, 255, 255, i > 0 ? 255 : 0), Projectile.ai[selectAI] + MathHelper.PiOver2, new Vector2(texture.Width / 2, texture.Height / Main.projFrames[694]), Projectile.scale, Projectile.ai[selectAI] < -MathHelper.PiOver2 || Projectile.ai[selectAI] > MathHelper.PiOver2 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			return false;
		}
		public override bool? CanDamage() => false;
		public override bool ShouldUpdatePosition() => false;
	}
}