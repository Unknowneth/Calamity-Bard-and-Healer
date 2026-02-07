using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.Particles;

namespace CalamityBardHealer.Projectiles
{
	public class PhoenicianBeakSlice : ModProjectile
	{
		public override string Texture => "CalamityMod/Particles/SemiCircularSmear";
		public override void SetStaticDefaults() {
			Terraria.ID.ProjectileID.Sets.TrailCacheLength[Type] = 7;
			Terraria.ID.ProjectileID.Sets.TrailingMode[Type] = 0;
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 2, base.Projectile.type);
			mor.Call("addElementProj", 14, base.Projectile.type);
		}
		public override void SetDefaults() {
			Projectile.width = 180;
			Projectile.height = 180;
			Projectile.aiStyle = -1;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.DamageType = ModLoader.GetMod("ThoriumMod").Find<DamageClass>("HealerDamage");
			Projectile.ArmorPenetration = 50;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
			Projectile.extraUpdates = 2;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 180;
		}
		public override void AI() => GeneralParticleHandler.SpawnParticle(new HeavySmokeParticle(Projectile.Center + Projectile.velocity * 0.5f, Projectile.velocity * 0.5f, Color.DarkOrange, 10, Main.rand.NextFloat(2.1f, 2.5f) * Projectile.scale, 0.8f, Main.rand.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4), true, 0f, true));
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) target.AddBuff(calamity.Find<ModBuff>("Dragonfire").Type, 240);
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			lightColor = Color.DarkOrange;
			lightColor.A = 0;
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.velocity.ToRotation(), texture.Size() / 2, Projectile.scale, SpriteEffects.FlipHorizontally, 0);
			return false;
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			bool trailHit = false;
			for(int i = 0; i < Projectile.oldPos.Length; i++) trailHit |= targetHitbox.Intersects(new Rectangle((int)Projectile.oldPos[i].X, (int)Projectile.oldPos[i].Y, projHitbox.Width, projHitbox.Height));
			return projHitbox.Intersects(targetHitbox) || trailHit;
		}
	}
}