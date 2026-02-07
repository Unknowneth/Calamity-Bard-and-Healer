using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles;
using ThoriumMod.Projectiles.Bard;

namespace CalamityBardHealer.Projectiles
{
	public class ToxicQuarterNote : BardProjectile
	{
		public override string Texture => "Terraria/Images/Projectile_76";
		public override BardInstrumentType InstrumentType => BardInstrumentType.Brass;
		public override void SetBardDefaults() {
			base.Projectile.CloneDefaults(76);
			base.Projectile.DamageType = BardDamage.Instance;
			base.Projectile.usesLocalNPCImmunity = true;
			base.Projectile.localNPCHitCooldown = 10;
			AIType = 76;
		}
		public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			target.AddBuff(70, 180);
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) target.AddBuff(calamity.Find<ModBuff>("SulphuricPoisoning").Type, 120);
		}
		public override Color? GetAlpha(Color lightColor) => new Color(255, 255, 255, 0);
	}
}