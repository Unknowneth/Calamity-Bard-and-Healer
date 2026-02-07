using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Utilities;
using ThoriumMod.Projectiles;
using ThoriumMod.Projectiles.Bard;

namespace CalamityBardHealer.Projectiles
{
	public class CosmicEncoreStar : BardProjectile
	{
		public override string Texture => "CalamityBardHealer/Projectiles/Sparkle";
		public override string GlowTexture => "Terraria/Images/Extra_98";
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
		public override BardInstrumentType InstrumentType => BardInstrumentType.Wind;
		public override void SetBardDefaults() {
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.aiStyle = -1;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 600;
			Projectile.extraUpdates = 1;
			Projectile.alpha = 255;
			base.Projectile.hostile = false;
			base.Projectile.friendly = true;
			base.Projectile.DamageType = BardDamage.Instance;
			base.Projectile.ArmorPenetration = 75;
			base.Projectile.penetrate = -1;
			base.Projectile.usesLocalNPCImmunity = true;
			base.Projectile.localNPCHitCooldown = 50;
		}
		public override void AI() {
			double speed = Projectile.velocity.Length();
			if(Projectile.timeLeft < 17) Projectile.alpha += 15;
			else if(Projectile.alpha > 0) Projectile.alpha -= 15;
			if(Projectile.ai[0] == 1f) {
				Projectile.velocity = Vector2.Zero;
				if(Projectile.timeLeft > 17) Projectile.timeLeft = 17;
				Projectile.ai[0] = 2f;
			}
			else if(Projectile.timeLeft < 540 && base.WindHomingCommon(null, 1000f, null, null, speed < 4f) && speed < 10f) Projectile.velocity *= 1.05f;
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Texture2D glowTexture = (Texture2D)ModContent.Request<Texture2D>(GlowTexture);
			for(int i = 0; i < base.Projectile.oldPos.Length; i++) {
				float rotation = MathHelper.ToRadians(Projectile.timeLeft * 4f + i * 12f);
				if(rotation < 0f) rotation += MathHelper.TwoPi;
				else if(rotation > MathHelper.TwoPi) rotation -= MathHelper.TwoPi;
				lightColor = Main.hslToRgb(System.Math.Abs(rotation / MathHelper.TwoPi) % 1f, 1f, 0.66f) * MathHelper.Lerp(0.5f, 0f, (float)base.Projectile.alpha / 255f) * MathHelper.Min(Projectile.velocity.Length() / 4f, 1f);
				lightColor.G = (byte)(0.5f * (float)lightColor.G);
				lightColor.A = 0;
				if(i == 0) {
					Main.EntitySpriteDraw(texture, base.Projectile.oldPos[i] + new Vector2(base.Projectile.width, base.Projectile.height) * 0.5f - Main.screenPosition, null, lightColor * 2.5f, Main.GlobalTimeWrappedHourly * MathHelper.Pi, texture.Size() / 2, base.Projectile.scale, SpriteEffects.None, 0);
					Main.EntitySpriteDraw(texture, base.Projectile.oldPos[i] + new Vector2(base.Projectile.width, base.Projectile.height) * 0.5f - Main.screenPosition, null, new Color(255, 255, 255, 0), Main.GlobalTimeWrappedHourly * MathHelper.Pi, texture.Size() / 2, base.Projectile.scale * 0.6f, SpriteEffects.None, 0);
				}
				else Main.EntitySpriteDraw(glowTexture, base.Projectile.oldPos[i] + new Vector2(base.Projectile.width, base.Projectile.height) * 0.5f - Main.screenPosition, null, lightColor, (base.Projectile.oldPos[i] - base.Projectile.oldPos[i - 1]).ToRotation() + MathHelper.PiOver2, glowTexture.Size() / 2, new Vector2(0.8f, base.Projectile.scale), SpriteEffects.None, 0);
			}
			return false;
		}
		public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			if(Main.myPlayer == Projectile.owner) {
				Projectile.ai[0] = 1f;
				NetMessage.SendData(27, -1, -1, null, Projectile.whoAmI);
			}
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) target.AddBuff(calamity.Find<ModBuff>("ElementalMix").Type, 240);
		}
	}
}