using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityBardHealer.Projectiles
{
	public class ColdShoulderIcicle : ModProjectile
	{
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 4, base.Projectile.type);
		}
		public override void SetDefaults() {
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.aiStyle = 1;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.DamageType = ModLoader.GetMod("ThoriumMod").Find<DamageClass>("HealerDamage");
			Projectile.extraUpdates = 1;
			Projectile.alpha = 255;
			Projectile.timeLeft = 300;
			Projectile.ArmorPenetration = 10;
		}
		public override void AI() => Projectile.rotation = Projectile.velocity.ToRotation();
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) => target.AddBuff(BuffID.Frostburn, 180);
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			float fade = MathHelper.Lerp(0.5f, 0f, (int)Projectile.alpha / 255f) * (MathHelper.Min(Projectile.timeLeft, 30) / 30);
			if(Projectile.velocity != Vector2.Zero) for(int i = 0; i < Projectile.oldPos.Length; i++) Main.EntitySpriteDraw(texture, Projectile.oldPos[i] + Projectile.Size * 0.5f - Main.screenPosition, null, lightColor * (i > 0 ? MathHelper.Lerp(0.25f, 0f, (float)i / (float)Projectile.oldPos.Length) : 1f), Projectile.oldRot[i] + MathHelper.PiOver4, texture.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0);
			return false;
		}
	}
}