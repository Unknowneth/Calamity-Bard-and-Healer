using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles;
using ThoriumMod.Projectiles.Bard;

namespace CalamityBardHealer.Projectiles
{
	public class Windward : BardProjectile
	{
		public override string Texture => "CalamityMod/Projectiles/Magic/StickyFeather";
		public override BardInstrumentType InstrumentType => BardInstrumentType.Wind;
		public override void SetStaticDefaults() {
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 6, base.Projectile.type);
		}
		public override void SetBardDefaults() {
			base.Projectile.width = 14;
			base.Projectile.height = 14;
			base.Projectile.aiStyle = -1;
			base.Projectile.hostile = false;
			base.Projectile.friendly = true;
			base.Projectile.tileCollide = true;
			base.Projectile.timeLeft = 300;
			base.Projectile.alpha = 255;
			base.Projectile.DamageType = BardDamage.Instance;
		}
		public override void AI() {
			if(base.Projectile.timeLeft < 17) base.Projectile.alpha += 15;
			else if(base.Projectile.alpha > 0) base.Projectile.alpha -= 15;
			base.WindHomingCommon(null, 384f, null, null, true);
			base.Projectile.rotation = base.Projectile.velocity.ToRotation() + MathHelper.PiOver2;
		}
	}
}