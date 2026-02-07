using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CalamityBardHealer.Projectiles
{
	public class DryMouthSand : ModProjectile
	{
		public override string Texture => "CalamityMod/Particles/SemiCircularSmear";
		public override void SetStaticDefaults() {
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 5, base.Projectile.type);
		}
		public override void SetDefaults() {
			Projectile.width = 135;
			Projectile.height = 135;
			Projectile.aiStyle = -1;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.DamageType = ModLoader.GetMod("ThoriumMod").Find<DamageClass>("HealerDamage");
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			Projectile.extraUpdates = 1;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 20;
			Projectile.ArmorPenetration = 5;
		}
		public override void AI() {
			Dust dust = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(Projectile.width, Projectile.height), Main.rand.NextBool(2) ? 288 : 207, Projectile.velocity, 0, default(Color), 0.6f);
			dust.noGravity = true;
			dust.scale = Main.rand.NextFloat(0.9f, 1.5f);
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			lightColor = Color.Peru * ((float)Projectile.timeLeft / 20f);
			lightColor.A = 0;
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.velocity.ToRotation(), texture.Size() / 2, Projectile.scale * 0.75f, SpriteEffects.FlipHorizontally, 0);
			return false;
		}
	}
}