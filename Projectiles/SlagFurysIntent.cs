using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityBardHealer.Projectiles
{
	public class SlagFurysIntent : ModProjectile
	{
		public override string Texture => "Terraria/Images/Extra_98";
		public override string GlowTexture => "CalamityBardHealer/Projectiles/Sparkle";
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 2, base.Projectile.type);
			mor.Call("addElementProj", 12, base.Projectile.type);
		}
		public override void SetDefaults() {
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.DamageType = ModLoader.GetMod("ThoriumMod").Find<DamageClass>("HealerDamage");
			Projectile.extraUpdates = 2;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
			Projectile.timeLeft = 180;
		}
		public override void AI() {
			if(Projectile.ai[2] < 120f) Projectile.ai[2]++;
			if(Projectile.ai[1] < -20f) Projectile.ai[1] = 20f;
			else if(Projectile.ai[1] < 20f) Projectile.ai[1]++;
			Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(Projectile.ai[0]) * MathHelper.Lerp(1f, 0f, Projectile.ai[2] / 120f) * Projectile.ai[1] / 20f);
			Projectile.rotation = Projectile.velocity.ToRotation();
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) target.AddBuff(calamity.Find<ModBuff>("BrimstoneFlames").Type, 180);
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			float fade = MathHelper.Lerp(1f, 0f, (int)Projectile.alpha / 255f) * (MathHelper.Min(Projectile.timeLeft, 30) / 30);
			if(Projectile.velocity != Vector2.Zero) for(int i = 0; i < Projectile.oldPos.Length; i++) Main.EntitySpriteDraw(texture, Projectile.oldPos[i] + new Vector2(Projectile.width, Projectile.height) / 2 + Main.rand.NextVector2Circular(i, i) / (float)Projectile.oldPos.Length- Main.screenPosition, null, new Color(255, 0, 0, 0) * fade, Projectile.oldRot[i] + MathHelper.PiOver2, texture.Size() * 0.5f, new Vector2(MathHelper.Lerp(Projectile.scale * 0.65f, 0.05f, (float)i / (float)Projectile.oldPos.Length)), SpriteEffects.None, 0);
			texture = (Texture2D)ModContent.Request<Texture2D>(GlowTexture);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, new Color(255, 0, 0, 0) * fade, Projectile.rotation, texture.Size() * 0.5f, Projectile.scale * fade, SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, new Color(255, 0, 0, 0) * fade, Projectile.rotation + MathHelper.PiOver4, texture.Size() * 0.5f, Projectile.scale * fade * 0.5f, SpriteEffects.None, 0);
			return false;
		}
	}
}