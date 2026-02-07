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
	public class AcidicSaxMist : BardProjectile
	{
		public override string Texture => "CalamityMod/Projectiles/Magic/AcidicSaxMist";
		public override BardInstrumentType InstrumentType => BardInstrumentType.Brass;
		public override void SetStaticDefaults() {
			Main.projFrames[base.Projectile.type] = 10;
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 11, base.Projectile.type);
		}
		public override void SetBardDefaults() {
			base.Projectile.CloneDefaults(ModLoader.GetMod("CalamityMod").Find<ModProjectile>("AcidicSaxMist").Type);
			base.Projectile.DamageType = BardDamage.Instance;
		}
		public override void AI() {
			if(++base.Projectile.frameCounter > 6) {
				if(++base.Projectile.frame > Main.projFrames[base.Projectile.type] - 1) base.Projectile.frame = 0;
				base.Projectile.frameCounter = 0;
			}
			if(base.Projectile.ai[1] == 0f) {
				base.Projectile.ai[1] = 1f;
				SoundEngine.PlaySound(Terraria.ID.SoundID.Item111, base.Projectile.position);
			}
			if(base.Projectile.velocity.X < 0f) {
				base.Projectile.spriteDirection = -1;
				base.Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.Pi;
			}
			else {
				base.Projectile.spriteDirection = 1;
				base.Projectile.rotation = Projectile.velocity.ToRotation();
			}
			if(++base.Projectile.ai[0] >= 90f) if(base.Projectile.alpha < 255) {
				base.Projectile.alpha += 5;
				if(base.Projectile.alpha > 255) {
					base.Projectile.alpha = 255;
					base.Projectile.Kill();
					return;
				}
			}
			else if(base.Projectile.alpha > 80) {
				base.Projectile.alpha -= 30;
				if(base.Projectile.alpha < 80) base.Projectile.alpha = 80;
			}
		}
		public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) target.AddBuff(calamity.Find<ModBuff>("Irradiated").Type, 180);
		}
	}
}