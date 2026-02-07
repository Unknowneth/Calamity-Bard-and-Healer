using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityBardHealer.Projectiles
{
	public class DeathAdderSoul : ModProjectile
	{
		public override string GlowTexture => "CalamityBardHealer/Projectiles/Sparkle";
		public override void SetStaticDefaults() {
			Main.projFrames[Type] = 3;
			ProjectileID.Sets.TrailCacheLength[Type] = 10;
			ProjectileID.Sets.TrailingMode[Type] = 2;
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 9, base.Projectile.type);
			mor.Call("addElementProj", 12, base.Projectile.type);
		}
		public override void SetDefaults() {
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.aiStyle = -1;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.DamageType = ThoriumMod.HealerDamage.Instance;
			Projectile.ArmorPenetration = 35;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 2;
			Projectile.timeLeft = 600;
		}
		public override void AI() {
			if(++Projectile.frameCounter >= 12) Projectile.frameCounter = 0;
			Projectile.frame = (int)(Projectile.frameCounter / 4f);
			if(Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height)) {
				if(Projectile.timeLeft > 15) Projectile.timeLeft = 15;
				Projectile.ai[0] = 0f;
			}
			else if(Projectile.timeLeft > 595 && Projectile.ai[1] == 0f && Main.myPlayer == Projectile.owner) {
				float maxRange = 3200f;
				for(int i = 0; i < Main.maxNPCs; i++) if(Main.npc[i].CanBeChasedBy(this, false) && Collision.CanHitLine(Main.MouseWorld, 0, 0, Main.npc[i].Center, 0, 0) && Main.npc[i].Distance(Main.MouseWorld) < maxRange) {
					maxRange = Main.npc[i].Distance(Main.MouseWorld);
					Projectile.ai[1] = i + 1;
				}
				if(Projectile.ai[1] > 0f) NetMessage.SendData(27, -1, -1, null, Projectile.whoAmI);
			}
			else if(Projectile.ai[1] > 0f) if(!Main.npc[(int)Projectile.ai[1] - 1].CanBeChasedBy(this, false)) Projectile.ai[0] = Projectile.ai[1] = 0f;
			else if(Projectile.timeLeft < 570) Projectile.velocity = Vector2.Normalize(Vector2.Lerp(Vector2.Normalize(Projectile.velocity), Vector2.Normalize(Main.npc[(int)Projectile.ai[1] - 1].Center - Projectile.Center), MathHelper.Min(++Projectile.ai[0] / 120f, 1f) * 0.75f)) * Projectile.velocity.Length() * 1.005f;
			Projectile.rotation = Projectile.velocity.ToRotation();
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) target.AddBuff(calamity.Find<ModBuff>("WhisperingDeath").Type, 180);
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			float fade = (float)MathHelper.Min(Projectile.timeLeft, 15) / 15f;
			for(int k = 1; k < Projectile.oldPos.Length; k++) Main.EntitySpriteDraw(texture, Projectile.oldPos[k] + Projectile.Size * 0.5f + Vector2.UnitY.RotatedBy(Projectile.oldRot[k]) * (k / (float)Projectile.oldPos.Length) * (float)System.Math.Sin(k * 4f / Projectile.oldPos.Length * MathHelper.TwoPi + Main.GlobalTimeWrappedHourly * MathHelper.Pi) * 3f - Main.screenPosition, new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Projectile.type]), Color.Lerp(new Color(150, 150, 150, 0), new Color(200, 50, 50, 0), MathHelper.Lerp(1f, 0f, (float)k / (float)Projectile.oldPos.Length) * fade) * MathHelper.Lerp(1f, 0f, (float)k / (float)Projectile.oldPos.Length) * fade, Projectile.oldRot[k] - MathHelper.PiOver2, new Vector2(8, 20), Projectile.scale * MathHelper.Lerp(1.25f, 0.75f, (float)k / (float)Projectile.oldPos.Length) * fade, SpriteEffects.None, 0);
			texture = (Texture2D)ModContent.Request<Texture2D>(GlowTexture);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, new Color(200, 50, 50, 0) * fade, Main.GlobalTimeWrappedHourly * MathHelper.TwoPi, texture.Size() / 2f, Projectile.scale * 0.5f * fade, SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, new Color(200, 50, 50, 0) * fade, Main.GlobalTimeWrappedHourly * MathHelper.Pi, texture.Size() / 2f, Projectile.scale * fade, SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, new Color(200, 50, 50, 0) * fade, Main.GlobalTimeWrappedHourly * MathHelper.PiOver2, texture.Size() / 2f, Projectile.scale * 1.5f * fade, SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, new Color(150, 150, 150, 0) * fade, Main.GlobalTimeWrappedHourly * MathHelper.TwoPi, texture.Size() / 2f, Projectile.scale * 0.25f * fade, SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, new Color(150, 150, 150, 0) * fade, Main.GlobalTimeWrappedHourly * MathHelper.Pi, texture.Size() / 2f, Projectile.scale * 0.75f * fade, SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, new Color(150, 150, 150, 0) * fade, Main.GlobalTimeWrappedHourly * MathHelper.PiOver2, texture.Size() / 2f, Projectile.scale * 1.25f * fade, SpriteEffects.None, 0);
			texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Projectile.type]), Color.White * fade, Projectile.rotation - MathHelper.PiOver2, new Vector2(8, 20), Projectile.scale * fade, SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Projectile.type]), new Color(100, 100, 100, 0) * fade, Projectile.rotation - MathHelper.PiOver2, new Vector2(8, 20), Projectile.scale * fade + Vector2.UnitX.RotatedBy(Main.GlobalTimeWrappedHourly * MathHelper.Pi).Y * 0.1f, SpriteEffects.None, 0);
			return false;
		}
	}
}