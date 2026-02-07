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
	public class Violince : BardProjectile
	{
		public override string Texture => "Terraria/Images/Projectile_280";
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[base.Projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[base.Projectile.type] = 0;
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 12, base.Projectile.type);
		}
		public override BardInstrumentType InstrumentType => BardInstrumentType.String;
		public override void SetBardDefaults() {
			base.Projectile.width = 6;
			base.Projectile.height = 6;
			base.Projectile.aiStyle = -1;
			base.Projectile.hostile = false;
			base.Projectile.friendly = true;
			base.Projectile.tileCollide = true;
			base.Projectile.extraUpdates = 2;
			base.Projectile.timeLeft = 300;
			base.Projectile.light = 0.25f;
			base.Projectile.DamageType = BardDamage.Instance;
		}
		public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) => target.AddBuff(BuffID.Ichor, 60);
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			float fade = (float)MathHelper.Min(base.Projectile.timeLeft, 15) / 15f;
			for(int k = 0; k < base.Projectile.oldPos.Length; k++) {
				Color glowColor = new Color(255, 255, 255, 0) * fade * MathHelper.Lerp(0.9f, 0f, (float)k / (float)base.Projectile.oldPos.Length);
				glowColor.A = 0;
				Main.EntitySpriteDraw(texture, base.Projectile.oldPos[k] + new Vector2(base.Projectile.width, base.Projectile.height) * 0.5f - Main.screenPosition, null, glowColor, Main.rand.NextFloat(MathHelper.TwoPi), texture.Size() * 0.5f, base.Projectile.scale * MathHelper.Lerp(0.9f, 0.6f, (float)k / (float)base.Projectile.oldPos.Length), SpriteEffects.None, 0);
			}
			return false;
		}
		public override bool OnTileCollide(Vector2 oldVelocity) {
			if(++base.Projectile.ai[2] < 3 + Main.player[base.Projectile.owner].GetModPlayer<ThoriumPlayer>().bardBounceBonus) {
				if(base.Projectile.velocity.X != oldVelocity.X) base.Projectile.velocity.X = -oldVelocity.X;
				if(base.Projectile.velocity.Y != oldVelocity.Y) base.Projectile.velocity.Y = -oldVelocity.Y;
				return false;
			}
			return true;
		}
	}
}