using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityBardHealer.Projectiles
{
	public class ElementalSyzygy : ModProjectile
	{
		public override string Texture => "CalamityBardHealer/Projectiles/Sparkle";
		public override string GlowTexture => "Terraria/Images/Extra_98";
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 50;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 1, base.Projectile.type);
			mor.Call("addElementProj", 14, base.Projectile.type);
		}
		public override void SetDefaults() {
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.aiStyle = -1;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.DamageType = ModLoader.GetMod("ThoriumMod").Find<DamageClass>("HealerDamage");
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 40;
			Projectile.timeLeft = 180;
			Projectile.extraUpdates = 3;
			Projectile.alpha = 255;
			Projectile.ArmorPenetration = 25;
		}
		public override void AI() {
			if(Projectile.timeLeft < 85) Projectile.alpha += 3;
			else if(Projectile.alpha > 0) Projectile.alpha -= 3;
			if(Projectile.localAI[0] == 0f && Projectile.localAI[1] == 0f) {
				Projectile.localAI[0] = Projectile.Center.X;
				Projectile.localAI[1] = Projectile.Center.Y;
			}
			Projectile.Center = new Vector2(Projectile.localAI[0], Projectile.localAI[1]) + Vector2.UnitX.RotatedBy(Projectile.ai[1]) * ++Projectile.ai[2];
			Projectile.ai[1] += Projectile.ai[0] / Projectile.ai[2];
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) target.AddBuff(calamity.Find<ModBuff>("ElementalMix").Type, 60);
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Texture2D glowTexture = (Texture2D)ModContent.Request<Texture2D>(GlowTexture);
			for(int i = 0; i < Projectile.oldPos.Length; i++) {
				float rotation = (Projectile.oldPos[i] + new Vector2(Projectile.width, Projectile.height) * 0.5f - new Vector2(Projectile.localAI[0], Projectile.localAI[1])).ToRotation();
				if(rotation < 0f) rotation += MathHelper.TwoPi;
				lightColor = Main.hslToRgb(System.Math.Abs(rotation / MathHelper.TwoPi) % 1f, 1f, 0.66f) * MathHelper.Lerp(0.5f, 0f, (float)Projectile.alpha / 255f);
				lightColor.A = 0;
				if(i == 0) Main.EntitySpriteDraw(texture, Projectile.oldPos[i] + new Vector2(Projectile.width, Projectile.height) * 0.5f - Main.screenPosition, null, lightColor * 2.5f, Main.GlobalTimeWrappedHourly * MathHelper.Pi, texture.Size() / 2, Projectile.scale * 0.75f, SpriteEffects.None, 0);
				else Main.EntitySpriteDraw(glowTexture, Projectile.oldPos[i] + new Vector2(Projectile.width, Projectile.height) * 0.5f - Main.screenPosition, null, lightColor, (Projectile.oldPos[i] - Projectile.oldPos[i - 1]).ToRotation() + MathHelper.PiOver2, glowTexture.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
			}
			return false;
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			bool trailHit = false;
			for(int i = 0; i < Projectile.oldPos.Length; i++) trailHit |= targetHitbox.Intersects(new Rectangle((int)Projectile.oldPos[i].X, (int)Projectile.oldPos[i].Y, projHitbox.Width, projHitbox.Height));
			return projHitbox.Intersects(targetHitbox) || trailHit;
		}
		public override bool ShouldUpdatePosition() => false;
	}
}