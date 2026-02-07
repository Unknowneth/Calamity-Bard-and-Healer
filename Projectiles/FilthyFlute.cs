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
	public class FilthyFlute : BardProjectile
	{
		public override string Texture => "Terraria/Images/Projectile_95";
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[base.Projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[base.Projectile.type] = 0;
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 2, base.Projectile.type);
			mor.Call("addElementProj", 9, base.Projectile.type);
		}
		public override BardInstrumentType InstrumentType => BardInstrumentType.Wind;
		public override void SetBardDefaults() {
			base.Projectile.width = 10;
			base.Projectile.height = 10;
			base.Projectile.aiStyle = -1;
			base.Projectile.hostile = false;
			base.Projectile.friendly = true;
			base.Projectile.tileCollide = false;
			base.Projectile.extraUpdates = 2;
			base.Projectile.timeLeft = 300;
			base.Projectile.alpha = 255;
			base.Projectile.light = 0.25f;
			base.Projectile.DamageType = BardDamage.Instance;
			base.Projectile.penetrate = -1;
			base.Projectile.usesLocalNPCImmunity = true;
			base.Projectile.localNPCHitCooldown = -1;
		}
		public override void AI() {
			if(Collision.SolidCollision(base.Projectile.position, base.Projectile.width, base.Projectile.height)) {
				if(base.Projectile.timeLeft > 5) base.Projectile.timeLeft = 5;
				base.Projectile.ai[2]++;
			}
			else if(base.Projectile.ai[2] == 0f) {
				base.WindHomingCommon(null, 384f, null, null, false);
				if(base.Projectile.alpha > 0) base.Projectile.alpha -= 17;
			}
			else {
				if(base.Projectile.timeLeft > 5) base.Projectile.timeLeft = 5;
				base.Projectile.velocity *= 0.9f;
			}
		}
		public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			if(Main.myPlayer == base.Projectile.owner) {
				base.Projectile.ai[2]++;
				NetMessage.SendData(27, -1, -1, null, base.Projectile.owner);
			}
			target.AddBuff(BuffID.CursedInferno, 120);
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			float fade = (float)MathHelper.Min(base.Projectile.timeLeft, 5) / 5f * MathHelper.Lerp(1f, 0f, (float)base.Projectile.alpha / 255f);
			for(int k = 0; k < base.Projectile.oldPos.Length; k++) {
				Color glowColor = new Color(255, 255, 255, 0) * fade * MathHelper.Lerp(0.9f, 0f, (float)k / (float)base.Projectile.oldPos.Length);
				glowColor.A = 0;
				Main.EntitySpriteDraw(texture, base.Projectile.oldPos[k] + Main.rand.NextFloat(MathHelper.TwoPi).ToRotationVector2() * Main.rand.Next(1 + k) + new Vector2(base.Projectile.width, base.Projectile.height) * 0.5f - Main.screenPosition, null, glowColor, Main.rand.NextFloat(MathHelper.TwoPi), texture.Size() * 0.5f, base.Projectile.scale * MathHelper.Lerp(0.9f, 0.6f, (float)k / (float)base.Projectile.oldPos.Length), SpriteEffects.None, 0);
			}
			return false;
		}
		public override bool? CanDamage() => Projectile.ai[2] == 0f;
	}
}