using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityBardHealer.Projectiles
{
	public class TherapeuticSludge : ModProjectile
	{
		public override string Texture => "CalamityMod/Projectiles/Boss/UnstableCrimulanGlob";
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}
		public override void SetDefaults() {
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.aiStyle = -1;
			Projectile.tileCollide = true;
			Projectile.timeLeft = 300;
			Projectile.extraUpdates = 1;
		}
		public override void AI() {
			if(Projectile.localAI[0] < 30f) Projectile.localAI[0]++;
			else for(int i = 0; i < Main.maxPlayers; i++) if(Projectile.Hitbox.Intersects(Main.player[i].Hitbox) && Main.player[i].active && !Main.LocalPlayer.dead && Main.player[i].statLife < Main.player[i].statLifeMax2 && Main.player[i].team == Main.player[Projectile.owner].team && Projectile.localAI[0] >= 30f) {
				if(HealerHelper.HealPlayer(Main.player[Projectile.owner], Main.player[i], 13, 120, true, delegate(Player p) { p.AddBuff(ModContent.BuffType<ThoriumMod.Buffs.Healer.Cured>(), 30, true, false); })) {
					Projectile.localAI[0] = 0f;
					Main.player[i].AddBuff(ModContent.BuffType<ThoriumMod.Buffs.Healer.Cured>(), 30, true, false);
				}
				break;
			}
			Projectile.velocity.Y += 0.2f;
			Projectile.rotation = Projectile.velocity.ToRotation();
		}

		public override bool OnTileCollide(Vector2 oldVelocity) {
			if(base.Projectile.velocity.X != oldVelocity.X) base.Projectile.velocity.X = -oldVelocity.X;
			if(base.Projectile.velocity.Y != oldVelocity.Y) base.Projectile.velocity.Y = -oldVelocity.Y;
			return false;
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			for(int i = 1; i < Projectile.oldPos.Length; i++) Main.EntitySpriteDraw(texture, Projectile.oldPos[i] + Projectile.Size * 0.5f - Main.screenPosition, null, new Color(100, 100, 100, 0) * MathHelper.Lerp(1f, 0f, (float)i / (float)Projectile.oldPos.Length), Projectile.oldRot[i], texture.Size() * 0.5f, new Vector2(1f + Projectile.velocity.Length() * 0.01f, 1f - Projectile.velocity.Length() * 0.01f) * Projectile.scale, SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, texture.Size() * 0.5f, new Vector2(1f + Projectile.velocity.Length() * 0.01f, 1f - Projectile.velocity.Length() * 0.01f) * Projectile.scale, SpriteEffects.None, 0);
			return false;
		}
	}
}