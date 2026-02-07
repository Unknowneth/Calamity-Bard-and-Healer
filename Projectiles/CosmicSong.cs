using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles;
using ThoriumMod.Projectiles.Bard;
using CalamityMod.Particles;

namespace CalamityBardHealer.Projectiles
{
	public class CosmicSong : BardProjectile
	{
		public override string Texture => "CalamityBardHealer/Projectiles/Sparkle";
		public override string GlowTexture => "Terraria/Images/Extra_98";
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[base.Projectile.type] = 30;
			ProjectileID.Sets.TrailingMode[base.Projectile.type] = 0;
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 8, base.Projectile.type);
			mor.Call("addElementProj", 14, base.Projectile.type);
		}
		public override BardInstrumentType InstrumentType => BardInstrumentType.Wind;
		public override void SetBardDefaults() {
			base.Projectile.width = 32;
			base.Projectile.height = 32;
			base.Projectile.aiStyle = -1;
			base.Projectile.hostile = false;
			base.Projectile.friendly = true;
			base.Projectile.DamageType = BardDamage.Instance;
			base.Projectile.tileCollide = false;
			base.Projectile.penetrate = -1;
			base.Projectile.usesLocalNPCImmunity = true;
			base.Projectile.localNPCHitCooldown = 50;
			base.Projectile.timeLeft = 240;
			base.Projectile.extraUpdates = 2;
			base.Projectile.alpha = 255;
			base.Projectile.ArmorPenetration = 75;
		}
		public override void AI() {
			float colorProgress = MathHelper.ToRadians(base.Projectile.ai[1] * 4f);
			if(colorProgress < 0f) colorProgress += MathHelper.TwoPi;
			else if(colorProgress > MathHelper.TwoPi) colorProgress -= MathHelper.TwoPi;
			Color glowColor = Main.hslToRgb(System.Math.Abs(colorProgress / MathHelper.TwoPi) % 1f, 1f, 0.7f, byte.MaxValue);
			glowColor.G = (byte)(0.5f * (float)glowColor.G);
			Lighting.AddLight(base.Projectile.Center, glowColor.ToVector3());
			if(++base.Projectile.localAI[2] > ProjectileID.Sets.TrailCacheLength[base.Projectile.type]) {
				GeneralParticleHandler.SpawnParticle(new HeavySmokeParticle(base.Projectile.Center, base.Projectile.velocity * 0.5f, glowColor * 0.9f, 15, Main.rand.NextFloat(0.4f, 0.7f) * base.Projectile.scale * 0.8f, 0.8f, Main.rand.NextFloat(-0.03f, 0.03f), true, 0.01f, true));
				base.Projectile.localAI[2] -= base.Projectile.MaxUpdates;
			}
			if(base.Projectile.timeLeft < 51) base.Projectile.alpha += 5;
			else if(base.Projectile.alpha > 0) base.Projectile.alpha -= 15;
			if(!base.WindHomingCommon(null, 640f, null, null, true) || base.Projectile.velocity.Length() < base.Projectile.ai[0]) base.Projectile.velocity = Vector2.Normalize(base.Projectile.velocity) * base.Projectile.ai[0];
			base.Projectile.position += (base.Projectile.velocity.ToRotation() + Vector2.UnitY.RotatedBy(MathHelper.ToRadians(base.Projectile.ai[1] * 4f)).X * MathHelper.PiOver4 * base.Projectile.ai[2]).ToRotationVector2() * base.Projectile.velocity.Length();
			if(++base.Projectile.ai[1] >= 90f) base.Projectile.ai[1] = 0f;
		}
		public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			if(Main.player[Projectile.owner].HeldItem?.ModItem is Items.SongoftheCosmos modItem) modItem.charge += damageDone / 50;
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) target.AddBuff(calamity.Find<ModBuff>("ElementalMix").Type, 240);
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Texture2D glowTexture = (Texture2D)ModContent.Request<Texture2D>(GlowTexture);
			for(int i = 0; i < base.Projectile.oldPos.Length; i++) {
				float rotation = MathHelper.ToRadians(base.Projectile.ai[1] * 4f + i * 12f);
				if(rotation < 0f) rotation += MathHelper.TwoPi;
				else if(rotation > MathHelper.TwoPi) rotation -= MathHelper.TwoPi;
				lightColor = Main.hslToRgb(System.Math.Abs(rotation / MathHelper.TwoPi) % 1f, 1f, 0.66f) * MathHelper.Lerp(0.5f, 0f, (float)base.Projectile.alpha / 255f);
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
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			bool trailHit = false;
			for(int i = 0; i < base.Projectile.oldPos.Length; i++) trailHit |= targetHitbox.Intersects(new Rectangle((int)base.Projectile.oldPos[i].X, (int)base.Projectile.oldPos[i].Y, projHitbox.Width, projHitbox.Height));
			return projHitbox.Intersects(targetHitbox) || trailHit;
		}
		public override bool ShouldUpdatePosition() => false;
	}
}