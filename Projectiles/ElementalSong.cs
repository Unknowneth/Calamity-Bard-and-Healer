using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles;
using ThoriumMod.Projectiles.Bard;

namespace CalamityBardHealer.Projectiles
{
	public class ElementalSong : BardProjectile
	{
		public override string Texture => "CalamityBardHealer/Projectiles/Sparkle";
		public override string GlowTexture => "Terraria/Images/Extra_98";
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[base.Projectile.type] = 50;
			ProjectileID.Sets.TrailingMode[base.Projectile.type] = 0;
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 1, base.Projectile.type);
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
			base.Projectile.localNPCHitCooldown = 40;
			base.Projectile.timeLeft = 180;
			base.Projectile.extraUpdates = 1;
			base.Projectile.alpha = 255;
			base.Projectile.ArmorPenetration = 25;
		}
		public override void AI() {
			if(base.Projectile.timeLeft < 17) base.Projectile.alpha += 15;
			else if(base.Projectile.alpha > 0) base.Projectile.alpha -= 15;
			if(!base.WindHomingCommon(null, 640f, null, null, true) || base.Projectile.velocity.Length() < base.Projectile.ai[0]) base.Projectile.velocity = Vector2.Normalize(base.Projectile.velocity) * base.Projectile.ai[0];
			base.Projectile.position += (base.Projectile.velocity.ToRotation() + Vector2.UnitY.RotatedBy(MathHelper.ToRadians(base.Projectile.ai[1] * 4f)).X * MathHelper.PiOver4 * base.Projectile.ai[2]).ToRotationVector2() * base.Projectile.velocity.Length();
			if(++base.Projectile.ai[1] >= 90f) base.Projectile.ai[1] = 0f;
		}
		public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) target.AddBuff(calamity.Find<ModBuff>("ElementalMix").Type, 120);
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Texture2D glowTexture = (Texture2D)ModContent.Request<Texture2D>(GlowTexture);
			for(int i = 0; i < base.Projectile.oldPos.Length; i++) {
				float rotation = MathHelper.ToRadians(base.Projectile.ai[1] * 4f + i * 9f);
				if(rotation < 0f) rotation += MathHelper.TwoPi;
				lightColor = Main.hslToRgb(System.Math.Abs(rotation / MathHelper.TwoPi) % 1f, 1f, 0.66f) * MathHelper.Lerp(0.5f, 0f, (float)base.Projectile.alpha / 255f);
				lightColor.A = 0;
				if(i == 0) Main.EntitySpriteDraw(texture, base.Projectile.oldPos[i] + new Vector2(base.Projectile.width, base.Projectile.height) * 0.5f - Main.screenPosition, null, lightColor * 2.5f, Main.GlobalTimeWrappedHourly * MathHelper.Pi, texture.Size() / 2, base.Projectile.scale, SpriteEffects.None, 0);
				else Main.EntitySpriteDraw(glowTexture, base.Projectile.oldPos[i] + new Vector2(base.Projectile.width, base.Projectile.height) * 0.5f - Main.screenPosition, null, lightColor, (base.Projectile.oldPos[i] - base.Projectile.oldPos[i - 1]).ToRotation() + MathHelper.PiOver2, glowTexture.Size() / 2, base.Projectile.scale, SpriteEffects.None, 0);
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