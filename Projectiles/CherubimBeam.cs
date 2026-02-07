using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityBardHealer.Projectiles
{
	public class CherubimBeam : ModProjectile
	{
		public override string Texture => "Terraria/Images/Extra_98";
		public override string GlowTexture => "CalamityBardHealer/Projectiles/Sparkle";
		public override void SetStaticDefaults() {
			Main.projFrames[Type] = 3;
			ProjectileID.Sets.TrailCacheLength[Type] = 15;
			ProjectileID.Sets.TrailingMode[Type] = 2;
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 8, base.Projectile.type);
			mor.Call("addElementProj", 14, base.Projectile.type);
		}
		public override void SetDefaults() {
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.aiStyle = -1;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.DamageType = ModLoader.GetMod("ThoriumMod").Find<DamageClass>("HealerDamage");
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 2;
			Projectile.timeLeft = 180;
			Projectile.ArmorPenetration = 75;
		}
		public override void AI() {
			if(++Projectile.frameCounter >= 12) Projectile.frameCounter = 0;
			Projectile.frame = (int)(Projectile.frameCounter / 4f);
			if(Projectile.ai[1] == 0f && Main.myPlayer == Projectile.owner) {
				float maxRange = 1600f;
				for(int i = 0; i < Main.maxNPCs; i++) if(Main.npc[i].CanBeChasedBy(this, false) && Main.npc[i].Distance(Main.MouseWorld) < maxRange) {
					maxRange = Main.npc[i].Distance(Main.MouseWorld);
					Projectile.ai[1] = i + 1;
				}
				if(Projectile.ai[1] > 0f) NetMessage.SendData(27, -1, -1, null, Projectile.whoAmI);
			}
			else if(Projectile.ai[1] > 0f) if(!Main.npc[(int)Projectile.ai[1] - 1].CanBeChasedBy(this, false)) Projectile.ai[0] = Projectile.ai[1] = 0f;
			else Projectile.velocity = Vector2.Normalize(Vector2.Lerp(Vector2.Normalize(Projectile.velocity), Vector2.Normalize(Main.npc[(int)Projectile.ai[1] - 1].Center - Projectile.Center), MathHelper.Min(++Projectile.ai[0] / 60f, 1f) * 0.75f)) * Projectile.velocity.Length() * 1.005f;
			Projectile.rotation = Projectile.velocity.ToRotation();
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) target.AddBuff(calamity.Find<ModBuff>("MiracleBlight").Type, 180);
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			float fade = (float)MathHelper.Min(Projectile.timeLeft, 15) / 15f;
			float rotation = 0f;
			for(int k = 1; k < Projectile.oldPos.Length; k++) {
				rotation = MathHelper.ToRadians(Projectile.timeLeft + k * Projectile.oldPos.Length) - base.Projectile.oldRot[k];
				while(rotation > MathHelper.Pi) rotation -= MathHelper.TwoPi;
				while(rotation < -MathHelper.Pi) rotation += MathHelper.TwoPi;
				lightColor = Main.hslToRgb(System.Math.Abs(rotation / MathHelper.TwoPi) % 1f, 1f, 0.66f) * 0.9f;
				lightColor.A = 0;
				Main.EntitySpriteDraw(texture, Projectile.oldPos[k] + Projectile.Size * 0.5f - Main.screenPosition, null, Color.Lerp(new Color(150, 150, 150, 0), lightColor, MathHelper.Lerp(1f, 0f, (float)k / (float)Projectile.oldPos.Length) * fade) * MathHelper.Lerp(1f, 0f, (float)k / (float)Projectile.oldPos.Length) * fade, Projectile.oldRot[k] - MathHelper.PiOver2, texture.Size() * 0.5f, Projectile.scale * MathHelper.Lerp(1.1f, 0.2f, (float)k / (float)Projectile.oldPos.Length) * fade, SpriteEffects.None, 0);
			}
			rotation = MathHelper.ToRadians(Projectile.timeLeft) - base.Projectile.rotation;
			while(rotation > MathHelper.Pi) rotation -= MathHelper.TwoPi;
			while(rotation < -MathHelper.Pi) rotation += MathHelper.TwoPi;
			lightColor = Main.hslToRgb(System.Math.Abs(rotation / MathHelper.TwoPi) % 1f, 1f, 0.66f) * 0.7f;
			lightColor.A = 0;
			texture = (Texture2D)ModContent.Request<Texture2D>(GlowTexture);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor * fade, Main.GlobalTimeWrappedHourly * MathHelper.TwoPi, texture.Size() / 2f, Projectile.scale * 0.25f * fade, SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor * fade, Main.GlobalTimeWrappedHourly * MathHelper.Pi, texture.Size() / 2f, Projectile.scale * 0.5f * fade, SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor * fade, Main.GlobalTimeWrappedHourly * MathHelper.PiOver2, texture.Size() / 2f, Projectile.scale * 0.75f * fade, SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, new Color(150, 150, 150, 0) * fade, Main.GlobalTimeWrappedHourly * MathHelper.TwoPi, texture.Size() / 2f, Projectile.scale * 0.125f * fade, SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, new Color(150, 150, 150, 0) * fade, Main.GlobalTimeWrappedHourly * MathHelper.Pi, texture.Size() / 2f, Projectile.scale * 0.375f * fade, SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, new Color(150, 150, 150, 0) * fade, Main.GlobalTimeWrappedHourly * MathHelper.PiOver2, texture.Size() / 2f, Projectile.scale * 0.625f * fade, SpriteEffects.None, 0);
			return false;
		}
	}
}