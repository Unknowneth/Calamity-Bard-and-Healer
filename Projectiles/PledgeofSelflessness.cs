using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityBardHealer.Projectiles
{
	public class PledgeofSelflessness : ModProjectile
	{
		public override string Texture => "Terraria/Images/Extra_98";
		public override string GlowTexture => "CalamityBardHealer/Projectiles/Sparkle";
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 100;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}
		public override void SetDefaults() {
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.aiStyle = -1;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 3;
			Projectile.timeLeft = 180;
		}
		public override void AI() {
			Projectile.rotation = Projectile.velocity.ToRotation();
			if(Projectile.ai[0] == 0f) {
				if(Projectile.timeLeft > 30) Projectile.timeLeft = 30;
				Projectile.velocity = Vector2.Normalize(Projectile.velocity) * float.Epsilon;
			}
			else if(Projectile.Hitbox.Intersects(Main.player[(int)Projectile.ai[0] - 1].Hitbox) && Main.player[(int)Projectile.ai[0] - 1].active && !Main.player[(int)Projectile.ai[0] - 1].dead && Main.player[(int)Projectile.ai[0] - 1].statLife < Main.player[(int)Projectile.ai[0] - 1].statLifeMax2 && HealerHelper.HealPlayer(Main.player[Projectile.owner], Main.player[(int)Projectile.ai[0] - 1], 25, 60) && Main.myPlayer == Projectile.owner) {
				Projectile.ai[0] = 0f;
				NetMessage.SendData(27, -1, -1, null, Projectile.whoAmI);
			}
			else Projectile.velocity = Vector2.Normalize(Vector2.Lerp(Vector2.Normalize(Projectile.velocity), Vector2.Normalize(Main.player[(int)Projectile.ai[0] - 1].Center - Projectile.Center), ++Projectile.ai[1] / 90f * 0.5f)) * Projectile.velocity.Length();
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			float fade = MathHelper.Lerp(1f, 0f, (int)Projectile.alpha / 255f) * (MathHelper.Min(Projectile.timeLeft, 30) / 30);
			if(Projectile.velocity != Vector2.Zero) for(int i = 0; i < Projectile.oldPos.Length; i++) Main.EntitySpriteDraw(texture, Projectile.oldPos[i] + new Vector2(Projectile.width, Projectile.height) / 2 + Main.rand.NextVector2Circular(i, i) / (float)Projectile.oldPos.Length- Main.screenPosition, null, Color.DarkRed * fade, Projectile.oldRot[i] + MathHelper.PiOver2, texture.Size() * 0.5f, new Vector2(MathHelper.Lerp(0.05f, Projectile.scale * 0.65f, (float)i / (float)Projectile.oldPos.Length)), SpriteEffects.None, 0);
			return false;
		}
	}
}