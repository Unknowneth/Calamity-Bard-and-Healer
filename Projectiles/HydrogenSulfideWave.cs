using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Particles;

namespace CalamityBardHealer.Projectiles
{
	public class HydrogenSulfideWave : ModProjectile
	{
		public override string Texture => "CalamityMod/Particles/SemiCircularSmear";
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[Type] = 20;
			ProjectileID.Sets.TrailingMode[Type] = 2;
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 2, base.Projectile.type);
			mor.Call("addElementProj", 5, base.Projectile.type);
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
			Projectile.localNPCHitCooldown = 20;
			Projectile.extraUpdates = 1;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 180;
		}
		public override void AI() {
			if(Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height) && Projectile.timeLeft > 15 && Projectile.timeLeft < 150) Projectile.timeLeft = 15;
			if(Projectile.timeLeft % Projectile.MaxUpdates == 0) GeneralParticleHandler.SpawnParticle(new MediumMistParticle(Projectile.Center, -Projectile.velocity * 0.25f - Main.rand.NextVector2Circular(3.1f, 3.1f), Color.OrangeRed, Color.Blue, Main.rand.NextFloat(2.9f, 3.2f), 100f, Main.rand.NextFloat(0.03f, -0.03f)));
			Projectile.rotation = Projectile.velocity.ToRotation();
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			for(int i = 0; i < 7; i++) {
				float rot = MathHelper.ToRadians(MathHelper.ToRadians(36f) * i);
				Vector2 velocity = Projectile.velocity / 4f;
				Vector2 offset = velocity.RotatedBy((double)(rot * Main.rand.NextFloat(3.1f, 9.1f))) * new Vector2(Main.rand.NextFloat(1.5f, 5.5f));
				Vector2 velOffset = velocity.RotatedBy((double)(rot * Main.rand.NextFloat(3.1f, 9.1f))) * new Vector2(Main.rand.NextFloat(1.5f, 5.5f));
				GeneralParticleHandler.SpawnParticle(new MediumMistParticle(Projectile.Center + offset, velOffset * Main.rand.NextFloat(1.5f, 3f), Color.OrangeRed, Color.Blue, Main.rand.NextFloat(0.9f, 1.2f), 160f, Main.rand.NextFloat(0.03f, -0.03f)));
			}
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) target.AddBuff(calamity.Find<ModBuff>("CrushDepth").Type, 240);
			target.AddBuff(BuffID.OnFire3, 240);
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			for(int i = 0; i < Projectile.oldPos.Length; i++) {
				float colorLerp = (float)i / (float)Projectile.oldPos.Length;
				colorLerp *= colorLerp;
				lightColor = Color.Lerp(Color.OrangeRed, Color.Blue, colorLerp) * MathHelper.Lerp(0.8f * (float)MathHelper.Min(15, Projectile.timeLeft) / 15f, 0f, colorLerp);
				lightColor.A = 0;
				Main.EntitySpriteDraw(texture, Projectile.oldPos[i] + Projectile.Size * 0.5f - Main.screenPosition, null, lightColor, Projectile.oldRot[i], texture.Size() / 2, Projectile.scale, SpriteEffects.FlipHorizontally, 0);
			}
			return false;
		}
	}
}