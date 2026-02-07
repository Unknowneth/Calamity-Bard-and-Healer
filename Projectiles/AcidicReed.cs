using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles;
using ThoriumMod.Projectiles.Bard;

namespace CalamityBardHealer.Projectiles
{
	public class AcidicReed : BardProjectile
	{
		public override string Texture => "CalamityMod/Projectiles/Magic/AcidicReed";
		public override BardInstrumentType InstrumentType => BardInstrumentType.Brass;
		public override void SetStaticDefaults() {
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 11, base.Projectile.type);
		}
		public override void SetBardDefaults() {
			base.Projectile.CloneDefaults(ModLoader.GetMod("CalamityMod").Find<ModProjectile>("AcidicReed").Type);
			base.Projectile.DamageType = BardDamage.Instance;
		}
		public override void AI() {
			if(base.Projectile.velocity.Y < 10f) base.Projectile.velocity.Y += 0.25f;
			base.Projectile.rotation = base.Projectile.velocity.ToRotation();
		}
		public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) target.AddBuff(calamity.Find<ModBuff>("Irradiated").Type, 180);
		}
	}
}