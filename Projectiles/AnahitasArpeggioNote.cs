using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles;
using ThoriumMod.Projectiles.Bard;

namespace CalamityBardHealer.Projectiles
{
	public class AnahitasArpeggioNote : BardProjectile
	{
		public override string Texture => "CalamityMod/Projectiles/Magic/AnahitasArpeggioNote";
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[base.Projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[base.Projectile.type] = 0;
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 3, base.Projectile.type);
		}
		public override BardInstrumentType InstrumentType => BardInstrumentType.String;
		public override void SetBardDefaults() {
			base.Projectile.CloneDefaults(ModLoader.GetMod("CalamityMod").Find<ModProjectile>("AnahitasArpeggioNote").Type);
			base.Projectile.DamageType = BardDamage.Instance;
		}
		public override void AI() {
			if(base.Projectile.velocity.Length() > 4f) base.Projectile.velocity *= 0.985f;
			if(base.Projectile.localAI[0] == 0f) {
				base.Projectile.scale += 0.02f;
				if(base.Projectile.scale >= 1.25f) base.Projectile.localAI[0] = 1f;
			}
			else if(base.Projectile.localAI[0] == 1f) {
				base.Projectile.scale -= 0.02f;
				if (base.Projectile.scale <= 0.75f) base.Projectile.localAI[0] = 0f;
			}
			if(base.Projectile.ai[1] == 0f) {
				base.Projectile.ai[1] = 1f;
				Main.musicPitch = base.Projectile.ai[0];
				SoundEngine.PlaySound(SoundID.Item26, base.Projectile.position);
			}
			Lighting.AddLight(base.Projectile.Center, 0f, 0f, 1.2f);
			if(base.Projectile.velocity.X > 0f) base.Projectile.rotation = Projectile.velocity.ToRotation();
			else base.Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.Pi;
		}
		public override bool OnTileCollide(Vector2 oldVelocity) {
			if(++base.Projectile.ai[2] < 3 + Main.player[base.Projectile.owner].GetModPlayer<ThoriumPlayer>().bardBounceBonus) {
				if(base.Projectile.velocity.X != oldVelocity.X) base.Projectile.velocity.X = -oldVelocity.X;
				if(base.Projectile.velocity.Y != oldVelocity.Y) base.Projectile.velocity.Y = -oldVelocity.Y;
				return false;
			}
			return true;
		}
		public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			target.immune[base.Projectile.owner] = 7;
			target.AddBuff(31, 300, false);
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			for(int i = 0; i < base.Projectile.oldPos.Length; i++) Main.EntitySpriteDraw(texture, base.Projectile.oldPos[i] + base.Projectile.Size * 0.5f - Main.screenPosition, null, Projectile.GetAlpha(lightColor) * MathHelper.Lerp(1f, 0f, (float)i / (float)base.Projectile.oldPos.Length), base.Projectile.rotation, texture.Size() / 2, base.Projectile.scale, SpriteEffects.None, 0);
			return false;
		}
		public override Color? GetAlpha(Color lightColor) {
			if(base.Projectile.timeLeft < 85) {
				byte b2 = (byte)(base.Projectile.timeLeft * 3);
				byte a2 = (byte)(50f * ((float)b2 / 255f));
				return new Color?(new Color((int)b2, (int)b2, (int)b2, (int)a2));
			}
			return new Color?(new Color(255, 255, 255, 50));
		}
	}
}