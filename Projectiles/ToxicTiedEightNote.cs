using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles;
using ThoriumMod.Projectiles.Bard;

namespace CalamityBardHealer.Projectiles
{
	public class ToxicTiedEightNote : BardProjectile
	{
		public override string Texture => "Terraria/Images/Projectile_78";
		public override BardInstrumentType InstrumentType => BardInstrumentType.Brass;
		public override void SetStaticDefaults() {
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 3, base.Projectile.type);
			mor.Call("addElementProj", 11, base.Projectile.type);
		}
		public override void SetBardDefaults() {
			base.Projectile.CloneDefaults(78);
			base.Projectile.DamageType = BardDamage.Instance;
			base.Projectile.usesLocalNPCImmunity = true;
			base.Projectile.localNPCHitCooldown = 10;
			AIType = 78;
		}
		public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			target.AddBuff(70, 180);
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) target.AddBuff(calamity.Find<ModBuff>("SulphuricPoisoning").Type, 120);
		}
		public override Color? GetAlpha(Color lightColor) => new Color(255, 255, 255, 0);
	}
}