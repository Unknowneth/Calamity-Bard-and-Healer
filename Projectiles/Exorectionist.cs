using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items;
using System.Collections.Generic;

namespace CalamityBardHealer.Projectiles
{
	public class Exorectionist : ModProjectile
	{
		public override string Texture => "CalamityMod/Particles/Light";
		public override void SetStaticDefaults() => ProjectileID.Sets.DrawScreenCheckFluff[Type] = 1200;
		public override void SetDefaults() {
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.aiStyle = -1;
			Projectile.tileCollide = true;
			Projectile.timeLeft = 2;
		}
		public override void AI() {
			float range = 960f;
			if(Main.myPlayer == Projectile.owner) {
				Projectile.Center = Main.player[Projectile.owner].MountedCenter;
				Projectile.velocity = Vector2.Normalize(Main.MouseWorld - Projectile.Center);
				Projectile.position += Projectile.velocity * 30f;
				if(Main.player[Projectile.owner].itemTime > 0 && Main.player[Projectile.owner].HeldItem.GetGlobalItem<CalamityGlobalItem>().Charge > 0f) Projectile.ai[1] = 2f;
			}
			if(Projectile.ai[1] > 0f) {
				Projectile.timeLeft = 2;
				Projectile.ai[1]--;
			}
			if(!Collision.CanHitLine(Projectile.Center, 0, 0, Projectile.Center + Vector2.Normalize(Projectile.velocity) * Projectile.ai[0], 0, 0)) while(!Collision.CanHitLine(Projectile.Center, 0, 0, Projectile.Center + Vector2.Normalize(Projectile.velocity) * Projectile.ai[0], 0, 0)) {
				if(Projectile.ai[0] > 0f) {
					Projectile.ai[0]--;
					continue;
				}
				else break;
			}
			else while(Collision.CanHitLine(Projectile.Center, 0, 0, Projectile.Center + Vector2.Normalize(Projectile.velocity) * Projectile.ai[0], 0, 0)) if(Projectile.ai[0] < range) {
				Projectile.ai[0]++;
				continue;
			}
			else break;
			float point = 0f;
			Vector2 laserEnd = Projectile.Center + Vector2.Normalize(Projectile.velocity) * Projectile.ai[0];
			for(int i = 0; i < Main.maxPlayers; i++) if(Main.player[i].active && i != Projectile.owner && !Main.player[i].dead) if(Collision.CheckAABBvLineCollision(Main.player[i].Hitbox.TopLeft(), Main.player[i].Hitbox.Size(), Projectile.Center, laserEnd, Projectile.width + Projectile.height, ref point) && HealerHelper.HealPlayer(Main.player[Projectile.owner], Main.player[i], 14, 60) && Main.myPlayer == Projectile.owner) {
				Projectile.ai[0] = (int)Projectile.Distance(Main.player[i].Center);
				NetMessage.SendData(27, -1, -1, null, Projectile.whoAmI);
				break;
			}
		}
		public override bool PreDraw(ref Color lightColor) {
			if(Projectile.ai[0] <= 0f) return false;
			Vector2 startPos = Projectile.Center;
			Vector2 endPos = Projectile.Center + Vector2.Normalize(Projectile.velocity) * Projectile.ai[0];
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			List<Vector2> arcPoints = new List<Vector2>();
			int arcCount = (int)(Vector2.Distance(startPos, endPos) / 16f);
			for(int h = 1; h < arcCount; h++) arcPoints.Add(Vector2.SmoothStep(startPos, endPos, (float)h / (float)arcCount) + (h > 1 ? 1f : 0f) * Main.rand.NextVector2Circular(texture.Width, texture.Height) / 5f);
			for(int j = 0; j < 2; j++) {
				float scale = Projectile.scale * (j > 0 ? 0.1f : 0.2f);
				lightColor = j > 0 ? Color.White : Color.Cyan;
				Main.EntitySpriteDraw(texture, endPos - Main.screenPosition, null, lightColor, Main.rand.NextFloat(MathHelper.TwoPi), texture.Size() / 2f, scale, SpriteEffects.None, 0);
				for(int i = 0; i < arcPoints.Count; i++) {
					if(i > 0) startPos = arcPoints[i - 1];
					else startPos = Projectile.Center;
					if(i < arcPoints.Count - 1) endPos = arcPoints[i];
					else endPos = Projectile.Center + Vector2.Normalize(Projectile.velocity) * Projectile.ai[0];
					Main.EntitySpriteDraw(texture, startPos - Main.screenPosition, null, lightColor, Main.rand.NextFloat(MathHelper.TwoPi), texture.Size() / 2f, scale, SpriteEffects.None, 0);
					Main.EntitySpriteDraw(texture, startPos - Main.screenPosition, new Rectangle(0, texture.Height / 2 - 1, texture.Width, 1), lightColor, (startPos - endPos).ToRotation() + MathHelper.PiOver2, new Vector2(texture.Width / 2f, 0f), new Vector2(scale, Vector2.Distance(startPos, endPos)), SpriteEffects.None, 0);
				}
			}
			return false;
		}
	}
}