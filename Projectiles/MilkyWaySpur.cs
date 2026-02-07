using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.Particles;

namespace CalamityBardHealer.Projectiles
{
	public class MilkyWaySpur : ModProjectile
	{
		public override string Texture => "CalamityBardHealer/Projectiles/Sparkle";
		public override void SetStaticDefaults() {
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 8, base.Projectile.type);
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
			Projectile.localNPCHitCooldown = 30;
			Projectile.timeLeft = 60;
			Projectile.extraUpdates = 2;
			Projectile.alpha = 255;
			Projectile.ArmorPenetration = 35;
		}
		public override void AI() {
			Lighting.AddLight(base.Projectile.Center, (Projectile.ai[2] != 0 ? Color.DodgerBlue : Color.MediumVioletRed).ToVector3());
			if(++Projectile.localAI[2] > Projectile.extraUpdates) {
				GeneralParticleHandler.SpawnParticle(new HeavySmokeParticle(base.Projectile.Center, base.Projectile.velocity * 0.5f, Projectile.ai[2] != 0 ? Color.DodgerBlue : Color.MediumVioletRed, 10, Main.rand.NextFloat(0.6f, 1.2f) * base.Projectile.scale, 0.28f, 0f, false, 0f, true));
				Projectile.localAI[2] = 0f;
			}
			if(Projectile.timeLeft < 15) Projectile.alpha += 17;
			else if(Projectile.alpha > 0) Projectile.alpha -= 17;
			if(Projectile.localAI[0] == 0f && Projectile.localAI[1] == 0f) {
				Projectile.localAI[0] = Projectile.Center.X;
				Projectile.localAI[1] = Projectile.Center.Y;
			}
			float distance = MathHelper.Lerp(32f, 480f, (float)Projectile.timeLeft / 60f);
			Projectile.Center = new Vector2(Projectile.localAI[0], Projectile.localAI[1]) + Vector2.UnitX.RotatedBy(Projectile.ai[1]) * distance;
			Projectile.ai[1] += Projectile.ai[0] / distance;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) target.AddBuff(calamity.Find<ModBuff>("GodSlayerInferno").Type, 60);
		}
		public override bool PreDraw(ref Color lightColor) {
			float fade = MathHelper.Lerp(1f, 0f, (float)Projectile.alpha / 255f);
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			if(Projectile.ai[2] == 0f) lightColor = Color.Magenta;
			else lightColor = Color.Cyan;
			lightColor.A = 0;
			lightColor *= fade;
			float spinDirection = Projectile.ai[2] == 0f ? MathHelper.Pi : -MathHelper.Pi;
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Main.GlobalTimeWrappedHourly * -spinDirection, texture.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Main.GlobalTimeWrappedHourly * spinDirection, texture.Size() / 2, Projectile.scale * 1.25f, SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, new Color(255, 255, 255, 0) * fade, Main.GlobalTimeWrappedHourly * spinDirection, texture.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
			return false;
		}
		public override bool ShouldUpdatePosition() => false;
	}
}