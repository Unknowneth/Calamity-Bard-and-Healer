using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles;
using ThoriumMod.Projectiles.Bard;

namespace CalamityBardHealer.Projectiles
{
	public class VivapollumExplosion : BardProjectile
	{
		public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod("CalamityEntropy");
		public override string Texture => "CalamityEntropy/Assets/Extra/shockwave";
		public override void SetStaticDefaults() {
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 3, base.Projectile.type);
		}
		public override BardInstrumentType InstrumentType => BardInstrumentType.Brass;
		public override void SetBardDefaults() {
			base.Projectile.width = 1;
			base.Projectile.height = 1;
			base.Projectile.aiStyle = -1;
			base.Projectile.hostile = false;
			base.Projectile.friendly = true;
			base.Projectile.DamageType = BardDamage.Instance;
			base.Projectile.tileCollide = false;
			base.Projectile.penetrate = -1;
			base.Projectile.usesLocalNPCImmunity = true;
			base.Projectile.localNPCHitCooldown = -1;
			base.Projectile.timeLeft = 20;
		}
		public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			if(ModLoader.TryGetMod("CalamityEntropy", out Mod entropy)) target.AddBuff(entropy.Find<ModBuff>("LifeOppress").Type, 600);
		}
		public override bool PreDraw(ref Color lightColor) {
			lightColor = Color.SlateBlue;
			lightColor.A = 0;
			lightColor *= base.Projectile.timeLeft * 0.05f;
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, lightColor, Main.rand.NextFloat(MathHelper.TwoPi), texture.Size() / 2, MathHelper.SmoothStep(0.5f, 0f, base.Projectile.timeLeft * 0.05f), SpriteEffects.None, 0);
			return false;
		}
		public override void ModifyDamageHitbox(ref Rectangle hitbox) {
			float size = MathHelper.SmoothStep(108f, 0f, base.Projectile.timeLeft * 0.05f);
			hitbox = new Rectangle((int)(base.Projectile.Center.X - size), (int)(base.Projectile.Center.Y - size), (int)(size * 2f), (int)(size * 2f));
		}
		public override bool? CanHitNPC(NPC target) => target.Distance(base.Projectile.Center) > MathHelper.SmoothStep(108f, 0f, base.Projectile.timeLeft * 0.05f) ? false : null;
		public override bool ShouldUpdatePosition() => false;
	}
}