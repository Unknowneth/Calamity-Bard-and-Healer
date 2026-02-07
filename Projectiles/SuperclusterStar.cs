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
	public class SuperclusterStar : BardProjectile
	{
		public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod("CatalystMod");
		public override string Texture => "CalamityBardHealer/Projectiles/Sparkle";
		public override string GlowTexture => "CatalystMod/Assets/Glow";
		public override BardInstrumentType InstrumentType => BardInstrumentType.Brass;
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[base.Projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[base.Projectile.type] = 0;
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 14, base.Projectile.type);
		}
		public override void SetBardDefaults() {
			base.Projectile.width = 28;
			base.Projectile.height = 28;
			base.Projectile.aiStyle = -1;
			base.Projectile.hostile = false;
			base.Projectile.friendly = true;
			base.Projectile.DamageType = BardDamage.Instance;
			base.Projectile.tileCollide = false;
			base.Projectile.timeLeft = 120;
			base.Projectile.penetrate = -1;
			base.Projectile.usesLocalNPCImmunity = true;
			base.Projectile.localNPCHitCooldown = 30;
		}
		public override void AI() => base.Projectile.velocity *= 0.98f;
		public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			if(ModLoader.TryGetMod("CatalystMod", out Mod catalyst)) target.AddBuff(catalyst.Find<ModBuff>("AstralBlight").Type, 240);
		}
		public override bool PreDraw(ref Color lightColor) {
			float fade = (float)MathHelper.Min(15, base.Projectile.timeLeft) / 15f;
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(GlowTexture);
			Vector2 origin = texture.Size() / 2f;
			int trailLength = base.Projectile.oldPos.Length;
			for(int i = 0; i < trailLength; i++) Main.spriteBatch.Draw(texture, base.Projectile.oldPos[i] + base.Projectile.Size * 0.5f - Main.screenPosition, null, new Color(220, 95, 210, 0) * (1f - 1f / (float)trailLength * (float)i) * fade * 0.5f, base.Projectile.rotation, origin, base.Projectile.scale * (1f - 1f / (float)trailLength * (float)i) * fade, 0, 0f);
			texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			origin = texture.Size() / 2f;
			lightColor = new Color(255, 233, 2, 0) * fade; 
			Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, lightColor, base.Projectile.rotation, origin, base.Projectile.scale * fade * 0.5f, SpriteEffects.None, 0);
			return false;
		}
	}
}