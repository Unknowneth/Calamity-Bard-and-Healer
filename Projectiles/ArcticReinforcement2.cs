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
	public class ArcticReinforcement2 : BardProjectile
	{
		public override string Texture => "CalamityBardHealer/Projectiles/ArcticReinforcement";
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[base.Projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[base.Projectile.type] = 2;
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 4, base.Projectile.type);
		}
		public override BardInstrumentType InstrumentType => BardInstrumentType.Percussion;
		public override void SetBardDefaults() {
			base.Projectile.width = 18;
			base.Projectile.height = 18;
			base.Projectile.aiStyle = 1;
			base.Projectile.friendly = true;
			base.Projectile.tileCollide = true;
			base.Projectile.DamageType = BardDamage.Instance;
			base.Projectile.extraUpdates = 2;
			base.Projectile.alpha = 255;
			base.Projectile.timeLeft = 300;
		}
		public override bool PreAI() {
			int flyTime = 270 - (int)base.Projectile.ai[2] * 30;
			if(base.Projectile.timeLeft < flyTime) return true;
			if(base.Projectile.timeLeft == flyTime && Main.myPlayer == base.Projectile.owner) {
				base.Projectile.velocity = Vector2.Normalize(Main.MouseWorld - base.Projectile.Center) * Main.player[base.Projectile.owner].HeldItem.shootSpeed;
				NetMessage.SendData(27, -1, -1, null, base.Projectile.whoAmI);
				return true;
			}
			base.Projectile.rotation += base.Projectile.velocity.X / 15f;
			base.Projectile.velocity *= 0.95f;
			return false;
		}
		public override void AI() => base.Projectile.rotation = base.Projectile.velocity.ToRotation();
		public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) => target.AddBuff(BuffID.Frostburn2, 180);
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			float fade = MathHelper.Lerp(0.5f, 0f, (int)base.Projectile.alpha / 255f) * (MathHelper.Min(base.Projectile.timeLeft, 30) / 30);
			if(base.Projectile.velocity != Vector2.Zero) for(int i = 0; i < base.Projectile.oldPos.Length; i++) Main.EntitySpriteDraw(texture, base.Projectile.oldPos[i] + base.Projectile.Size * 0.5f - Main.screenPosition, null, lightColor * (i > 0 ? MathHelper.Lerp(0.25f, 0f, (float)i / (float)base.Projectile.oldPos.Length) : 1f), base.Projectile.oldRot[i] + MathHelper.PiOver4, texture.Size() * 0.5f, base.Projectile.scale, SpriteEffects.None, 0);
			return false;
		}
	}
}